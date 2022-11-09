﻿using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.FileOperation
{
    internal class copy : IContract
    {
        public string CommandName => "copy";

        public string Description => "copy file from a to b";

        public void Execute(Dictionary<string, string> arguments)
        {
            if(arguments.Count < 2)
            {
                Console.Error.WriteLine("Command syntax was not correct");
                return;
            }
            if (arguments.ContainsKey("/y"))
            {
                File.Copy(arguments.Keys.ToArray()[2], arguments.Keys.ToArray()[3],true);
                if(File.Exists(arguments.Keys.ToArray()[3]))
                {
                    Console.WriteLine("Copy file successfully");
                }
                return;
            }
            File.Copy(arguments.Keys.ToArray()[1], arguments.Keys.ToArray()[2]);
            if (File.Exists(arguments.Keys.ToArray()[2]))
            {
                Console.WriteLine("Copy file successfully");
            }
        }
    }
}
