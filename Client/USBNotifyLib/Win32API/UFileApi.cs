using System;
using System.Runtime.InteropServices;

namespace USBNotifyLib.Win32API
{
    public sealed class UFileApi
    {
        public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        // dwDesiredAccess
        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;

        // dwShareMode
        public const int FILE_SHARE_READ = 0x1;
        public const int FILE_SHARE_WRITE = 0x2;
        public const int FILE_SHARE_DELETE = 0x4;

        // dwCreationDisposition
        public const int OPEN_EXISTING = 0x3;
        public const int CREATE_ALWAYS = 0x2;

        // dwFlagsAndAttributes
        public const int FILE_ATTRIBUTE_READONLY = 0x1;
        public const int FILE_ATTRIBUTE_NORMAL = 0x80;
        public const int FILE_FLAG_OVERLAPPED = 0x40000000;

        public const int ERROR_IO_PENDING = 997;


        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, int dwShareMode,
           IntPtr lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr hObject);

        #region readonly
        public static IntPtr CreateFile_ReadOnly(string devicePath)
        {
            return CreateFile(devicePath, FILE_SHARE_READ, FILE_SHARE_READ, IntPtr.Zero, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
        }
        #endregion
    }
}
