using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static FProperties.Helper.FileHelper;

namespace FProperties.Helper
{
    public class Propertie
    {
        public bool ReadOnly { get; set; }
        public bool Hidden { get; set; }
    }

    public class Times
    {
        public DateTime Creat { get; set; }
        public DateTime? Change { get; set; }
        public DateTime? Access { get; set; }
    }
    public class FileHelper
    {


        public FileHelper(string fileName, string fullName, ImageSource icon, string openWith, long size, Propertie properties, Times times)
        {
            FileName = fileName;
            FullName = fullName;
            Icon = icon;
            OpenWith = openWith;
            Size = size;
            Properties = properties;
            Time = times;
        }

        public string FileName { get; set; }
        public string FullName { get; set; }
        public ImageSource Icon { get; set; }
        public string OpenWith { get; set; }
        public long Size { get; set; }
        public Times Time { get; set; }
        public Propertie Properties { get; set; }
    }

    public class FolderHelper
    {
        public FolderHelper(string fileName, string fullName, ImageSource icon, long size, Propertie properties, Times times)
        {
            FileName = fileName;
            FullName = fullName;
            Icon = icon;
            Size = size;
            Properties = properties;
            Time = times;
        }

        public string FileName { get; set; }
        public string FullName { get; set; }
        public ImageSource Icon { get; set; }
        public long Size { get; set; }
        public Times Time { get; set; }
        public class Content
        {
            public int FileNum { get; set; }
            public int FolderNum { get; set; }
        }
        public Propertie Properties { get; set; }
    }
}
