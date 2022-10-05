using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Command command = new CommandWrapper();
            SharpCmd sharpCmd = new SharpCmd(command);
            sharpCmd.Init();
        }
    }
}
