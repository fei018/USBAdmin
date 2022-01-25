using System;
using System.Runtime.InteropServices;

namespace USBNotifyLib.Win32API
{
    internal class ProcessApi
    {
        #region struct
        [StructLayout(LayoutKind.Sequential)]
        public struct STARTUPINFO
        {
            public Int32 cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public Int32 dwProcessID;
            public Int32 dwThreadID;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public Int32 Length;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TokPriv1Luid
        {
            public int Count;
            public long Luid;
            public int Attr;
        }
        #endregion

        #region Win32 API
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LookupPrivilegeValue(string host, string name, ref long pluid);

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall, ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);

        [DllImport("advapi32.dll", EntryPoint = "ImpersonateLoggedOnUser", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr ImpersonateLoggedOnUser(IntPtr hToken);

        [DllImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true,CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll",EntryPoint = "CreateProcessAsUser", SetLastError = true,CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern bool CreateProcessAsUser(IntPtr hToken, string lpApplicationName, string lpCommandLine,
                                                       ref SECURITY_ATTRIBUTES lpProcessAttributes, ref SECURITY_ATTRIBUTES lpThreadAttributes,
                                                       bool bInheritHandle, Int32 dwCreationFlags, IntPtr lpEnvrionment,
                                                       string lpCurrentDirectory, ref STARTUPINFO lpStartupInfo,
                                                       ref PROCESS_INFORMATION lpProcessInformation);

        [DllImport("advapi32.dll", EntryPoint = "DuplicateTokenEx")]
        public static extern bool DuplicateTokenEx(IntPtr hExistingToken, Int32 dwDesiredAccess,
                                                    ref SECURITY_ATTRIBUTES lpThreadAttributes,
                                                    Int32 ImpersonationLevel, Int32 dwTokenType,
                                                    ref IntPtr phNewToken);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool RevertToSelf();

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern IntPtr WTSGetActiveConsoleSessionId();

        [DllImport("advapi32.dll")]
        public static extern IntPtr SetTokenInformation(IntPtr TokenHandle, IntPtr TokenInformationClass, IntPtr TokenInformation, IntPtr TokenInformationLength);


        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern bool WTSQueryUserToken(uint sessionId, out IntPtr Token);
        #endregion
    }
}
