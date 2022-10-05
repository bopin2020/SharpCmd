using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.Misc
{
    internal class exit : IContract
    {
        public string CommandName => "exit";

        public void Execute(Dictionary<string, string> arguments)
        {
            Environment.Exit(0);
        }
    }
}
