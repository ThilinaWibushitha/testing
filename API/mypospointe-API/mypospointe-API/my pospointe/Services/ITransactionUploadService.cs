using System.Threading;
using System.Threading.Tasks;
using my_pospointe_api.Models;

namespace my_pospointe_api.Services
{
    public interface ITransactionUploadService
    {
        Task<bool> UploadAsync(TblSyncTransaction syncTransaction, CancellationToken cancellationToken = default);
    }
}
