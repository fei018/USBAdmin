using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsCommon;

namespace AgentLib
{
    public class UsbWhitelist
    {
        private static string _UsbWhitelistFile;

        private static HashSet<string> CacheDb { get; set; }

        private static readonly object _locker_CacheDb = new object();

        public UsbWhitelist()
        {
            try
            {
                _UsbWhitelistFile = AgentRegistry.UsbWhitelistPath ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "usbwhitelist.dat");
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region + private void Reload_UsbWhitelistCache()
        private void Reload_UsbWhitelistCache()
        {
            try
            {
                var table = ReadFile_UsbWhitelist();

                var cache = new HashSet<string>();

                foreach (var line in table)
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            var data = UtilityTools.Base64Decode(line.Trim());
                            cache.Add(data);
                        }
                    }
                    catch (Exception) { }
                }

                lock (_locker_CacheDb)
                {
                    CacheDb = cache;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + private string[] ReadFile_UsbWhitelist()
        private static readonly object _locker_UsbWhitelist = new object();
        private string[] ReadFile_UsbWhitelist()
        {
            lock (_locker_UsbWhitelist)
            {
                try
                {
                    if (File.Exists(_UsbWhitelistFile))
                    {
                        return File.ReadAllLines(_UsbWhitelistFile, new UTF8Encoding(false));
                    }
                    else
                    {
                        throw new Exception("USB Whitelist File not exist.");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        #endregion

        #region + private void WriteFile_UsbWhitelist(string txt)
        private void WriteFile_UsbWhitelist(string txt)
        {
            lock (_locker_UsbWhitelist)
            {
                try
                {
                    File.WriteAllText(_UsbWhitelistFile, txt, new UTF8Encoding(false));
                }
                catch (Exception ex)
                {
                    throw new Exception("Write down USB Whitelist Error:\r\n" + ex.GetBaseException().Message);
                }
            }
        }
        #endregion

        #region + public static bool IsFind(UsbDisk usb)
        public static bool IsFind(UsbDisk usb)
        {
            try
            {
                if (CacheDb == null || CacheDb.Count <= 0)
                {
                    new UsbWhitelist().Reload_UsbWhitelistCache();
                }

                if (CacheDb != null && CacheDb.Count > 0)
                {
                    foreach (var t in CacheDb)
                    {
                        if (t.ToLower() == usb.UsbIdentity)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                AgentLogger.Error(ex.GetBaseException().Message);
                return false;
            }
        }
        #endregion

        #region + public static void Set_UsbWhitelist_byHttp(UsbFilterDbHttp setting)
        public static void Set_And_Load_UsbWhitelist_byHttp(string usbWhitelist)
        {
            try
            {
                new UsbWhitelist().WriteFile_UsbWhitelist(usbWhitelist);

                new UsbWhitelist().Reload_UsbWhitelistCache();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion      
    }
}
