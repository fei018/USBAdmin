//using Microsoft.Win32;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.IO.Compression;
//using System.Linq;
//using System.Management;
//using System.Security.AccessControl;
//using System.ServiceProcess;
//using System.Text;
//using System.Text.RegularExpressions;

//namespace SetupClient
//{
//    public class SetupHelp2
//    {
//        public static string LogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "setup_log.txt");

//        string _newAppDir; 
//        string _newDataDir;
//        string InstallUtilExe;
//        string _serviceExe;
//        string _setupDir;

//        string _installServiceBatch;
//        string _uninstallServiceBatch;

//        string _registryKeyLocation;

//        string _serviceName;

//        public SetupHelp2()
//        {
//            InstallUtilExe = "C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\InstallUtil.exe";

//            _serviceName = "HHITtoolsService";

//            _setupDir = AppDomain.CurrentDomain.BaseDirectory;

//            _newAppDir = Environment.ExpandEnvironmentVariables(@"%ProgramFiles%\HHITtools");
//            _newDataDir = Environment.ExpandEnvironmentVariables(@"%ProgramData%\HHITtools");

//            _serviceExe = Path.Combine(_newAppDir, "HHITtoolsService.exe");

//            _installServiceBatch = Path.Combine(_newAppDir, "Service_Install.bat");

//            _uninstallServiceBatch = Path.Combine(_newAppDir, "Service_Uninstall.bat");

//            _registryKeyLocation = "SOFTWARE\\HipHing\\HHITtools";

//        }

//        #region + public void Install()
//        public void Install()
//        {
//            try
//            {
//                if (File.Exists(LogPath))
//                {
//                    File.Delete(LogPath);
//                }

//                UninstallService();

//                CreateNewAppDir();

//                UnzipDll();

//                WriteBatchFile();

//                InitialRegistryKey();

//                InstallService();

//                CheckNewDataDir();
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }
//        #endregion

//        #region + private void InitialRegistryKey()
//        public void InitialRegistryKey()
//        {
//            try
//            {
//                var keys = SetupRegistryKey.Get_HHITtoolsKeys();

//                // Registry key location: Computer\HKEY_LOCAL_MACHINE
//                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
//                {
//                    // delete old key
//                    hklm.DeleteSubKey(_registryKeyLocation, false);

//                    using (var usbKey = hklm.CreateSubKey(_registryKeyLocation, true))
//                    {
//                        foreach (var s in keys)
//                        {
//                            usbKey.SetValue(s.Key, s.Value, RegistryValueKind.String);
//                        }
//                    }
//                }
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }
//        #endregion

//        #region + private void CreateNewAppDir()
//        private void CreateNewAppDir()
//        {
//            // _newAppDir

//            DirectoryInfo dir = new DirectoryInfo(_newAppDir);

//            if (!dir.Exists)
//            {
//                dir.Create();
//            }
//            else
//            {
//                dir.Delete(true);
//                dir.Create();
//            }


//            // 設置權限
//            try
//            {
//                var dirACL = dir.GetAccessControl();
//                var rule = new FileSystemAccessRule("Authenticated Users",
//                                FileSystemRights.ReadAndExecute,
//                                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
//                                PropagationFlags.None,
//                                AccessControlType.Allow);
//                dirACL.AddAccessRule(rule);
//                dir.SetAccessControl(dirACL);
//            }
//            catch (Exception ex) 
//            { 
//                File.AppendAllText(LogPath, ex.Message+"\r\n"); 
//            }
//        }

//        private void CheckNewDataDir()
//        {
//            var rule = new FileSystemAccessRule("Authenticated Users",
//                                FileSystemRights.Modify,
//                                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
//                                PropagationFlags.None,
//                                AccessControlType.Allow);

//            try
//            {
//                var dir = new DirectoryInfo(_newDataDir);

//                if (!dir.Exists)
//                {
//                    dir.Create();
//                }

//                var dirACL = dir.GetAccessControl();

//                dirACL.AddAccessRule(rule);
//                dir.SetAccessControl(dirACL);
//            }
//            catch (Exception ex)
//            {
//                File.AppendAllText(LogPath, ex.Message+"\r\n");
//            }
//        }
//        #endregion

//        #region + private bool InstallService()
//        private bool InstallService()
//        {
//            var start = new ProcessStartInfo();
//            start.FileName = "cmd.exe";
//            start.UseShellExecute = false;
//            start.WorkingDirectory = Environment.CurrentDirectory;
//            start.CreateNoWindow = true;
//            start.RedirectStandardError = true;
//            start.RedirectStandardInput = true;
//            start.RedirectStandardOutput = true;

