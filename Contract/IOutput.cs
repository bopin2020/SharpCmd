using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.Contract
{
    internal interface IOutput
    {
        void WriteLine(params string[] message);
    }

    
}
