using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfMain
{
    /// <summary>
    /// NewJVM.xaml 的交互逻辑
    /// </summary>
    public partial class NewJVM : Window
    {
        public NewJVM()
        {
            InitializeComponent();
            Title = "选择jvm";
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "Java.exe|Java.exe";
            ofd.DefaultExt = ".exe";
            ofd.Title = "选择一个java.exe";
            ofd.CheckFileExists = false;
            bool? result = ofd.ShowDialog();
            if (result == true)
            {
                Java.Text = ofd.FileName;
            }
        }

        private void GetResult(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Java.Text)) return;
            else
            {
                string JvmJavaexePath = Java.Text;
                System.IO.DirectoryInfo topDir = System.IO.Directory.GetParent(Java.Text);
                string javapath = System.IO.Directory.GetParent(topDir.FullName).FullName;
                MainControls.MainWindow.Addjvm("手动添加", javapath, "", JvmJavaexePath);
                Close();
            }
        }
    }
}
