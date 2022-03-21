using AgentLib;

namespace HHITtoolsUSB
{
    public static class AppManager_Entity
    {
        public static PipeClient_USB PipeClient_USB { get; set; }

        public static AgentTimer AppTimer { get; set; }


        public static void Initial()
        {
            PipeClient_USB = new PipeClient_USB();

            AppTimer = new AgentTimer();
        }
    }
}
