using System;
using System.Runtime.InteropServices;
using System.Text;

namespace USBNotifyLib.Win32API
{
    internal partial class USetupApi
    {
        #region SetupDi

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, IntPtr enumerator, IntPtr hwndParent, int flags); // 1st form using a ClassGUID

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr SetupDiGetClassDevs(int classGuid, string enumerator, IntPtr hwndParent, int flags); // 2nd form uses an Enumerator

        //[DllImport("Setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //internal static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, uint iEnumerator, int hwndParent, int flags);

        //[DllImport("setupapi.dll")]
        //internal static extern IntPtr SetupDiGetClassDevsEx(IntPtr classGuid,
        //    [MarshalAs(UnmanagedType.LPStr)] String enumerator, IntPtr hwndParent, Int32 flags, IntPtr deviceInfoSet,
        //    [MarshalAs(UnmanagedType.LPStr)] String machineName, IntPtr reserved);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool SetupDiEnumDeviceInterfaces(IntPtr deviceInfoSet, IntPtr deviceInfoData,
            ref Guid interfaceClassGuid, int memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
            uint deviceInterfaceDetailDataSize, ref uint requiredSize, ref SP_DEVINFO_DATA deviceInfoData);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool SetupDiEnumDeviceInfo(IntPtr deviceInfoSet, int memberIndex, ref SP_DEVINFO_DATA deviceInfoData);

        //[DllImport("Setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //internal static extern bool SetupDiEnumDeviceInfo(IntPtr lpInfoSet, UInt32 dwIndex, SP_DEVINFO_DATA devInfoData);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool SetupDiGetDeviceInstanceId(IntPtr deviceInfoSet, ref SP_DEVINFO_DATA deviceInfoData,
            StringBuilder deviceInstanceId, int deviceInstanceIdSize, out int requiredSize);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool SetupDiGetDeviceRegistryProperty(IntPtr deviceInfoSet,
           ref SP_DEVINFO_DATA deviceInfoData, int iProperty, ref int propertyRegDataType, IntPtr propertyBuffer,
           int propertyBufferSize, ref uint requiredSize);

        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern bool SetupDiGetDeviceRegistryProperty(IntPtr lpInfoSet, SP_DEVINFO_DATA deviceInfoData,
            UInt32 property, UInt32 propertyRegDataType, StringBuilder propertyBuffer, UInt32 propertyBufferSize,
            IntPtr requiredSize);

        //[DllImport("setupapi.dll")]
        //internal static extern bool SetupDiCallClassInstaller(int installFunction, IntPtr deviceInfoSet,
        //    ref SP_DEVINFO_DATA deviceInfoData);

        //[DllImport("setupapi.dll")]
        //internal static extern bool SetupDiClassGuidsFromNameA(string classN, ref Guid guids, UInt32 classNameSize,
        //    ref UInt32 reqSize);

        //[DllImport("quickusb.dll", CharSet = CharSet.Ansi)]
        //internal static extern int QuickUsbOpen(out IntPtr handle, string devName);
        #endregion

        #region CfgMgr

        [DllImport("setupapi.dll")]
        public static extern int CM_Get_Parent(out uint pdnDevInst, uint dnDevInst, int ulFlags);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        public static extern int CM_Get_Device_ID(uint dnDevInst, StringBuilder buffer, uint bufferLen, int ulFlags);

        [DllImport("setupapi.dll")]
        public static extern int CM_Get_Device_ID_Size(out uint pulLen, uint dnDevInst, uint ulFlags = 0);

        [DllImport("setupapi.dll")]
        public static extern int CM_Request_Device_Eject(uint dnDevInst, out PNP_VETO_TYPE pVetoType, StringBuilder pszVetoName, int ulNameLength, uint ulFlags);

        [DllImport("setupapi.dll", EntryPoint = "CM_Request_Device_Eject")]
        public static extern int CM_Request_Device_Eject_NoUi(uint dnDevInst, IntPtr pVetoType, StringBuilder pszVetoName, uint ulNameLength, uint ulFlags);
        #endregion

        #region Extention
        public static bool SetupDiGetDeviceInterfaceDetail(IntPtr devInfoSet, ref SP_DEVICE_INTERFACE_DATA devInterfaceData, out string devicePath, out SP_DEVINFO_DATA devInfoData)
        {
            devInfoData = new SP_DEVINFO_DATA();
            devInfoData.cbSize = (uint)Marshal.SizeOf(devInfoData);

            var devInterfaceDataDetail = new SP_DEVICE_INTERFACE_DETAIL_DATA { cbSize = IntPtr.Size == 4 ? 4U + (uint)Marshal.SystemDefaultCharSize : 8U };

            uint requiredsize = 0;
            if (SetupDiGetDeviceInterfaceDetail(devInfoSet, ref devInterfaceData, ref devInterfaceDataDetail,
                                               1024, ref requiredsize, ref devInfoData))
            {
                devicePath = devInterfaceDataDetail.devicePath;
                return true;
            }
            else
            {
                devicePath = null;
                return false;
            }
        }
        #endregion

        #region CreateFile

        #endregion
    }
}
