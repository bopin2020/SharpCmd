using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.Recon
{
    internal class ipconfig : IContract
    {
        public string CommandName => "ipconfig";

        public void Execute(Dictionary<string, string> arguments)
        {
            
        }
    }
}
