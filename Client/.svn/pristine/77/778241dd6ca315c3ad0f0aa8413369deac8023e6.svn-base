using System;
using System.Runtime.InteropServices;

namespace USBNotifyLib.Win32API
{
    internal partial class USetupApi
    {
        [Flags]
        public enum DeviceCapabilities
        {
            Unknown = 0x00000000,
            LockSupported = 0x00000001,
            EjectSupported = 0x00000002,
            Removable = 0x00000004,
            DockDevice = 0x00000008,
            UniqueId = 0x00000010,
            SilentInstall = 0x00000020,
            RawDeviceOk = 0x00000040,
            SurpriseRemovalOk = 0x00000080,
            HardwareDisabled = 0x00000100,
            NonDynamic = 0x00000200
        }

        [Flags]
        public enum DICFG
        {
            /// <summary>
            ///     Return only the device that is associated with the system default device interface, if one is set.
            /// </summary>
            DEFAULT = 0x00000001,

            /// <summary>
            ///     Return only devices that are currently present in a system.
            /// </summary>
            PRESENT = 0x00000002,

            /// <summary>
            ///     Return a list of installed devices for all device setup classes or all device interface classes.
            /// </summary>
            ALLCLASSES = 0x00000004,

            /// <summary>
            ///     Return only devices that are a part of the current hardware profile.
            /// </summary>
            PROFILE = 0x00000008,

            /// <summary>
            ///     Return devices that support device interfaces for the specified device interface classes.
            /// </summary>
            DEVICEINTERFACE = 0x00000010
        }

        public enum PNP_VETO_TYPE
        {
            Ok,
            TypeUnknown,
            LegacyDevice,
            PendingClose,
            WindowsApp,
            WindowsService,
            OutstandingOpen,
            Device,
            Driver,
            IllegalDeviceRequest,
            InsufficientPower,
            NonDisableable,
            LegacyDriver
        }

        /// <summary>
        ///     SEE: https://msdn.microsoft.com/en-us/library/windows/hardware/ff542548(v=vs.85).aspx
        /// </summary>
        [Flags]
        public enum SPDRP
        {
            /// <summary>
            ///     Requests a string describing the device, such as "Microsoft PS/2 Port Mouse", typically defined by the
            ///     manufacturer.
            ///     String
            /// </summary>
            DeviceDesc = 0x00000000,

            /// <summary>
            ///     Requests the hardware IDs provided by the device that identify the device.
            ///     String[]
            /// </summary>
            HardwareId = 0x00000001,

            /// <summary>
            ///     Requests the compatible IDs reported by the device.
            ///     string[]
            /// </summary>
            CompatibleIds = 0x00000002,
            Unused0 = 0x00000003,

            /// <summary>
            ///     Service device property represents the name of the service that is installed for a device instance.
            ///     string
            /// </summary>
            Service = 0x00000004,
            Unused1 = 0x00000005,
            Unused2 = 0x00000006,

            /// <summary>
            ///     Requests the name of the device's setup class, in text format.
            ///     string
            /// </summary>
            Class = 0x00000007,

            /// <summary>
            ///     Requests the GUID for the device's setup class.
            ///     GUID
            /// </summary>
            ClassGuid = 0x00000008,

            /// <summary>
            ///     Requests the name of the driver-specific registry key.
            ///     string
            /// </summary>
            Driver = 0x00000009,

            /// <summary>
            ///     ConfigFlags device property represents the configuration flags that are set for a device instance.
            ///     Int32
            /// </summary>
            ConfigFlags = 0x0000000A,

            /// <summary>
            ///     Requests a string identifying the manufacturer of the device.
            ///     string
            /// </summary>
            Mfg = 0x0000000B,

            /// <summary>
            ///     Requests a string that can be used to distinguish between two similar devices, typically defined by the class
            ///     installer.
            ///     string
            /// </summary>
            FriendlyName = 0x0000000C,

