using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.ProcessManage
{
    internal abstract class ProcessManageBase : Contractbase
    {
        public override string ModuleName => "ProcessManage";
    }
}
