using SharpCmd.Lib.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpCmd.Lib.Delegates
{
    public delegate byte GetUserNameEx(ExtendedNameFormat nameFormat,StringBuilder userName, ref int userNameSize);

    public class secur32
    {
        public static byte GetUserNameEx(ExtendedNameFormat nameFormat, StringBuilder userName, ref int userNameSize)
        {
            GetUserNameEx anonymous = MiniDInvoke.GetFunctionPointer<GetUserNameEx>("secur32.dll", "GetUserNameExA");
            return anonymous(nameFormat, userName, ref userNameSize);
        }
    }
}
