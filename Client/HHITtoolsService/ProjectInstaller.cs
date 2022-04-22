using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;

namespace HHITtoolsService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            this.BeforeUninstall += ProjectInstaller_BeforeUninstall;
        }


        private void ProjectInstaller_BeforeUninstall(object sender, InstallEventArgs e)
        {
            try
            {
                using (ServiceController sc = new ServiceController(this.serviceInstaller1.ServiceName))
                {
                    if (sc != null)
                    {
                        if (sc.Status == ServiceControllerStatus.Running)
                        {
                            sc.Stop();

                            sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(5));
                        }
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
