using SharpCmd.Contract;
using SharpCmd.Lib.Help;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.Misc
{
    internal class test : IContract
    {
        public string CommandName => nameof(test);

        public string Description => "just for test";

        public void Execute(Dictionary<string, string> arguments)
        {
            QueryRegInfo queryRegInfo = new QueryRegInfo(RootRegistry.ClassesRoot, "cmdfile");
            foreach (var item in queryRegInfo.EnumSubKeysName("",true))
            {
                Console.WriteLine(item);

                //queryRegInfo.SetSubkeyName($"cmdfile\\{item}");
                //if(queryRegInfo.CurrentKey.GetSubKeyNames().Where(x => x == "Elevation").Any())
                //{
                //}
            }

            queryRegInfo.SetRootkey(RootRegistry.LocalMachine);
            queryRegInfo.SetSubkeyName("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\AeDebug");

            foreach (var item in queryRegInfo.EnumKeyValues())
            {
                Console.WriteLine(String.Format("{0,-30} {1,-15} {2}",item.ValueNames,item.ValueKind,item.Value));
            }
        }

        private void WriteToTable(params object[] args)
        {
            foreach (var item in args)
            {
                Console.Write("{0,-20}",item);
            }
        }
    }
}
