using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static SharpCmd.Lib.Native.netapi32;

namespace SharpCmd.Lib.Delegates
{
    public delegate NET_API_STATUS NetUseEnum(string UncServerName,int Level,ref IntPtr Buf,uint PreferedMaximumSize,out int EntriesRead,out int TotalEntries,IntPtr resumeHandle);

    public delegate int NetApiBufferFree(IntPtr Buffer);

    public delegate int NetGetJoinInformation(string server, out IntPtr domain, out NetJoinStatus status);


    public class netapi32
    {
        public static NET_API_STATUS NetUseEnum(string UncServerName, int Level, ref IntPtr Buf, uint PreferedMaximumSize, out int EntriesRead, out int TotalEntries, IntPtr resumeHandle)
        {
            NetUseEnum anonymous = MiniDInvoke.GetFunctionPointer<NetUseEnum>("netapi32.dll", "NetUseEnum");
            return anonymous(UncServerName, Level,ref Buf, PreferedMaximumSize,out EntriesRead,out TotalEntries, resumeHandle);
        }

        public static int NetApiBufferFree(IntPtr Buffer)
        {
            NetApiBufferFree anonymous = MiniDInvoke.GetFunctionPointer<NetApiBufferFree>("netapi32.dll", "NetApiBufferFree");
            return anonymous(Buffer);
        }

        public static int NetGetJoinInformation(string server, out IntPtr domain, out NetJoinStatus status)
        {
            NetGetJoinInformation anonymous = MiniDInvoke.GetFunctionPointer<NetGetJoinInformation>("netapi32.dll", "NetGetJoinInformation");
            return anonymous(server,out domain,out status);
        }
    }
}
