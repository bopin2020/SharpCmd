#define MoreDetails
using SharpCmd.Contract;
using SharpCmd.Lib.Delegates;
using SharpCmd.Lib.Help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpCmd.ConcreteCommand.Recon
{
    internal partial class quser : ReconBase
    {
        public override string CommandName => "quser";

        public override string Description => "WTS query user login info";

        public override string CommandHelp => @"
QUERY USER [username | sessionname | sessionid] [/SERVER:servername]

  username            标识用户名。
  sessionname         用名称 sessionname 识别会话。
  sessionid           用 ID sessionid 识别会话。
  /SERVER:servername  要查询的服务器(默认值是当前值)。
                ";

        /// <summary>
        /// WTSEnumerateSessions
        /// 
        /// WinStationEnumerate --> 
        /// </summary>
        /// <param name="arguments"></param>
        public override void Execute(Dictionary<string, string> arguments)
        {
            if (base.HelpCheck(arguments)) return;

            string Server = "";
            IntPtr hServer = IntPtr.Zero;
            IntPtr sessionId = IntPtr.Zero;
            int Count = 0;

            if (arguments.ContainsKey("/server"))
            {
                Server = arguments["/server"];
                hServer = winsta.WinStationOpenServer(Server);
            }


            if (!winsta.WinStationEnumerateW(hServer, ref sessionId, ref Count))
            {
                Console.WriteLine(Marshal.GetLastWin32Error().ExceptionMessage());
            }
            else
            {
                Int64 sessionIdDWORD = (long)sessionId;
                Int32 size = Marshal.SizeOf(typeof(SessionID));
                for (int i = 0; i < Count; i++)
                {
                    SessionID si = (SessionID)Marshal.PtrToStructure((IntPtr)sessionIdDWORD, typeof(SessionID));
                    sessionIdDWORD += size;
                    Console.WriteLine(si.sessionID + Constant.T + Encoding.ASCII.GetString(si.WinStationName) + Constant.T + si.infoType.ToString());
                    DisplayUserInfo(si);
                }
            }


#if WTSEnumerateServerA
            IntPtr intPtr = IntPtr.Zero;
            int counts = 0;
            if(!wtsapi32.WTSEnumerateServersA(Environment.UserDomainName,ref intPtr,ref counts))
            {
                Console.WriteLine(Marshal.GetLastWin32Error().ExceptionMessage());
            }    
#endif

#if MoreDetails
                    IntPtr ppSessionInfo = IntPtr.Zero;
            Int32 count = 0;
            Int32 dataSize = Marshal.SizeOf(typeof(WTS_SESSION_INFO_1A));
            hServer = wtsapi32.WTSOpenServerExA("localhost");
            wtsapi32.WTSEnumerateSessionsExA(hServer, ref ppSessionInfo, ref count);
            if (wtsapi32.WTSEnumerateSessionsExA(hServer, ref ppSessionInfo, ref count) != 0)
#else
            IntPtr ppSessionInfo = IntPtr.Zero;
            Int32 count = 0;
            Int32 dataSize = Marshal.SizeOf(typeof(WTS_SESSION_INFO));
            IntPtr hServer = CurrentServer;
            if (wtsapi32.WTSEnumerateSessions(hServer,ref ppSessionInfo,ref count) !=0)
#endif
            {
                Int64 current = (int)ppSessionInfo;
                for (int i = 0; i < count; i++)
                {
#if MoreDetails
                    //WTS_SESSION_INFO_1A si = (WTS_SESSION_INFO_1A)Marshal.PtrToStructure((System.IntPtr)current, typeof(WTS_SESSION_INFO_1A));
                    //current += dataSize;
                    //Console.WriteLine(si.pUserName + Constant.T + si.pSessionName + Constant.T + si.SessionId + Constant.T + si.State + Constant.T + si.pHostName);
#else
                    WTS_SESSION_INFO si = (WTS_SESSION_INFO)Marshal.PtrToStructure((System.IntPtr)current, typeof(WTS_SESSION_INFO));
                    current += dataSize;
                    Console.WriteLine(si.SessionID + Constant.T + si.State + Constant.T + si.pWinStationName);
#endif
                }
            }
            else
            {
                Console.WriteLine(Marshal.GetLastWin32Error().ExceptionMessage());
            }
            wtsapi32.WTSFreeMemory(ppSessionInfo);
            wtsapi32.WTSCloseServer(hServer);
        }
    }

    internal partial class quser
    {
        private IntPtr CurrentServer => wtsapi32.WTSOpenServer("localhost");

        private void DisplayUserInfo(SessionID sessionID)
        {
            IntPtr addr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SessionID)));
            Marshal.StructureToPtr(sessionID,addr,false);
            winsta.WinStationFreeMemory(addr);
        }
    }
}
