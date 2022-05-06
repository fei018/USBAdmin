using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using HHITtoolsService.Setup;

namespace HHITtoolsService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            this.Committed += ProjectInstaller_Committed;
        }

        private void ProjectInstaller_Committed(object sender, InstallEventArgs e)
        {
            try
            {
                SetupRegistryKey.InitialRegistryKey();

                using (ServiceController sc = new ServiceController(this.serviceInstaller1.ServiceName))
                {
                    if (sc.Status == ServiceControllerStatus.Stopped)
                    {
                        sc.Start();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void ProjectInstaller_BeforeUninstall(object sender, InstallEventArgs e)
        {
            try
            {
                using (ServiceController sc = new ServiceController(this.serviceInstaller1.ServiceName))
                {
                    if (sc.Status == ServiceControllerStatus.Running)
                    {
                        sc.Stop();

                        sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(5));
                    }
                }
            }
            catch (Exception)
            {
            }

            try
            {
                var usb = Process.GetProcessesByName("HHITtoolsUSB.exe");
                if (usb.Any())
                {
                    foreach (var u in usb)
                    {
                        u.Kill();
                    }
                }
            }
            catch (Exception)
            {
            }

            try
            {
                var tray = Process.GetProcessesByName("HHITtoolsTray.exe");
                if (tray.Any())
                {
                    foreach (var t in tray)
                    {
                        t.Kill();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
