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

        public override void Execute(Dictionary<string, string> arguments)
        {
            if (base.HelpCheck(arguments)) return;

            foreach (var item in Process.GetProcesses())
            {
                Console.WriteLine(item.ProcessName);
            }
        }
    }

    public struct ProcessInformation : IComparable
    {
        public string ProcessName { get; }
        public bool IsDotNet { get; }
        public int PID { get; }
        public int PPID { get; }
        public IntPtr Handle { get; }
        public Arch Arch { get; }

        public int CompareTo(object obj)
        {
            return PID == ((ProcessInformation)obj).PID ? 1 : 0;
        }
    }
    public enum Arch
    {
        None,
        x86,
        x64,
    }
}
