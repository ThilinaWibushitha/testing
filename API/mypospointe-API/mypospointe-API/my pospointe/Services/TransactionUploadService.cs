using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using my_pospointe_api.Models;
using RestSharp;

namespace my_pospointe_api.Services
{
    public class TransactionUploadService : ITransactionUploadService
    {
        private readonly ILogger<TransactionUploadService> _logger;
        private readonly string _primaryEndpoint;
        private readonly string _backupEndpoint;

        public TransactionUploadService(IConfiguration configuration, ILogger<TransactionUploadService> logger)
        {
            _logger = logger;
            var syncSection = configuration.GetSection("TransactionSync");
            _primaryEndpoint = syncSection.GetValue<string>("PrimaryEndpoint");
            _backupEndpoint = syncSection.GetValue<string>("BackupEndpoint");
        }

        public async Task<bool> UploadAsync(TblSyncTransaction syncTransaction, CancellationToken cancellationToken = default)
        {
            if (syncTransaction == null)
            {
                throw new ArgumentNullException(nameof(syncTransaction));
            }

            if (string.IsNullOrWhiteSpace(syncTransaction.TransactionData))
            {
                _logger.LogWarning("Sync transaction {SyncId} has empty TransactionData.", syncTransaction.SyncId);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(_primaryEndpoint))
            {
                var primaryResult = await TryPostAsync(_primaryEndpoint, syncTransaction.TransactionData, cancellationToken);
                if (primaryResult)
                {
                    _logger.LogInformation("Transaction {SyncId} uploaded to primary endpoint.", syncTransaction.SyncId);
                    return true;
                }
            }

            if (!string.IsNullOrWhiteSpace(_backupEndpoint))
            {
                var backupResult = await TryPostAsync(_backupEndpoint, syncTransaction.TransactionData, cancellationToken);
                if (backupResult)
                {
                    _logger.LogInformation("Transaction {SyncId} uploaded to backup endpoint.", syncTransaction.SyncId);
                    return true;
                }
            }

            _logger.LogWarning("Transaction {SyncId} failed to upload to both primary and backup endpoints.", syncTransaction.SyncId);
            return false;
        }

        private async Task<bool> TryPostAsync(string url, string payload, CancellationToken cancellationToken)
        {
            try
            {
                var options = new RestClientOptions
                {
                    MaxTimeout = -1
                };

                var client = new RestClient(options);
                var request = new RestRequest(url, Method.Post);

                request.AddHeader("Content-Type", "application/json");
                request.AddStringBody(payload, DataFormat.Json);

                var response = await client.ExecuteAsync(request, cancellationToken);

                if (response.IsSuccessful)
                {
                    return true;
                }

                _logger.LogWarning("Upload to {Url} failed with status code {StatusCode}.", url, response.StatusCode);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while uploading transaction to {Url}.", url);
                return false;
            }
        }
    }
}
