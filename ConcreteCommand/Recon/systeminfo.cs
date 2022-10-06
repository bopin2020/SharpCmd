using SharpCmd.Contract;
using SharpCmd.Lib.Help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.Recon
{
    internal class systeminfo : IContract
    {
        public string CommandName => "systeminfo";

        /// <summary>
        /// https://github.com/tlewiscpp/SystemInfo/blob/master/src/osinfo.cpp
        /// </summary>
        /// <param name="arguments"></param>
        public void Execute(Dictionary<string, string> arguments)
        {
            systeminfoModel systeminfoModel = new systeminfoModel();
            systeminfoModel.MachineName = Environment.MachineName;
            systeminfoModel.WinOSName = "Microsoft Windows 11 专业版";
            systeminfoModel.WinOSVersion = "10.0.22000 暂缺 Build 22000";
            systeminfoModel.WinOSManufacture = "Microsoft Corporation";


            Console.WriteLine("\n\n");
            foreach (var item in systeminfoModel.GetType().GetProperties())
            {
                Console.WriteLine(item.Name + Constant.PropertyKeyValue + item.GetValue(systeminfoModel, null));
            }

        }
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
