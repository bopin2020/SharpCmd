using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.FileOperation
{
    internal class dir : IContract
    {
        public string CommandName => "dir";

        public void Execute(Dictionary<string, string> arguments)
        {
            
        }
    }
}
