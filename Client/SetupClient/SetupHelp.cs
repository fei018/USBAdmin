using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Security.AccessControl;
using System.ServiceProcess;
using System.Text;

namespace SetupClient
{
    public class SetupHelp
    {
        public static string LogPath;

        string _newAppDir;
        string _newDataDir;
        string InstallUtilExe;
        string _serviceExe;
        string _setupDir;

        string _installServiceBatch;
        string _uninstallServiceBatch;

        string _serviceName;
        string _DllDir;

        public SetupHelp()
        {
            LogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "setup_log.txt");

            InstallUtilExe = "C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\InstallUtil.exe";

            _serviceName = "HHITtoolsService";

            _setupDir = AppDomain.CurrentDomain.BaseDirectory;

            _newAppDir = Environment.ExpandEnvironmentVariables(@"%ProgramFiles%\HHITtools");
            _newDataDir = Environment.ExpandEnvironmentVariables(@"%ProgramData%\HHITtools");

            _serviceExe = Path.Combine(_newAppDir, "HHITtoolsService.exe");

            _installServiceBatch = Path.Combine(_newAppDir, "Service_Install.bat");

            _uninstallServiceBatch = Path.Combine(_newAppDir, "Service_Uninstall.bat");
            _DllDir = Path.Combine(_setupDir, "dll");
        }

