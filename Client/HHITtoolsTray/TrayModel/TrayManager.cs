﻿using System;
using System.Windows;
using AgentLib;

namespace HHITtoolsTray
{
    public class TrayManager
    {
        #region Start
        public static void Start()
        {
            try
            {
                PipeClient_Tray.Entity_Tray = new PipeClient_Tray();
                PipeClient_Tray.Entity_Tray.Start();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                TrayIcon.Entity = new TrayIcon();
                TrayIcon.Entity.AddTrayIcon();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
          
        }
        #endregion

        #region Stop
        public static void Stop()
        {
            try
            {
                TrayIcon.Entity?.RemoveTrayIcon();
            }
            catch (Exception)
            {
            }

            try
            {
                PipeClient_Tray.Entity_Tray?.Stop();
            }
            catch (Exception)
            {
            }           
        }
        #endregion
    }
}
