using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SharpCmd.ConcreteCommand.ExecutePayload
{
    internal class executeassembly : ExecutePayloadBase
    {
        public override string CommandName => "execute-assembly";

        public override string Description => "execute .net assembly exe and dll without patch amsi/etw";

        public override string CommandHelp => "execute-assembly [filename exe / dll]";

        public override void Execute(Dictionary<string, string> arguments)
        {
            if (base.HelpCheck(arguments))
            {
                return;
            }
            string filename = null;
            object[] args = null;
            //filename = arguments.Keys.ToArray()[1];
            filename = @"C:\Users\User\Desktop\DynamicPInvoke.exe";
            //args = arguments.Keys.ToArray()[2].Split();
            args = new object[] { new string[] { "" } };
            try
            {
                Assembly.LoadFile(filename).EntryPoint.Invoke(null, args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
