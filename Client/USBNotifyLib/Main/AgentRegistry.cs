using Microsoft.Win32;
using System;
using System.IO;

namespace USBNotifyLib
{
    public class AgentRegistry
    {
        private const string _usbRegistryKey = "SOFTWARE\\HipHing\\HHITtools";


        // Registry

        public static string AgentHttpKey
        {
            get => ReadRegKey(nameof(AgentHttpKey));
            set => SetRegKey(nameof(AgentHttpKey), value, RegistryValueKind.String);
        }

        public static string AgentAppDir
        {
            get => Environment.ExpandEnvironmentVariables(ReadRegKey(nameof(AgentAppDir)));
            set => SetRegKey(nameof(AgentAppDir), value, RegistryValueKind.String);
        }

        public static string AgentDataDir
        {
            get => Environment.ExpandEnvironmentVariables(ReadRegKey(nameof(AgentDataDir)));
            set => SetRegKey(nameof(AgentDataDir), value, RegistryValueKind.String);
        }

        public static string AgentServiceExe
        {
            get => Path.Combine(AgentAppDir, ReadRegKey(nameof(AgentServiceExe)));
            set => SetRegKey(nameof(AgentServiceExe), value, RegistryValueKind.String);
        }

        public static string AgentExe
        {
            get => Path.Combine(AgentAppDir, ReadRegKey(nameof(AgentExe)));
            set => SetRegKey(nameof(AgentExe), value, RegistryValueKind.String);
        }

        public static string AgentTrayExe
        {
            get => Path.Combine(AgentAppDir, ReadRegKey(nameof(AgentTrayExe)));
            set => SetRegKey(nameof(AgentTrayExe), value, RegistryValueKind.String);
        }

        public static string RemoteSupportExe
        {
            get => Path.Combine(AgentAppDir, ReadRegKey(nameof(RemoteSupportExe)));
            set => SetRegKey(nameof(RemoteSupportExe), value, RegistryValueKind.String);
        }

        public static bool UsbFilterEnabled
        {
            get => Convert.ToBoolean(ReadRegKey(nameof(UsbFilterEnabled)));
            set => SetRegKey(nameof(UsbFilterEnabled), value, RegistryValueKind.String);
        }

        public static bool UsbHistoryEnabled
        {
            get => Convert.ToBoolean(ReadRegKey(nameof(UsbHistoryEnabled)));
            set => SetRegKey(nameof(UsbHistoryEnabled), value, RegistryValueKind.String);
        }

        public static string UsbWhitelistUrl
        {
            get => ReadRegKey(nameof(UsbWhitelistUrl));
            set => SetRegKey(nameof(UsbWhitelistUrl), value, RegistryValueKind.String);
        }

        public static string AgentSettingUrl
        {
            get => ReadRegKey(nameof(AgentSettingUrl));
            set => SetRegKey(nameof(AgentSettingUrl), value, RegistryValueKind.String);
        }

        public static int AgentTimerMinute
        {
            get => Convert.ToInt32(ReadRegKey(nameof(AgentTimerMinute)));
            set => SetRegKey(nameof(AgentTimerMinute), value, RegistryValueKind.String);
        }

        public static string AgentVersion
        {
            get => ReadRegKey(nameof(AgentVersion));
            set => SetRegKey(nameof(AgentVersion), value, RegistryValueKind.String);
        }

        public static string AgentUpdateUrl
        {
            get => ReadRegKey(nameof(AgentUpdateUrl));
            set => SetRegKey(nameof(AgentUpdateUrl), value, RegistryValueKind.String);
        }

        public static string PostPerComputerUrl
        {
            get => ReadRegKey(nameof(PostPerComputerUrl));
            set => SetRegKey(nameof(PostPerComputerUrl), value, RegistryValueKind.String);
        }

        public static string PostPerUsbHistoryUrl
        {
            get => ReadRegKey(nameof(PostPerUsbHistoryUrl));
            set => SetRegKey(nameof(PostPerUsbHistoryUrl), value, RegistryValueKind.String);
        }

        public static string PostUsbRequestUrl
        {
            get => ReadRegKey(nameof(PostUsbRequestUrl));
            set => SetRegKey(nameof(PostUsbRequestUrl), value, RegistryValueKind.String);
        }

        public static string PrintTemplateUrl
        {
            get => ReadRegKey(nameof(PrintTemplateUrl));
            set => SetRegKey(nameof(PrintTemplateUrl), value, RegistryValueKind.String);
        }

        public static string SitePrinterListUrl
        {
            get => ReadRegKey(nameof(SitePrinterListUrl));
            set => SetRegKey(nameof(SitePrinterListUrl), value, RegistryValueKind.String);
        }

        public static string PostPerPrintJobUrl
        {
            get => ReadRegKey(nameof(PostPerPrintJobUrl));
            set => SetRegKey(nameof(PostPerPrintJobUrl), value, RegistryValueKind.String);
        }


        #region + private string ReadRegKey(string name)
        private static string ReadRegKey(string name)
        {
            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
                {
                    using (var usbKey = hklm.OpenSubKey(_usbRegistryKey))
                    {
                        var value = usbKey.GetValue(name) as string;
                        return value;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + private static void SetRegKey(string name, object value, RegistryValueKind kind)
        private static void SetRegKey(string name, object value, RegistryValueKind kind)
        {
            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
                {
                    using (var usb = hklm.CreateSubKey(_usbRegistryKey))
                    {
                        usb.SetValue(name, value, kind);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
