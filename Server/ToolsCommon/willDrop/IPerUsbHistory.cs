﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsCommon
{
    public interface IPerUsbHistory : IUsbBase
    {

        string ComputerIdentity { get; set; }

        DateTime PluginTime { get; set; }

        string PluginTimeString { get; }
    }
}