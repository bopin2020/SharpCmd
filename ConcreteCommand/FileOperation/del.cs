﻿using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace SharpCmd.ConcreteCommand.FileOperation
{
    internal class del : FileOperationBase
    {
        public override string CommandName => "del";

        public override string Description => "delete files";

        public override string CommandHelp => "del c:\\3.txt";

        public override void Execute(Dictionary<string, string> arguments)
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
