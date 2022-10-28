using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpCmd.Lib.Native
{
    public class kernel32
    {
        [DllImport("kernel32")]
        public static extern bool GetVersionEx(ref OSVERSIONINFOEX osvi);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct OSVERSIONINFOEX
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
            public UInt16 wServicePackMajor;
            public UInt16 wServicePackMinor;
            public UInt16 wSuiteMask;
            public byte wProductType;
            public byte wReserved;
        }

        [DllImport("kernel32.dll")]
        public static extern uint GetSystemDirectory([Out] StringBuilder lpBuffer, uint uSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hHandle);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern bool FreeConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetCurrentThread();

        [DllImport("kernel32.dll", SetLastError = false)]
        public static extern bool GetProductInfo(
             int dwOSMajorVersion,
             int dwOSMinorVersion,
             int dwSpMajorVersion,
             int dwSpMinorVersion,
             out int pdwReturnedProductType
            );
        public static void InstallationType()
        {
            int ProductNum;

            GetProductInfo(
             Environment.OSVersion.Version.Major,
             Environment.OSVersion.Version.Minor,
             0,
             0,
             out ProductNum);
        }
        // https://learn.microsoft.com/en-us/windows/win32/api/sysinfoapi/nf-sysinfoapi-getproductinfo?redirectedfrom=MSDN
        public enum InstallVersion
        {
            PRODUCT_BUSINESS = 0x06,
            PRODUCT_BUSINESS_N = 0x10,
            PRODUCT_CLUSTER_SERVER = 0x12,
            PRODUCT_PROFESSIONAL = 0x30,
        }


        // ================  sysinfoapi.h
        [DllImport("kernel32.dll")]
        public static extern ulong GetTickCount64();

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MEMORYSTATUSEX
        {
            /// <summary>
            /// Size of the structure, in bytes. You must set this member before calling GlobalMemoryStatusEx.
            /// </summary>
            public uint dwLength;

            /// <summary>
            /// Number between 0 and 100 that specifies the approximate percentage of physical memory that is in use (0 indicates no memory use and 100 indicates full memory use).
            /// </summary>
            public uint dwMemoryLoad;

            /// <summary>
            /// Total size of physical memory, in bytes.
            /// </summary>
            public ulong ullTotalPhys;

            /// <summary>
            /// Size of physical memory available, in bytes.
            /// </summary>
            public ulong ullAvailPhys;

            /// <summary>
            /// Size of the committed memory limit, in bytes. This is physical memory plus the size of the page file, minus a small overhead.
            /// </summary>
            public ulong ullTotalPageFile;
            /// <summary>
            /// Size of available memory to commit, in bytes. The limit is ullTotalPageFile.
            /// </summary>
            public ulong ullAvailPageFile;

            /// <summary>
            /// Total size of the user mode portion of the virtual address space of the calling process, in bytes.
            /// </summary>
            public ulong ullTotalVirtual;

            /// <summary>
            /// Size of unreserved and uncommitted memory in the user mode portion of the virtual address space of the calling process, in bytes.
            /// </summary>
            public ulong ullAvailVirtual;

            /// <summary>
            /// Size of unreserved and uncommitted memory in the extended portion of the virtual address space of the calling process, in bytes.
            /// </summary>
            public ulong ullAvailExtendedVirtual;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:MEMORYSTATUSEX"/> class.
            /// </summary>
            public MEMORYSTATUSEX()
            {
                this.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPhysicallyInstalledSystemMemory(out ulong MemoryInKilobytes);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int GetLocaleInfoEx(String lpLocaleName, LCTYPE LCType, StringBuilder lpLCData, int cchData);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

        public enum LCTYPE : uint
        {
            LOCALE_NOUSEROVERRIDE = 0x80000000,   // do not use user overrides
            LOCALE_RETURN_NUMBER = 0x20000000,   // return number instead of string

            // Modifier for genitive names
            LOCALE_RETURN_GENITIVE_NAMES = 0x10000000,   //Flag to return the Genitive forms of month names

            //
            //  The following LCTypes are mutually exclusive in that they may NOT
            //  be used in combination with each other.
            //

            //
            // These are the various forms of the name of the locale:
            //
            LOCALE_SLOCALIZEDDISPLAYNAME = 0x00000002,   // localized name of locale, eg "German (Germany)" in UI language
            LOCALE_SENGLISHDISPLAYNAME = 0x00000072,   // Display name (language + country usually) in English, eg "German (Germany)"
            LOCALE_SNATIVEDISPLAYNAME = 0x00000073,   // Display name in native locale language, eg "Deutsch (Deutschland)

            LOCALE_SLOCALIZEDLANGUAGENAME = 0x0000006f,   // Language Display Name for a language, eg "German" in UI language
            LOCALE_SENGLISHLANGUAGENAME = 0x00001001,   // English name of language, eg "German"
            LOCALE_SNATIVELANGUAGENAME = 0x00000004,   // native name of language, eg "Deutsch"

            LOCALE_SLOCALIZEDCOUNTRYNAME = 0x00000006,   // localized name of country, eg "Germany" in UI language
            LOCALE_SENGLISHCOUNTRYNAME = 0x00001002,   // English name of country, eg "Germany"
            LOCALE_SNATIVECOUNTRYNAME = 0x00000008,   // native name of country, eg "Deutschland"

            // Additional LCTYPEs
            LOCALE_SABBREVLANGNAME = 0x00000003,   // abbreviated language name

            LOCALE_ICOUNTRY = 0x00000005,   // country code
            LOCALE_SABBREVCTRYNAME = 0x00000007,   // abbreviated country name
            LOCALE_IGEOID = 0x0000005B,   // geographical location id

            LOCALE_IDEFAULTLANGUAGE = 0x00000009,   // default language id
            LOCALE_IDEFAULTCOUNTRY = 0x0000000A,   // default country code
            LOCALE_IDEFAULTCODEPAGE = 0x0000000B,   // default oem code page
            LOCALE_IDEFAULTANSICODEPAGE = 0x00001004,   // default ansi code page
            LOCALE_IDEFAULTMACCODEPAGE = 0x00001011,   // default mac code page

            LOCALE_SLIST = 0x0000000C,   // list item separator
            LOCALE_IMEASURE = 0x0000000D,   // 0 = metric, 1 = US

            LOCALE_SDECIMAL = 0x0000000E,   // decimal separator
            LOCALE_STHOUSAND = 0x0000000F,   // thousand separator
            LOCALE_SGROUPING = 0x00000010,   // digit grouping
            LOCALE_IDIGITS = 0x00000011,   // number of fractional digits
            LOCALE_ILZERO = 0x00000012,   // leading zeros for decimal
            LOCALE_INEGNUMBER = 0x00001010,   // negative number mode
            LOCALE_SNATIVEDIGITS = 0x00000013,   // native digits for 0-9

            LOCALE_SCURRENCY = 0x00000014,   // local monetary symbol
            LOCALE_SINTLSYMBOL = 0x00000015,   // uintl monetary symbol
            LOCALE_SMONDECIMALSEP = 0x00000016,   // monetary decimal separator
            LOCALE_SMONTHOUSANDSEP = 0x00000017,   // monetary thousand separator
            LOCALE_SMONGROUPING = 0x00000018,   // monetary grouping
            LOCALE_ICURRDIGITS = 0x00000019,   // # local monetary digits
            LOCALE_IINTLCURRDIGITS = 0x0000001A,   // # uintl monetary digits
            LOCALE_ICURRENCY = 0x0000001B,   // positive currency mode
            LOCALE_INEGCURR = 0x0000001C,   // negative currency mode

            LOCALE_SDATE = 0x0000001D,   // date separator (derived from LOCALE_SSHORTDATE, use that instead)
            LOCALE_STIME = 0x0000001E,   // time separator (derived from LOCALE_STIMEFORMAT, use that instead)
            LOCALE_SSHORTDATE = 0x0000001F,   // short date format string
            LOCALE_SLONGDATE = 0x00000020,   // long date format string
            LOCALE_STIMEFORMAT = 0x00001003,   // time format string
            LOCALE_IDATE = 0x00000021,   // short date format ordering (derived from LOCALE_SSHORTDATE, use that instead)
            LOCALE_ILDATE = 0x00000022,   // long date format ordering (derived from LOCALE_SLONGDATE, use that instead)
            LOCALE_ITIME = 0x00000023,   // time format specifier (derived from LOCALE_STIMEFORMAT, use that instead)
            LOCALE_ITIMEMARKPOSN = 0x00001005,   // time marker position (derived from LOCALE_STIMEFORMAT, use that instead)
            LOCALE_ICENTURY = 0x00000024,   // century format specifier (short date, LOCALE_SSHORTDATE is preferred)
            LOCALE_ITLZERO = 0x00000025,   // leading zeros in time field (derived from LOCALE_STIMEFORMAT, use that instead)
            LOCALE_IDAYLZERO = 0x00000026,   // leading zeros in day field (short date, LOCALE_SSHORTDATE is preferred)
            LOCALE_IMONLZERO = 0x00000027,   // leading zeros in month field (short date, LOCALE_SSHORTDATE is preferred)
            LOCALE_S1159 = 0x00000028,   // AM designator
            LOCALE_S2359 = 0x00000029,   // PM designator

            LOCALE_ICALENDARTYPE = 0x00001009,   // type of calendar specifier
            LOCALE_IOPTIONALCALENDAR = 0x0000100B,   // additional calendar types specifier
            LOCALE_IFIRSTDAYOFWEEK = 0x0000100C,   // first day of week specifier
            LOCALE_IFIRSTWEEKOFYEAR = 0x0000100D,   // first week of year specifier

            LOCALE_SDAYNAME1 = 0x0000002A,   // long name for Monday
            LOCALE_SDAYNAME2 = 0x0000002B,   // long name for Tuesday
            LOCALE_SDAYNAME3 = 0x0000002C,   // long name for Wednesday
            LOCALE_SDAYNAME4 = 0x0000002D,   // long name for Thursday
            LOCALE_SDAYNAME5 = 0x0000002E,   // long name for Friday
            LOCALE_SDAYNAME6 = 0x0000002F,   // long name for Saturday
            LOCALE_SDAYNAME7 = 0x00000030,   // long name for Sunday
            LOCALE_SABBREVDAYNAME1 = 0x00000031,   // abbreviated name for Monday
            LOCALE_SABBREVDAYNAME2 = 0x00000032,   // abbreviated name for Tuesday
            LOCALE_SABBREVDAYNAME3 = 0x00000033,   // abbreviated name for Wednesday
            LOCALE_SABBREVDAYNAME4 = 0x00000034,   // abbreviated name for Thursday
            LOCALE_SABBREVDAYNAME5 = 0x00000035,   // abbreviated name for Friday
            LOCALE_SABBREVDAYNAME6 = 0x00000036,   // abbreviated name for Saturday
            LOCALE_SABBREVDAYNAME7 = 0x00000037,   // abbreviated name for Sunday
            LOCALE_SMONTHNAME1 = 0x00000038,   // long name for January
            LOCALE_SMONTHNAME2 = 0x00000039,   // long name for February
            LOCALE_SMONTHNAME3 = 0x0000003A,   // long name for March
            LOCALE_SMONTHNAME4 = 0x0000003B,   // long name for April
            LOCALE_SMONTHNAME5 = 0x0000003C,   // long name for May
            LOCALE_SMONTHNAME6 = 0x0000003D,   // long name for June
            LOCALE_SMONTHNAME7 = 0x0000003E,   // long name for July
            LOCALE_SMONTHNAME8 = 0x0000003F,   // long name for August
            LOCALE_SMONTHNAME9 = 0x00000040,   // long name for September
            LOCALE_SMONTHNAME10 = 0x00000041,   // long name for October
            LOCALE_SMONTHNAME11 = 0x00000042,   // long name for November
            LOCALE_SMONTHNAME12 = 0x00000043,   // long name for December
            LOCALE_SMONTHNAME13 = 0x0000100E,   // long name for 13th month (if exists)
            LOCALE_SABBREVMONTHNAME1 = 0x00000044,   // abbreviated name for January
            LOCALE_SABBREVMONTHNAME2 = 0x00000045,   // abbreviated name for February
            LOCALE_SABBREVMONTHNAME3 = 0x00000046,   // abbreviated name for March
            LOCALE_SABBREVMONTHNAME4 = 0x00000047,   // abbreviated name for April
            LOCALE_SABBREVMONTHNAME5 = 0x00000048,   // abbreviated name for May
            LOCALE_SABBREVMONTHNAME6 = 0x00000049,   // abbreviated name for June
            LOCALE_SABBREVMONTHNAME7 = 0x0000004A,   // abbreviated name for July
            LOCALE_SABBREVMONTHNAME8 = 0x0000004B,   // abbreviated name for August
            LOCALE_SABBREVMONTHNAME9 = 0x0000004C,   // abbreviated name for September
            LOCALE_SABBREVMONTHNAME10 = 0x0000004D,   // abbreviated name for October
            LOCALE_SABBREVMONTHNAME11 = 0x0000004E,   // abbreviated name for November
            LOCALE_SABBREVMONTHNAME12 = 0x0000004F,   // abbreviated name for December
            LOCALE_SABBREVMONTHNAME13 = 0x0000100F,   // abbreviated name for 13th month (if exists)

            LOCALE_SPOSITIVESIGN = 0x00000050,   // positive sign
            LOCALE_SNEGATIVESIGN = 0x00000051,   // negative sign
            LOCALE_IPOSSIGNPOSN = 0x00000052,   // positive sign position (derived from INEGCURR)
            LOCALE_INEGSIGNPOSN = 0x00000053,   // negative sign position (derived from INEGCURR)
            LOCALE_IPOSSYMPRECEDES = 0x00000054,   // mon sym precedes pos amt (derived from ICURRENCY)
            LOCALE_IPOSSEPBYSPACE = 0x00000055,   // mon sym sep by space from pos amt (derived from ICURRENCY)
            LOCALE_INEGSYMPRECEDES = 0x00000056,   // mon sym precedes neg amt (derived from INEGCURR)
            LOCALE_INEGSEPBYSPACE = 0x00000057,   // mon sym sep by space from neg amt (derived from INEGCURR)

            LOCALE_FONTSIGNATURE = 0x00000058,   // font signature
            LOCALE_SISO639LANGNAME = 0x00000059,   // ISO abbreviated language name
            LOCALE_SISO3166CTRYNAME = 0x0000005A,   // ISO abbreviated country name

            LOCALE_IDEFAULTEBCDICCODEPAGE = 0x00001012,   // default ebcdic code page
            LOCALE_IPAPERSIZE = 0x0000100A,   // 1 = letter, 5 = legal, 8 = a3, 9 = a4
            LOCALE_SENGCURRNAME = 0x00001007,   // english name of currency
            LOCALE_SNATIVECURRNAME = 0x00001008,   // native name of currency
            LOCALE_SYEARMONTH = 0x00001006,   // year month format string
            LOCALE_SSORTNAME = 0x00001013,   // sort name
            LOCALE_IDIGITSUBSTITUTION = 0x00001014,   // 0 = context, 1 = none, 2 = national

            LOCALE_SNAME = 0x0000005c,   // locale name (with sort info) (ie: de-DE_phoneb)
            LOCALE_SDURATION = 0x0000005d,   // time duration format
            LOCALE_SKEYBOARDSTOINSTALL = 0x0000005e,   // (windows only) keyboards to install
            LOCALE_SSHORTESTDAYNAME1 = 0x00000060,   // Shortest day name for Monday
            LOCALE_SSHORTESTDAYNAME2 = 0x00000061,   // Shortest day name for Tuesday
            LOCALE_SSHORTESTDAYNAME3 = 0x00000062,   // Shortest day name for Wednesday
            LOCALE_SSHORTESTDAYNAME4 = 0x00000063,   // Shortest day name for Thursday
            LOCALE_SSHORTESTDAYNAME5 = 0x00000064,   // Shortest day name for Friday
            LOCALE_SSHORTESTDAYNAME6 = 0x00000065,   // Shortest day name for Saturday
            LOCALE_SSHORTESTDAYNAME7 = 0x00000066,   // Shortest day name for Sunday
            LOCALE_SISO639LANGNAME2 = 0x00000067,   // 3 character ISO abbreviated language name
            LOCALE_SISO3166CTRYNAME2 = 0x00000068,   // 3 character ISO country name
            LOCALE_SNAN = 0x00000069,   // Not a Number
            LOCALE_SPOSINFINITY = 0x0000006a,   // + Infinity
            LOCALE_SNEGINFINITY = 0x0000006b,   // - Infinity
            LOCALE_SSCRIPTS = 0x0000006c,   // Typical scripts in the locale
            LOCALE_SPARENT = 0x0000006d,   // Fallback name for resources
            LOCALE_SCONSOLEFALLBACKNAME = 0x0000006e,   // Fallback name for within the console
        }
    }
}
