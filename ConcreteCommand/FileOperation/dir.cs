using SharpCmd.Contract;
using SharpCmd.Lib.Help;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SharpCmd.ConcreteCommand.FileOperation
{
    internal class dir : FileOperationBase
    {
        public override string CommandName => "dir";

        public override string Description => "enumerate files or dirs";

        public override string CommandHelp => "dir c:\\ [option]";

        public override void Execute(Dictionary<string, string> arguments)
        {
            List<FileModel> files = new List<FileModel>();
            string newPath = null;
            try
            {
                newPath = arguments.Keys.ToArray()[1];
            }
            catch (Exception ex)
            {
                newPath = Directory.GetCurrentDirectory();
            }
            finally
            {
                foreach (var item in Directory.GetDirectories(newPath))
                {
                    FileModel fileModel = new FileModel();
                    var dirinfo = new DirectoryInfo(item);
                    fileModel.CreateTime = dirinfo.CreationTime;
                    if((dirinfo.Attributes | FileAttributes.ReparsePoint) == dirinfo.Attributes)
                    {
                        fileModel.FileType = FileType.JUNCTION;
                    }
                    else
                    {
                        fileModel.FileType = FileType.DIR;
                    }
                    fileModel.Name = dirinfo.Name;

                    files.Add(fileModel);
                }
                foreach (var item in Directory.GetFiles(newPath))
                {
                    FileModel fileModel = new FileModel();
                    var fileinfo = new FileInfo(item);
                    fileModel.CreateTime = fileinfo.CreationTime;
                    fileModel.Name = fileinfo.Name;
                    fileModel.FileType = FileType.File;
                    fileModel.filesize = fileinfo.Length;
                    files.Add(fileModel);
                }

            }
            files.Sort();
            foreach (var item in files)
            {
                Console.WriteLine(item.CreateTime.ToString() + Constant.T + (item.FileType == FileType.File ? "" : item.FileType.ToString()) + Constant.T + (item.filesize == 0 ? "" : item.filesize.ToString()) + Constant.T + item.Name);
            }
        }
    }
    internal enum FileType
    {
        DIR,
        JUNCTION,
        File
    }
    internal class FileModel : IComparable<FileModel>
    {
        public DateTime CreateTime { get; set; }
        public FileType FileType { get; set; }
        public long filesize { get; set; }
        public string Name { get; set; }

        public int CompareTo(FileModel other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }
}
