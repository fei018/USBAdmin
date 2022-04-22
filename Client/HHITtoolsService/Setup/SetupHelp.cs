using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Reflection;
using System.ServiceProcess;

namespace HHITtoolsService.Setup
{
    class SetupHelp
    {
        private string _registryKeyLocation = "SOFTWARE\\HipHing\\HHITtools";

        #region + private void InitialRegistryKey()
        public void InitialRegistryKey()
        {
            try
            {
                var keys = SetupRegistryKey.Get_HHITtoolsKeys();

                // Registry key location: Computer\HKEY_LOCAL_MACHINE
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    // delete old key
                    hklm.DeleteSubKey(_registryKeyLocation, false);

                    using (var usbKey = hklm.CreateSubKey(_registryKeyLocation, true))
                    {
                        foreach (var s in keys)
                        {
                            usbKey.SetValue(s.Key, s.Value, RegistryValueKind.String);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion


    }
}
