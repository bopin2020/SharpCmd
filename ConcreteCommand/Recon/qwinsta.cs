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
    internal partial class qwinsta : IContract
    {
        public string CommandName => "qwinsta";

        public string Description => "WinSta for sessions";

        /// <summary>
        /// 
        /// WinStationOpenServer -> WinStationEnumerateProcesses --> WinStationEnumerateProcesses --> WinStationInformation
        /// </summary>
        /// <param name="arguments"></param>
        public void Execute(Dictionary<string, string> arguments)
        {
            string Server = "";
            IntPtr hServer = IntPtr.Zero;
            IntPtr hProcesses = IntPtr.Zero;
            SessionID sessionID = new SessionID();
            IntPtr sessionId = IntPtr.Zero;
            int Count = 0;

            if (arguments.ContainsKey("/?"))
            {
                string help = @"
                    QUERY SESSION [sessionname | username | sessionid [/SERVER:servername]
                ";
                Console.WriteLine(help);
                return;
            }

            if (arguments.ContainsKey("/server"))
            {
                Server = arguments["/server"];
                hServer = winsta.WinStationOpenServer(Server);
            }
#if WinStationGetAllProcesses_RPC

            uint numberProcesses;
            IntPtr processes;
            /*
             http://www.rohitab.com/discuss/topic/36562-undocumented-function-for-process-listing/
             */
            if (!winsta.WinStationGetAllProcesses(hServer, (uint)0, out numberProcesses, out processes))
            {
                Console.WriteLine(Marshal.GetLastWin32Error().ExceptionMessage());
            }
            else
            {
                Int64 current = (long)processes;
                Int32 dataSize = Marshal.SizeOf(typeof(TS_ALL_PROCESSES_INFO_NT6));
                Console.WriteLine("SessionID\tPID\tPPID\tImageName");
                for (int i = 0; i < numberProcesses; i++)
                {
                    TS_ALL_PROCESSES_INFO_NT6 allProcess = (TS_ALL_PROCESSES_INFO_NT6)Marshal.PtrToStructure((IntPtr)current, typeof(TS_ALL_PROCESSES_INFO_NT6));
                    current += dataSize;

                    Int64 addrProcess = (long)allProcess.pTsProcessInfo;
                    Int32 ts_dataSize = Marshal.SizeOf(typeof(TS_SYS_PROCESS_INFORMATION));
                    TS_SYS_PROCESS_INFORMATION ts_sys_process = (TS_SYS_PROCESS_INFORMATION)Marshal.PtrToStructure((IntPtr)addrProcess, typeof(TS_SYS_PROCESS_INFORMATION));
                    if (ts_sys_process.ImageName.MaximumLength != 0)
                    {
                        Console.WriteLine(ts_sys_process.SessionId + Constant.T + ts_sys_process.UniqueProcessId + Constant.T + ts_sys_process.InheritedFromUniqueProcessId + Constant.T + ts_sys_process.ImageName.Buffer);
                    }
                    else
                    {
                        Console.WriteLine(ts_sys_process.SessionId + Constant.T + ts_sys_process.UniqueProcessId + Constant.T + ts_sys_process.InheritedFromUniqueProcessId);
                    }
                }
            }

#endif
            if(!winsta.WinStationEnumerateW(hServer,ref sessionId, ref Count))
            {
                Console.WriteLine(Marshal.GetLastWin32Error().ExceptionMessage());
            }
            else
            {
                Int64 current = (int)sessionId;
                Int32 dataSize = Marshal.SizeOf(typeof(SessionID));
                for (int i = 0; i < Count; i++)
                {
                    SessionID si = (SessionID)Marshal.PtrToStructure((System.IntPtr)current, typeof(SessionID));
                    current += dataSize;
                    Console.WriteLine(si.sessionID + Constant.T + Encoding.ASCII.GetString(si.WinStationName) + Constant.T + si.infoType.ToString());

#if !WinStationQueryInformation
                    WINSTATIONINFORMATIONW winStation = new WINSTATIONINFORMATIONW();
                    int len = 0;
                    IntPtr pSessionID = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SessionID)));
                    Marshal.StructureToPtr(si, pSessionID,false);
                    if (!winsta.WinStationQueryInformation(hServer, si.sessionID, WinStationInformationClass.WinStationInformation, ref winStation, Marshal.SizeOf(winStation), ref len))
                    {
                        Console.WriteLine(Marshal.GetLastWin32Error().ExceptionMessage());
                    }
                    else
                    {
                        winsta.WinStationQueryInformation(hServer, si.sessionID, WinStationInformationClass.WinStationInformation, ref winStation, Marshal.SizeOf(winStation), ref len);
                        Console.WriteLine(Encoding.UTF8.GetString(winStation.UserName));
                    }

#endif
                }
            }


            //PtrToStructCollection<SessionID> sessionIds = new PtrToStructCollection<SessionID>();
            //foreach (var item in sessionIds.Convert(sessionId, Count))
            //{
            //    WINSTATIONINFORMATIONW winStation = new WINSTATIONINFORMATIONW();
            //    int len = 0;
            //    if (!winsta.WinStationQueryInformation(hServer,item.sessionID,WinStationInformationClass.WinStationInformation, ref winStation,Marshal.SizeOf(winStation),ref len))
            //    {
            //        Console.WriteLine(Marshal.GetLastWin32Error());
            //    }
            //    Console.WriteLine(winStation.CurrentTime);
            //    Console.WriteLine(item.sessionID);
            //}

            Marshal.FreeHGlobal(sessionId);
            winsta.WinStationFreeMemory(hServer);
        }


    }
    internal partial class qwinsta
    {
        private void DisplayUserInfo()
        {

        }
    }


    /// <summary>
    /// convert ptr to a ton of struct List<Struct>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PtrToStructCollection<T> where T: struct
    {
        public IList<T> Convert(IntPtr address,int len = 1)
        {
            IList<T> values = new List<T>();
            for (int i = 0; i < len; i++)
            {
                IntPtr newaddr = new IntPtr(address.ToInt64() + (long)i * Marshal.SizeOf(typeof(T)));
                T value = new T();
                value = (T)Marshal.PtrToStructure(newaddr,typeof(T));
                values.Add(value);
            }
            return values;
        }
    }
}
