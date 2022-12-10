#define MiniDInvoke
using SharpCmd.Contract;
using SharpCmd.Lib.Delegates;
using SharpCmd.Lib.Help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static SharpCmd.Lib.Native.kernel32;

namespace SharpCmd.ConcreteCommand.Misc
{
    internal class ver : MiscBase
    {
        public override string CommandName => nameof(ver);

        public override string Description => "show current os version and clr version";

        public override string CommandHelp => "ver";

        public override void Execute(Dictionary<string, string> arguments)
        {
            Console.WriteLine();
            OperatingSystem operatingSystem = Environment.OSVersion;
            Console.WriteLine("OperatingSystem:\t" + operatingSystem.VersionString);
            Console.WriteLine("CLR:\t" + Environment.Version);

            OSVERSIONINFOEX osversioninfo = new OSVERSIONINFOEX();
            osversioninfo.dwOSVersionInfoSize = Marshal.SizeOf(osversioninfo);

#if PInvoke
            /// 
            /// in windows 8.1 and later,GetVersion and GetVersionEx as well as Environment.OSVersion have been deprecated
            /// 
            if (GetVersionEx(ref osversioninfo))
            {
                Console.WriteLine(osversioninfo.dwMajorVersion + Constant.Dot + osversioninfo.dwMinorVersion + Constant.Dot + osversioninfo.dwBuildNumber + Constant.T + osversioninfo.szCSDVersion);
            }
#else

#if MiniDInvoke
            if(ntdll.RtlGetVersion(ref osversioninfo) == 0)
#else
            if (ntdll.RtlGetVersion(ref osversioninfo) == 0)
#endif
            {
                Console.WriteLine(osversioninfo.dwMajorVersion + Constant.Dot + osversioninfo.dwMinorVersion + Constant.Dot + osversioninfo.dwBuildNumber + Constant.T + osversioninfo.szCSDVersion);
            }
#endif
        }
    }
    // 封装 版本函数
    // 服务器还是PC
    // 域内
}
