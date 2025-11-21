using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using my_pospointe.Models;

namespace my_pospointe_api.Services
{
    public class ShiftSyncService : IShiftSyncService
    {
        private readonly ILogger<ShiftSyncService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly SharedStateService _sharedStateService;
        private readonly ITransactionUploadService _transactionUploadService;

        public ShiftSyncService(ILogger<ShiftSyncService> logger, IServiceProvider serviceProvider, SharedStateService sharedStateService, ITransactionUploadService transactionUploadService)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _sharedStateService = sharedStateService;
            _transactionUploadService = transactionUploadService;
        }

        public async Task OnShiftOpenedAsync(long shiftId)
        {
            try
            {
                _logger.LogInformation($"Shift {shiftId} opened. Checking for pending transactions to sync...");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<PosavanceInventoryContext>();

                    var pendingTransactions = dbContext.TblSyncTransactions
                        .Where(t => (t.Status == SyncStatus.Pending || t.Status == SyncStatus.Failed) && t.ShiftId != shiftId)
                        .ToList();

                    if (pendingTransactions.Any())
                    {
                        _logger.LogInformation($"Found {pendingTransactions.Count} pending transactions from previous shifts. Setting priority sync for shift {shiftId}.");
                        _sharedStateService.SetPrioritySync(shiftId);
                    }
                    else
                    {
                        _logger.LogInformation($"No pending transactions from previous shifts for shift {shiftId}.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in OnShiftOpenedAsync for shift {shiftId}.");
            }
        }

        public async Task OnShiftClosingAsync(long shiftId)
        {
            try
            {
                _logger.LogInformation($"Shift {shiftId} is closing. Initiating transaction sync for remaining pending transactions...");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<PosavanceInventoryContext>();

                    var shiftTransactions = dbContext.TblSyncTransactions
                        .Where(t => t.ShiftId == shiftId && (t.Status == SyncStatus.Pending || t.Status == SyncStatus.Failed || t.Status == SyncStatus.Uploading))
                        .ToList();

                    if (shiftTransactions.Any())
                    {
                        _logger.LogInformation($"Processing {shiftTransactions.Count} transactions for shift {shiftId} at shift close.");

                        foreach (var transaction in shiftTransactions)
                        {
                            try
                            {
                                if (transaction.Status != SyncStatus.Uploading)
                                {
                                    transaction.Status = SyncStatus.Uploading;
                                    transaction.AttemptCount++;
                                    transaction.LastAttemptTime = DateTimeOffset.UtcNow;
                                    dbContext.Update(transaction);
                                    await dbContext.SaveChangesAsync();
                                }

                                var uploadSucceeded = await _transactionUploadService.UploadAsync(transaction);

                                if (uploadSucceeded)
                                {
                                    transaction.Status = SyncStatus.Synced;
                                    _logger.LogInformation($"Transaction {transaction.SyncId} synced successfully at shift close.");
                                }
                                else
                                {
                                    transaction.Status = SyncStatus.Failed;
                                    _logger.LogWarning($"Transaction {transaction.SyncId} failed at shift close and will be retried when the next shift opens.");
                                }

                                dbContext.Update(transaction);
                                await dbContext.SaveChangesAsync();
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"Error syncing transaction {transaction.SyncId} at shift close.");
                                transaction.Status = SyncStatus.Failed;
                                dbContext.Update(transaction);
                                await dbContext.SaveChangesAsync();
                            }
                        }

                        var successfulCount = shiftTransactions.Count(t => t.Status == SyncStatus.Synced);
                        var failedCount = shiftTransactions.Count(t => t.Status == SyncStatus.Failed);
                        _logger.LogInformation($"Shift {shiftId} close complete: {successfulCount} synced, {failedCount} failed (will retry on next shift open).");
                    }
                    else
                    {
                        _logger.LogInformation($"No transactions to sync for shift {shiftId} at close.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in OnShiftClosingAsync for shift {shiftId}.");
            }
        }
    }
}
