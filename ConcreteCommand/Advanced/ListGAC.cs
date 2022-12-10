using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpCmd.ConcreteCommand.Advanced
{
    using GacWithFusion;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// https://stackoverflow.com/questions/1599575/enumerating-assemblies-in-gac
    /// list gac
    /// </summary>
    internal partial class ListGAC : AdvancedBase
    { 
        public override string CommandName => "listgac";

        public override string Description => "enumerate gac assembly -> types -> methods about GetProcAddress and GetModuleHandle";

        public override string CommandHelp => "listgac";

        public override void Execute(Dictionary<string, string> arguments)
        {
            // C:\Windows\Microsoft.NET\Framework\v2.0.50727
            // C:\Windows\Microsoft.NET\Framework64\v4.0.30319
            var mm = Directory.GetFiles(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319", "*.dll",SearchOption.AllDirectories);

            foreach (var assemblyName in mm) // GetGacAssemblyFullNames()
            {
                try
                {
                    string frontPath = @"C:\Windows\Microsoft.NET\Framework\v2.0.50727\";
                    Assembly assembly = Assembly.LoadFrom(assemblyName);



                    foreach (var item in assembly.GetTypes())
                    {
                        foreach (var method in item.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                        {
                            if (method.Name.Contains("GetProcAddress"))
                            {
                                Console.WriteLine(item.FullName + "\t" + assemblyName + "\t" + method);
                            }
                            if (method.Name.Contains("GetModuleHandle"))
                            {
                                Console.WriteLine(item.FullName + "\t" + assemblyName + "\t" + method.ToString());
                            }
                            if (method.Name.Contains("LoadLibrary"))
                            {
                                Console.WriteLine(item.FullName + "\t" + assemblyName + "\t" + method.ToString());
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                }

            }
        }
    }


    internal partial class ListGAC
    {
        private IEnumerable<AssemblyName> GetGacAssemblyFullNames()
        {
            IApplicationContext applicationContext;
            IAssemblyEnum assemblyEnum;
            IAssemblyName assemblyName;

            Fusion.CreateAssemblyEnum(out assemblyEnum, null, null, 2, 0);
            while (assemblyEnum.GetNextAssembly(out applicationContext, out assemblyName, 0) == 0)
            {
                uint nChars = 0;
                assemblyName.GetDisplayName(null, ref nChars, 0);

                StringBuilder name = new StringBuilder((int)nChars);
                assemblyName.GetDisplayName(name, ref nChars, 0);

                AssemblyName a = null;
                try
                {
                    a = new AssemblyName(name.ToString());
                }
                catch (Exception)
                {
                }

                if (a != null)
                {
                    // 迭代器返回
                    yield return a;
                }
            }
        }

    }
}

namespace GacWithFusion
{
    // .NET Fusion COM interfaces
    [ComImport, Guid("CD193BC0-B4BC-11D2-9833-00C04FC31D2E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAssemblyName
    {
        [PreserveSig]
        int SetProperty(uint PropertyId, IntPtr pvProperty, uint cbProperty);

        [PreserveSig]
        int GetProperty(uint PropertyId, IntPtr pvProperty, ref uint pcbProperty);

        [PreserveSig]
        int Finalize();

        [PreserveSig]
        int GetDisplayName([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder szDisplayName,
                           ref uint pccDisplayName,
                           uint dwDisplayFlags);

        [PreserveSig]
        int BindToObject(object refIID,
                         object pAsmBindSink,
                         IApplicationContext pApplicationContext,
                         [MarshalAs(UnmanagedType.LPWStr)] string szCodeBase,
                         long llFlags,
                         int pvReserved,
                         uint cbReserved,
                         out int ppv);

        [PreserveSig]
        int GetName(ref uint lpcwBuffer,
                    [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwzName);

        [PreserveSig]
        int GetVersion(out uint pdwVersionHi, out uint pdwVersionLow);

        [PreserveSig]
        int IsEqual(IAssemblyName pName,
                    uint dwCmpFlags);

        [PreserveSig]
        int Clone(out IAssemblyName pName);
    }

    [ComImport(), Guid("7C23FF90-33AF-11D3-95DA-00A024A85B51"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IApplicationContext
    {
        void SetContextNameObject(IAssemblyName pName);

        void GetContextNameObject(out IAssemblyName ppName);

        void Set([MarshalAs(UnmanagedType.LPWStr)] string szName,
                 int pvValue,
                 uint cbValue,
                 uint dwFlags);

        void Get([MarshalAs(UnmanagedType.LPWStr)] string szName,
                 out int pvValue,
                 ref uint pcbValue,
                 uint dwFlags);

        void GetDynamicDirectory(out int wzDynamicDir,
                                 ref uint pdwSize);
    }

    [ComImport(), Guid("21B8916C-F28E-11D2-A473-00C04F8EF448"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAssemblyEnum
    {
        [PreserveSig]
        int GetNextAssembly(out IApplicationContext ppAppCtx,
                            out IAssemblyName ppName,
                            uint dwFlags);

        [PreserveSig]
        int Reset();

        [PreserveSig]
        int Clone(out IAssemblyEnum ppEnum);
    }

    internal static class Fusion
    {
        // dwFlags: 1 = Enumerate native image (NGEN) assemblies
        //          2 = Enumerate GAC assemblies
        //          4 = Enumerate Downloaded assemblies
        //
        [DllImport("fusion.dll", CharSet = CharSet.Auto)]
        internal static extern int CreateAssemblyEnum(out IAssemblyEnum ppEnum,
                                                      IApplicationContext pAppCtx,
                                                      IAssemblyName pName,
                                                      uint dwFlags,
                                                      int pvReserved);
    }
}
