using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SharpCmd.Lib.Native.kernel32;

namespace SharpCmd.Lib.Delegates
{
    public delegate int RtlGetVersion(ref OSVERSIONINFOEX versionInfo);
    public class ntdll
    {
        public static int RtlGetVersion(ref OSVERSIONINFOEX versionInfo)
        {
            RtlGetVersion anonymous = MiniDInvoke.GetFunctionPointer<RtlGetVersion>("ntdll.dll", "RtlGetVersion");
            return anonymous(ref versionInfo);
        }
    }
}
