using SharpCmd.Lib.Delegates;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpCmd.Lib.Delegates
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CACHE_STATISTICS
    {
        public ushort ProtocolType;
        public ushort Length;
        // dismiss others structs https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-tsts/81203ca2-e58b-4681-affa-924e59671b5c
        [MarshalAs(UnmanagedType.ByValArray,SizeConst = 20)]
        public uint[] Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PROTOCOLCOUNTERS
    {
        public uint WdBytes;
        public uint WdFrames;
        public uint WaitForOutBuf;
        public uint Frames;
        public uint Bytes;
        public uint CompressedBytes;
        public uint CompressFlushes;
        public uint Errors;
        public uint Timeouts;
        public uint AsyncFramingError;
        public uint AsyncOverrunError;
        public uint AsyncOverflowError;
        public uint AsyncParityError;
        public uint TdErrors;
        public ushort ProtocolType;
        public ushort Length;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public uint[] Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PROTOCOLSTATUS
    {
        PROTOCOLCOUNTERS Output;
        PROTOCOLCOUNTERS Input;
        CACHE_STATISTICS Cache;
        public uint AsyncSignal;
        public uint AsyncSignalMask;
    }

   [StructLayout(LayoutKind.Sequential)]
    public struct WINSTATIONINFORMATIONW
    {
        /// <summary>
        /// The current connect state of the session
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 70)]
        public Byte[] ConnectState;
        /// <summary>
        /// The name of the session
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr,SizeConst = 33)]
        public string WinStationName;
        public uint LogonId;
        public LARGE_INTEGER ConnectTime;
        public LARGE_INTEGER DisconnectTime;
        public LARGE_INTEGER LastInputTime;
        public LARGE_INTEGER LoginTime;
        public PROTOCOLSTATUS Status;
        [MarshalAs(UnmanagedType.LPTStr,SizeConst = 33)]
        public string DomainNmae;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1096)]
        public Byte[] UserName;
        public LARGE_INTEGER CurrentTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SessionID
    {
        public uint sessionID;
        // quser.c         TruncateString( _wcslwr(Info.WinStationName), 15 );
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] WinStationName;
        public WinStationInformationClass infoType;
    }
}

namespace SharpCmd.Lib.Delegates
{

    /// <summary>
    /// WinStationQueryInformation
    /// </summary>
    public enum WinStationInformationClass : ushort
    {
        WinStationCreateData,
        WinStationConfiguration,
        WinStationPdParams,
        WinStationWd,
        WinStationPd,
        WinStationPrinter,
        WinStationClient,
        WinStationModules,
        WinStationInformation,
        WinStationTrace,
        WinStationBeep,
        WinStationEncryptionOff,
        WinStationEncryptionPerm,
        WinStationNtSecurity,
        WinStationUserToken,
        WinStationUnused1,
        WinStationVideoData,
        WinStationInitialProgram,
        WinStationCd,
        WinStationSystemTrace,
        WinStationVirtualData,
        WinStationClientData,
        WinStationSecureDesktopEnter,
        WinStationSecureDesktopExit,
        WinStationLoadBalanceSessionTarget,
        WinStationLoadIndicator,
        WinStationShadowInfo,
        WinStationDigProductId,
        WinStationLockedState,
        /// <summary>
        /// Reference from https://en.delphipraxis.net/topic/6288-obtaining-client-ip-address-from-windows-virtual-desktop-app/
        /// </summary>
        WinStationRemoteAddress,
        WinStationIdleTime,
        WinStationLastReconnectType,
        WinStationDisallowAutoReconnect,
        WinStationUnused2,
        WinStationUnused3,
        WinStationUnused4,
        WinStationUnused5,
        WinStationReconnectedFromId,
        WinStationEffectsPolicy,
        WinStationType,
        WinStationInformationEx,
        WinStationValidationInfo,
    }

}

namespace SharpCmd.Lib.Delegates
{
    /*
     @Credits to https://processhacker.sourceforge.io/doc/winsta_8h.html#af9b0ec93a2859b29494e0148e3995c85
     */

