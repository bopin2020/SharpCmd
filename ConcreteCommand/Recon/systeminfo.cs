using SharpCmd.Contract;
using SharpCmd.Lib.Help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using static SharpCmd.Lib.Native.kernel32;

namespace SharpCmd.ConcreteCommand.Recon
{
    internal partial class systeminfo : IContract
    {
        public string CommandName => "systeminfo";

        /// <summary>
        /// https://github.com/tlewiscpp/SystemInfo/blob/master/src/osinfo.cpp
        /// </summary>
        /// <param name="arguments"></param>
        public void Execute(Dictionary<string, string> arguments)
        {
            systeminfoModel systeminfoModel = new systeminfoModel();
            systeminfoModel.MachineName = GetMachineName();
            systeminfoModel.WinOSName = GetWindowsOSName();
            systeminfoModel.WinOSVersion = GetWinOSVersion();
            systeminfoModel.WinOSManufacture = GetWinOSManufacture();
            systeminfoModel.WinOSConfiguration = GetWinOSConfiguration();
            systeminfoModel.WinOSBuildType = regInfo.CurrentKey.GetValue("CurrentType").ToString();
            systeminfoModel.RegisterOwner = regInfo.CurrentKey.GetValue("RegisteredOwner").ToString();
            systeminfoModel.RegisterOrgranization = regInfo.CurrentKey.GetValue("RegisteredOrganization").ToString();
            systeminfoModel.ProductionID = GetProductionID();
            systeminfoModel.InitializationInstalledDateTime = DateTime.FromFileTime(Convert.ToInt64(regInfo.CurrentKey.GetValue("InstallTime"))).ToString();
            systeminfoModel.WinPowerOnDateTime = DateTime.FromFileTimeUtc((long)GetTickCount64()).ToString();
            systeminfoModel.WinDirectory = regInfo.CurrentKey.GetValue("SystemRoot").ToString();
            systeminfoModel.BIOSVersion = GetBIOSVersion();
            systeminfoModel.CPUManufacture = GetCPUManufacture();
            systeminfoModel.CPUFlag = GetCPUFlag();
            systeminfoModel.Arch = GetArch();
            systeminfoModel.CPU = GetCPU();
            systeminfoModel.WinSysDirectory = GetSysDirectory();
            systeminfoModel.VPBDeviceObject = GetVPBDeviceObject();
            systeminfoModel.AreaSetting = GetAreaSetting();
            systeminfoModel.InputLanguageAreaSetting = GetInputLanguageAreaSetting();
            systeminfoModel.TimeZone = GetTimeZone();
            systeminfoModel.PhysicalMemoryTotally = GetPhysicalMemoryTotally();
            systeminfoModel.PhysicalMemoryFree = GetPhysicalMemoryFree();
            systeminfoModel.VirtualMemoryMax = GetVirtualMemoryMax();
            systeminfoModel.VirtualMemoryFree = GetVirtualMemoryFree();
            systeminfoModel.VirtualMemoryUsing = GetVirtualMemoryUsing();
            systeminfoModel.PageFileName = GetPageFileName();
            systeminfoModel.Domain = GetDomain();
            systeminfoModel.LogonServer = "\\\\" + GetMachineName();
            systeminfoModel.HotFixes = new string[] { "" };
            systeminfoModel.NetworkCards = new NetworkCard[] { };

            Console.WriteLine("\n\n");
            foreach (var item in systeminfoModel.GetType().GetProperties())
            {
                if(item.Name == "CPU")
                {
                    string[] tmp = item.GetValue(systeminfoModel, null) as string[]; 
                    Console.WriteLine(item.Name + Constant.PropertyKeyValue + tmp[0]);
                    for (int i = 1; i < tmp.Length; i++)
                    {
                        Console.WriteLine(Constant.T + tmp[i]);
                    }
                }
                else if(item.Name == "HotFixes")
                {
                    string[] tmp = item.GetValue(systeminfoModel, null) as string[];
                    Console.WriteLine("hotfix");
                }
                else if (item.Name == "NetworkCards")
                {
                    NetworkCard[] networkCards = item.GetValue(systeminfoModel, null) as NetworkCard[];
                    Console.WriteLine("网卡: ");
                }
                else
                {
                    Console.WriteLine(item.Name + Constant.PropertyKeyValue + item.GetValue(systeminfoModel, null));
                }
            }

            InstallationType();
            regInfo.ReSetSubkeyName("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion");
        }

    }
    /*
     HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\ProductOptions   RtlGetNtProductType   Workstation Server
     
     */

    /// <summary>
    /// https://unsafe.sh/go-17835.html  WMI架构
    /// 
    /// </summary>
    internal partial class systeminfo
    {
        private QueryRegInfo regInfo = new QueryRegInfo(RootRegistry.LocalMachine, "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion");

