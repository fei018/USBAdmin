using System;
using System.Threading.Tasks;

namespace AgentLib
{
    public class ToolsManager
    {
        #region Startup()
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

            try
            {
                // start printjob notify
                PrintJobNotify.Start();
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.GetBaseException().Message);
            }
        }
        #endregion

        #region CloseApp()
        public static void CloseApp()
        {
            try
            {
                AgentTimer.ClearTimerTask();
            }
            catch (Exception) { }

            try
            {
                PrintJobNotify.Stop();
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}
