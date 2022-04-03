using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsCommon
{
    public interface IPerUsbLog : IUsbInfo
    {

        string ComputerIdentity { get; set; }

        DateTime PluginTime { get; set; }

        string PluginTimeString { get; }
    }
}
