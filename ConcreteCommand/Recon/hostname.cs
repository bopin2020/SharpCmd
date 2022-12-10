using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;

namespace SharpCmd.ConcreteCommand.Recon
{
    /// <summary>
    /// dnsapi.dll
    /// </summary>
    internal class hostname : ReconBase
    {
        public override string CommandName => nameof(hostname);

        public override string Description => "gain the hostname";

        public override string CommandHelp => "hostname";

        public override void Execute(Dictionary<string, string> arguments)
        {
            Console.WriteLine(GetHostname());
        }

        private string GetHostname()
        {
            string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            string hostName = Dns.GetHostName();

            domainName = "." + domainName;
            if (!hostName.EndsWith(domainName))  // if hostname does not already include domain name
            {
                hostName += domainName;   // add the domain name part
            }
            return hostName;                    // return the fully qualified name
        }
    }
}