            /// <summary>
            ///     Requests information about the device's location on the bus; the interpretation of this information is
            ///     bus-specific.
            ///     string
            /// </summary>
            LocationInformation = 0x0000000D,

            /// <summary>
            ///     Requests the name of the PDO for this device.
            ///     Binary
            /// </summary>
            PhysicalDeviceObjectName = 0x0000000E,

            /// <summary>
            ///     Capabilities device property represents the capabilities of a device instance.
            ///     Int32
            /// </summary>
            Capabilities = 0x0000000F,

            /// <summary>
            ///     Requests a number associated with the device that can be displayed in the user interface.
            ///     Int32
            /// </summary>
            UiNumber = 0x00000010,

            /// <summary>
            ///     UpperFilters device property represents a list of the service names of the upper-level filter drivers that are
            ///     installed for a device instance.
            ///     string[]
            /// </summary>
            UpperFilters = 0x00000011,

            /// <summary>
            ///     LowerFilters device property represents a list of the service names of the lower-level filter drivers that are
            ///     installed for a device instance.
            ///     string[]
            /// </summary>
            LowerFilters = 0x00000012,

            /// <summary>
            ///     Requests the GUID for the bus that the device is connected to.
            ///     GUID
            /// </summary>
            BusTypeGuid = 0x00000013,

            /// <summary>
            ///     Requests the bus type, such as PCIBus or PCMCIABus.
            ///     Int32
            /// </summary>
            LegacyBusType = 0x00000014,

            /// <summary>
            ///     Requests the legacy bus number of the bus the device is connected to.
            ///     Int32
            /// </summary>
            BusNumber = 0x00000015,

            /// <summary>
            ///     Requests the name of the enumerator for the device, such as "USB".
            ///     string
            /// </summary>
            EnumeratorName = 0x00000016,

            /// <summary>
            ///     Security device property represents a security descriptor structure for a device instance.
            ///     SECURITY_DESCRIPTOR
            /// </summary>
            Security = 0x00000017,

            /// <summary>
            ///     SecuritySDS device property represents a security descriptor string for a device instance.
            /// </summary>
            Security_SDS = 0x00000018,

            /// <summary>
            ///     DevType device property represents the device type of a device instance.
            ///     Int32
            /// </summary>
            DevType = 0x00000019,

            /// <summary>
            ///     Exclusive device property represents a Boolean value that determines whether a device instance can be opened for
            ///     exclusive use.
            ///     bool
            /// </summary>
            Exclusive = 0x0000001A,

            /// <summary>
            ///     Characteristics device property represents the characteristics of a device instance.
            ///     INt32
            /// </summary>
            Characteristics = 0x0000001B,

            /// <summary>
            ///     Requests the address of the device on the bus.
            ///     Int32
            /// </summary>
            Address = 0x0000001C,

            /// <summary>
            ///     UINumberDescFormat device property represents a printf-compatible format string that you should use to display
            ///     the value of the DEVPKEY_DEVICE_UINumber device property for a device instance.
            ///     string
            /// </summary>
            UI_Number_Desc_Format = 0x0000001D,

            /// <summary>
            ///     PowerData device property represents power information about a device instance.
            ///     Binary
            /// </summary>
            Device_Power_Data = 0x0000001E,

            /// <summary>
            ///     (Windows XP and later.) Requests the device's current removal policy. The operating system uses this value as a
            ///     hint to determine how the device is normally removed.
            ///     Int32
            /// </summary>
            RemovalPolicy = 0x0000001F,

            /// <summary>
            ///     RemovalPolicyDefault device property represents the default removal policy for a device instance.
            ///     Int32
            /// </summary>
            Removal_Policy_Defualt = 0x00000020,

            /// <summary>
            ///     RemovalPolicyOverride device property represents the removal policy override for a device instance.
            ///     Int32
            /// </summary>
            Removal_Policy_Override = 0x00000021,

