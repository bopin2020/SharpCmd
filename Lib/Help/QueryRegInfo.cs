using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.Lib.Help
{
    public enum RootRegistry
    {
        ClassesRoot,
        CurrentUser,
        CurrentConfig,
        LocalMachine,
        Users
    }

    internal class QueryRegInfo
    {
        private volatile RegistryKey rootkey;
        private int recursionDeep = 10;
        public string RegistrySubKeyName { get; set; }
        public volatile RegistryKey CurrentKey;
        public QueryRegInfo(RegistryKey rootkey,string subkeyname = "")
        {
            this.rootkey = rootkey;
            this.RegistrySubKeyName = subkeyname;
            CurrentKey = rootkey.OpenSubKey(subkeyname);
        }

        public QueryRegInfo(RootRegistry rootRegistry, string subkeyname = "")
        {
            Convert(rootRegistry);
            this.RegistrySubKeyName = subkeyname;
            CurrentKey = rootkey.OpenSubKey(this.RegistrySubKeyName);
        }


        private RegistryKey Convert(RootRegistry rootRegistry)
        {
            switch (rootRegistry)
            {
                case RootRegistry.ClassesRoot:
                    rootkey = Registry.ClassesRoot;
                    break;
                case RootRegistry.CurrentUser:
                    rootkey = Registry.CurrentUser;
                    break;
                case RootRegistry.CurrentConfig:
                    rootkey = Registry.CurrentConfig;
                    break;
                case RootRegistry.LocalMachine:
                    rootkey = Registry.LocalMachine;
                    break;
                case RootRegistry.Users:
                    rootkey = Registry.Users;
                    break;
                default:
                    break;
            }
            return rootkey;
        }

        public bool SetRootkey(RegistryKey rootkey)
        {
            string lastStatus = this.rootkey.Name;
            this.rootkey = rootkey;
            if(lastStatus != this.rootkey.Name)
            {
                return true;
            }
            return false;
        }

        public bool SetRootkey(RootRegistry rootRegistry)
        {
            return SetRootkey(Convert(rootRegistry));
        }

        public void SetRecursionDeep(int len)
        {
            this.recursionDeep = len;
        }

        public void ReSetSubkeyName(string newsubkey)
        {
            RegistrySubKeyName = newsubkey;
            CurrentKey = rootkey.OpenSubKey(RegistrySubKeyName);
        }

        /// <summary>
        /// 枚举指定键下的name value
        /// 返回类型替换为  名称，类型，值  三元组
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> EnumKeyValues()
        {
            return EnumSpecifiedKeyValues(rootkey);
        }

        public IEnumerable<string> EnumSpecifiedKeyValues(RegistryKey registryKey)
        {
            List<string> values = new List<string>();

            using (var key = registryKey)
            {
                foreach (string valueName in key.GetValueNames())
                {
                    switch (key.GetValueKind(valueName))
                    {
                        case RegistryValueKind.String:
                        case RegistryValueKind.ExpandString:
                        case RegistryValueKind.DWord:
                        case RegistryValueKind.QWord:
                        case RegistryValueKind.Unknown:
#if NET40
                        case RegistryValueKind.None:
#endif
                            {
                                values.Add($"{valueName} : {key.GetValue(valueName)}");
                            }
                            break;
                        case RegistryValueKind.Binary:
                            {
                                //byte[] data = (byte[])key.GetValue(valueName);
                                //StringBuilder sb = new StringBuilder();
                                //using (MemoryStream ms = new MemoryStream())
                                //{
                                //    ms.Write(data, 0, data.Length);
                                //    ms.Position = 0;
                                //    for (int i = 0; i < data.Length / 32; i++)
                                //    {
                                //        byte[] row = new byte[32];
                                //        ms.Read(row, 0, 32);

                                //        sb.AppendLine(Convert.ToString(row, 16));
                                //    }
                                //    byte[] tail = new byte[data.Length % 32];
                                //    ms.Read(tail, 0, tail.Length);
                                //    sb.AppendLine(Convert.ToString(tail, 16));
                                //}


                                Console.WriteLine($"{valueName} : ");
                            }
                            break;
                        case RegistryValueKind.MultiString:
                            {
                                values.Add($"{valueName} : {String.Join("\n\t", (string[])key.GetValue(valueName))}");
                            }
                            break;
                        default:
                            break;
                    }

                }
            }

            return values;
        }

        public IEnumerable<string> EnumSubKeysName(string subkey = "")
        {
            if(!String.IsNullOrEmpty(RegistrySubKeyName))
            {
                return EnumSubKeysName(RegistrySubKeyName,false);
            }
            return EnumSubKeysName(subkey, false);

        }

        public IEnumerable<string> EnumSubKeysName(string subkey,bool recursion = false)
        {
            List<string> values = new List<string>();
            using (var key = rootkey.OpenSubKey(subkey))
            {
                if(key != null)
                    values.AddRange(key.GetSubKeyNames());
            }

            return values;
        }
    }
}
