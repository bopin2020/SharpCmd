﻿using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SharpCmd.ConcreteCommand.Recon
{
    internal class ipconfig : ReconBase
    {
        public override string CommandName => "ipconfig";

        public override string Description => "print network information";

        public override string CommandHelp => "ipconfig";

        public override void Execute(Dictionary<string, string> arguments)
        {
            string hostname = Dns.GetHostName();
            foreach (IPAddress item in Dns.GetHostAddresses(hostname))
            {
                Console.WriteLine(item.AddressFamily + "\t" + item.ToString());
            }
        }
    }
}
