using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SharpCmd.ConcreteCommand.FileOperation;

namespace SharpCmd.ConcreteCommand.ProcessManage
{
    internal class ps : ProcessManageBase
    {
        public override string CommandName => "ps";

        public override string Description => "list process";

        public override string CommandHelp => "ps -all";

        private IList<ProcessInformation> processInformationes = new List<ProcessInformation>();

        public override void Execute(Dictionary<string, string> arguments)
        {
            if (base.HelpCheck(arguments)) return;

            foreach (var item in Process.GetProcesses())
            {
                try
                {
                    ProcessModule[] currnetModules = new ProcessModule[item.Modules.Count];
                    item.Modules.CopyTo(currnetModules, 0);
                    ProcessInformation processInformation = new ProcessInformation()
                    {
                        ProcessName = item.ProcessName,
                        IsDotNet = currnetModules.Select(x => x.FileName == @"c:\windows\system32\amsi.dll").FirstOrDefault(),
                        PID = item.Id,
                        PPID = 0,
                        SessionID = item.SessionId,
                    };
                    processInformationes.Add(processInformation);
                }
                catch (Exception)
                {

                }
            }
            foreach (var process in processInformationes)
            {
                Console.WriteLine(process.ToString());
            }
        }
    }

    public struct ProcessInformation : IComparable
    {
        public string ProcessName { get; set; }
        public bool IsDotNet { get; set; }
        public int PID { get; set; }
        public int PPID { get; set; }
        public int SessionID { get; set; }
        public IntPtr Handle { get; set; }
        public Arch Arch { get; set; }

        public int CompareTo(object obj)
        {
            return PID == ((ProcessInformation)obj).PID ? 1 : 0;
        }

        public override string ToString()
        {
            return ProcessName + "|" + IsDotNet + "|" + PID + "}" + PPID + "|" + SessionID + "|" + Handle.ToString("x") + "|" + Arch.ToString();
        }
    }
    public enum Arch
    {
        None,
        x86,
        x64,
    }
}
