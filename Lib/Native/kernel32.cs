using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpCmd.Lib.Native
{
    internal class kernel32
    {
        [DllImport("kernel32.dll")]
        public static extern uint GetSystemDirectory([Out] StringBuilder lpBuffer,uint uSize);
    }
}
