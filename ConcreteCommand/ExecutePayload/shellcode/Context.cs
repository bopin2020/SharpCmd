using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.ExecutePayload
{
    public class Shellcode
    {
        public byte[] ShellCode { get; }
        public int Length { get; }
        public EncryptApproach Approach { get; }
        public IDictionary<string,object> DecryptArgs { get; }
    }

    public enum EncryptApproach : sbyte
    {
        None,
        Bin,
        Base64,
        Xor,
        RC4,
        AES,
    }

    public class ProcessHandle
    {
        public OpenProcessApproach Approach { get; set; }

        public IntPtr Handle { get; private set; }
    }

    public enum OpenProcessApproach : sbyte
    {
        OpenProcess,
        GetCurrentProcess,
        NtOpenProcess,
    }

    public class AllocVirtualMemory
    {
        public AllocVirtualMemoryApproach Approach { get; set; }
        public IntPtr Address { get; set; }
        public ProcessHandle Handle { get; }
        public Shellcode Shellcode { get; set; }
    }

    public enum AllocVirtualMemoryApproach
    {
        VirtualAllocEx,
        NtAllocateVirtualMemory,
    }

    public class WriteProcessMemory
    {
        public AllocVirtualMemory AllocVirtualMemory { get; set; }
    }

    public enum WriteProcessMemoryApproach
    {
        WriteProcessMemory,
        NtWriteVirtualMemory
    }


    public class Context
    {
        public Shellcode Shellcode { get; private set; }
    }
}
