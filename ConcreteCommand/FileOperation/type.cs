using SharpCmd.Contract;
using SharpCmd.Lib.Help;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SharpCmd.ConcreteCommand.FileOperation
{
    internal class type : IContract
    {
        public string CommandName => "type";

        public void Execute(Dictionary<string, string> arguments)
        {
            string filename = null;
            bool textfile = true;
            try
            {
                filename = arguments.Keys.ToArray()[1];
                if(!File.Exists(filename))
                {
                    Console.WriteLine(Constant.FileNotFound);
                    return;
                }
                byte[] data = File.ReadAllBytes(filename);
                byte[] pe = { 0x4d, 0x5a };
                byte[] buf = new byte[pe.Length];
                Array.Copy(data,0, buf, 0, pe.Length);
                if(buf.SequenceEqual(pe))
                {
                    textfile = false;
                }

                if(textfile)
                {
                    Console.WriteLine(File.ReadAllText(filename));
                }
                else
                {
                    Console.WriteLine(Convert.ToBase64String(data));
                }

            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine(Constant.CommandSyntaxInvalid);
            }

        }
    }
}
