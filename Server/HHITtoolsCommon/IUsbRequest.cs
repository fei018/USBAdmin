using System;

namespace HHITtoolsCommon
{
    public interface IUsbRequest : IUsbInfo
    {
        string RequestUserEmail { get; set; }

        string RequestComputerIdentity { get; set; }

        DateTime RequestTime { get; set; }

        string RequestReason { get; set; }
    }
}
