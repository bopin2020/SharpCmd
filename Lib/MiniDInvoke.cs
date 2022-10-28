using SharpCmd.Lib.Delegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpCmd.Lib
{
    /// <summary>
    /// Dynamic call win32 api using Reflection to hide ImplMapTable (pinvoke import table)
    /// </summary>
    internal sealed class MiniDInvoke
    {
        #region Private Members

        enum SupportVersion
        {
            NET35,
            NET40
        }
#if NET35
        private static SupportVersion supportVersion = SupportVersion.NET35;

#else
        private static SupportVersion supportVersion = SupportVersion.NET40;
#endif

        private static BindingFlags bindingFlags = SetBindingFlags();
        private static string typeFullName = SetTypeFullName();
        private static Assembly assembly = supportVersion == SupportVersion.NET40
                                                        ? Assembly.GetAssembly(typeof(Object)) :
                                                        AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name == "System").FirstOrDefault();
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
                    bindingFlags = BindingFlags.Public;
                    break;
                case SupportVersion.NET40:
                    bindingFlags = BindingFlags.NonPublic;
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
                    typeFullName = "Microsoft.Win32.UnsafeNativeMethods";
                    break;
                case SupportVersion.NET40:
                    typeFullName = "Microsoft.Win32.Win32Native";
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
                if (type == null)
                    type = assembly.GetType(typeFullName);
                if (_GetModuleHandle == null)
                    _GetModuleHandle = type.GetMethod("GetModuleHandle", BindingFlags.Static | bindingFlags);
                IntPtr module = (IntPtr)_GetModuleHandle.Invoke(null, new object[] { dllname });
                // try loadlibrary dll module
                if(module == IntPtr.Zero)
                {
                    module = kernel32.LoadLibrary(dllname);
                }
                return module;
            },
            throwException
            );
        }

        public static IntPtr GetProcAddress(IntPtr dll, string functionname, bool throwException = false)
        {
            return Entry(() =>
            {
                if (type == null)
                    type = assembly.GetType(typeFullName);
                if (_GetProcAddress == null)
                    _GetProcAddress = type.GetMethod("GetProcAddress", BindingFlags.Static | bindingFlags);
                return (IntPtr)_GetProcAddress.Invoke(null, new object[] { dll, functionname });
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
