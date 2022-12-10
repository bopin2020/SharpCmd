using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.FileOperation
{
    internal class copy : FileOperationBase
    {
        public override string CommandName => "copy";

        public override string Description => "copy file from a to b";

        public override string CommandHelp => "copy c:\\1.txt c:\\2.txt";

        public override void Execute(Dictionary<string, string> arguments)
        {
            if(arguments.Count < 2)
            {
                Console.Error.WriteLine("Command syntax was not correct");
                return;
            }
            if (arguments.ContainsKey("/y"))
            {
                File.Copy(arguments.Keys.ToArray()[2], arguments.Keys.ToArray()[3],true);
                if(File.Exists(arguments.Keys.ToArray()[3]))
                {
                    Console.WriteLine("Copy file successfully");
                }
                return;
            }
            File.Copy(arguments.Keys.ToArray()[1], arguments.Keys.ToArray()[2]);
            if (File.Exists(arguments.Keys.ToArray()[2]))
            {
                Console.WriteLine("Copy file successfully");
            }
        }
    }
}
