using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.FileOperation
{
    /// <summary>
    /// https://www.cnblogs.com/ngnetboy/p/5220128.html
    /// </summary>
    internal class tree : FileOperationBase
    {
        public override string CommandName => "tree";

        public override string Description => "show directory structure information";

        public override string CommandHelp => @"
TREE [drive:][path] [/F] [/A]

   /F   显示每个文件夹中文件的名称。
   /A   使用 ASCII 字符，而不使用扩展字符
";

        public override void Execute(Dictionary<string, string> arguments)
        {
            if (base.HelpCheck(arguments))
            {
                return;
            }
            // show files info
            bool file = false;
            if (arguments.ContainsKey("/f"))
            {
                file = true;
            }
            /// 构造上下文概念
            StringBuilder sb  = new StringBuilder();
            if(file)
            {
                DirectoryInfo din = new DirectoryInfo(Directory.GetCurrentDirectory());
                sb.Append("|-" + din.FullName);
                sb.Append('\n');
                if (file)
                {
                    foreach (var f in din.GetFiles())
                    {
                        sb.Append('\t' + f.Name);
                        sb.Append('\n');
                    }
                }
            }

            foreach (var item in Directory.GetDirectories(Directory.GetCurrentDirectory(),"*",SearchOption.AllDirectories))
            {
                DirectoryInfo din = new DirectoryInfo(item);
                sb.Append("|-" + din.FullName);
                sb.Append('\n');
                if(file)
                {
                    foreach (var f in din.GetFiles())
                    {
                        sb.Append('\t' + f.Name);
                        sb.Append('\n');
                    }
                }
                sb.Append('\n');
            }
            Console.WriteLine(sb.ToString());
        }
    }
}
