namespace NoDoze.Interfaces
{
    public interface ISleepingService
    {
        bool SleepingIsPermitted { get; }

        void PermitSleeping();
        void PreventSleeping();
    }
}