using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace SharpCmd.ConcreteCommand.FileOperation
{
    internal class del : IContract
    {
        public string CommandName => "del";

        public string Description => "delete files";

        public void Execute(Dictionary<string, string> arguments)
        {
            FileSecurity fileSecurity = new FileSecurity(arguments.Keys.ToArray()[1],AccessControlSections.Access);

            //var p = fileSecurity.GetAccessRules();

            //fileSecurity.SetAccessRule();
            //File.SetAccessControl(,)
            //File.Delete(arguments.Keys.ToArray()[1]);

            //FileInfo fi = new FileInfo(arguments.Keys.ToArray()[1]);
            //fi.IsReadOnly = true;

            File.SetAttributes(arguments.Keys.ToArray()[1], FileAttributes.ReadOnly);
            // unset readonly
            File.Delete(arguments.Keys.ToArray()[1]);

        }
    }
}
