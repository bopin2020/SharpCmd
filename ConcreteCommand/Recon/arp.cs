using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace SharpCmd.ConcreteCommand.Recon
{
    internal class arp : IContract
    {
        public string CommandName => "arp";
        public void Execute(Dictionary<string, string> arguments)
        {
            
        }
    }
}
