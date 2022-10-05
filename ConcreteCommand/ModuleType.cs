using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand
{
    /// <summary>
    /// As we all known,cmd.exe is a wrapper application,which communicated with condrv.sys that under kernel mode with io package
    /// I want to re-implement these features with win32 api.Becauss cmd /c with args that's not opsec during red team evalutation
    /// </summary>
    internal enum ModuleType
    {
        Misc,
        FileOperation,
        ProcessManage,
        Recon,
        ServiceManage,
        RegistryManage
    }
}
