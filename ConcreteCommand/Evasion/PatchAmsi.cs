using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.Evasion
{
    internal partial class PatchAmsi : EvasionBase
    {
        public override string CommandName => "amsi";

        public override string Description => "patch amsi!AmsiScanBuffer";

        public override string CommandHelp => "amsi enable|disable (0|1)";

        public override void Execute(Dictionary<string, string> arguments)
        {
            if(base.HelpCheck(arguments))
            {
                return;
            }
        }
    }

    internal partial class PatchAmsi
    {

    }
}
