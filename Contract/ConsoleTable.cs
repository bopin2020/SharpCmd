using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.Contract
{
    internal class ConsoleTable : IOutput
    {
        public void WriteLine(params string[] message)
        {
            for (int i = 0; i < String.Join("",message).Length; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();
            foreach (var item in message)
            {
                Console.Write("|" + item);
            }
            Console.Write("|");
            for (int i = 0; i < String.Join("", message).Length; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();
        }
    }
}
