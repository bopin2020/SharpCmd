using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.Misc
{
    internal class exit : MiscBase
    {
        public override string CommandName => "exit";

        public override string Description => "exit process";

        public override string CommandHelp => "exit";

        public override void Execute(Dictionary<string, string> arguments)
        {
            Environment.Exit(0);
        }
    }
}
