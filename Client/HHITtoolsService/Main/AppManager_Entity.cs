using AgentLib;

namespace HHITtoolsService
{
    public class AppManager_Entity
    {
        public static PipeServer_Service PipeServer_Service { get; set; }

        public static PrintJobNotify PrintJobNotify { get; set; }

        public static AgentTimer AppTimer { get; set; }

        public static void Initial()
        {
            PipeServer_Service = new PipeServer_Service();

            PrintJobNotify = new PrintJobNotify();

            AppTimer = new AgentTimer();
        }
    }
}
