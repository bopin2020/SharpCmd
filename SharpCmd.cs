﻿using SharpCmd.ConcreteCommand.Jobs;
using SharpCmd.Contract;
using SharpCmd.Lib.ArgsParser;
using SharpCmd.Lib.Help;
using SharpCmd.Lib.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SharpCmd
{
    internal class SharpCmd : IDisposable
    {
        private List<IContract> contracts = new List<IContract>();
        private Command command;

        private static SharpCmd sharpCmd;

        public SharpCmd(Command command)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsInterface) continue;
                if (type.IsAbstract) continue;
                if (typeof(IContract).IsAssignableFrom(type))
                {
                    var con = Activator.CreateInstance(type) as IContract;
                    contracts.Add(con);
                }
            }

            this.command = command;
            sharpCmd = this;
        }

        public void Dispose()
        {
            contracts.Clear();
        }

        public void Init(string[] args)
        {
            foreach (var item in contracts)
            {
                this.command.Register(item);
            }
#if Sys
            StringBuilder sb = new StringBuilder(256);
            kernel32.GetSystemDirectory(sb,256);
            Directory.SetCurrentDirectory(sb.ToString());
#endif
            if(args.Length != 0)
            {
                var result = ArgumentParser.Parse(args);
                try
                {
                    this.command.Notify(result.Arguments);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("[Error]\t" + ex.Message);
                    Console.Error.WriteLine(ex.StackTrace);
                }
                return;
            }

            while (true)
            {
                Console.Write(Environment.UserName + (Common.IsHighIntegrity() ? "#> " : "$> ") + Directory.GetCurrentDirectory() + Constant.SpecialChar);
                var result = ArgumentParser.Parse(Parse(Console.ReadLine()));
                try
                {
                    this.command.Notify(result.Arguments);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("[Error]\t" + ex.Message);
                    Console.Error.WriteLine(ex.StackTrace);
                }
            }
        }

        private string[] Parse(string input)
        {
            string[] result = input.Split();
            return result;
        }

        public static void JobProxy(Dictionary<string,string> proxyArgs,JobItem jobItem)
        {
            try
            {
                sharpCmd.command.Notify(proxyArgs,jobItem);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("[Error]\t" + ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
            }
        }
    }
}
