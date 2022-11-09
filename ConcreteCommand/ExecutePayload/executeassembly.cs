using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SharpCmd.ConcreteCommand.ExecutePayload
{
    internal class executeassembly : IContract
    {
        public string CommandName => "execute-assembly";

        public string Description => "execute .net assembly exe and dll without patch amsi/etw";

        public void Execute(Dictionary<string, string> arguments)
        {
            if (arguments.ContainsKey("/?"))
            {
                Console.WriteLine("execute-assembly [filename exe / dll]");
                return;
            }
            string filename = null;
            string[] args = null;
            filename = arguments.Keys.ToArray()[1];
            args = arguments.Keys.ToArray()[2].Split();
            try
            {
                Assembly.LoadFile(filename).EntryPoint.Invoke(null, args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
