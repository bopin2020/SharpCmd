using SharpCmd.Contract;
using System.Collections.Generic;

namespace SharpCmd.ConcreteCommand.Recon
{
    internal class net : IContract
    {
        public string CommandName => "net";
        public string Description => "net module";
        public void Execute(Dictionary<string, string> arguments)
        {

        }
    }

    public class net_Accounts
    {

    }
}
