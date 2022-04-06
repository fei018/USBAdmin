using System;
using System.Windows;
using AgentLib;

namespace HHITtoolsTray
{
    public class AppManager
    {
        #region Start
        public static void Start()
        {
            try
            {
                AppService.NamedPipeClient = new NamedPipeClient_Tray();
                AppService.NamedPipeClient.Start();                
            }
            catch (Exception)
            {
            }

            try
            {
                AppService.TrayIcon = new TrayIcon();
                AppService.TrayIcon.Start();
            }
            catch (Exception)
            {
            }
          
        }
        #endregion

        #region Stop
        public static void Stop()
        {
            try
            {
                AppService.TrayIcon.Stop();
            }
            catch (Exception)
            {
            }

            try
            {
                AppService.NamedPipeClient.Stop();
            }
            catch (Exception)
            {
            }           
        }
        #endregion
    }
}
