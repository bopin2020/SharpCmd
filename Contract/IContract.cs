using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.Contract
{
    internal interface IContract
    {
        string CommandName { get; }
        void Execute(Dictionary<string, string> arguments);
    }
}
