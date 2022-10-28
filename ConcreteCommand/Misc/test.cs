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
            queryRegInfo.SetSubkeyName("SYSTEM\\CurrentControlSet\\Control");

            foreach (var item in queryRegInfo.EnumKeyValues())
            {
                Console.WriteLine(item.ValueNames + Constant.T + item.ValueKind + Constant.T + item.Value);
            }
        }
    }
}