            /// <summary>
            ///     Windows XP and later.) Requests the device's installation state.
            ///     Int32
            /// </summary>
            InstallState = 0x00000022,

            /// <summary>
            ///     LocationPaths device property represents the location of a device instance in the device tree.
            ///     string[]
            /// </summary>
            LocationPaths = 0x00000023
        }

        public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        public const int MAX_DEV_LEN = 200;
        public const int DEVICE_NOTIFY_WINDOW_HANDLE = 0x00000000;
        public const int DEVICE_NOTIFY_SERVICE_HANDLE = 0x00000001;
        public const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 0x00000004;
        public const int DBT_DEVTYP_DEVICEINTERFACE = 0x00000005;
        public const int DBT_DEVNODES_CHANGED = 0x0007;
        public const int WM_DEVICECHANGE = 0x0219;
        public const int DIF_PROPERTYCHANGE = 0x00000012;
        public const int DICS_FLAG_GLOBAL = 0x00000001;
        public const int DICS_FLAG_CONFIGSPECIFIC = 0x00000002;
        public const int DICS_ENABLE = 0x00000001;
        public const int DICS_DISABLE = 0x00000002;

        public const int ERROR_INVALID_DATA = 13;
        public const int ERROR_NO_MORE_ITEMS = 259;
        public const int ERROR_INSUFFICIENT_BUFFER = 122;
        public const int GENERIC_READ = unchecked((int)0x80000000);
        public const int FILE_SHARE_READ = 0x00000001;
        public const int FILE_SHARE_WRITE = 0x00000002;
        public const int OPEN_EXISTING = 3;
        public const int IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS = 0x00560000;


