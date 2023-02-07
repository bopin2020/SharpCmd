using SharpCmd.ConcreteCommand.ProcessManage;
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
        private bool _walkfiles = false;
        public override void Execute(Dictionary<string, string> arguments)
        {
            if (base.HelpCheck(arguments))
            {
                return;
            }
            // show files info

            if (arguments.ContainsKey("/f"))
            {
                _walkfiles = true;
            }
            TreeShow();
            _walkfiles = false;
        }

        private void TreeShow(string directory = null)
        {
            if(String.IsNullOrEmpty(directory))
            {
                directory = Directory.GetCurrentDirectory();
            }

            if (!Directory.Exists(directory))
            {
                throw new ArgumentException(nameof(directory));
            }

            DirectoryInfo di = new DirectoryInfo(directory);
            FileTree ft = new DirectoryBranch(di.Root.Name, 0);
            TreeShowCore(ft,di);
            ft.Print();
        }
        private void TreeShowCore(FileTree ft, DirectoryInfo di)
        {
            foreach (var item in di.GetDirectories())
            {
                DirectoryBranch db1 = new DirectoryBranch(item.Name);
                ft.Add(db1);
                if (_walkfiles)
                {
                    foreach (var files in item.GetFiles())
                    {
                        db1.Add(new OrdinaryFileBranch(files.Name));
                    }
                }
                // db1 作为新的树枝的根节点
                TreeShowCore(db1, item);
            }
        }

        /// <summary>
        /// 展示树结构信息
        /// </summary>
        private void Demo()
        {
            FileTree ft = new DirectoryBranch("D:\\", 0);
            DirectoryBranch db1 = new DirectoryBranch("Desktop");
            DirectoryBranch db2 = new DirectoryBranch("Desktop2");
            DirectoryBranch db3 = new DirectoryBranch("Desktop3");
            DirectoryBranch db4 = new DirectoryBranch("Desktop4");
            ft.Add(db1);
            db1.Add(new OrdinaryFileBranch("file1"));
            db1.Add(new OrdinaryFileBranch("file2"));

            ft.Add(db2);
            db2.Add(new OrdinaryFileBranch("file1"));
            db2.Add(new OrdinaryFileBranch("file2"));
            db2.Add(db4);
            db4.Add(new OrdinaryFileBranch("file2"));
            db4.Add(new OrdinaryFileBranch("file2"));
            ft.Add(db3);
            db3.Add(new OrdinaryFileBranch("file1"));
            db3.Add(new OrdinaryFileBranch("file2"));

            ft.Add(new OrdinaryFileBranch("file1"));
            ft.Add(new OrdinaryFileBranch("file2"));

            ft.Print();
        }

        private string Repeat(string item,int times)
        {
            return String.Join("", Enumerable.Repeat(item, times).ToArray());
        }
    }
    internal class FileTreeElement
    {
        public int Size { get; set; }
        public string Name { get; set; }

        public List<string> Files { get; set; } = new List<string>();
        public List<FileTreeElement> Children { get; set; } = new List<FileTreeElement>();

        public override string ToString()
        {
            return base.ToString();
        }
    }


    internal abstract class FileTree
    {
        public abstract string Name { get; set; }
        public abstract int Width { get; set; }

        public FileTree(string name) : this(name, 0) { }

        public FileTree(string name,int width)
        {
            Name = name;
            Width = width;
        }

        public abstract void Add(FileTree fileTree);
        public abstract void Print();

        protected virtual string AlignChar => "-";

        public string Repeat(string item, int times)
        {
            return String.Join("", Enumerable.Repeat(item, times).ToArray());
        }

    }

    internal class OrdinaryFileBranch : FileTree
    {
        public OrdinaryFileBranch(string name,int width) : base(name, width)
        {
        }

        public OrdinaryFileBranch(string name) : this(name,0)
        {

        }

        public override string Name { get; set; }
        public override int Width { get; set; }


        public override void Add(FileTree fileTree)
        {
            throw new NotImplementedException();
        }

        public override void Print()
        {
            Console.WriteLine($"[F]{Repeat(base.AlignChar,Width)}" + Name);
        }
    }

    internal class DirectoryBranch : FileTree
    {
        public DirectoryBranch(string name, int width) : base(name, width)
        {

        }


        public DirectoryBranch(string name) : base(name)
        {
        }

        public override string Name { get; set; }
        public override int Width { get; set; }

        private List<FileTree> fileTrees= new List<FileTree>();

        public override void Add(FileTree fileTree)
        {
            fileTree.Width = Width + 1;
            fileTrees.Add(fileTree);
        }

        public override void Print()
        {
            Console.WriteLine($"[D]{Repeat(base.AlignChar, Width)}" + Name);
            foreach (var item in fileTrees)
            {
                item.Print();
            }
        }
    }

    /// <summary>
    /// Junction or Directory Junction are a type of reparse point
    /// which contains link to a directory that acts as an alias of that directory
    /// 
    /// Junction 针对目录
    /// Symbolic link 针对文件  软连接-快捷方式
    /// https://www.2brightsparks.com/resources/articles/ntfs-hard-links-junctions-and-symbolic-links.html
    /// </summary>
    internal class JunctionBranch : FileTree
    {
        public JunctionBranch(string name, int width) : base(name, width)
        {
        }

        public override string Name { get ; set ; }
        public override int Width { get; set; }

        public override void Add(FileTree fileTree)
        {
            throw new NotImplementedException();
        }

        public override void Print()
        {
            throw new NotImplementedException();
        }
    }
}
