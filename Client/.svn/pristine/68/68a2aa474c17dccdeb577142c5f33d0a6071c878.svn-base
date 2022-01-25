using System;
using System.IO;
using System.Security.AccessControl;

namespace USBNotifyLib
{
    public class AgentManager
    {
        #region + public static void Startup()
        public static void Startup()
        {
            try
            {
                // update agent setting
                new AgentHttpHelp().GetAgentSetting_Http();                        
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.GetBaseException().Message);
            }

            try
            {
                // update UsbWhitelist
                new AgentHttpHelp().GetUsbWhitelist_Http();
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.GetBaseException().Message);
            }

            try
            {
                // 上載 本機資訊
                new AgentHttpHelp().PostPerComputer_Http();                
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.GetBaseException().Message);
            }

            try
            {
                // registry 讀取 UsbFilterEnable 設定
                if (AgentRegistry.UsbFilterEnabled)
                {
                    // 載入 UsbWhitelist cache
                    UsbWhitelistHelp.Reload_UsbWhitelist();                   
                }               
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.GetBaseException().Message);
            }

            try
            {
                // 掃描過濾所有 usb disk
                if (AgentRegistry.UsbFilterEnabled)
                {
                    new UsbFilter().Filter_Scan_All_USB_Disk();
                }                   
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.GetBaseException().Message);
            }

            try
            {
                // 執行 agent 定時任務
                AgentTimer.ReloadTask();
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.GetBaseException().Message);
            }
        }
        #endregion

        #region Stop()
        public static void Stop()
        {
            AgentTimer.ClearTimerTask();
        }
        #endregion

        #region + public static void SetDirACL_AuthenticatedUsers_Modify(string dirPath)
        public static void SetDirACL_AuthenticatedUsers_Modify(string dirPath)
        {
            var dirInfo = new DirectoryInfo(dirPath);
            
            var dirACL = dirInfo.GetAccessControl();

            var rule = new FileSystemAccessRule("Authenticated Users",
                    FileSystemRights.Modify,
                    InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                    PropagationFlags.None,
                    AccessControlType.Allow);

            dirACL.AddAccessRule(rule);
            dirInfo.SetAccessControl(dirACL);
        }
        #endregion
    }
}