        #region + public void Install()
        public void Install()
        {
            try
            {
                if (File.Exists(LogPath))
                {
                    File.Delete(LogPath);
                }

                SetupRegistryKey.InitialRegistryKey();

                UninstallService();

                CreateNewAppDir();
                CheckNewDataDir();

                CopyDllFile();

                WriteBatchFile();

                InstallService();               
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + private void CreateNewAppDir()
        private void CreateNewAppDir()
        {
            // _newAppDir

            DirectoryInfo dir = new DirectoryInfo(_newAppDir);

            if (!dir.Exists)
            {
                dir.Create();
            }
            else
            {
                dir.Delete(true);
                dir.Create();
            }

            if (!dir.Exists)
            {
                throw new Exception("Error: " + dir.FullName + " create failed.");
            }

            // 設置權限
            try
            {
                var dirACL = dir.GetAccessControl();
                var rule = new FileSystemAccessRule("Authenticated Users",
                                FileSystemRights.ReadAndExecute,
                                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                                PropagationFlags.None,
                                AccessControlType.Allow);
                dirACL.AddAccessRule(rule);
                dir.SetAccessControl(dirACL);
            }
            catch (Exception ex)
            {
                File.AppendAllText(LogPath, ex.Message + "\r\n");
            }
        }
        #endregion

        #region + CheckNewDataDir()
        private void CheckNewDataDir()
        {
            var rule = new FileSystemAccessRule("Authenticated Users",
                                FileSystemRights.Modify,
                                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                                PropagationFlags.None,
                                AccessControlType.Allow);

            try
            {
                var dir = new DirectoryInfo(_newDataDir);

                if (!dir.Exists)
                {
                    dir.Create();
                }

                var dirACL = dir.GetAccessControl();

                dirACL.AddAccessRule(rule);
                dir.SetAccessControl(dirACL);
            }
            catch (Exception ex)
            {
                File.AppendAllText(LogPath, ex.Message + "\r\n");
            }
        }
        #endregion

        #region + private bool InstallService()
        private void InstallService()
        {
            //var start = new ProcessStartInfo();
            //start.FileName = "cmd.exe";
            //start.UseShellExecute = false;
            //start.WorkingDirectory = Environment.CurrentDirectory;
            //start.CreateNoWindow = true;
            //start.RedirectStandardError = true;
            //start.RedirectStandardInput = true;
            //start.RedirectStandardOutput = true;

            //using (Process p = new Process())
            //{
            //    p.StartInfo = start;

            //    var run = p.Start();

            //    p.StandardInput.WriteLine($"\"{InstallUtilExe}\" \"{_serviceExe}\"");

            //    p.StandardInput.WriteLine("exit");

            //    p.WaitForExit();

            //    string output = p.StandardOutput.ReadToEnd();

            //    if (!string.IsNullOrWhiteSpace(output))
            //    {
            //        File.AppendAllText(LogPath, output + "\r\n");
            //    }
            //}

            WinServiceHelper.Install(_serviceName, null, _serviceExe, null, ServiceStartType.Auto);

            //service

            var serviceExist = ServiceController.GetServices().Any(s => s.ServiceName.ToLower() == _serviceName.ToLower());
            if (!serviceExist)
            {
                throw new Exception("SetupHelp.InstallService() error: Service not exist.");
            }

            using (var serv = new ServiceController(_serviceName))
            {
                if (serv.Status == ServiceControllerStatus.Stopped)
                {
                    serv.Start();

                    serv.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
                }

            }

            Console.WriteLine("Install Service done.");
        }
        #endregion

        #region + private bool UninstallService()
        private void UninstallService()
        {
            Console.WriteLine("Unistall serive start...");

            var serviceExist = ServiceController.GetServices().Any(s => s.ServiceName.ToLower() == _serviceName.ToLower());
            if (!serviceExist)
            {
                return;
            }

            using (var serv = new ServiceController(_serviceName))
            {
                if (serv.Status == ServiceControllerStatus.Running)
                {
                    serv.Stop();
                    serv.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                }
            }

            WinServiceHelper.Uninstall(_serviceName);

            Console.WriteLine("Unistall serivce done.");

            //string servicePath = null;

            //using (ManagementObjectSearcher searcher = new ManagementObjectSearcher($"SELECT * FROM Win32_Service Where Name='{_serviceName}'"))
            //using (ManagementObjectCollection collection = searcher.Get())
            //{
            //    foreach (ManagementObject obj in collection)
            //    {
            //        servicePath = obj["PathName"] as string;
            //    }
            //}
           
            //var start = new ProcessStartInfo();
            //start.FileName = "cmd.exe";
            //start.UseShellExecute = false;
            //start.WorkingDirectory = Environment.CurrentDirectory;
            //start.CreateNoWindow = true;
            //start.RedirectStandardError = true;
            //start.RedirectStandardInput = true;
            //start.RedirectStandardOutput = true;

            //using (Process p = new Process())
            //{
            //    p.StartInfo = start;

            //    var run = p.Start();

            //    p.StandardInput.WriteLine($"\"{InstallUtilExe}\" /u {servicePath}");

            //    p.StandardInput.WriteLine("exit");

            //    p.WaitForExit();

            //    string output = p.StandardOutput.ReadToEnd();

            //    if (!string.IsNullOrWhiteSpace(output))
            //    {
            //        File.AppendAllText(LogPath, output + "\r\n");
            //    }
            //}
        }
        #endregion

        #region WriteBatchFile
        private string WriteBatchFile()
        {
            var sb = new StringBuilder();

            try
            {
                // service_install.bat
                sb.AppendLine($"\"{InstallUtilExe}\" \"{_serviceExe}\"");
                sb.AppendLine($"net start {_serviceName}");

                File.WriteAllText(_installServiceBatch, sb.ToString(), new UTF8Encoding(false));

                // service_uninstall.bat
                sb.Clear();
                sb.AppendLine($"net stop {_serviceName}");
                sb.AppendLine($"\"{InstallUtilExe}\" /u \"{_serviceExe}\"");
                File.WriteAllText(_uninstallServiceBatch, sb.ToString(), new UTF8Encoding(false));
            }
            catch (Exception ex)
            {
                File.AppendAllText(LogPath, ex.Message + "\r\n");
            }

            Console.WriteLine("Write batch file done.");
            return _installServiceBatch;
        }
        #endregion


        #region + private void CopyDllFile()
        private void CopyDllFile()
        {
            var dll = new DirectoryInfo(_DllDir);
            if (!dll.Exists)
            {
                throw new Exception("SetupHelp.CopyDllFile(): dll folder not exist.");
            }

            foreach (var file in dll.EnumerateFiles())
            {
                string destName = Path.Combine(_newAppDir, file.Name);
                file.CopyTo(destName, true);
            }

            Console.WriteLine("Copy dll files done.");
        }
        #endregion
    }
}
