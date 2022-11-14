using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static SharpCmd.Lib.Native.kernel32;

namespace SharpCmd.Lib.Delegates
{
    public delegate int MessageBox(IntPtr hWnd, String text, String caption, uint type);

    public class user32
    {
        public static int MessageBox(IntPtr hWnd, String text, String caption, uint type)
        {
            MessageBox anonymous = MiniDInvoke.GetFunctionPointer<MessageBox>("user32.dll", "MessageBoxA",true);
            return anonymous(hWnd,text,caption,type);
        }
    }
}
