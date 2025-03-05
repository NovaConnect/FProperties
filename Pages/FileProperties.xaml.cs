using FProperties.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

namespace FProperties.Pages
{
    /// <summary>
    /// FileProperties.xaml 的交互逻辑
    /// </summary>
    public partial class FileProperties : Page
    {
        FileHelper fh;
        public FileProperties(FileHelper fileHelper)
        {
            InitializeComponent();
            fh = fileHelper;
            PropertieIcon.Source = fileHelper.Icon;
            FileName.Text = fileHelper.FileName;
            FullName.Text = fileHelper.FullName.Replace(fileHelper.FileName,"");
            FileSize.Text = $"{GetFileInfo.GetReadableFileSize(fileHelper.Size)}({fileHelper.Size} Byte)";
            FileType.Text = $"{Path.GetExtension(fileHelper.FileName)} 类型";
            OpenWith.Text = fileHelper.OpenWith;

            CreatTime.Text = fileHelper.Time.Creat.ToString("yyyy年MM月dd日 HH:mm:ss");
            WriteTime.Text = ((DateTime)(fileHelper.Time.Change)).ToString("yyyy年MM月dd日 HH:mm:ss");
            AccessTime.Text = ((DateTime)(fileHelper.Time.Access)).ToString("yyyy年MM月dd日 HH:mm:ss");

            IsReadOnly.IsChecked = fileHelper.Properties.ReadOnly;
            IsHidden.IsChecked = fileHelper.Properties.Hidden;

            SetTextBlockColors();
        }
        public void SetTextBlockColors()
        {
            var theme = MainWindow.GetCurrentTheme();

            foreach (var control in FindLogicalChildren<TextBlock>(this))
            {
                if (theme == 1)
                {
                    control.Foreground = Brushes.White;
                }
                else if (theme == 0)
                {
                    control.Foreground = Brushes.White;
                }
            }
        }

        public static System.Collections.Generic.IEnumerable<T> FindLogicalChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield break;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T t) yield return t;
                foreach (var descendant in FindLogicalChildren<T>(child))
                {
                    yield return descendant;
                }
            }
        }

        private void Btn_OK_Click(object sender, RoutedEventArgs e)
        {
            SaveChange();
            Environment.Exit(0);
        }

        private void Btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            SaveChange();
            Btn_Apply.IsEnabled = false;
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            Btn_Apply.IsEnabled = true;
        }
        void SaveChange()
        {
            if (Path.GetExtension(FileName.Text) != Path.GetExtension(fh.FileName))
            {
                var result = MessageBox.Show(
                    "文件扩展名已更改，是否继续？",
                    "扩展名更改",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result != MessageBoxResult.Yes)
                {
                    return;
                }

            }
            GetFileInfo.RenameFile(fh.FullName, FileName.Text);
            GetFileInfo.SetFileAttributes(Path.Combine(Path.GetDirectoryName(fh.FullName), FileName.Text),
                                          (bool)IsReadOnly.IsChecked,
                                          (bool)IsHidden.IsChecked);
        }
    }
}
