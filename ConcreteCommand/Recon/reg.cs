using SharpCmd.Contract;
using SharpCmd.Lib.Help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace SharpCmd.ConcreteCommand.Recon
{
    /// <summary>
    /// https://github.com/airzero24/WMIReg/
    /// </summary>
    internal partial class reg : ReconBase
    {
        public override string CommandName => "reg";

        public override string Description => "Register Manager";

        public override string CommandHelp => "reg";

        public override void Execute(Dictionary<string, string> arguments)
        {
            if (arguments.ContainsKey("/?"))
            {
                Help();
                return;
            }
            try
            {
                RegArgsParser regArgsParser = new RegArgsParser(arguments.Keys.ToArray()[2]);

                QueryRegInfo queryRegInfo = new QueryRegInfo(regArgsParser.rootRegistry, regArgsParser._subkey);
                GetCurrentKeysValues(queryRegInfo);
            }
            catch (Exception)
            {
                Help();
                return;
            }


        }


    }
    /// <summary>
    /// 获取当前键下的值
    /// </summary>
    internal partial class reg
    {
        private void Help()
        {
            string help = @"
REG Operation [Parameter List]

  Operation  [ QUERY   | ADD    | DELETE  | COPY    |
               SAVE    | LOAD   | UNLOAD  | RESTORE |
               COMPARE | EXPORT | IMPORT  | FLAGS ]

HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug

";
            Console.WriteLine(help);
        }

        private void GetCurrentKeysValues(QueryRegInfo queryRegInfo)
        {
            foreach (var item in queryRegInfo.EnumKeyValues())
            {
                Profile.output.WriteLine(String.Format("{0,-30} {1,-15} {2}", item.ValueNames, item.ValueKind, item.Value));
            }
        }
    }

    internal class RegArgsParser
    {
        public RootRegistry rootRegistry { get; set; } = RootRegistry.LocalMachine;
        public string _subkey { get; set; } = "";

        public Dictionary<string, RootRegistry> getRootKey = new Dictionary<string, RootRegistry>()
        {
            {"HKEY_LOCAL_MACHINE",RootRegistry.LocalMachine },
            {"HKEY_CLASSES_ROOT", RootRegistry.ClassesRoot },
            {"HKEY_CURRENT_USER", RootRegistry.CurrentUser },
            {"HKEY_USERS",RootRegistry.Users },
            {"HKEY_CURRENT_CONFIG",RootRegistry.CurrentConfig },

        };

        public RegArgsParser(string subkey)
        {
            subkey = subkey.Trim('"');
            if (subkey.StartsWith("HKEY"))
            {
                if(!subkey.Contains("\\"))
                {
                    rootRegistry = getRootKey[subkey];
                    _subkey = "";
                }
                else
                {
                   rootRegistry = getRootKey[subkey.Split('\\')[0]];
                   _subkey = String.Join("\\", subkey.Split('\\').Skip(1).ToArray());
                }

            }
            else
            {
                _subkey = subkey;
            }
        }
    }

    internal enum RegAction : ushort
    {
        QUERY,
        ADD,
        DELETE,
        COPY,
        SAVE,
        RESTORE,
        LOAD,
        UNLOAD,
        COMPARE,
        EXPORT,
        IMPORT,
        FLAGS,
    }
}
