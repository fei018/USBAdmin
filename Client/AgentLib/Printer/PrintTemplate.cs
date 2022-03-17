using HHITtoolsCommon;

namespace AgentLib
{
    public class PrintTemplate : IPrintTemplate
    {
        public string SiteName { get; set; }
        public string SubnetAddr { get; set; }
        public string FilePath { get; set; }
    }
}