    public delegate IntPtr WinStationOpenServer([MarshalAs(UnmanagedType.LPStr)] string servername);

    public delegate bool WinStationEnumerateProcesses(IntPtr hServer, ref IntPtr process);

    public delegate bool WinStationEnumerateW(IntPtr hServer,ref IntPtr sessionIdPointer,ref int Count);

    public delegate bool WinStationFreeMemory(IntPtr address);

    /// <summary>
    /// Credits to https://github.com/ravel57/PCsearchMultitool/blob/9331d2e98f562e51dbeafa08e1e7078345c4149f/Cassia/Source/Cassia/Impl/NativeMethods.cs
    /// https://learn.microsoft.com/en-us/previous-versions/aa383827(v=vs.85)
    /// </summary>
    /// <param name="hServer"></param>
    /// <param name="sessionIdPointer"></param>
    /// <param name="winStationInformationClass"></param>
    /// <param name="buffer"></param>
    /// <param name="bufferLength"></param>
    /// <param name="returnedLength"></param>
    /// <returns></returns>
    public delegate bool WinStationQueryInformation(IntPtr hServer,uint sessionId, WinStationInformationClass winStationInformationClass, ref WINSTATIONINFORMATIONW buffer, int bufferLength,ref int returnedLength);


    #region Undocumentation
    /*
     WinStationGetAllProcesses -> RpcWinStationGetAllProcesses_NT6
     */
    public delegate bool WinStationGetAllProcesses(IntPtr hServer,uint Level,out uint pNumberOfProcesses,out IntPtr ppProcessArray);

    // WinStationGetAllSessionsEx
    #endregion

    public class winsta
    {
        public static IntPtr WinStationOpenServer(string servername)
        {
            WinStationOpenServer anonymous = MiniDInvoke.GetFunctionPointer<WinStationOpenServer>("winsta.dll", "WinStationOpenServerA");
            return anonymous(servername);
        }

        public static bool WinStationEnumerateProcesses(IntPtr hServer, ref IntPtr process)
        {
            WinStationEnumerateProcesses anonymous = MiniDInvoke.GetFunctionPointer<WinStationEnumerateProcesses>("winsta.dll", "WinStationEnumerateProcesses");
            return anonymous(hServer, ref process);
        }
        /// <summary>
        /// Method sig parameters  ref or out => reference from WTSEnumerateSession
        /// </summary>
        /// <param name="hServer"></param>
        /// <param name="sessionIdPointer"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public static bool WinStationEnumerateW(IntPtr hServer, ref IntPtr sessionIdPointer, ref int Count)
        {
            WinStationEnumerateW anonymous = MiniDInvoke.GetFunctionPointer<WinStationEnumerateW>("winsta.dll", "WinStationEnumerateW");
            return anonymous(hServer, ref sessionIdPointer, ref Count);
        }

        public static bool WinStationFreeMemory(IntPtr address)
        {
            WinStationFreeMemory anonymous = MiniDInvoke.GetFunctionPointer<WinStationFreeMemory>("winsta.dll", "WinStationFreeMemory");
            return anonymous(address);
        }

        public static bool WinStationQueryInformation(IntPtr hServer, uint sessionId, WinStationInformationClass winStationInformationClass, ref WINSTATIONINFORMATIONW buffer, int bufferLength, ref int returnedLength)
        {
            WinStationQueryInformation anonymous = MiniDInvoke.GetFunctionPointer<WinStationQueryInformation>("winsta.dll", "WinStationQueryInformationA");
            return anonymous(hServer, sessionId, winStationInformationClass, ref buffer, bufferLength, ref returnedLength);
        }

        public static bool WinStationGetAllProcesses(IntPtr hServer, uint Level, out uint pNumberOfProcesses, out IntPtr ppProcessArray)
        {
            WinStationGetAllProcesses anonymous = MiniDInvoke.GetFunctionPointer<WinStationGetAllProcesses>("winsta.dll", "WinStationGetAllProcesses");
            return anonymous(hServer, Level, out pNumberOfProcesses, out ppProcessArray);
        }
    }
}
