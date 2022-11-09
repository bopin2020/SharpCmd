using Microsoft.Win32;
using SharpCmd.Lib.ExceptionCollection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    internal struct SpecifiedKeyValues
    {
        public string ValueNames { get; }
        public RegistryValueKind ValueKind { get; }
        public object Value { get; }

        public SpecifiedKeyValues(string ValueNames, RegistryValueKind ValueKind, object Value)
        {
            this.ValueNames = ValueNames;
            this.ValueKind = ValueKind;
            this.Value = Value; 
        }
    }

    /// <summary>
    /// non-thread safe sealed class
    /// </summary>
    internal sealed class QueryRegInfo
    {
        // ==========================================================================================
        #region private fields

        private volatile RegistryKey rootkey;
        private int recursionDeep = 10;
        private ICollection<SpecifiedKeyValues> specifiedKeyValues { get; set; } = new Collection<SpecifiedKeyValues>();

        #endregion

        // ==========================================================================================
        #region public fields

        public string RegistrySubKeyName { get; set; }
        public volatile RegistryKey CurrentKey;

        #endregion

        // ==========================================================================================
        #region constructors

        public QueryRegInfo(RegistryKey rootkey, string subkeyname = "")
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


        #endregion

        // ==========================================================================================
        #region private methods

        /// <summary>
        /// Reset ICollection result Count equals 0
        /// </summary>
        private void Clear()
        {
            specifiedKeyValues.Clear();
        }

        private void Dispose(RegistryKey CurrentKey)
        {
#if NET40
            CurrentKey.Dispose();
#else
            CurrentKey.Close();
#endif
            CurrentKey = null;
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

        #endregion

        // ==========================================================================================
        #region public methods

        /// <summary>
        /// Enum The Current Key Values
        /// </summary>
        /// <returns></returns>
        public ICollection<SpecifiedKeyValues> EnumKeyValues()
        {
            return EnumSpecifiedKeyValues(CurrentKey);
        }

        public ICollection<SpecifiedKeyValues> EnumSpecifiedKeyValues(RegistryKey registryKey)
        {
            // Clear the last query collection
            Clear();
            var key = registryKey;
            // Add the default value
            specifiedKeyValues.Add(new SpecifiedKeyValues(
                ValueNames: "Default",
                ValueKind: RegistryValueKind.String,
                Value: key.GetValue("") == null ? "n/a" : key.GetValue("")
                ));

            foreach (string valueName in key.GetValueNames())
            {
                specifiedKeyValues.Add(new SpecifiedKeyValues(
                    ValueNames: valueName,
                    ValueKind: key.GetValueKind(valueName),
                    Value: key.GetValue(valueName)
                    ));

#if OptimizeResult
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
#endif
            }
            return specifiedKeyValues;
        }

        public IEnumerable<string> EnumSubKeysName(string subkey = "")
        {
            if (!String.IsNullOrEmpty(RegistrySubKeyName))
            {
                return EnumSubKeysName(RegistrySubKeyName, false);
            }
            return EnumSubKeysName(subkey, false);

        }

        /// <summary>
        /// recursion call
        /// </summary>
        /// <param name="subkey"></param>
        /// <param name="recursion"></param>
        /// <returns></returns>
        public IEnumerable<string> EnumSubKeysName(string subkey, bool recursion = false)
        {
            if(String.IsNullOrEmpty(subkey))
            {
                subkey = RegistrySubKeyName;
            }
            return EnumSubKeysName(rootkey.OpenSubKey(subkey),true);
        }

        public IEnumerable<string> EnumSubKeysName(RegistryKey registryKey, bool recursion = false)
        {
            List<string> values = new List<string>();
            if (registryKey is null)
            {
                return values;
            }
            //CurrentKey = registryKey;
            var key = registryKey;
            var tmp = key.GetSubKeyNames();
            if (tmp.Count() == 0)
            {
                return values;
            }
            if (key != null)
            {
                values.AddRange(tmp);
            }

            if (recursion && this.recursionDeep != 0)
            {
                this.recursionDeep--;
                foreach (var item in tmp)
                {
                    values.AddRange(EnumSubKeysName(CurrentKey.OpenSubKey(item), true));
                }
            }
            return values;
        }

        public ICollection<SpecifiedKeyValues> GetKeyValues(bool throwException = true)
        {
            if (specifiedKeyValues.Count == 0 && throwException)
            {
                throw new CollectionEmptyException();
            }

            return specifiedKeyValues;
        }

        /// <summary>
        /// 重新设置 根Rootkey
        /// </summary>
        /// <param name="rootkey"></param>
        /// <returns></returns>
        public bool SetRootkey(RegistryKey rootkey)
        {
            string lastStatus = this.rootkey.Name;
            this.rootkey = rootkey;
            Dispose(this.CurrentKey);
            if (lastStatus != this.rootkey.Name)
            {
                return true;
            }
            return false;
        }

        public bool SetRootkey(RootRegistry rootRegistry)
        {
            return SetRootkey(Convert(rootRegistry));
        }
        /// <summary>
        /// 枚举子键时  设置递归深度
        /// </summary>
        /// <param name="len"></param>
        public void SetRecursionDeep(int len)
        {
            this.recursionDeep = len;
        }

        /// <summary>
        /// 重新设置子键名
        /// </summary>
        /// <param name="newsubkey"></param>
        public void SetSubkeyName(string newsubkey, bool fromRoot = true)
        {
            if (fromRoot)
            {
                RegistrySubKeyName = newsubkey;
                CurrentKey = rootkey.OpenSubKey(RegistrySubKeyName);
            }
            else
            {
                RegistrySubKeyName += Constant.BackSlash;
                RegistrySubKeyName += newsubkey;
                CurrentKey = CurrentKey.OpenSubKey(RegistrySubKeyName);
            }
        }

        #endregion
    }
}
