using System;
using System.IO;
using System.Linq;
using System.ServiceProcess;

namespace HHITtoolsService.Setup
{
    public class SetupHelp
    {
        private string _logPath = Path.Combine(Environment.ExpandEnvironmentVariables(@"%ProgramData%\HHITtools"), "Setup_log.txt");

        private string _ServiceName = "HHITtoolsService";

        private string _ServicePath = Environment.ExpandEnvironmentVariables(@"%ProgramFiles%\HHITtools\HHITtoolsService.exe");

        public void Install()
        {
            try
            {
                if (File.Exists(_logPath))
                {
                    File.Delete(_logPath);
                }

                if (ServiceController.GetServices().Any(s => s.ServiceName.ToLower() == _ServiceName.ToLower()))
                {
                    WinServiceHelper.Uninstall(_ServiceName);
                }

                WinServiceHelper.Install(_ServiceName, null, _ServicePath, null, ServiceStartType.Auto);

                if (!ServiceController.GetServices().Any(s => s.ServiceName.ToLower() == _ServiceName.ToLower()))
                {
                    throw new Exception("Service not exist.");
                }

                using (var serv = new ServiceController(_ServiceName))
                {
                    if (serv.Status == ServiceControllerStatus.Stopped)
                    {
                        serv.Start();
                        serv.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
                    }
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(_logPath, "Error: SetupHelp.Install():\r\n" + ex.Message + "\r\n");
            }
        }

        public void Uninstall()
        {
            try
            {
                if (File.Exists(_logPath))
                {
                    File.Delete(_logPath);
                }

                if (ServiceController.GetServices().Any(s => s.ServiceName.ToLower() == _ServiceName.ToLower()))
                {
                    WinServiceHelper.Uninstall(_ServiceName);
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(_logPath, "Error: SetupHelp.Uninstall():\r\n" + ex.Message + "\r\n");
            }
        }
    }
}
