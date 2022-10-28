using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace SharpCmd.Lib.Help
{
    internal static class Common
    {
        /// <summary>
        /// returns true if the current process is running with adminstrative privs in a high integrity context
        /// </summary>
        /// <returns></returns>
        public static bool IsHighIntegrity()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }


    }
}
