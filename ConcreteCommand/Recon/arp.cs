using Microsoft.Win32;
using SharpCmd.Contract;
using SharpCmd.Lib.Help;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
namespace SharpCmd.ConcreteCommand.Recon
{
    internal partial class arp : IContract
    {
        public string CommandName => "arp";

        public string Description => "collect arp information";

        public void Execute(Dictionary<string, string> arguments)
        {

        }
    }
}
