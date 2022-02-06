using System;
using System.Diagnostics;
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
                //Debugger.Break();
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
            try
            {
                AgentTimer.ClearTimerTask();
            }
            catch (Exception) { }
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

        #region + public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }
        #endregion
    }
}
