using Microsoft.Win32;
using SharpCmd.Contract;
using SharpCmd.Lib.Help;
using SharpCmd.Lib.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

using static SharpCmd.Lib.Native.IpHlpApi;

namespace SharpCmd.ConcreteCommand.Recon
{
    internal partial class arp : ReconBase
    {
        public override string CommandName => "arp";

        public override string Description => "collect arp information";

        public override string CommandHelp => "arp /?";

        public override void Execute(Dictionary<string, string> arguments)
        {
            // The number of bytes needed.
            int bytesNeeded = 0;

            // The result from the API call.
            int result = GetIpNetTable(IntPtr.Zero, ref bytesNeeded, false);

            // Call the function, expecting an insufficient buffer.
            if (result != ERROR_INSUFFICIENT_BUFFER)
            {
                // Throw an exception.
                throw new Win32Exception(result);
            }

            // Allocate the memory, do it in a try/finally block, to ensure
            // that it is released.
            IntPtr buffer = IntPtr.Zero;

            // Try/finally.
            try
            {
                // Allocate the memory.
                buffer = Marshal.AllocCoTaskMem(bytesNeeded);

                // Make the call again. If it did not succeed, then
                // raise an error.
                result = GetIpNetTable(buffer, ref bytesNeeded, false);

                // If the result is not 0 (no error), then throw an exception.
                if (result != 0)
                {
                    // Throw an exception.
                    throw new Win32Exception(result);
                }

                // Now we have the buffer, we have to marshal it. We can read
                // the first 4 bytes to get the length of the buffer.
                int entries = Marshal.ReadInt32(buffer);

                // Increment the memory pointer by the size of the int.
                IntPtr currentBuffer = new IntPtr(buffer.ToInt64() +
                   Marshal.SizeOf(typeof(int)));

                // Allocate an array of entries.
                MIB_IPNETROW[] table = new MIB_IPNETROW[entries];

                // Cycle through the entries.
                for (int index = 0; index < entries; index++)
                {
                    // Call PtrToStructure, getting the structure information.
                    table[index] = (MIB_IPNETROW)Marshal.PtrToStructure(new
                       IntPtr(currentBuffer.ToInt64() + (index *
                       Marshal.SizeOf(typeof(MIB_IPNETROW)))), typeof(MIB_IPNETROW));
                }
                Console.WriteLine("IP  \t\tMAC");
                for (int index = 0; index < entries; index++)
                {
                    MIB_IPNETROW row = table[index];
                    IPAddress ip = new IPAddress(BitConverter.GetBytes(row.dwAddr));
                    Console.Write(ip.ToString() + "\t\t");

                    Console.Write(row.mac0.ToString("X2") + '-');
                    Console.Write(row.mac1.ToString("X2") + '-');
                    Console.Write(row.mac2.ToString("X2") + '-');
                    Console.Write(row.mac3.ToString("X2") + '-');
                    Console.Write(row.mac4.ToString("X2") + '-');
                    Console.WriteLine(row.mac5.ToString("X2"));

                }
            }
            finally
            {
                // Release the memory.
                FreeMibTable(buffer);
            }
        }
    }
}
