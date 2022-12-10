using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.FileOperation
{
    internal class cd : FileOperationBase
    {
        public override string CommandName => "cd";

        public override string Description => "change the current directory";

        public override string CommandHelp => "cd c:\\windows";

        public override void Execute(Dictionary<string, string> arguments)
        {
            if(base.HelpCheck(arguments))
            {
                return;
            }

            string newPath = null;
            try
            {
                newPath = arguments.Keys.ToArray()[1];
                Directory.SetCurrentDirectory(newPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