        private string GetSysDirectory()
        {
            StringBuilder sb = new StringBuilder();
            GetSystemDirectory(sb,256);
            return sb.ToString();
        }

        private string GetMachineName()
        {
            return Environment.MachineName;
        }

        private string GetWindowsOSName()
        {
            string DisplayVersion = regInfo.CurrentKey.GetValue("DisplayVersion")?.ToString();
            string EditionID = regInfo.CurrentKey.GetValue("EditionID").ToString();
            return $"Microsoft {DisplayVersion} {EditionID}";
        }

        private string GetWinOSVersion()
        {
#if Above81
            int CurrentMajorVersionNumber = (int)regInfo.CurrentKey.GetValue("CurrentMajorVersionNumber");
            int CurrentMinorVersionNumber = (int)regInfo.CurrentKey.GetValue("CurrentMinorVersionNumber");
            int CurrentBuildNumber = Convert.ToInt32(regInfo.CurrentKey.GetValue("CurrentBuildNumber"));
            return $"{CurrentMajorVersionNumber}.{CurrentMinorVersionNumber}.{CurrentBuildNumber} 暂缺 Build {CurrentBuildNumber}";
#endif

            string ProductName = regInfo.CurrentKey.GetValue("ProductName").ToString();
            return ProductName;
        }

        private string GetWinOSManufacture()
        {
            return "Microsoft Corporation";
        }
        /// <summary>
        /// HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion
        /// </summary>
        /// <returns></returns>
        private string GetProductionID()
        {
            return regInfo.CurrentKey.GetValue("ProductId").ToString();
        }

        /// <summary>
        /// HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\System\BIOS
        /// </summary>
        /// <returns></returns>
        private string GetBIOSVersion()
        {
            regInfo.ReSetSubkeyName("HARDWARE\\DESCRIPTION\\System\\BIOS");
            string BIOSVendor = regInfo.CurrentKey.GetValue("BIOSVendor").ToString();
            string BIOSVersion = regInfo.CurrentKey.GetValue("BIOSVersion").ToString();
            string BIOSReleaseDate = regInfo.CurrentKey.GetValue("BIOSReleaseDate").ToString();
            return $"{BIOSVendor} {BIOSVersion} {BIOSReleaseDate}";
        }

        private string GetDomain()
        {
            Domain domain;
            try
            {
                domain = Domain.GetComputerDomain();
            }
            catch (Exception ex)
            {
                return "WORKGROUP";
            }
            return domain.Name;
        }

        private string GetWinOSConfiguration()
        {
            try
            {
                Domain domain = Domain.GetComputerDomain();
            }
            catch (Exception ex)
            {
                return "独立工作站";
            }

            return "域环境";
        }

        private string GetArch()
        {
#if NET40
            return Environment.Is64BitOperatingSystem ? "x64-based PC" : "x86-based PC";

#else
            return IntPtr.Size == 0 ? "x64-based PC" : "x86-based PC";

#endif
        }

