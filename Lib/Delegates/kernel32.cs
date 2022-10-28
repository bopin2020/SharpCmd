using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using static SharpCmd.Lib.Native.advapi32;
using static SharpCmd.Lib.Native.kernel32;

namespace SharpCmd.Lib.Delegates
{
    public delegate bool GetVersionEx(ref OSVERSIONINFOEX osvi);

    public delegate uint GetSystemDirectory([Out] StringBuilder lpBuffer, uint uSize);

    public delegate bool CloseHandle(IntPtr hHandle);

    public delegate bool FreeConsole();

    public delegate IntPtr GetCurrentThread();

    public delegate bool GetProductInfo(int dwOSMajorVersion,int dwOSMinorVersion,int dwSpMajorVersion,int dwSpMinorVersion,out int pdwReturnedProductType);

    public delegate ulong GetTickCount64();

    public delegate bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

    public delegate bool GetPhysicallyInstalledSystemMemory(out ulong MemoryInKilobytes);

    public delegate int GetLocaleInfoEx(String lpLocaleName, LCTYPE LCType, StringBuilder lpLCData, int cchData);

    public delegate IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);


    public class kernel32
    {
        public static bool GetVersionEx(ref OSVERSIONINFOEX osvi)
        {
            // kernel32 A/W
            GetVersionEx anonymous = MiniDInvoke.GetFunctionPointer<GetVersionEx>("kernel32.dll", "GetVersionExA");
            return anonymous(ref osvi);
        }

        public static uint GetSystemDirectory([Out] StringBuilder lpBuffer, uint uSize)
        {
            GetSystemDirectory anonymous = MiniDInvoke.GetFunctionPointer<GetSystemDirectory>("kernel32.dll", "GetSystemDirectory");
            return anonymous(lpBuffer,uSize);
        }

        public static bool CloseHandle(IntPtr hHandle)
        {
            CloseHandle anonymous = MiniDInvoke.GetFunctionPointer<CloseHandle>("kernel32.dll", "CloseHandle");
            return anonymous(hHandle);
        }

        public static bool FreeConsole()
        {
            FreeConsole anonymous = MiniDInvoke.GetFunctionPointer<FreeConsole>("kernel32.dll", "FreeConsole");
            return anonymous();
        }

        public static IntPtr GetCurrentThread()
        {
            GetCurrentThread anonymous = MiniDInvoke.GetFunctionPointer<GetCurrentThread>("kernel32.dll", "GetCurrentThread");
            return anonymous();
        }

        public static bool GetProductInfo(int dwOSMajorVersion,int dwOSMinorVersion,int dwSpMajorVersion,int dwSpMinorVersion,out int pdwReturnedProductType)
        {
            GetProductInfo anonymous = MiniDInvoke.GetFunctionPointer<GetProductInfo>("kernel32.dll", "GetProductInfo");
            return anonymous(dwOSMajorVersion, dwOSMinorVersion, dwSpMajorVersion, dwSpMinorVersion,out pdwReturnedProductType);
        }

        public static ulong GetTickCount64()
        {
            GetTickCount64 anonymous = MiniDInvoke.GetFunctionPointer<GetTickCount64>("kernel32.dll", "GetTickCount64");
            return anonymous();
        }

        public static bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer)
        {
            GlobalMemoryStatusEx anonymous = MiniDInvoke.GetFunctionPointer<GlobalMemoryStatusEx>("kernel32.dll", "GlobalMemoryStatusEx");
            return anonymous(lpBuffer);
        }

        public static bool GetPhysicallyInstalledSystemMemory(out ulong MemoryInKilobytes)
        {
            GetPhysicallyInstalledSystemMemory anonymous = MiniDInvoke.GetFunctionPointer<GetPhysicallyInstalledSystemMemory>("kernel32.dll", "GetPhysicallyInstalledSystemMemory");
            return anonymous(out MemoryInKilobytes);
        }

        public static int GetLocaleInfoEx(String lpLocaleName, LCTYPE LCType, StringBuilder lpLCData, int cchData)
        {
            GetLocaleInfoEx anonymous = MiniDInvoke.GetFunctionPointer<GetLocaleInfoEx>("kernel32.dll", "GetLocaleInfoEx");
            return anonymous(lpLocaleName, LCType, lpLCData, cchData);
        }

        public static IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName)
        {
            LoadLibrary anonymous = MiniDInvoke.GetFunctionPointer<LoadLibrary>("kernel32.dll", "LoadLibraryA");
            return anonymous(lpFileName);
        }
    }
}
