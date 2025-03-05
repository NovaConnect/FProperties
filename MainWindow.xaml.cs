using FProperties.Helper;
using FProperties.Pages;
using iNKORE.UI.WPF.Modern.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using MessageBox = iNKORE.UI.WPF.Modern.Controls.MessageBox;

namespace FProperties
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var Cmd = GetProcessCommandLine(Process.GetCurrentProcess().Id).Split(' ');
            if (Cmd.Length > 1)
            {
                string fullPath = string.Join(" ", Cmd, 1, Cmd.Length - 1);
                if (File.Exists(fullPath) || Directory.Exists(fullPath))
                {
                    //MessageBox.Show(Cmd[1]);
                    var icon = FileIconHelper.ConvertIconToBitmapImage
                                (FileIconHelper.GetFileIcon(fullPath));


                    var fileHelper = new FileHelper(Path.GetFileName(fullPath),
                                                    fullPath,
                                                    icon,
                                                    GetFileInfo.GetDefaultProgram(fullPath),
                                                    GetFileInfo.GetFileSize(fullPath),
                                                    GetFileInfo.GetFileAttributes(fullPath),
                                                    GetFileInfo.GetFileTimes(fullPath));

                    Title = $"{fileHelper.FileName} - 文件属性";

                    FileProperties filePropertiesPage = new FileProperties(fileHelper);
                    MainFrame.NavigationService.Navigate(filePropertiesPage);
                }
            }
        }

        static string GetProcessCommandLine(int processId)
        {
            string query = $"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {processId}";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            try
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    return obj["CommandLine"]?.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取命令行信息时出错: {ex.Message}");
            }

            return null;
        }

        public static int GetCurrentTheme()
        {
            try
            {
                string registryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
                string registryKeyName = "AppsUseLightTheme";

                var key = Registry.CurrentUser.OpenSubKey(registryKeyPath);
                if (key != null)
                {
                    var value = key.GetValue(registryKeyName);
                    if (value != null)
                    {
                        return (int)value;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取系统主题时发生错误: " + ex.Message);
                return -1;
            }
        }
    }
}
