using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Security.AccessControl;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;

namespace SetupClient
{
    public class SetupHelp
    {
        public static string LogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "setup_log.txt");

        string _newAppDir; 
        string _newDataDir;

        string _serviceExe;
        string _setupDir;

        string _installServiceBatch;
        string _uninstallServiceBatch;

        string _serviceName;

        public SetupHelp()
        {
            _serviceName = "HHITtoolsService";

            _setupDir = AppDomain.CurrentDomain.BaseDirectory;

            _newAppDir = Environment.ExpandEnvironmentVariables(@"%ProgramFiles%\HHITtools");
            _newDataDir = Environment.ExpandEnvironmentVariables(@"%ProgramData%\HHITtools");

            _serviceExe = Path.Combine(_newAppDir, "HHITtoolsService.exe");

            _installServiceBatch = Path.Combine(_newAppDir, "Service_Install.bat");

            _uninstallServiceBatch = Path.Combine(_newAppDir, "Service_Uninstall.bat");

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

                UnzipDll();

                CheckNewDataDir();

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
                throw new Exception("Error: " + dir.FullName + " not exist.");
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
                File.AppendAllText(LogPath, ex.Message+"\r\n"); 
            }
        }


        #endregion

        #region CheckNewDataDir
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
            var start = new ProcessStartInfo();
            start.FileName = "cmd.exe";
            start.UseShellExecute = false;
            start.WorkingDirectory = Environment.CurrentDirectory;
            start.CreateNoWindow = true;
            start.RedirectStandardError = true;
            start.RedirectStandardInput = true;
            start.RedirectStandardOutput = true;

            using (Process p = new Process())
            {
                p.StartInfo = start;

                var run = p.Start();

                p.StandardInput.WriteLine($"call \"{_installServiceBatch}\" {_serviceName} {_serviceExe}");

                p.StandardInput.WriteLine("exit");

                p.WaitForExit();

                string output = p.StandardOutput.ReadToEnd();

                if (!string.IsNullOrWhiteSpace(output))
                {
                    File.AppendAllText(LogPath, output + "\r\n");
                }
            }

            using (ServiceController sc = new ServiceController(_serviceName))
            {
                if (sc == null)
                {
                    throw new Exception("Error: InstallService(): Service not exist.");
                }

                if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    sc.Start();
                }
            }

            return;
        }
        #endregion

        #region + private bool UninstallService()
        private void UninstallService()
        {
            using (var serv = new ServiceController(_serviceName))
            {
                if (serv == null)
                {
                    return;
                }

                //if (serv.Status == ServiceControllerStatus.Running)
                //{
                //    serv.Stop();
                //    serv.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                //}
            }

            var start = new ProcessStartInfo();
            start.FileName = "cmd.exe";
            start.UseShellExecute = false;
            start.WorkingDirectory = Environment.CurrentDirectory;
            start.CreateNoWindow = true;
            start.RedirectStandardError = true;
            start.RedirectStandardInput = true;
            start.RedirectStandardOutput = true;

            using (Process p = new Process())
            {
                p.StartInfo = start;

                var run = p.Start();

                p.StandardInput.WriteLine($"call \"{_uninstallServiceBatch}\" {_serviceName} {_serviceExe}");

                p.StandardInput.WriteLine("exit");

                p.WaitForExit();

                string output = p.StandardOutput.ReadToEnd();

                if (!string.IsNullOrWhiteSpace(output))
                {
                    File.AppendAllText(LogPath, output + "\r\n");
                }

                return;
            }
        }
        #endregion

        #region + private void UnzipDll()
        private void UnzipDll()
        {
            string zip = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dll.zip");

            if (File.Exists(zip))
            {
                File.Delete(zip);
            }

            byte[] cache = SetupClient.Properties.Resources.dll;
            File.WriteAllBytes(zip, cache);

            ZipFile.ExtractToDirectory(zip, _newAppDir);
        }
        #endregion
    }
}
