namespace AgentLib.AppService
{
    public interface IAppService
    {
        AppServiceType ServiceType { get; }

        void Start();

        void Stop();

    }
}
