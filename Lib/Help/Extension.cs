using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.Lib.Help
{
    public static class Win32APIExtension
    {
        public static string ExceptionMessage(this int errorcode)
        {
            return errorcode + Constant.T + new System.ComponentModel.Win32Exception(errorcode).Message;
        }
    }
}
