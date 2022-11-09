using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SharpCmd.Lib.Delegates
{
    #region Structs

    [StructLayout(LayoutKind.Sequential)]
    public struct WTS_SESSION_INFO
    {
        public Int32 SessionID;

        [MarshalAs(UnmanagedType.LPStr, SizeConst = 33)]
        public string pWinStationName;

        public WTS_CONNECTSTATE_CLASS State;
    }

    /// <summary>
    /// @Credits to https://github.com/jinhuca/Crystal.PInvoke
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WTS_SESSION_INFO_1A
    {
        [MarshalAs(UnmanagedType.I4)]
        public int ExecEnvId;
        public WTS_CONNECTSTATE_CLASS State;
        [MarshalAs(UnmanagedType.I4)]
        public int SessionId;
        [MarshalAs(UnmanagedType.LPStr)]
        public string pSessionName;
        [MarshalAs(UnmanagedType.LPStr)]
        public string pHostName;
        [MarshalAs(UnmanagedType.LPStr)]
        public string pUserName;
        [MarshalAs(UnmanagedType.LPStr)]
        public string pDomainName;
        [MarshalAs(UnmanagedType.LPStr)]
        public string pFarmName;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SID_IDENTIFIER_AUTHORITY
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] Value;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SID
    {
        [MarshalAs(UnmanagedType.I1)]
        public byte Revision;
        [MarshalAs(UnmanagedType.I1)]
        public byte SubAuthorityCount;
        public SID_IDENTIFIER_AUTHORITY IdentifierAuthority;
        public IntPtr SubAuthority;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WTS_PROCESS_INFOA
    {
        public uint SessionId;
        public uint ProcessId;
        public string pProcessName;
        public SID pUserSid;
    }

    /// <summary>
    /// mininum win7 and 2008
    /// @Credits to https://github.com/dahall/Vanara/blob/d8c787f1586d98e7bf2f3d23c862173489e20909/PInvoke/WTSApi32/WTSApi32.cs
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WTS_PROCESS_INFO_EXA
    {
        [MarshalAs(UnmanagedType.I4)]
        public int SessionId;
        [MarshalAs(UnmanagedType.I4)]
        public int ProcessId;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pProcessName;
        /// <summary>
        /// A pointer to the user security identifiers(SID) in the primary access token of the process
        /// </summary>
        public SID pUserSid;
        [MarshalAs(UnmanagedType.I4)]
        public int NumberOfThreads;
        [MarshalAs(UnmanagedType.I4)]
        public int HandleCount;
        [MarshalAs(UnmanagedType.I4)]
        public int PagefileUsage;
        [MarshalAs(UnmanagedType.I4)]
        public int PeakPagefileUsage;
        [MarshalAs(UnmanagedType.I4)]
        public int WorkingSetSize;
        [MarshalAs(UnmanagedType.I8)]
        public long UserTime;
        [MarshalAs(UnmanagedType.I8)]
        public long KernelTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WTS_CLIENT_ADDRESS
    {
        public uint AddressFamily;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] Address;
    }

    /// <summary>
    /// Contains information about the display of a Remote Desktop Connection (RDC) client
    /// used by WTSQuerySessionInformation
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WTS_CLIENT_DISPLAY
    {
        /// <summary>
        /// Horizontal dimension, in pixels, of the client's display
        /// </summary>
        public int HorizontalResolution;
        /// <summary>
        /// Vertical dimension, in pixels, of the client's display
        /// </summary>
        public int VerticalResolution;
        /// <summary>
        /// Color depth of the client's display. This member can be one of the following values
        /// </summary>
        public int ColorDepth;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WTSUSERCONFIGA
    {
        public int Source;
        public int InheritInitialProgram;
        public int AllowLogonTerminalServer;
        public int TimeoutSettingsConnections;
        public int TimeoutSettingsDisconnections;
        public int TimeoutSettingsIdle;
        public int DeviceClientDrives;
        public int DeviceClientPrinters;
        public int ClientDefaultPrinter;
        public int BrokenTimeoutSettings;
        public int ReconnectSettings;
        public int ShadowingSettings;
        public int TerminalServerRemoteHomeDir;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string InitialProgram;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string WorkDirectory;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string TerminalServerProfilePath;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string TerminalServerHomeDir;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string TerminalServerHomeDirDrive;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WTS_SERVER_INFOA
    {
        /// <summary>
        /// Name of the server
        /// </summary>
        public string pServerName;
    }


    #endregion

    [StructLayout(LayoutKind.Sequential)]
    public struct TS_UNICODE_STRING
    {
        public ushort Length;
        public ushort MaximumLength;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string Buffer;
    }
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct LARGE_INTEGER
    {
        [FieldOffset(0)] public Int64 QuadPart;
        [FieldOffset(0)] public UInt32 LowPart;
        [FieldOffset(4)] public Int32 HighPart;
    }

    /// <summary>
    /// xp
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TS_SYS_PROCESS_INFORMATION
    {
        public uint NextEntryOffset;
        public uint NumberOfThreads;
        public LARGE_INTEGER SpareLi1;
        public LARGE_INTEGER SpareLi2;
        public LARGE_INTEGER SpareLi3;
        public LARGE_INTEGER CreateTime;
        public LARGE_INTEGER UserTime;
        public LARGE_INTEGER KernelTime;
        public TS_UNICODE_STRING ImageName;
        public int BasePriority;                     // KPRIORITY in ntexapi.h
        public int UniqueProcessId;                 // HANDLE in ntexapi.h
        public int InheritedFromUniqueProcessId;    // HANDLE in ntexapi.h
        public uint HandleCount;
        public uint SessionId;
        public uint SpareUl3;
        public ulong PeakVirtualSize;
        public ulong VirtualSize;
        public uint PageFaultCount;
        public uint PeakWorkingSetSize;
        public uint WorkingSetSize;
        public ulong QuotaPeakPagedPoolUsage;
        public ulong QuotaPagedPoolUsage;
        public ulong QuotaPeakNonPagedPoolUsage;
        public ulong QuotaNonPagedPoolUsage;
        public ulong PagefileUsage;
        public ulong PeakPagefileUsage;
        public ulong PrivatePageCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TS_ALL_PROCESSES_INFO_NT6
    {
        public IntPtr pTsProcessInfo;
        public int SizeOfSid;
        public IntPtr pSid;
    }

}

namespace SharpCmd.Lib.Delegates
{
    #region Enum

    public enum WTS_TYPE_CLASS
    {
        WTSTypeProcessInfoLevel0,
        WTSTypeProcessInfoLevel1,
        WTSTypeSessionInfoLevel1
    }

    public enum WTS_CONNECTSTATE_CLASS
    {
        WTSActive,
        WTSConnected,
        WTSConnectQuery,
        WTSShadow,
        WTSDisconnected,
        WTSIdle,
        WTSListen,
        WTSReset,
        WTSDown,
        WTSInit
    }

    public enum WTS_INFO_CLASS
    {
        WTSInitialProgram,
        WTSApplicationName,
        WTSWorkingDirectory,
        WTSOEMId,
        WTSSessionId,
        WTSUserName,
        WTSWinStationName,
        WTSDomainName,
        WTSConnectState,
        WTSClientBuildNumber,
        WTSClientName,
        WTSClientDirectory,
        WTSClientProductId,
        WTSClientHardwareId,
        WTSClientAddress,
        WTSClientDisplay,
        WTSClientProtocolType,
        WTSIdleTime,
        WTSLogonTime,
        WTSIncomingBytes,
        WTSOutgoingBytes,
        WTSIncomingFrames,
        WTSOutgoingFrames,
        WTSClientInfo,
        WTSSessionInfo
    }
    /// <summary>
    /// Contains values that indicate the type of user configuration information to set or retrieve in a call to
    /// the WTSQueryUserConfig and WTSSetUserConfig functions
    /// </summary>
    public enum WTS_CONFIG_CLASS
    {
        /// <summary>
        /// A null-terminated string that contains the path of the initial program that Remote Desktop Services runs when the user logs on.
        /// If the WTSUserConfigfInheritInitialProgram value is 1, the initial program can be
        /// any program specified by the client
        /// </summary>
        WTSUserConfigInitialProgram,
        /// <summary>
        /// A null-terminated string that contains the path of the working directory for the initial program
        /// </summary>
        WTSUserConfigWorkingDirectory,
        /// <summary>
        /// A value that indicates whether the client can specify the initial program
        /// </summary>
        WTSUserConfigfInheritInitialProgram,
        WTSUserConfigfAllowLogonTerminalServer,
        WTSUserConfigTimeoutSettingsConnections,
        WTSUserConfigTimeoutSettingsDisconnections,
        WTSUserConfigTimeoutSettingsIdle,
        WTSUserConfigfDeviceClientDrives,
        WTSUserConfigfDeviceClientPrinters,
        WTSUserConfigfDeviceClientDefaultPrinter,
        WTSUserConfigBrokenTimeoutSettings,
        WTSUserConfigReconnectSettings,
        WTSUserConfigModemCallbackSettings,
        WTSUserConfigModemCallbackPhoneNumber,
        WTSUserConfigShadowingSettings,
        WTSUserConfigTerminalServerProfilePath,
        WTSUserConfigTerminalServerHomeDir,
        WTSUserConfigTerminalServerHomeDirDrive,
        WTSUserConfigfTerminalServerRemoteHomeDir,
        WTSUserConfigUser
    }
    /// <summary>
    /// used by WTSQueryUserConfig
    /// </summary>
    public enum WTS_CONFIG_SOURCE
    {
        /// <summary>
        /// The configuration information came from the Security Accounts Manager (SAM) database
        /// </summary>
        WTSUserConfigSourceSAM
    }

    #endregion
}


namespace SharpCmd.Lib.Delegates
{
    /*
    Credits to 
    https://www.pinvoke.net/default.aspx/wtsapi32.wtsenumeratesessions
    https://social.msdn.microsoft.com/Forums/en-US/3d10c8c5-61d8-4445-bc71-03b0d59a9459/how-can-i-find-the-active-user-owner-of-current-desktop-window-?forum=vcgeneral

    wtsapi   Remote Desktop Services
    https://learn.microsoft.com/en-us/windows/win32/api/wtsapi32/

    WTS* api是对  WinStation* API的封装
     */




    /// <summary>
    /// WTSEnumerateSessions -> WinStationEnumerateA
    /// </summary>
    /// <param name="hServer"></param>
    /// <param name="Reserved"></param>
    /// <param name="Version"></param>
    /// <param name="ppSessionInfo"></param>
    /// <param name="pCount"></param>
    /// <returns></returns>
    public delegate int WTSEnumerateSessions(IntPtr hServer,int Reserved,int Version,ref IntPtr ppSessionInfo,ref int pCount);
    /// <summary>
    /// Reference from https://learn.microsoft.com/en-us/windows/win32/api/wtsapi32/nf-wtsapi32-wtsenumeratesessionsexa
    /// gain more details on username,domainname,sessionname
    /// 
    /// https://stackoverflow.com/questions/63655482/trying-to-enumerate-terminal-server-sessions-produces-accessviolationexception
    /// </summary>
    /// <param name="hServer"></param>
    /// <param name="Reserved"></param>
    /// <param name="Version"></param>
    /// <param name="ppSessionInfo"></param>
    /// <param name="pCount"></param>
    /// <returns></returns>
    public delegate int WTSEnumerateSessionsExA(IntPtr hServer,int Reserved,int Version, ref IntPtr ppSessionInfo, ref int pCount);

    /// <summary>
    /// Returns a list of all Remote Desktop Session Host (RD Session Host) servers within the specified domain
    /// </summary>
    /// <returns></returns>
    public delegate bool WTSEnumerateServersA([MarshalAs(UnmanagedType.LPTStr)] string pDomainName,uint Reserved,uint Version,ref IntPtr ppServerInfo,ref int pCount);

    public delegate IntPtr WTSOpenServer([MarshalAs(UnmanagedType.LPStr)] String pServerName);

    public delegate IntPtr WTSOpenServerExA([MarshalAs(UnmanagedType.LPStr)] String pServerName);

    public delegate void WTSCloseServer(IntPtr hServer);

    public delegate void WTSFreeMemory(IntPtr pMemory);
    /// <summary>
    /// Free WTS_PROCESS_INFO or  WTS_SESSION_INFO_1
    /// </summary>
    public delegate bool WTSFreeMemoryExA(WTS_TYPE_CLASS typeClass,IntPtr pMemory,uint NumberOfEntries);

    public delegate bool WTSConnectSessionA(uint LogonId,uint TargetLogonId,string pPassword,bool wait);

    public delegate bool WTSQuerySessionInformation(IntPtr hServer, int sessionId, WTS_INFO_CLASS wtsInfoClass, out IntPtr ppBuffer, out uint pBytesReturned);

    public delegate bool WTSGetChildSessionId(out uint pSessionID);


    public class wtsapi32
    {
        public static int WTSEnumerateSessions(IntPtr hServer, int Reserved, int Version, ref IntPtr ppSessionInfo, ref int pCount)
        {
            WTSEnumerateSessions anonymous = MiniDInvoke.GetFunctionPointer<WTSEnumerateSessions>("wtsapi32.dll", "WTSEnumerateSessionsA");
            return anonymous(hServer, Reserved,Version,ref ppSessionInfo,ref pCount);
        }

        public static int WTSEnumerateSessions(IntPtr hServer,ref IntPtr ppSessionInfo, ref int pCount)
        {
            return WTSEnumerateSessions(hServer,1,0,ref ppSessionInfo,ref pCount);
        }


        public static bool WTSEnumerateServersA([MarshalAs(UnmanagedType.LPTStr)] string pDomainName, uint Reserved, uint Version, ref IntPtr ppServerInfo, ref int pCount)
        {
            WTSEnumerateServersA anonymous = MiniDInvoke.GetFunctionPointer<WTSEnumerateServersA>("wtsapi32.dll", "WTSEnumerateServersA");
            return anonymous(pDomainName, Reserved, Version,ref ppServerInfo, ref pCount);
        }

        public static bool WTSEnumerateServersA([MarshalAs(UnmanagedType.LPTStr)] string pDomainName,ref IntPtr ppServerInfo, ref int pCount)
        {
            return WTSEnumerateServersA(pDomainName,0,1,ref ppServerInfo,ref pCount);   
        }


        public static int WTSEnumerateSessionsExA(IntPtr hServer, int Reserved, int Version, ref IntPtr ppSessionInfo, ref int pCount)
        {
            WTSEnumerateSessionsExA anonymous = MiniDInvoke.GetFunctionPointer<WTSEnumerateSessionsExA>("wtsapi32.dll", "WTSEnumerateSessionsExA");
            return anonymous(hServer, Reserved, Version, ref ppSessionInfo, ref pCount);
        }

        public static int WTSEnumerateSessionsExA(IntPtr hServer, ref IntPtr ppSessionInfo, ref int pCount)
        {
            /*
            (PS) Microsoft WTSEnumerateSessionsExA documentation has mistakes
            Reference from https://stackoverflow.com/questions/63655482/trying-to-enumerate-terminal-server-sessions-produces-accessviolationexception
             
            This parameter is reserved. Always set this parameter to one. On output, WTSEnumerateSessionsEx does not change the value of this parameter
            This parameter is reserved. Always set this parameter to zero

            reverse the two parameters above 
             */
            return WTSEnumerateSessionsExA(hServer, 0, 1, ref ppSessionInfo, ref pCount);
        }

        public static void WTSCloseServer(IntPtr hServer)
        {
            WTSCloseServer anonymous = MiniDInvoke.GetFunctionPointer<WTSCloseServer>("wtsapi32", "WTSCloseServer");
            anonymous(hServer);
        }

        public static void WTSFreeMemory(IntPtr pMemory)
        {
            WTSFreeMemory anonymous = MiniDInvoke.GetFunctionPointer<WTSFreeMemory>("wtsapi32", "WTSFreeMemory");
            anonymous(pMemory);
        }

        public static bool WTSFreeMemoryExA(WTS_TYPE_CLASS typeClass, IntPtr pMemory, uint NumberOfEntries)
        {
            WTSFreeMemoryExA anonymous = MiniDInvoke.GetFunctionPointer<WTSFreeMemoryExA>("wtsapi32", "WTSFreeMemoryExA");
            return anonymous(typeClass, pMemory, NumberOfEntries);
        }


        public static bool WTSConnectSessionA(uint LogonId, uint TargetLogonId, string pPassword, bool wait)
        {
            WTSConnectSessionA anonymous = MiniDInvoke.GetFunctionPointer<WTSConnectSessionA>("wtsapi32", "WTSConnectSessionA");
            return anonymous(LogonId, TargetLogonId, pPassword, wait);
        }

        public static bool WTSQuerySessionInformation(IntPtr hServer, int sessionId, WTS_INFO_CLASS wtsInfoClass, out IntPtr ppBuffer, out uint pBytesReturned)
        {
            WTSQuerySessionInformation anonymous = MiniDInvoke.GetFunctionPointer<WTSQuerySessionInformation>("wtsapi32", "WTSQuerySessionInformation");
            return anonymous(hServer, sessionId, wtsInfoClass,out ppBuffer,out pBytesReturned);
        }

        public static IntPtr WTSOpenServer([MarshalAs(UnmanagedType.LPStr)] String pServerName)
        {
            WTSOpenServer anonymous = MiniDInvoke.GetFunctionPointer<WTSOpenServer>("wtsapi32", "WTSOpenServerA");
            return anonymous(pServerName);
        }

        public static IntPtr WTSOpenServerExA([MarshalAs(UnmanagedType.LPStr)] String pServerName)
        {
            WTSOpenServerExA anonymous = MiniDInvoke.GetFunctionPointer<WTSOpenServerExA>("wtsapi32", "WTSOpenServerExA");
            return anonymous(pServerName);
        }

        public static bool WTSGetChildSessionId(out uint pSessionID)
        {
            WTSGetChildSessionId anonymous = MiniDInvoke.GetFunctionPointer<WTSGetChildSessionId>("wtsapi32", "WTSGetChildSessionId");
            return anonymous(out pSessionID);
        }

    }
}
