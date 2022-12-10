using SharpCmd.Lib.Delegates;
using SharpCmd.Lib.Help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Management;

namespace SharpCmd.Lib
{

    internal sealed partial class MiniDInvoke
    {
        class HPInvokeTemplate
        {
            // 判断是.net35 还是.net40
            public int CLRVersion { get; }
            // 指定assembly 最好是已经加载进来的
            public Assembly ThisAssembly { get; }
            // 类的名称
            public string ThisClass { get; }
            // 方法的访问属性
            public BindingFlags ThisMethod_AccessProperties { get; }
            // GetProcAddress方法第一个参数的类型不一定总是 HandleRef
            public bool ThisMethod_ArgsTypeIsHandleRef { get; }

            public HPInvokeTemplate(int cLRVersion, Assembly thisAssembly, string thisClass, BindingFlags thisMethod_AccessProperties, bool thisMethod_ArgsTypeIsHandleRef)
            {
                CLRVersion = cLRVersion;
                ThisAssembly = thisAssembly;
                ThisClass = thisClass;
                ThisMethod_AccessProperties = thisMethod_AccessProperties;
                ThisMethod_ArgsTypeIsHandleRef = thisMethod_ArgsTypeIsHandleRef;
            }
        }

        private static List<HPInvokeTemplate> templates = new List<HPInvokeTemplate>()
        {
#if NET35
            new HPInvokeTemplate(2,AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name.StartsWith("System")).FirstOrDefault(),"Microsoft.Win32.UnsafeNativeMethods",BindingFlags.NonPublic | BindingFlags.Public,true),
            new HPInvokeTemplate(2,Assembly.GetAssembly(typeof(System.Windows.Forms.MessageBox)),"System.Windows.Forms.UnsafeNativeMethods",BindingFlags.NonPublic | BindingFlags.Public,true),
            new HPInvokeTemplate(2,Assembly.GetAssembly(typeof(System.Management.MethodData)),"System.Management.WmiNetUtilsHelper",BindingFlags.NonPublic | BindingFlags.Public,false),

#elif NET40
            new HPInvokeTemplate(4,Assembly.GetAssembly(typeof(object)),"Microsoft.Win32.Win32Native",BindingFlags.NonPublic,false),
            new HPInvokeTemplate(4,Assembly.GetAssembly(typeof(System.Windows.Forms.MessageBox)),"System.Windows.Forms.UnsafeNativeMethods",BindingFlags.Public,true),
            new HPInvokeTemplate(4,AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name.StartsWith("System")).FirstOrDefault(),"Microsoft.Win32.UnsafeNativeMethods",BindingFlags.Public,true),
#endif
        };
    }


    /// <summary>
    /// Dynamic call win32 api using Reflection to hide ImplMapTable (pinvoke import table)
    /// </summary>
    internal sealed partial class MiniDInvoke
    {
#region Private Members

        enum SupportVersion
        {
            NET35,
            NET40,
            DOTNET
        }
#if NET35
        private static SupportVersion supportVersion = SupportVersion.NET35;

#elif NET40
        private static SupportVersion supportVersion = SupportVersion.NET40;

#else
        private static SupportVersion supportVersion = SupportVersion.NET40;
#endif

        // 这里通过索引切换要使用哪一套调用链
        private const int net35 = 2;
        private const int net40 = 2;


        private static BindingFlags bindingFlags = SetBindingFlags();
        private static string typeFullName = SetTypeFullName();
        private static Assembly assembly = supportVersion == SupportVersion.NET40 ? templates[net40].ThisAssembly : templates[net35].ThisAssembly;


        private static Type type;

        private static MethodInfo _GetModuleHandle;
        private static MethodInfo _GetProcAddress;

#endregion


        public static Exception LastException { get; private set; }


#region Private Static Methods
                private static BindingFlags SetBindingFlags()
                {
                    switch (supportVersion)
                    {
                        case SupportVersion.NET35:
                            bindingFlags = templates[net35].ThisMethod_AccessProperties;
                            break;
                        case SupportVersion.NET40:
                            bindingFlags = templates[net40].ThisMethod_AccessProperties;
                            break;
                        default:
                            break;
                    }
                    return bindingFlags;
                }

                private static string SetTypeFullName()
                {
                    switch (supportVersion)
                    {
                        case SupportVersion.NET35:
                            typeFullName = templates[net35].ThisClass;
                            break;
                        case SupportVersion.NET40:
                            typeFullName = templates[net40].ThisClass;
                             break;
                        case SupportVersion.DOTNET:
                            typeFullName = "System.Runtime.InteropServices";
                            break;
                        default:
                            break;
                    }
                    return typeFullName;
                }
#endregion


#region Public Static Methods

                public static IntPtr GetModuleHandle(string dllname, bool throwException = false)
                {
                    return Entry(() =>
                    {
#if DOTNET
                        if(NativeLibrary.TryLoad(dllname,out IntPtr handle))
                        {
                            return handle;
                        }
                        else
                        {
                            throw new Exception();
                        }

                
#else
                        if (type == null)
                            type = assembly.GetType(typeFullName);
                        if (_GetModuleHandle == null)
                            _GetModuleHandle = type.GetMethod("GetModuleHandle", BindingFlags.Static | bindingFlags);
                        IntPtr module = (IntPtr)_GetModuleHandle.Invoke(null, new object[] { dllname });
                        // try loadlibrary dll module
                        if(module == IntPtr.Zero)
                        {
                            module = kernel32.LoadLibrary(dllname);
                            Console.WriteLine(Constant.InitDll + dllname + "\tat:" + module.ToString("x"));
                            return module;
                        }
                        return (IntPtr)_GetModuleHandle.Invoke(null, new object[] { dllname });
#endif
                    },
                    throwException
                    );
                }

                public static IntPtr GetProcAddress(IntPtr dll, string functionname, bool throwException = false)
                {
                    return Entry(() =>
                    {
#if DOTNET
                        if(NativeLibrary.TryGetExport(dll,functionname,out IntPtr address))
                        {
                            return address;
                        }
                        else
                        {
                            throw new Exception();
                        }
#else

#if 反射查找Win32API
                        foreach (var item in assembly.GetTypes())
                        {
                            Console.WriteLine(item.FullName);
                            if (item.FullName == "Microsoft.Win32.Win32Native")
                            {

                            }
                            if (item.FullName == "Microsoft.Win32.UnsafeNativeMethods")
                            {

                            }

                            foreach (var method in item.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                            {
                                if (method.Name.Contains("GetProcAddress"))
                                {
                                    Console.WriteLine(item.FullName + "\t" + method.Name + "\t" + method);
                                }
                                if (method.Name.Contains("GetModuleHandle"))
                                {
                                    Console.WriteLine(item.FullName + "\t" + method.Name + "\t" + method.ToString());
                                }
                                if (method.Name.Contains("LoadLibrary"))
                                {
                                    Console.WriteLine(item.FullName + "\t" + method.Name + "\t" + method.ToString());
                                }
                            }

                        }
#endif

                        if (type == null)
                            type = assembly.GetType(typeFullName);
                        if(type == null)
                        {
                            throw new Exception(typeFullName + "was not found" + assembly.FullName);
                        }
                        if (_GetProcAddress == null)
                        {
#if NET40
                            if (templates[net40].ThisMethod_ArgsTypeIsHandleRef)
                            {
                                _GetProcAddress = type.GetMethod("GetProcAddress", BindingFlags.Static | bindingFlags,null,new Type[] { typeof(HandleRef),typeof(string) },default);
                            }
                            else
                            {
                                _GetProcAddress = type.GetMethod("GetProcAddress", BindingFlags.Static | bindingFlags);
                            }
#endif
                        }

#if NET35
                        //  .NET3.5 GetProcAddress 第一个方法参数类型不是 IntPtr  而是HandleRef 结构体
                        object arg1 = default;
                        if (!templates[net35].ThisMethod_ArgsTypeIsHandleRef)
                        {

                            arg1 = dll;
                        }
                        else
                        {
                            arg1 = new HandleRef(null, dll);
                        }

#elif NET40
                        object arg1 = default;
                        if (!templates[net40].ThisMethod_ArgsTypeIsHandleRef)
                        {

                            arg1 = dll;

                        }
                        else
                        {
                            arg1 = new HandleRef(null, dll);
                        }
#endif
                        return (IntPtr)_GetProcAddress.Invoke(null, new object[] { arg1, functionname });

#endif
                    },
                    throwException
                    );
                }

                public static IntPtr GetProcAddress(string dllname, string functionname, bool throwException = false)
                {
                    return GetProcAddress(GetModuleHandle(dllname), functionname, throwException);
                }

                public static string GetLastErrorMessage(uint ErrorMessageLevel = 0)
                {
                    if (LastException != null)
                    {
                        switch (ErrorMessageLevel)
                        {
                            case 0:
                                return LastException.Message;
                            case 1:
                                return LastException.StackTrace;
                            default:
                                break;
                        }
                    }
                    throw new Exception();
                }
                public static IntPtr Entry(Func<IntPtr> func, bool throwException = false)
                {
                    try
                    {
                        return func();
                    }
                    catch (Exception ex)
                    {
                        if (!throwException)
                            LastException = ex;
                        else
                            throw ex;
                    }
                    return IntPtr.Zero;
                }

                public static T GetFunctionPointer<T>(string dllname, string functionname, bool throwException = false) where T : Delegate
                {
                    IntPtr address = GetProcAddress(dllname, functionname, throwException);
                    if(address == IntPtr.Zero)
                    {
                        throw new Exception("0x00000000 Invalid address");
                    }
                    return GetFunctionPointer<T>(address);
                }

                public static T GetFunctionPointer<T>(IntPtr address) where T : Delegate
                {
                    return Marshal.GetDelegateForFunctionPointer(address, typeof(T)) as T;
                }
#endregion
    }
}
