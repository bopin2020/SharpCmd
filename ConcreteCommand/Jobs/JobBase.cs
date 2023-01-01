using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.Jobs
{
    internal abstract class JobBase : Contractbase
    {
        public override string ModuleName => "job";
    }
}