        public class GUID_DEVINTERFACE
        {
            public static Guid BUS1394_CLASS_GUID = new Guid("6BDD1FC1-810F-11d0-BEC7-08002BE2092F");
            public static Guid GUID_61883_CLASS = new Guid("7EBEFBC0-3200-11d2-B4C2-00A0C9697D07");
            public static Guid GUID_DEVICE_APPLICATIONLAUNCH_BUTTON = new Guid("629758EE-986E-4D9E-8E47-DE27F8AB054D");
            public static Guid GUID_DEVICE_BATTERY = new Guid("72631E54-78A4-11D0-BCF7-00AA00B7B32A");
            public static Guid GUID_DEVICE_LID = new Guid("4AFA3D52-74A7-11d0-be5e-00A0C9062857");
            public static Guid GUID_DEVICE_MEMORY = new Guid("3FD0F03D-92E0-45FB-B75C-5ED8FFB01021");
            public static Guid GUID_DEVICE_MESSAGE_INDICATOR = new Guid("CD48A365-FA94-4CE2-A232-A1B764E5D8B4");
            public static Guid GUID_DEVICE_PROCESSOR = new Guid("97FADB10-4E33-40AE-359C-8BEF029DBDD0");
            public static Guid GUID_DEVICE_SYS_BUTTON = new Guid("4AFA3D53-74A7-11d0-be5e-00A0C9062857");
            public static Guid GUID_DEVICE_THERMAL_ZONE = new Guid("4AFA3D51-74A7-11d0-be5e-00A0C9062857");
            public static Guid GUID_BTHPORT_DEVICE_INTERFACE = new Guid("0850302A-B344-4fda-9BE9-90576B8D46F0");
            public static Guid GUID_DEVINTERFACE_BRIGHTNESS = new Guid("FDE5BBA4-B3F9-46FB-BDAA-0728CE3100B4");
            public static Guid GUID_DEVINTERFACE_DISPLAY_ADAPTER = new Guid("5B45201D-F2F2-4F3B-85BB-30FF1F953599");
            public static Guid GUID_DEVINTERFACE_I2C = new Guid("2564AA4F-DDDB-4495-B497-6AD4A84163D7");
            public static Guid GUID_DEVINTERFACE_IMAGE = new Guid("6BDD1FC6-810F-11D0-BEC7-08002BE2092F");
            public static Guid GUID_DEVINTERFACE_MONITOR = new Guid("E6F07B5F-EE97-4a90-B076-33F57BF4EAA7");
            public static Guid GUID_DEVINTERFACE_OPM = new Guid("BF4672DE-6B4E-4BE4-A325-68A91EA49C09");
            public static Guid GUID_DEVINTERFACE_VIDEO_OUTPUT_ARRIVAL = new Guid("1AD9E4F0-F88D-4360-BAB9-4C2D55E564CD");
            public static Guid GUID_DISPLAY_DEVICE_ARRIVAL = new Guid("1CA05180-A699-450A-9A0C-DE4FBE3DDD89");
            public static Guid GUID_DEVINTERFACE_HID = new Guid("4D1E55B2-F16F-11CF-88CB-001111000030");
            public static Guid GUID_DEVINTERFACE_KEYBOARD = new Guid("884b96c3-56ef-11d1-bc8c-00a0c91405dd");
            public static Guid GUID_DEVINTERFACE_MOUSE = new Guid("378DE44C-56EF-11D1-BC8C-00A0C91405DD");
            public static Guid GUID_DEVINTERFACE_MODEM = new Guid("2C7089AA-2E0E-11D1-B114-00C04FC2AAE4");
            public static Guid GUID_DEVINTERFACE_NET = new Guid("CAC88484-7515-4C03-82E6-71A87ABAC361");
            public static Guid GUID_DEVINTERFACE_SENSOR = new Guid(0XBA1BB692, 0X9B7A, 0X4833, 0X9A, 0X1E, 0X52, 0X5E, 0XD1, 0X34, 0XE7, 0XE2);
            public static Guid GUID_DEVINTERFACE_COMPORT = new Guid("86E0D1E0-8089-11D0-9CE4-08003E301F73");
            public static Guid GUID_DEVINTERFACE_PARALLEL = new Guid("97F76EF0-F883-11D0-AF1F-0000F800845C");
            public static Guid GUID_DEVINTERFACE_PARCLASS = new Guid("811FC6A5-F728-11D0-A537-0000F8753ED1");
            public static Guid GUID_DEVINTERFACE_SERENUM_BUS_ENUMERATOR = new Guid("4D36E978-E325-11CE-BFC1-08002BE10318");
            public static Guid GUID_DEVINTERFACE_CDCHANGER = new Guid("53F56312-B6BF-11D0-94F2-00A0C91EFB8B");
            public static Guid GUID_DEVINTERFACE_CDROM = new Guid("53F56308-B6BF-11D0-94F2-00A0C91EFB8B");
            public static Guid GUID_DEVINTERFACE_DISK = new Guid("53F56307-B6BF-11D0-94F2-00A0C91EFB8B");
            public static Guid GUID_DEVINTERFACE_FLOPPY = new Guid("53F56311-B6BF-11D0-94F2-00A0C91EFB8B");
            public static Guid GUID_DEVINTERFACE_MEDIUMCHANGER = new Guid("53F56310-B6BF-11D0-94F2-00A0C91EFB8B");
            public static Guid GUID_DEVINTERFACE_PARTITION = new Guid("53F5630A-B6BF-11D0-94F2-00A0C91EFB8B");
            public static Guid GUID_DEVINTERFACE_STORAGEPORT = new Guid("2ACCFE60-C130-11D2-B082-00A0C91EFB8B");
            public static Guid GUID_DEVINTERFACE_TAPE = new Guid("53F5630B-B6BF-11D0-94F2-00A0C91EFB8B");
            public static Guid GUID_DEVINTERFACE_VOLUME = new Guid("53F5630D-B6BF-11D0-94F2-00A0C91EFB8B");
            public static Guid GUID_DEVINTERFACE_WRITEONCEDISK = new Guid("53F5630C-B6BF-11D0-94F2-00A0C91EFB8B");
            public static Guid GUID_IO_VOLUME_DEVICE_INTERFACE = new Guid("53F5630D-B6BF-11D0-94F2-00A0C91EFB8B");
            public static Guid MOUNTDEV_MOUNTED_DEVICE_GUID = new Guid("53F5630D-B6BF-11D0-94F2-00A0C91EFB8B");
            public static Guid GUID_AVC_CLASS = new Guid("095780C3-48A1-4570-BD95-46707F78C2DC");
            public static Guid GUID_VIRTUAL_AVC_CLASS = new Guid("616EF4D0-23CE-446D-A568-C31EB01913D0");
            public static Guid KSCATEGORY_ACOUSTIC_ECHO_CANCEL = new Guid("BF963D80-C559-11D0-8A2B-00A0C9255AC1");
            public static Guid KSCATEGORY_AUDIO = new Guid("6994AD04-93EF-11D0-A3CC-00A0C9223196");
            public static Guid KSCATEGORY_AUDIO_DEVICE = new Guid("FBF6F530-07B9-11D2-A71E-0000F8004788");
            public static Guid KSCATEGORY_AUDIO_GFX = new Guid("9BAF9572-340C-11D3-ABDC-00A0C90AB16F");
            public static Guid KSCATEGORY_AUDIO_SPLITTER = new Guid("9EA331FA-B91B-45F8-9285-BD2BC77AFCDE");
            public static Guid KSCATEGORY_BDA_IP_SINK = new Guid("71985F4A-1CA1-11d3-9CC8-00C04F7971E0");
            public static Guid KSCATEGORY_BDA_NETWORK_EPG = new Guid("71985F49-1CA1-11d3-9CC8-00C04F7971E0");
            public static Guid KSCATEGORY_BDA_NETWORK_PROVIDER = new Guid("71985F4B-1CA1-11d3-9CC8-00C04F7971E0");
            public static Guid KSCATEGORY_BDA_NETWORK_TUNER = new Guid("71985F48-1CA1-11d3-9CC8-00C04F7971E0");
            public static Guid KSCATEGORY_BDA_RECEIVER_COMPONENT = new Guid("FD0A5AF4-B41D-11d2-9C95-00C04F7971E0");
            public static Guid KSCATEGORY_BDA_TRANSPORT_INFORMATION = new Guid("A2E3074F-6C3D-11d3-B653-00C04F79498E");
            public static Guid KSCATEGORY_BRIDGE = new Guid("085AFF00-62CE-11CF-A5D6-28DB04C10000");
            public static Guid KSCATEGORY_CAPTURE = new Guid("65E8773D-8F56-11D0-A3B9-00A0C9223196");
            public static Guid KSCATEGORY_CLOCK = new Guid("53172480-4791-11D0-A5D6-28DB04C10000");
            public static Guid KSCATEGORY_COMMUNICATIONSTRANSFORM = new Guid("CF1DDA2C-9743-11D0-A3EE-00A0C9223196");
            public static Guid KSCATEGORY_CROSSBAR = new Guid("A799A801-A46D-11D0-A18C-00A02401DCD4");
            public static Guid KSCATEGORY_DATACOMPRESSOR = new Guid("1E84C900-7E70-11D0-A5D6-28DB04C10000");
            public static Guid KSCATEGORY_DATADECOMPRESSOR = new Guid("2721AE20-7E70-11D0-A5D6-28DB04C10000");
            public static Guid KSCATEGORY_DATATRANSFORM = new Guid("2EB07EA0-7E70-11D0-A5D6-28DB04C10000");
            public static Guid KSCATEGORY_DRM_DESCRAMBLE = new Guid("FFBB6E3F-CCFE-4D84-90D9-421418B03A8E");
            public static Guid KSCATEGORY_ENCODER = new Guid("19689BF6-C384-48fd-AD51-90E58C79F70B");
            public static Guid KSCATEGORY_ESCALANTE_PLATFORM_DRIVER = new Guid("74F3AEA8-9768-11D1-8E07-00A0C95EC22E");
            public static Guid KSCATEGORY_FILESYSTEM = new Guid("760FED5E-9357-11D0-A3CC-00A0C9223196");
            public static Guid KSCATEGORY_INTERFACETRANSFORM = new Guid("CF1DDA2D-9743-11D0-A3EE-00A0C9223196");
            public static Guid KSCATEGORY_MEDIUMTRANSFORM = new Guid("CF1DDA2E-9743-11D0-A3EE-00A0C9223196");
            public static Guid KSCATEGORY_MICROPHONE_ARRAY_PROCESSOR = new Guid("830A44F2-A32D-476B-BE97-42845673B35A");
            public static Guid KSCATEGORY_MIXER = new Guid("AD809C00-7B88-11D0-A5D6-28DB04C10000");
            public static Guid KSCATEGORY_MULTIPLEXER = new Guid("7A5DE1D3-01A1-452c-B481-4FA2B96271E8");
            public static Guid KSCATEGORY_NETWORK = new Guid("67C9CC3C-69C4-11D2-8759-00A0C9223196");
            public static Guid KSCATEGORY_PREFERRED_MIDIOUT_DEVICE = new Guid("D6C50674-72C1-11D2-9755-0000F8004788");
            public static Guid KSCATEGORY_PREFERRED_WAVEIN_DEVICE = new Guid("D6C50671-72C1-11D2-9755-0000F8004788");
            public static Guid KSCATEGORY_PREFERRED_WAVEOUT_DEVICE = new Guid("D6C5066E-72C1-11D2-9755-0000F8004788");
            public static Guid KSCATEGORY_PROXY = new Guid("97EBAACA-95BD-11D0-A3EA-00A0C9223196");
            public static Guid KSCATEGORY_QUALITY = new Guid("97EBAACB-95BD-11D0-A3EA-00A0C9223196");
            public static Guid KSCATEGORY_REALTIME = new Guid("EB115FFC-10C8-4964-831D-6DCB02E6F23F");
            public static Guid KSCATEGORY_RENDER = new Guid("65E8773E-8F56-11D0-A3B9-00A0C9223196");
            public static Guid KSCATEGORY_SPLITTER = new Guid("0A4252A0-7E70-11D0-A5D6-28DB04C10000");
            public static Guid KSCATEGORY_SYNTHESIZER = new Guid("DFF220F3-F70F-11D0-B917-00A0C9223196");
            public static Guid KSCATEGORY_SYSAUDIO = new Guid("A7C7A5B1-5AF3-11D1-9CED-00A024BF0407");
            public static Guid KSCATEGORY_TEXT = new Guid("6994AD06-93EF-11D0-A3CC-00A0C9223196");
            public static Guid KSCATEGORY_TOPOLOGY = new Guid("DDA54A40-1E4C-11D1-A050-405705C10000");
            public static Guid KSCATEGORY_TVAUDIO = new Guid("A799A802-A46D-11D0-A18C-00A02401DCD4");
            public static Guid KSCATEGORY_TVTUNER = new Guid("A799A800-A46D-11D0-A18C-00A02401DCD4");
            public static Guid KSCATEGORY_VBICODEC = new Guid("07DAD660-22F1-11D1-A9F4-00C04FBBDE8F");
            public static Guid KSCATEGORY_VIDEO = new Guid("6994AD05-93EF-11D0-A3CC-00A0C9223196");
            public static Guid KSCATEGORY_VIRTUAL = new Guid("3503EAC4-1F26-11D1-8AB0-00A0C9223196");
            public static Guid KSCATEGORY_VPMUX = new Guid("A799A803-A46D-11D0-A18C-00A02401DCD4");
            public static Guid KSCATEGORY_WDMAUD = new Guid("3E227E76-690D-11D2-8161-0000F8775BF1");
            public static Guid KSMFT_CATEGORY_AUDIO_DECODER = new Guid("9ea73fb4-ef7a-4559-8d5d-719d8f0426c7");
            public static Guid KSMFT_CATEGORY_AUDIO_EFFECT = new Guid("11064c48-3648-4ed0-932e-05ce8ac811b7");
            public static Guid KSMFT_CATEGORY_AUDIO_ENCODER = new Guid("91c64bd0-f91e-4d8c-9276-db248279d975");
            public static Guid KSMFT_CATEGORY_DEMULTIPLEXER = new Guid("a8700a7a-939b-44c5-99d7-76226b23b3f1");
            public static Guid KSMFT_CATEGORY_MULTIPLEXER = new Guid("059c561e-05ae-4b61-b69d-55b61ee54a7b");
            public static Guid KSMFT_CATEGORY_OTHER = new Guid("90175d57-b7ea-4901-aeb3-933a8747756f");
            public static Guid KSMFT_CATEGORY_VIDEO_DECODER = new Guid("d6c02d4b-6833-45b4-971a-05a4b04bab91");
            public static Guid KSMFT_CATEGORY_VIDEO_EFFECT = new Guid("12e17c21-532c-4a6e-8a1c-40825a736397");
            public static Guid KSMFT_CATEGORY_VIDEO_ENCODER = new Guid("f79eac7d-e545-4387-bdee-d647d7bde42a");
            public static Guid KSMFT_CATEGORY_VIDEO_PROCESSOR = new Guid("302ea3fc-aa5f-47f9-9f7a-c2188bb16302");
            public static Guid GUID_DEVINTERFACE_USB_DEVICE = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED");
            public static Guid GUID_DEVINTERFACE_USB_HOST_CONTROLLER = new Guid("3ABF6F2D-71C4-462A-8A92-1E6861E6AF27");
            public static Guid GUID_DEVINTERFACE_USB_HUB = new Guid("F18A0E88-C30C-11D0-8815-00A0C906BED8");
            public static Guid GUID_DEVINTERFACE_WPD = new Guid("6AC27878-A6FA-4155-BA85-F98F491D4F33");
            public static Guid GUID_DEVINTERFACE_WPD_PRIVATE = new Guid("BA0C718F-4DED-49B7-BDD3-FABE28661211");
            public static Guid GUID_DEVINTERFACE_SIDESHOW = new Guid("152E5811-FEB9-4B00-90F4-D32947AE1681");
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DISK_EXTENT
        {
            internal uint DiskNumber;
            internal ulong StartingOffset;
            internal ulong ExtentLength;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class VOLUME_DISK_EXTENTS
        {
            public uint NumberOfDiskExtents;

            public uint ZBUf;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class DEV_BROADCAST_DEVICEINTERFACE
        {
            public int dbcc_devicetype;
            public int dbcc_reserved;
            public int dbcc_size;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVINFO_DATA
        {
            public uint cbSize;
            public Guid classGuid;
            public uint devInst;
            public UIntPtr reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class SP_DEVINSTALL_PARAMS
        {
            public int cbSize;
            public IntPtr ClassInstallReserved;

            [MarshalAs(UnmanagedType.LPTStr)] public string DriverPath;

            public IntPtr FileQueue;
            public int Flags;
            public int FlagsEx;
            public IntPtr hwndParent;
            public IntPtr InstallMsgHandler;
            public IntPtr InstallMsgHandlerContext;
            public int Reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class SP_PROPCHANGE_PARAMS
        {
            public SP_CLASSINSTALL_HEADER ClassInstallHeader = new SP_CLASSINSTALL_HEADER();
            public int HwProfile;
            public int Scope;
            public int StateChange;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class SP_CLASSINSTALL_HEADER
        {
            public int cbSize;
            public int InstallFunction;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public uint cbSize;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            public string devicePath;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVICE_INTERFACE_DATA
        {
            public uint cbSize;
            public uint flags;
            public Guid interfaceClassGuid;
            public UIntPtr reserved;
        }
    }
}
