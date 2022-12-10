using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.ExecutePayload
{
    internal abstract class ExecutePayloadBase : Contractbase
    {
        public override string ModuleName => "ExecutePayload";
    }
}
