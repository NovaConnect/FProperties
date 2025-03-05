using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace FProperties.Helper
{
    public class FileIconHelper
    {
        [DllImport("Shell32.dll")]
        public static extern int SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        public const uint SHGFI_ICON = 0x000000100;
        public const uint SHGFI_SMALLICON = 0x000000001;
        public const uint SHGFI_LARGEICON = 0x000000000;
        public const uint SHGFI_EXETYPE = 0x000002000;

        /// <summary>
        /// 获取指定文件或文件夹的图标，如果是exe文件则返回应用程序图标，否则返回文件格式图标。
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>文件的图标</returns>
        public static Icon GetFileIcon(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path) && !Directory.Exists(path))
            {
                throw new ArgumentException("路径无效或文件不存在");
            }

            SHFILEINFO shinfo = new SHFILEINFO();
            uint flags = SHGFI_ICON | SHGFI_LARGEICON;

            // 如果是 EXE 文件，获取应用程序的图标
            if (Path.GetExtension(path).Equals(".exe", StringComparison.OrdinalIgnoreCase))
            {
                flags |= SHGFI_EXETYPE;
            }
            else
            {
                // 否则获取文件类型的图标
                flags |= SHGFI_ICON;
            }

            SHGetFileInfo(path, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), flags);

            // 获取图标
            Icon icon = Icon.FromHandle(shinfo.hIcon);
            return icon;
        }

        /// <summary>
        /// 获取文件格式图标
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>文件格式图标</returns>
        public static Icon GetFileTypeIcon(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path) && !Directory.Exists(path))
            {
                throw new ArgumentException("路径无效或文件不存在");
            }

            SHFILEINFO shinfo = new SHFILEINFO();
            SHGetFileInfo(path, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_SMALLICON);

            // 获取图标
            Icon icon = Icon.FromHandle(shinfo.hIcon);
            return icon;
        }
        public static BitmapImage ConvertIconToBitmapImage(Icon icon)
        {
            // 将 Icon 转换为 Bitmap 并保存到 MemoryStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                icon.ToBitmap().Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Seek(0, SeekOrigin.Begin);

                // 创建 BitmapImage
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
    }

}
