using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.Recon
{
    internal class quser : IContract
    {
        public string CommandName => "quser";

        public void Execute(Dictionary<string, string> arguments)
        {
            
        }
    }
}
