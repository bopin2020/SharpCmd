using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.Contract
{
    internal interface IContract
    {
        string CommandName { get; }
        string Description { get; }
        void Execute(Dictionary<string, string> arguments);
    }

    internal abstract class Contractbase : IContract
    {
        public abstract string ModuleName { get; }
        public abstract string CommandName { get; }
        public abstract string Description { get; }
        public abstract string CommandHelp { get; }

        public abstract void Execute(Dictionary<string, string> arguments);

        public virtual bool HelpCheck(Dictionary<string, string> arguments)
        {
            string[] helpStr = new string[] { "-h", "--h", "--help", "/?" };
            if(helpStr.Select(s => arguments.ContainsKey(s)).Contains(true))
            {
                Console.WriteLine("CommandName:\t" + CommandName);
                Console.WriteLine("Description:\t" + Description);
                Console.WriteLine("[Example]\t" + CommandHelp);
                return true;
            }
            return false;
        }
    }
}
