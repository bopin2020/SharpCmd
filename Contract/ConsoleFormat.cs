using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.Contract
{
    internal class ConsoleFormat : IOutput
    {
        public void WriteLine(params string[] message)
        {
            foreach (var item in message)
            {
                Console.Write(String.Format("{0,-20}",item));
            }
            Console.WriteLine();
        }
    }
}
