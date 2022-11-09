using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.FileOperation
{
    internal class pwd : IContract
    {
        public string CommandName => "pwd";

        public string Description => "print the current path";

        public void Execute(Dictionary<string, string> arguments)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
        }
    }
}
