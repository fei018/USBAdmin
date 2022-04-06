using System.Collections.Generic;

namespace ToolsCommon
{
    public class AgentHttpResponseResult
    {
        public AgentHttpResponseResult(bool succeed = true, string msg = null)
        {
            Succeed = succeed;
            Msg = msg;
        }

        public bool Succeed { get; set; }

        public string Msg { get; set; }

        public IAgentRule AgentRule { get; set; }

        public string UsbWhitelist { get; set; }

        public string DownloadFileBase64 { get; set; }

        // will be drop
        public string UsbFilterData { get; set; }

        public IAgentSetting AgentSetting { get; set; }

        public IPrintTemplate PrintTemplate { get; set; }

        public List<IIPPrinterInfo> SitePrinterList { get; set; }
    }
}
