using System.Threading.Tasks;

namespace my_pospointe_api.Services
{
    public interface IShiftSyncService
    {
        Task OnShiftOpenedAsync(long shiftId);
        Task OnShiftClosingAsync(long shiftId);
    }
}
