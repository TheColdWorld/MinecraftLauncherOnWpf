using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using WpfMain;

namespace WpfMain
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Jvms> JVMSShowList = new ObservableCollection<Jvms>();
        public MainWindow()
        {
            InitializeComponent();
            Title = "启动器";
            JavaCombox.ItemsSource = JVMSShowList;
            try
            {

            }
            catch (System.IO.FileNotFoundException e)
            {
                MessageBox.Show(Application.Current.MainWindow, "未找到Minecraft", "", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                MessageBox.Show(Application.Current.MainWindow, "未找到Minecraft", "", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }
        private void OfflineLogin_button_click(object sender, RoutedEventArgs e)
        {

        }

        private void BugJumpLogin_button_click(object sender, RoutedEventArgs e)
        {

        }

        private void MicrosoftLogin_button_click(object sender, RoutedEventArgs e)
        {

        }

        private void JVM_Find_Button_click(object sender, RoutedEventArgs e)
        {
            DateTime StartTime = DateTime.Now;
            int jvmlenght = 0;
            FindJavaBut.IsEnabled = false;
            RegistryKey LocalMachine = Registry.LocalMachine;
            RegistryKey RegkeyJRE = LocalMachine.OpenSubKey(@"SOFTWARE\JavaSoft\Java Runtime Environment");
            if (RegkeyJRE != null)
            {
                string[] JVMS = RegkeyJRE.GetSubKeyNames();
                for (int JVMlen = 0; JVMlen < JVMS.Length; JVMlen++)
                {
                    RegistryKey RegkeyJREs = RegkeyJRE.OpenSubKey(JVMS[JVMlen]);
                    string JVMPAth = (string)RegkeyJREs.GetValue(@"JavaHome");
                    string JVMExePAth = System.IO.Path.Combine(System.IO.Path.Combine(JVMPAth, @"bin"), @"java.exe");
                    if (System.IO.File.Exists(JVMExePAth))
                    {
                        Jvms jvms = new Jvms("JRE", JVMPAth, JVMS[JVMlen], JVMExePAth);
                        JVMSShowList.Add(jvms);
                        jvmlenght++;
                    }
                }
            }
            RegistryKey RegkeyJDK = LocalMachine.OpenSubKey(@"SOFTWARE\JavaSoft\JDK");
            if (RegkeyJDK != null)
            {
                string[] JDKS = RegkeyJDK.GetSubKeyNames();
                for (int JVMlen = 0; JVMlen < JDKS.Length; JVMlen++)
                {
                    RegistryKey RegkeyJREs = RegkeyJDK.OpenSubKey(JDKS[JVMlen]);
                    string JVMPAth = (string)RegkeyJREs.GetValue(@"JavaHome");
                    string JVMExePAth = System.IO.Path.Combine(System.IO.Path.Combine(JVMPAth, @"bin"), @"javaw.exe");
                    if (System.IO.File.Exists(JVMExePAth))
                    {
                        Jvms jvms = new Jvms("JDK", JVMPAth, JDKS[JVMlen], JVMExePAth);
                        JVMSShowList.Add(jvms);
                        jvmlenght++;
                    }
                }
            }
            JavaCombox.Items.Refresh();
            TimeSpan ts = DateTime.Now - StartTime;
            MessageBox.Show(Application.Current.MainWindow, "注册表检索完毕，共" + jvmlenght + "个JVM\n用时" + ts.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Information);
            FindJavaBut.IsEnabled = false;
        }

        private void Launch_button_click(object sender, RoutedEventArgs e)
        {

        }

        private void JVM_User_Find_Button_click(object sender, RoutedEventArgs e)
        {
            NewJVM newJVM = new NewJVM();
            MainControls.MainWindow = this;
            newJVM.ShowDialog();
            JavaCombox.Items.Refresh();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">JVM的种类</param>
        /// <param name="path">JVM的安装路径</param>
        /// <param name="ver">JVM的版本</param>
        /// <param name="exepath">JVM对于的java.exe的绝对路径</param>
        public async void Addjvm(string type, string path, string ver, string exepath)
        {
            await Dispatcher.BeginInvoke(new Action(delegate ()
            {
                JVMSShowList.Add(new Jvms(type, path, ver, exepath));
            }));
        }
    }
    public class Jvms
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">JVM的种类</param>
        /// <param name="path">JVM的安装路径</param>
        /// <param name="ver">JVM的版本</param>
        /// <param name="exepath">JVM对于的java.exe的绝对路径</param>
        public Jvms(string type, string path, string ver, string exepath)
        {
            JavaType = type;
            JvmPath = path;
            JvmJavaexepath = exepath;

            JvmVersion = ver;
        }
        public string JavaType { get; set; }
        public string JvmPath { get; set; }
        public string JvmVersion { get; set; }
        public string JvmJavaexepath { get; set; }
    }
}
