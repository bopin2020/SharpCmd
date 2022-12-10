using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.FileOperation
{
    internal class pwd : FileOperationBase
    {
        public override string CommandName => "pwd";

        public override string Description => "print the current path";

        public override string CommandHelp => "pwd";

        public override void Execute(Dictionary<string, string> arguments)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
        }
    }
}
