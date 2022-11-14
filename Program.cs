using SharpCmd.Contract;
using SharpCmd.Lib.Delegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static SharpCmd.Lib.Native.kernel32;

namespace SharpCmd
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Command command = new CommandWrapper();
            using (SharpCmd sharpCmd = new SharpCmd(command))
            {
                sharpCmd.Init(args);
            }
        }
    }
}