//            using (Process p = new Process())
//            {
//                p.StartInfo = start;

//                var run = p.Start();

//                p.StandardInput.WriteLine($"\"{InstallUtilExe}\" \"{_serviceExe}\"");

//                p.StandardInput.WriteLine("exit");

//                p.WaitForExit();

//                string output = p.StandardOutput.ReadToEnd();

//                if (!string.IsNullOrWhiteSpace(output))
//                {
//                    File.AppendAllText(LogPath, output + "\r\n");
//                }
//            }

//            // service

//            //var serviceExist = ServiceController.GetServices().Any(s => s.ServiceName == _serviceName);
//            //if (!serviceExist)
//            //{
//            //    throw new Exception("SetupHelp.InstallService() error: Service not exist.");
//            //}

//            //using (var serv = new ServiceController(_serviceName))
//            //{
//            //    if (serv.Status == ServiceControllerStatus.Stopped)
//            //    {
//            //        serv.Start();

//            //        serv.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMinutes(2));
//            //    }
               
//            //}

//            return true;
//        }
//        #endregion

//        #region + private bool UninstallService()
//        private bool UninstallService()
//        {
//            var serviceExist = ServiceController.GetServices().Any(s => Regex.IsMatch(s.ServiceName, _serviceName, RegexOptions.IgnoreCase));
//            if (!serviceExist)
//            {
//                return true;
//            }

//            string unistallServicePath = null;

//            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Service"))
//            using (ManagementObjectCollection collection = searcher.Get())
//            {
//                foreach (ManagementObject obj in collection)
//                {
//                    string name = obj["Name"] as string;
//                    string pathName = obj["PathName"] as string;

//                    if (name.Trim().ToLower() == _serviceName.ToLower())
//                    {
//                        unistallServicePath = pathName;
//                    }
//                }
//            }

//            using (var serv = new ServiceController(_serviceName))
//            {
//                if (serv.CanStop)
//                {
//                    serv.Stop();
//                }

//                serv.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMinutes(2));
//            }

//            var start = new ProcessStartInfo();
//            start.FileName = "cmd.exe";
//            start.UseShellExecute = false;
//            start.WorkingDirectory = Environment.CurrentDirectory;
//            start.CreateNoWindow = true;
//            start.RedirectStandardError = true;
//            start.RedirectStandardInput = true;
//            start.RedirectStandardOutput = true;

//            using (Process p = new Process())
//            {
//                p.StartInfo = start;

//                var run = p.Start();

//                p.StandardInput.WriteLine($"\"{InstallUtilExe}\" /u {unistallServicePath}");

//                p.StandardInput.WriteLine("exit");

//                p.WaitForExit();

//                string output = p.StandardOutput.ReadToEnd();

//                if (!string.IsNullOrWhiteSpace(output))
//                {
//                    File.AppendAllText(LogPath, output + "\r\n");
//                }

//                return run;
//            }
//        }
//        #endregion

//        #region WriteBatchFile
//        private string WriteBatchFile()
//        {
//            var sb = new StringBuilder();

//            try
//            {
//                // service_install.bat
//                sb.AppendLine($"\"{InstallUtilExe}\" \"{_serviceExe}\"");
//                sb.AppendLine($"net start {_serviceName}");

//                File.WriteAllText(_installServiceBatch, sb.ToString(), new UTF8Encoding(false));

//                // service_uninstall.bat
//                sb.Clear();
//                sb.AppendLine($"net stop {_serviceName}");
//                sb.AppendLine($"\"{InstallUtilExe}\" /u \"{_serviceExe}\"");
//                File.WriteAllText(_uninstallServiceBatch, sb.ToString(), new UTF8Encoding(false));
//            }
//            catch (Exception ex)
//            {
//                File.AppendAllText(LogPath, ex.Message+"\r\n");
//            }

//            return _installServiceBatch;
//        }
//        #endregion


//        #region + private void UnzipDll()
//        private void UnzipDll()
//        {
//            string zip = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dll.zip");

//            if (File.Exists(zip))
//            {
//                File.Delete(zip);
//            }

//            byte[] cache = SetupClient.Properties.Resources.dll;
//            File.WriteAllBytes(zip, cache);

//            ZipFile.ExtractToDirectory(zip, _newAppDir);
//        }
//        #endregion
//    }
//}
