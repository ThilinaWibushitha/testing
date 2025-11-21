using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using my_pospointe.Models;
using my_pospointe_api.Models;
using my_pospointe_api.Services;

namespace my_pospointe_api.Services.BackgroundServices
{
    public class TransactionSyncService : BackgroundService
    {
        private readonly ILogger<TransactionSyncService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly SharedStateService _sharedStateService;
        private readonly ITransactionUploadService _transactionUploadService;
        private readonly IConfiguration _configuration;
        private Timer _timer;
        private const int RegularSyncIntervalMinutes = 20;
        private const int PrioritySyncIntervalMinutes = 20;

        public TransactionSyncService(ILogger<TransactionSyncService> logger, IServiceProvider serviceProvider, SharedStateService sharedStateService, ITransactionUploadService transactionUploadService, IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _sharedStateService = sharedStateService;
            _transactionUploadService = transactionUploadService;
            _configuration = configuration;
        }

        protected override async System.Threading.Tasks.Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var defaultConnectionFromEnv = Environment.GetEnvironmentVariable("DefaultConnection");
            var defaultConnectionFromConfig = _configuration.GetConnectionString("DefaultConnection");
            var defaultConnection = (!string.IsNullOrWhiteSpace(defaultConnectionFromEnv)) ? defaultConnectionFromEnv
                : (!string.IsNullOrWhiteSpace(defaultConnectionFromConfig)) ? defaultConnectionFromConfig
                : null;
            
            var hasValidConnection = !string.IsNullOrWhiteSpace(defaultConnection);
            
            if (!hasValidConnection)
            {
                _logger.LogInformation("Transaction Sync Service disabled - database not configured.");
                return;
            }
            
            _logger.LogInformation("Transaction Sync Service enabled - SQL Server database configured.");
            
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<PosavanceInventoryContext>();
                    var pendingCount = dbContext.TblSyncTransactions
                        .Where(t => t.Status == SyncStatus.Pending || t.Status == SyncStatus.Failed)
                        .Count();
                    
                    if (pendingCount > 0)
                    {
                        _logger.LogInformation($"Application startup: Found {pendingCount} pending transactions. Starting sync immediately.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking for pending transactions on startup.");
            }
            
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(RegularSyncIntervalMinutes));
        }

        private async void DoWork(object state)
        {
            _logger.LogInformation("Transaction Sync Service is running.");

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<PosavanceInventoryContext>();
                    
                    var priorityShiftId = _sharedStateService.PrioritySyncShiftId;

                    if (priorityShiftId.HasValue)
                    {
                        _logger.LogInformation($"Priority sync mode for shift {priorityShiftId.Value}.");
                        _timer.Change(TimeSpan.FromMinutes(PrioritySyncIntervalMinutes), TimeSpan.FromMinutes(PrioritySyncIntervalMinutes));

                        var priorityTransactions = dbContext.TblSyncTransactions
                            .Where(t => t.ShiftId == priorityShiftId.Value && (t.Status == SyncStatus.Pending || t.Status == SyncStatus.Failed || t.Status == SyncStatus.Uploading))
                            .ToList();

                        if (priorityTransactions.Any())
                        {
                            await ProcessTransactions(priorityTransactions, dbContext);
                        }
                        else
                        {
                            _logger.LogInformation($"All transactions for priority shift {priorityShiftId.Value} are synced. Clearing priority.");
                            _sharedStateService.ClearPrioritySync();
                            _timer.Change(TimeSpan.FromMinutes(RegularSyncIntervalMinutes), TimeSpan.FromMinutes(RegularSyncIntervalMinutes));
                        }
                    }
                    else
                    {
                        var pendingTransactions = dbContext.TblSyncTransactions
                            .Where(t => t.Status == SyncStatus.Pending || t.Status == SyncStatus.Failed || t.Status == SyncStatus.Uploading)
                            .ToList();

                        if (pendingTransactions.Any())
                        {
                            await ProcessTransactions(pendingTransactions, dbContext);
                        }
                        else
                        {
                            _logger.LogInformation("No pending transactions to sync.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Transaction Sync Service. Database may not be configured.");
            }
        }

        private async System.Threading.Tasks.Task ProcessTransactions(System.Collections.Generic.List<TblSyncTransaction> transactions, PosavanceInventoryContext dbContext)
        {
            _logger.LogInformation($"Found {transactions.Count} transactions to sync.");

            foreach (var trans in transactions)
            {
                try
                {
                    trans.Status = SyncStatus.Uploading;
                    trans.AttemptCount++;
                    trans.LastAttemptTime = DateTimeOffset.UtcNow;
                    dbContext.Update(trans);
                    await dbContext.SaveChangesAsync();

                    var uploadSucceeded = await _transactionUploadService.UploadAsync(trans);

                    if (uploadSucceeded)
                    {
                        trans.Status = SyncStatus.Synced;
                        _logger.LogInformation($"Transaction {trans.SyncId} synced successfully.");
                    }
                    else
                    {
                        trans.Status = SyncStatus.Failed;
                        _logger.LogWarning($"Transaction {trans.SyncId} failed to upload and will be retried.");
                    }

                    dbContext.Update(trans);
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to sync transaction {trans.SyncId}.");
                    trans.Status = SyncStatus.Failed;
                    dbContext.Update(trans);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        public override async System.Threading.Tasks.Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Transaction Sync Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            await base.StopAsync(stoppingToken);
        }
    }
}
