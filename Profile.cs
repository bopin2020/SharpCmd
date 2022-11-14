using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd
{
    internal class Profile
    {
        public const string version = "0.1";

        public static IOutput output = new ConsoleTable();
    }
}
