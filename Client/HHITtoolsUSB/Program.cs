using System;
using System.Threading;
using System.Windows.Forms;

namespace HHITtoolsUSB
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            OpenAppOneOnly();

            Application.Run(new HHITtoolsUSBForm());
        }

        #region OpenAppOneOnly()
        private const string _mutexGuid = "32956814-4b61-4bd0-9571-cb6905995f23";
        private static void OpenAppOneOnly()
        {
            Mutex mutex = new Mutex(true, _mutexGuid, out bool flag);
            if (!flag)
            {
                Environment.Exit(1);
            }
        }
        #endregion
    }
}
