using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.Evasion
{
    internal class PatchEtw : EvasionBase
    {
        public override string CommandName => "etw";

        public override string Description => "patch ntdll!EtwEventWrite";

        public override string CommandHelp => "patchetw enable|disable 0|1";

        public override void Execute(Dictionary<string, string> arguments)
        {
            if (base.HelpCheck(arguments))
            {
                return;
            }
        }
    }
}
