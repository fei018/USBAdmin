using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace SetupClient
{
    public class SetupHelp
    {
        string _newAppDir;
        string _newDataDir;
        string InstallUtilExe;
        string _serviceExe;
        string _setupDir;
        string _setupiniPath;
        string _installServiceBatch;
        string _uninstallServiceBatch;
        string _dllDir;
        string _registryKeyLocation;
        string _oldRegistryKey;

        public SetupHelp()
        {
            InstallUtilExe = "C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\InstallUtil.exe";

            _setupDir = AppDomain.CurrentDomain.BaseDirectory;

            _setupiniPath = Path.Combine(_setupDir, "setup.ini");

            GetNewAppAndDataDir();

            _serviceExe = Path.Combine(_newAppDir, "usbnservice.exe");        

            _installServiceBatch = Path.Combine(_newAppDir, "Service_Install.bat");

            _uninstallServiceBatch = Path.Combine(_newAppDir, "Service_Uninstall.bat");

            _dllDir = Path.Combine(_setupDir, "dll");

            _registryKeyLocation = "SOFTWARE\\Hiphing\\ITSupportTools";

            _oldRegistryKey = "SOFTWARE\\Hiphing\\USBNotify";
        }

        #region + public void Install()
        public void Install()
        {
            try
            {
                UninstallService(out string error);
                Console.WriteLine(error);

                CreateAndCopyNewAppDir();

                WriteBatchFile();

                InitialRegistryKey();

                InstallService(out error);
                Console.WriteLine(error);

                CheckNewDataDir();

                DeleteOldDir();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + private Setupini GetRegistryKey()
        private Dictionary<string, string> GetRegistryKey()
        {
            try
            {
#if DEBUG
                _setupiniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "setupDebug.ini");
                Console.WriteLine(_setupiniPath);
#endif

                Dictionary<string, string> registry = new Dictionary<string, string>();

                var iniInfo = new FileInfo(_setupiniPath);
                if (!iniInfo.Exists)
                {
                    throw new Exception("Setup.ini not exist.");
                }

                var ini = File.ReadAllLines(_setupiniPath);
                if (ini.Length <= 0) throw new Exception("Setup.ini not exist.");

                int count = -1;

                for (int i = 0; i < ini.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(ini[i]))
                    {
                        if (ini[i].Trim().ToLower() == "[registry]")
                        {
                            count = i;
                            continue;
                        }
                        if (count >= 0)
                        {
                            if (Regex.IsMatch(ini[i].Trim().ToLower(), "\\[[a-z]{1,}\\]"))
                            {
                                break;
                            }

                            if (ini[i].Contains('='))
                            {
                                registry.Add(ini[i].Split('=')[0].Trim(), ini[i].Split('=')[1].Trim());
                            }
                        }
                    }
                }

                return registry;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + private void InitialRegistryKey()
        public void InitialRegistryKey()
        {
            try
            {
                var keys = GetRegistryKey();

                // Registry key location: Computer\HKEY_LOCAL_MACHINE
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    var subKey = _registryKeyLocation;

                    // delete old key
                    hklm.DeleteSubKey(subKey, false);

                    hklm.DeleteSubKey(_oldRegistryKey, false);

                    using (var usbKey = hklm.CreateSubKey(subKey, true))
                    {
                        foreach (var s in keys)
                        {
                            usbKey.SetValue(s.Key, s.Value, RegistryValueKind.String);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + private void CreateAndCopyInstallDir()
        private void CreateAndCopyNewAppDir()
        {
            // _newAppDir

            if (Directory.Exists(_newAppDir))
            {
                Directory.Delete(_newAppDir, true);
            }

            var dir = Directory.CreateDirectory(_newAppDir);
            var dirACL = dir.GetAccessControl();

            var rule = new FileSystemAccessRule("Authenticated Users",
                            FileSystemRights.ReadAndExecute,
                            InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                            PropagationFlags.None,
                            AccessControlType.Allow);
            dirACL.AddAccessRule(rule);
            dir.SetAccessControl(dirACL);

            var files = Directory.GetFiles(_dllDir);
            foreach (var f in files)
            {
                File.Copy(f, Path.Combine(_newAppDir, Path.GetFileName(f)), true);
            }
        }

        private void CheckNewDataDir()
        {
            var rule = new FileSystemAccessRule("Authenticated Users",
                                FileSystemRights.Modify,
                                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                                PropagationFlags.None,
                                AccessControlType.Allow);

            if (Directory.Exists(_newDataDir))
            {
                var dir = new DirectoryInfo(_newDataDir);

                var dirACL = dir.GetAccessControl();
              
                dirACL.AddAccessRule(rule);
                dir.SetAccessControl(dirACL);
            }
            else
            {
                var dir = Directory.CreateDirectory(_newDataDir);

                var dirACL = dir.GetAccessControl();

                dirACL.AddAccessRule(rule);
                dir.SetAccessControl(dirACL);
            }                      
        }
        #endregion
      
        #region + private bool InstallService(out string error)
        private bool InstallService(out string error)
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

                p.StandardInput.WriteLine($"\"{InstallUtilExe}\" \"{_serviceExe}\"");
                Thread.Sleep(new TimeSpan(0, 0, 5));

                p.StandardInput.WriteLine("net start usbnservice");
                Thread.Sleep(new TimeSpan(0, 0, 2));

                p.StandardInput.WriteLine("exit");

                error = p.StandardError.ReadToEnd();

                return run;
            }

        }
        #endregion

        #region + private bool UninstallService(out string error)
        private bool UninstallService(out string error)
        {
            var serviceExist = ServiceController.GetServices().Any(s => s.ServiceName == "usbnservice");
            if (!serviceExist)
            {
                error = null;
                return true;
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

                p.StandardInput.WriteLine("net stop usbnservice");
                Thread.Sleep(new TimeSpan(0, 0, 2));

                p.StandardInput.WriteLine($"\"{InstallUtilExe}\" /u \"{_serviceExe}\"");
                Thread.Sleep(new TimeSpan(0, 0, 5));

                p.StandardInput.WriteLine("exit");

                error = p.StandardError.ReadToEnd();

                return run;
            }

        }
        #endregion

        #region WriteBatchFile
        private string WriteBatchFile()
        {
            var sb = new StringBuilder();

            // service_install.bat
            sb.AppendLine($"\"{InstallUtilExe}\" \"{_serviceExe}\"");
            sb.AppendLine("net start usbnservice");

            File.WriteAllText(_installServiceBatch, sb.ToString(), new UTF8Encoding(false));

            // service_uninstall.bat
            sb.Clear();
            sb.AppendLine("net stop usbnservice");
            sb.AppendLine($"\"{InstallUtilExe}\" /u \"{_serviceExe}\"");
            File.WriteAllText(_uninstallServiceBatch, sb.ToString(), new UTF8Encoding(false));

            return _installServiceBatch;
        }
        #endregion

        #region GetNewAppAndDataDir()
        private void GetNewAppAndDataDir()
        {
            try
            {
                if (File.Exists(_setupiniPath))
                {
                    var lines = File.ReadAllLines(_setupiniPath);
                    if (lines == null || lines.Length <= 0)
                    {
                        throw new Exception("Setup.ini is empty.");
                    }

                    foreach (var l in lines)
                    {
                        if (l.Contains("newAppDir"))
                        {
                            var path = l.Split('=')[1];
                            var dir = Environment.ExpandEnvironmentVariables(path);

                            _newAppDir = dir;
                        }

                        if (l.Contains("newDataDir"))
                        {
                            var path = l.Split('=')[1];
                            var dir = Environment.ExpandEnvironmentVariables(path);

                            _newDataDir = dir;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region DeleteOldDir()
        private void DeleteOldDir()
        {
            try
            {
                if (File.Exists(_setupiniPath))
                {
                    var lines = File.ReadAllLines(_setupiniPath);
                    if (lines == null || lines.Length <= 0)
                    {
                        return;
                    }
 
                    foreach (var l in lines)
                    {
                        if (l.Contains("oldAppDir"))
                        {
                            var path = l.Split('=')[1];
                            var dir = Environment.ExpandEnvironmentVariables(path);
                            if (Directory.Exists(dir))
                            {
                                Directory.Delete(dir);
                            }
                        }

                        if (l.Contains("oldDataDir"))
                        {
                            var path = l.Split('=')[1];
                            var dir = Environment.ExpandEnvironmentVariables(path);
                            if (Directory.Exists(dir))
                            {
                                Directory.Delete(dir);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}
