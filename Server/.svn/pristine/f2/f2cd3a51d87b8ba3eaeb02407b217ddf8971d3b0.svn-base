using System;
using System.IO;
using USBModel;

namespace USBAdminDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("start...");
            Console.WriteLine();
            try
            {
                string sql = null;
#if DEBUG
                sql = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SqlDebug.txt");
#else
                sql = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql.txt");
#endif
                if (!File.Exists(sql))
                {
                    throw new Exception("connectionString file: \"sql.txt\" not exist.");
                }
               
                string conn = File.ReadAllText(sql)?.Trim();
                Console.WriteLine(conn);
                new USBAdminDatabaseHelp(conn).TryCreateDatabase();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("done...");
            Console.ReadLine();
        }
    }
}
