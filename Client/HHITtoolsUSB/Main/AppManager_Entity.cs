using AgentLib;

namespace HHITtoolsUSB
{
    public static class AppManager_Entity
    {
        public static NamedPipeClient_USB PipeClient_USB { get; set; }

        public static AgentTimer AppTimer { get; set; }

    }
}
