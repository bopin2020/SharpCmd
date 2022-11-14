using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SharpCmd.ConcreteCommand.Recon
{
    internal class ipconfig : IContract
    {
        public string CommandName => "ipconfig";

        public string Description => "print network information";

        public void Execute(Dictionary<string, string> arguments)
        {
            string hostname = Dns.GetHostName();
            foreach (IPAddress item in Dns.GetHostAddresses(hostname))
            {
                Console.WriteLine(item.AddressFamily + "\t" + item.ToString());
            }
        }
    }
}
