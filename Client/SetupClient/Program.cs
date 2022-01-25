using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SetupClient
{
    class Program
    {
        static void Main(string[] args)
        {
//#if DEBUG
//            InitKey();
//#else
//            Setup();
//#endif
            Setup();
        }

        static void Setup()
        {
            var log = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "setup_log.txt");
            try
            {
                Console.WriteLine("Setup Start ...");
                Console.WriteLine();

                new SetupHelp().Install();

                Console.WriteLine("Setup Done !!!");
                File.WriteAllText(log, "Setup done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                File.WriteAllText(log, ex.Message);
            }

            Environment.Exit(0);
        }

        static void InitKey()
        {
            try
            {
                new SetupHelp().InitialRegistryKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Done.");
            Console.ReadLine();
        }
    }
}
