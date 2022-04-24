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
#if DEBUG
            InitKey();
#else
            Setup();
#endif
           
        }

        static void Setup()
        {
            try
            {
                Console.WriteLine("Setup Start ...");
                Console.WriteLine();

                new SetupHelp().Install();

                Console.WriteLine("Setup Done !!!");
                File.AppendAllText(SetupHelp.LogPath, "Setup done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                File.AppendAllText(SetupHelp.LogPath, ex.Message);
            }

            Environment.Exit(0);
        }

        static void InitKey()
        {
            try
            {
                
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
