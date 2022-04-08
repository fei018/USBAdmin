﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetupClient
{
    public class AgentRegistryKey
    {
        public static Dictionary<string, string> Get_HHITtoolsKeys()
        {
            var keys = new Dictionary<string, string>()
            {
                {"AgentAppDir",@"%ProgramFiles%\HHITtools"},
                {"AgentDataDir",@"%ProgramData%\HHITtools"},
                {"RemoteSupportApp","RemoteSupport.exe"},
                {"HHITtoolsServiceApp","HHITtoolsService.exe"},
                {"HHITtoolsUSBApp","HHITtoolsUSB.exe"},
                {"HHITtoolsTrayApp","HHITtoolsTray.exe"},
                {"UsbFilterEnabled","true"},
                {"PrintJobLogEnabled","true"},
                {"AgentTimerMinute","10"},
                {"AgentVersion","1.0.5"},
                {"AgentHttpKey","usbb50ae7e95f144874a2739e119e8791e1"},
                {"UsbWhitelistUrl","http://hhdmstest02.hiphing.com.hk/USBAdmin/ClientGet/UsbWhitelist"},
                {"AgentRuleUrl","http://hhdmstest02.hiphing.com.hk/USBAdmin/ClientGet/AgentRule"},
                {"AgentDownloadUrl","http://hhdmstest02.hiphing.com.hk/USBAdmin/ClientGet/AgentDownload"},
                {"PostUsbRequestUrl","http://hhdmstest02.hiphing.com.hk/USBAdmin/ClientPost/PostUsbRequest"},
                {"PostComputerInfoUrl","http://hhdmstest02.hiphing.com.hk/USBAdmin/ClientPost/PostComputerInfo"},
                {"PostUsbLogUrl","http://hhdmstest02.hiphing.com.hk/USBAdmin/ClientPost/PostUsbLog"},
                {"SitePrinterListUrl","http://hhdmstest02.hiphing.com.hk/USBAdmin/ClientGet/SitePrinterList"},
                {"PostPrintJobLogUrl","http://hhdmstest02.hiphing.com.hk/USBAdmin/ClientPost/PostPrintJobLog"}
            };

            return keys;
        }

        public static Dictionary<string, string> Get_HHITtoolsKeys_Debug()
        {
            var keys = new Dictionary<string, string>()
            {
                {"newAppDir", @"%ProgramFiles%\HHITtools"},
                {"newDataDir",@"%ProgramData%\HHITtools"},
                {"AgentAppDir",@"%ProgramFiles%\HHITtools"},
                {"AgentDataDir",@"%ProgramData%\HHITtools"},
                {"RemoteSupportApp","RemoteSupport.exe"},
                {"HHITtoolsServiceApp","HHITtoolsService.exe"},
                {"HHITtoolsUSBApp","HHITtoolsUSB.exe"},
                {"HHITtoolsTrayApp","HHITtoolsTray.exe"},
                {"UsbFilterEnabled","true"},
                {"PrintJobLogEnabled","true"},
                {"AgentTimerMinute","10"},
                {"AgentVersion","1.0.5"},
                {"AgentHttpKey","usbb50ae7e95f144874a2739e119e8791e1"},
                {"UsbWhitelistUrl","http://127.0.0.1/ClientGet/UsbWhitelist"},
                {"AgentRuleUrl","http://127.0.0.1/ClientGet/AgentRule"},
                {"AgentDownloadUrl","http://127.0.0.1/ClientGet/AgentDownload"},
                {"PostUsbRequestUrl","http://127.0.0.1/ClientPost/PostUsbRequest"},
                {"PostComputerInfoUrl","http://127.0.0.1/ClientPost/PostComputerInfo"},
                {"PostUsbLogUrl","http://127.0.0.1/ClientPost/PostUsbLog"},
                {"SitePrinterListUrl","http://127.0.0.1/ClientGet/SitePrinterList"},
                {"PostPrintJobLogUrl","http://127.0.0.1/ClientPost/PostPrintJobLog"}
            };

            return keys;
        }
    }
}