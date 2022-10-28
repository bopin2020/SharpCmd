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
        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern int RtlGetVersion(ref OSVERSIONINFOEX versionInfo);
    }
}
