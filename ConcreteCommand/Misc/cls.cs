using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static SharpCmd.Lib.Native.kernel32;

namespace SharpCmd.ConcreteCommand.Misc
{
    internal class cls : IContract
    {
        public string CommandName => "cls";

        public string Description => "clear the current screen";

        public void Execute(Dictionary<string, string> arguments)
        {
            Console.Clear();
        }
    }
}
