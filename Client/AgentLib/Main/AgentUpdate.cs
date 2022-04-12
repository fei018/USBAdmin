using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Security.AccessControl;
using ToolsCommon;

namespace AgentLib
{
    public class AgentUpdate
    {
        // C:\ProgramData\HHITtools
        private string _baseDir;

        private string _downloadFileDir;

        private string _updateZipFile;

        private string _updateDir;

        private string _setupExe;

        public AgentUpdate()
        {
            _baseDir = AgentRegistry.AgentDataDir;

            _downloadFileDir = Path.Combine(_baseDir, "download");

            _updateZipFile = Path.Combine(_downloadFileDir, "update.zip");

            _updateDir = Path.Combine(_baseDir, "update");

            _setupExe = Path.Combine(_updateDir, "Setup.exe");
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
                var agentResult =  AgentHttpHelp.HttpClient_Get(AgentRegistry.AgentConfigUrl);

                string newVersion = agentResult.AgentConfig.AgentVersion;

                if (AgentRegistry.AgentVersion.Equals(newVersion, StringComparison.OrdinalIgnoreCase))
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
                CleanUpdateDir();

                DownloadFile();

                if (File.Exists(_setupExe))
                {
                    Process.Start(_setupExe);
                }
                else
                {
                    throw new Exception("setup.exe not exist.");
                }
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.Message);
                throw;
            }
        }
        #endregion

        #region + private void CleanUpdateDir()
        private void CleanUpdateDir()
        {
            try
            {

                if (Directory.Exists(_updateDir))
                {
                    Directory.Delete(_updateDir, true);
                }
                Directory.CreateDirectory(_updateDir);
                UtilityTools.SetDirACL_AuthenticatedUsers_Modify(_updateDir);

                if (Directory.Exists(_downloadFileDir))
                {
                    Directory.Delete(_downloadFileDir, true);
                }
                Directory.CreateDirectory(_downloadFileDir);
                UtilityTools.SetDirACL_AuthenticatedUsers_Modify(_downloadFileDir);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + private void DownloadFile()
        private void DownloadFile()
        {

            try
            {
                var agentResult = AgentHttpHelp.HttpClient_Get(AgentRegistry.AgentUpdateUrl);

                byte[] downloadFile = Convert.FromBase64String(agentResult.DownloadFileBase64); // convert base64String to byte[]

                using (FileStream fs = new FileStream(_updateZipFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    fs.Write(downloadFile, 0, downloadFile.Length);
                }

                // unzip download file
                ZipFile.ExtractToDirectory(_updateZipFile, _updateDir);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

    }
}
