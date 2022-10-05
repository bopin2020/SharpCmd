using SharpCmd.Contract;
using SharpCmd.Lib.Help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.Recon
{
    internal class whoami : IContract
    {
        public string CommandName => "whoami";

        public void Execute(Dictionary<string, string> arguments)
        {
            Console.WriteLine(Environment.MachineName + Constant.SpecialSlash + Environment.UserName);
            Console.WriteLine();

            bool priv;
            bool user;
            if(arguments.ContainsKey("/user"))
            {
                Console.WriteLine("user");
            }
        }
    }
}
