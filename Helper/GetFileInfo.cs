using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FProperties.Helper
{
    internal class GetFileInfo
    {
        public static string GetDefaultProgram(string filePath)
        {
            string extension = System.IO.Path.GetExtension(filePath)?.ToLower();

            if (string.IsNullOrEmpty(extension))
            {
                return null;
            }

            string key = $@"HKEY_CLASSES_ROOT\{extension}\shell\open\command";
            string programPath = (string)Registry.GetValue(key, "", null);

            if (programPath != null)
            {
                programPath = programPath.Split(' ')[0].Trim('\"');
                return programPath;
            }

            return null;
        }
        public static long GetFileSize(string filePath)
        {
            if (File.Exists(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                return fileInfo.Length;
            }
            else
            {
                return -1;
            }
        }
        public static Propertie GetFileAttributes(string filePath)
        {
            if (File.Exists(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);

                bool isReadOnly = (fileInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
                bool isHidden = (fileInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
                var info = new Propertie();
                info.Hidden = isHidden;
                info.ReadOnly = isReadOnly;
                return info;
            }
            else
            {
                return null;
            }
        }
        public static Times GetFileTimes(string filePath)
        {
            if (File.Exists(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);

                DateTime creationTime = fileInfo.CreationTime;
                DateTime lastWriteTime = fileInfo.LastWriteTime;
                DateTime lastAccessTime = fileInfo.LastAccessTime;

                var timeInfo = new Times();
                timeInfo.Access = lastAccessTime;
                timeInfo.Creat = creationTime;
                timeInfo.Change = lastWriteTime;
                return timeInfo;
            }
            else
            {
                return null;
            }
        }

        public static string GetReadableFileSize(long size)
        {
            if (size >= 1073741824)
                return (size / 1073741824.0).ToString("0.##") + " GB";
            else if (size >= 1048576)
                return (size / 1048576.0).ToString("0.##") + " MB";
            else if (size >= 1024)
                return (size / 1024.0).ToString("0.##") + " KB";
            else
                return size + " Bytes";
        }
        public static void RenameFile(string currentFullFileName, string newFileName)
        {
            string directoryPath = Path.GetDirectoryName(currentFullFileName);
            string newFullFileName = Path.Combine(directoryPath, newFileName);

            if (currentFullFileName != newFullFileName)
            {
                try
                {
                    File.Move(currentFullFileName, newFullFileName);
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
            }
        }
        public static void SetFileAttributes(string filePath, bool isReadOnly, bool isHidden)
        {
            try
            {
                FileAttributes attributes = File.GetAttributes(filePath);

                if (isReadOnly)
                {
                    attributes |= FileAttributes.ReadOnly;
                }
                else
                {
                    attributes &= ~FileAttributes.ReadOnly;
                }

                if (isHidden)
                {
                    attributes |= FileAttributes.Hidden;
                }
                else
                {
                    attributes &= ~FileAttributes.Hidden;
                }

                File.SetAttributes(filePath, attributes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"设置文件属性时发生错误: {ex.Message}");
            }
        }
    }
}
