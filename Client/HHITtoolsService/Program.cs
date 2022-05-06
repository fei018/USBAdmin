﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace HHITtoolsService
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ToolsService()
            };
            ServiceBase.Run(ServicesToRun);

            //if (Environment.UserInteractive)
            //{
            //    ToolsServiceHelp.Start_Service();
            //    Console.WriteLine("start...");

            //    Console.ReadLine();
            //    ToolsServiceHelp.Stop_Service();
            //    Console.WriteLine("stop.");
            //}
            //else
            //{
            //    ServiceBase[] ServicesToRun;
            //    ServicesToRun = new ServiceBase[]
            //    {
            //    new ToolsService()
            //    };
            //    ServiceBase.Run(ServicesToRun);
            //}
        }
    }
}
