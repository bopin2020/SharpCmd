using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.FileOperation
{
    internal abstract class FileOperationBase : Contractbase
    {
        public override string ModuleName => "FileOperation";
    }
}
