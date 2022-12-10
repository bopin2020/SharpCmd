using SharpCmd.Contract;
using System.Collections.Generic;

namespace SharpCmd.ConcreteCommand.Recon
{
    internal class net : ReconBase
    {
        public override string CommandName => "net";
        public override string Description => "net module";

        public override string CommandHelp => "net";

        public override void Execute(Dictionary<string, string> arguments)
        {

        }
    }

    public class net_Accounts
    {

    }
}
