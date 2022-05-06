using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using ToolsCommon;

namespace AgentLib
{
    public class AgentUpdate
    {
        // C:\ProgramData\HHITtools
        private string _baseDir;

        private string _downloadDir;

        private string _setupExe;

        private string _zipFile;

        public AgentUpdate()
        {
            _baseDir = Environment.ExpandEnvironmentVariables(@"%ProgramData%\HHITtools");

            _downloadDir = Path.Combine(_baseDir, "download");

            _setupExe = Path.Combine(_downloadDir, "Setup.exe");

            _zipFile = Path.Combine(_downloadDir, "Release.zip");
        }

        #region + public static void CheckAndUpdate()
        public static void CheckAndUpdate()
        {
            try
            {
                if (new AgentUpdate().CheckNeedUpdate())
                {
                    new AgentUpdate().Update();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public bool CheckNeedUpdate()
        public bool CheckNeedUpdate()
        {
            try
            {
                var agentResult = AgentHttpHelp.HttpClient_Get(AgentRegistry.AgentConfigUrl);

                string newVersion = agentResult.AgentConfig.AgentVersion;

                // get local version
                var ver = AgentRegistry.AgentVersion;

                if (ver.Equals(newVersion, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.GetBaseException().Message);
                throw;
            }
        }
        #endregion

        #region + public void Update()
        public void Update()
        {
            try
            {
                CleanDownloadDir();

                DownloadFile();

                if (File.Exists(_setupExe))
                {
                    Process.Start(_setupExe);
                }
                else
                {
                    throw new Exception("AgentUpdate.Update(): setup.exe download failed.");
                }
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.Message);
                throw;
            }
        }
        #endregion

        #region + private void CleanDownloadDir()
        private void CleanDownloadDir()
        {
            try
            {
                if (Directory.Exists(_downloadDir))
                {
                    Directory.Delete(_downloadDir, true);
                }

                Directory.CreateDirectory(_downloadDir);
                UtilityTools.SetDirACL_AuthenticatedUsers_Modify(_downloadDir);
            }
            catch (Exception ex)
            {
                throw new Exception("AgentUpdate.CleanDownloadDir(): " + ex.Message);
            }
        }
        #endregion

        #region + private void DownloadFile()
        private void DownloadFile()
        {

            try
            {
                var agentResult = AgentHttpHelp.HttpClient_Get(AgentRegistry.AgentUpdateUrl);

                byte[] downloadCache = Convert.FromBase64String(agentResult.DownloadFileBase64); // convert base64String to byte[]

                using (FileStream fs = new FileStream(_zipFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    fs.Write(downloadCache, 0, downloadCache.Length);
                }

                if (!File.Exists(_zipFile))
                {
                    throw new Exception("File.Exists(_zipFile) failed.");
                }

                ZipFile.ExtractToDirectory(_zipFile, _downloadDir);
            }
            catch (Exception ex)
            {
                throw new Exception("AgentUpdate.DownloadFile(): " + ex.Message);
            }
        }
        #endregion

    }
}
