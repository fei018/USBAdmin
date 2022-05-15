using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AgentLib
{
    public class AgentLogger
    {
        private static readonly string _baseDir = AgentRegistry.AgentDataDir;


        private static string LogPath => Path.Combine(_baseDir, "log.txt");

        private static string ErrorPath => Path.Combine(_baseDir, "error.txt");

        public static void Log(string log)
        {
            LogToFile(LogPath, log);
        }

        public static void Error(string error)
        {
            LogToFile(ErrorPath, error);
        }

        private readonly static object _locker = new object();
        private const string _lockerGuid = "91825b3e-7810-475d-9559-baf831952561";

        static void LogToFile(string path, string log)
        {
            if (!File.Exists(_baseDir))
            {
                Directory.CreateDirectory(_baseDir);
            }

            Task.Run(() =>
            {
                //lock (_locker)
                //{
                //    try
                //    {
                //        var l = Environment.NewLine + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine + log + Environment.NewLine;
                //        File.AppendAllText(path, l);
                //    }
                //    catch (Exception)
                //    {
                //    }
                //}

                // lock multi process write one file
                Mutex mutex = null;
                try
                {
                    mutex = new Mutex(false, _lockerGuid);
                    mutex.WaitOne();

                    var l = Environment.NewLine + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine + log + Environment.NewLine;
                    File.AppendAllText(path, l);
                }
                catch (Exception)
                {
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            });         
        }
    }
}
