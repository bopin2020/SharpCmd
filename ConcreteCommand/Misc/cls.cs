using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static SharpCmd.Lib.Native.kernel32;

namespace SharpCmd.ConcreteCommand.Misc
{
    internal class cls : MiscBase
    {
        public override string CommandName => "cls";

        public override string Description => "clear the current screen";

        public override string CommandHelp => "cls";

        public override void Execute(Dictionary<string, string> arguments)
        {
            Console.Clear();
        }
    }
}