        private string[] GetCPU()
        {
            regInfo.ReSetSubkeyName("HARDWARE\\DESCRIPTION\\System\\CentralProcessor");
            List<string> cpues = new List<string>();
            int count = 1;
            foreach (var item in regInfo.EnumSubKeysName())
            {
                regInfo.ReSetSubkeyName("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\" + item);
                string Identifier = regInfo.CurrentKey.GetValue("Identifier").ToString();
                string VendorIdentifier = regInfo.CurrentKey.GetValue("VendorIdentifier").ToString();
                string ProcessorNameString = regInfo.CurrentKey.GetValue("ProcessorNameString").ToString();
                if(count < 10)
                {
                    cpues.Add($"[0{count}]: " + Identifier + " " + VendorIdentifier + "\n\t\t" + ProcessorNameString);
                }
                else
                {
                    cpues.Add($"[{count}]: " + Identifier + " " + VendorIdentifier + "\n\t\t" + ProcessorNameString);
                }

                count++;
            }
            cpues.Insert(0, $"Installed {count - 1} CPU");

            return cpues.ToArray();
        }

        private string GetCPUFlag()
        {
            string SystemProductName = regInfo.CurrentKey.GetValue("SystemProductName").ToString();
            return SystemProductName;
        }

        private string GetCPUManufacture()
        {
            string SystemManufacturer = regInfo.CurrentKey.GetValue("SystemManufacturer").ToString();
            return SystemManufacturer;
        }


        private string GetPhysicalMemoryTotally()
        {
            MEMORYSTATUSEX mEMORYSTATUSEX = new MEMORYSTATUSEX();
            
            if(GlobalMemoryStatusEx(mEMORYSTATUSEX))
            {
                return (mEMORYSTATUSEX.ullTotalPhys / (1024 * 1024)).ToString() + " MB";
            }
            return default;
        }

        private string GetPhysicalMemoryFree()
        {
            MEMORYSTATUSEX mEMORYSTATUSEX = new MEMORYSTATUSEX();

            if (GlobalMemoryStatusEx(mEMORYSTATUSEX))
            {
                return (mEMORYSTATUSEX.ullAvailPhys / (1024 * 1024)).ToString() + " MB";
            }
            return default;
        }

        private string GetVirtualMemoryMax()
        {
            MEMORYSTATUSEX mEMORYSTATUSEX = new MEMORYSTATUSEX();

            if (GlobalMemoryStatusEx(mEMORYSTATUSEX))
            {
                return (mEMORYSTATUSEX.ullTotalVirtual / (1024 * 1024)).ToString() + " MB";
            }
            return default;
        }

        private string GetVirtualMemoryFree()
        {
            MEMORYSTATUSEX mEMORYSTATUSEX = new MEMORYSTATUSEX();

            if (GlobalMemoryStatusEx(mEMORYSTATUSEX))
            {
                return (mEMORYSTATUSEX.ullAvailVirtual / (1024 * 1024)).ToString() + " MB";
            }
            return default;
        }

        private string GetVirtualMemoryUsing()
        {
            MEMORYSTATUSEX mEMORYSTATUSEX = new MEMORYSTATUSEX();

            if (GlobalMemoryStatusEx(mEMORYSTATUSEX))
            {
                return ((mEMORYSTATUSEX.ullTotalVirtual - mEMORYSTATUSEX.ullAvailVirtual) / (1024 * 1024)).ToString() + " MB";
            }
            return default;
        }

        private string GetPageFileName()
        {
            return "C:\\pagefile.sys";
        }

        private string GetTimeZone()
        {
            TimeZone timeZone = TimeZone.CurrentTimeZone;
            return timeZone.StandardName;
        }

        private string GetInputLanguageAreaSetting()
        {
            StringBuilder data = new StringBuilder(500);
            int result;

            // Try GetLocaleInfoEx
            // LOCALE_NAME_SYSTEM_DEFAULT = "!x-sys-default-locale"
            // en-US
            result = GetLocaleInfoEx("!x-sys-default-locale", LCTYPE.LOCALE_SENGLISHDISPLAYNAME, data, 500);
            Console.WriteLine(data);
            return data.ToString();
        }

        private string GetAreaSetting()
        {
            StringBuilder data = new StringBuilder(500);
            int result;

            // Try GetLocaleInfoEx
            result = GetLocaleInfoEx("!x-sys-default-locale", LCTYPE.LOCALE_SENGLISHDISPLAYNAME, data, 500);
            Console.WriteLine(data);
            return data.ToString();
        }


        private string GetVPBDeviceObject()
        {
            return "\\Device\\HarddiskVolume1";
        }
    }

    internal enum QueryType : ushort
    {
        //
        // CIM架构 CIM Object Manager是 WMI Core 向上封装了 WMI COM API.
        // .NET 使用的是 System.Management  COM Interop查询
        //
        CIM,
        /// <summary>
        /// API查询
        /// </summary>
        API,
    }

    internal class systeminfoModel
    {
        public string MachineName { get; set; }
        public string WinOSName { get; set; }
        public string WinOSVersion { get; set; }
        public string WinOSManufacture { get; set; }
        public string WinOSConfiguration { get; set; }
        public string WinOSBuildType { get; set; }
        public string RegisterOwner { get; set; }
        public string RegisterOrgranization { get; set; }
        public string ProductionID { get; set; }
        public string InitializationInstalledDateTime { get; set; }
        public string WinPowerOnDateTime { get; set; }
        public string CPUManufacture { get; set; }
        public string CPUFlag { get; set; }
        public string Arch { get; set; }
        public string[] CPU { get; set; }
        public string BIOSVersion { get; set; }
        public string WinDirectory { get; set; }
        public string WinSysDirectory { get; set; }
        public string VPBDeviceObject { get; set; }
        public string AreaSetting { get; set; }
        public string InputLanguageAreaSetting { get; set; }
        public string TimeZone { get; set; }
        public string PhysicalMemoryTotally { get; set; }
        public string PhysicalMemoryFree { get; set; }
        public string VirtualMemoryMax { get; set; }
        public string VirtualMemoryFree { get; set; }
        public string VirtualMemoryUsing { get; set; }
        public string PageFileName { get; set; }
        public string Domain { get; set; }
        public string LogonServer { get; set; }
        public string[] HotFixes { get; set; }
        public NetworkCard[] NetworkCards { get; set; }

    }

    internal class NetworkCard
    {
        public string Description { get; set; }
        public string ConnectName { get; set; }
        public bool EnableDHCP { get; set; }
        public string[] IPAddress { get; set; }
    }

    internal class HyperV
    {
        public bool VMMonitorModeExtensible { get; set; }
        public bool EnableVMFirmware { get; set; }
        public bool SecondAddressConvertor { get; set; }
        public bool DEP { get; set; }
    }
}
