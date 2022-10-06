using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.FileOperation
{
    internal class cd : IContract
    {
        public string CommandName => "cd";

        public void Execute(Dictionary<string, string> arguments)
        {
            string newPath = null;
            try
            {
                newPath = arguments.Keys.ToArray()[1];
                Directory.SetCurrentDirectory(newPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
