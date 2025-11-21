namespace my_pospointe_api.Services
{
    public class SharedStateService
    {
        public long? PrioritySyncShiftId { get; private set; }

        public void SetPrioritySync(long shiftId)
        {
            PrioritySyncShiftId = shiftId;
        }

        public void ClearPrioritySync()
        {
            PrioritySyncShiftId = null;
        }
    }
}
