using SharpCmd.Contract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static SharpCmd.Lib.Native.netapi32;

namespace SharpCmd.ConcreteCommand.Recon
{
    internal class net : IContract
    {
        public string CommandName => "net";

        public void Execute(Dictionary<string, string> arguments)
        {
            if (arguments.ContainsKey("accounts"))
            {
                //IntPtr lBuffer = IntPtr.Zero;
                //int lRead;
                //int lTotal;
                //IntPtr lHandle = IntPtr.Zero;

                //NET_API_STATUS status = NetUseEnum(null, 2, ref lBuffer, 0xffffffff, out lRead, out lTotal, lHandle);

                //// now step through all network shares and check if we have already a connection to the server
                //int li = 0;
                //USE_INFO_2 lInfo;
                //while (li < lRead)
                //{
                //    IntPtr ptr = IntPtr.Add(lBuffer, Marshal.SizeOf(typeof(USE_INFO_2)) * li);
                //    // lInfo=(USE_INFO_2)Marshal.PtrToStructure(new IntPtr(lBuffer.ToInt32()+(Marshal.SizeOf(typeof(USE_INFO_2))*li)),typeof(USE_INFO_2));
                //    // previous line has been causing "arithmetic operation resulted in an overflow" exception on x86 systems for me. Fixed with the .Add method.
                //    lInfo = (USE_INFO_2)Marshal.PtrToStructure(ptr, typeof(USE_INFO_2));

                //    //if (lInfo.ui2_remote.StartsWith(lUNCPath))
                //    //{
                //    //    lBack = true;
                //    //    break;
                //    //}
                //    ++li;
                //}

                //NetApiBufferFree(lBuffer);
                //return;
            }
        }
    }

    public class net_Accounts
    {

    }
}
