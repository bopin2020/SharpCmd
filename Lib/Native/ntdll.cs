using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static SharpCmd.Lib.Native.kernel32;

namespace SharpCmd.Lib.Native
{
    public class ntdll
    {
        private const string NTDLL = "ntdll.dll";

        [DllImport(NTDLL, SetLastError = true)]
        public static extern int RtlGetVersion(ref OSVERSIONINFOEX versionInfo);

        [DllImport(NTDLL,CharSet = CharSet.Unicode)]
        public static extern string RtlIpv4AddressToStringW(IntPtr addr, out StringBuilder ipaddr);
    }
}
