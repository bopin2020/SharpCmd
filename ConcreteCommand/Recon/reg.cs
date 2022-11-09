using SharpCmd.Contract;
using SharpCmd.Lib.Help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.Recon
{
    /// <summary>
    /// https://github.com/airzero24/WMIReg/
    /// </summary>
    internal class reg : IContract
    {
        public string CommandName => "reg";

        public string Description => "Register Manager";

        public void Execute(Dictionary<string, string> arguments)
        {
            QueryRegInfo queryRegInfo = new QueryRegInfo(RootRegistry.LocalMachine, "SYSTEM\\ControlSet001");
            Console.WriteLine(queryRegInfo.RegistrySubKeyName);
        }
    }

    internal enum RegAction : ushort
    {
        QUERY,
        ADD,
        DELETE,
        COPY,
        SAVE,
        RESTORE,
        LOAD,
        UNLOAD,
        COMPARE,
        EXPORT,
        IMPORT,
        FLAGS,
    }
}
