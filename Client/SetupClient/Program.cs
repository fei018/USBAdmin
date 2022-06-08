using System;
using System.IO;
using System.Linq;

namespace SetupClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Count() > 0)
                {
                    if (args[0].ToLower() == "uninstall")
                    {
                        UnSetup();
                    }
                }
                else
                {
                    Setup();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void Setup()
        {
            try
            {
                Console.WriteLine("Setup Start ...");
                Console.WriteLine();

                new SetupHelp().Setup();

                Console.WriteLine("Setup Done !!!");
                File.AppendAllText(SetupHelp.LogPath, "Setup done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                File.AppendAllText(SetupHelp.LogPath, ex.Message);
            }
        }

        static void UnSetup()
        {
            try
            {
                Console.WriteLine("UnSetup Start ...");

                new SetupHelp().UnSetup();

                Console.WriteLine("UnSetup Done !!!");
                File.AppendAllText(SetupHelp.LogPath, "UnSetup done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                File.AppendAllText(SetupHelp.LogPath, ex.Message);
            }
        }
    }
}
