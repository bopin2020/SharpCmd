using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.Recon
{
    internal class ping : ReconBase
    {
        public override string CommandName => "ping";

        public override string Description => "ICMP Package Management";

        public override string CommandHelp => "ping ip | fqdn";

        public override void Execute(Dictionary<string, string> arguments)
        {

        }
    }
}
