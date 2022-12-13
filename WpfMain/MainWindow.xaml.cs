using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Windows.Shell;
using WpfMain;

namespace WpfMain
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Jvms> JVMSShowList = new ObservableCollection<Jvms>();
        private ObservableCollection<MCCores> VersionList;
        public MainWindow()
        {
            InitializeComponent();
            Title = "启动器";
            JavaCombox.ItemsSource = JVMSShowList;
            
            FindJvm();
            try
            {
                VersionList = new ObservableCollection<MCCores>(CheckMccores());
                McVersionChoComboBox.ItemsSource = VersionList;
            }
            catch (MCCoresSearchExpection e)
            {
                if (MainControls.DebugAble)
                {
                    MessageBox.Show(Application.Current.MainWindow, e.Message + "\n在" + e.StackTrace, "", MessageBoxButton.OK, MessageBoxImage.Error);
                    MainControls.AddConsoleLine("[debug][errer]"+e.Message + "\n在" + e.StackTrace);
                }
                else
                {
                    MessageBox.Show(Application.Current.MainWindow, e.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            if(MainControls.DebugAble) MainControls.AddConsoleLine("[debug][Info]初始化完毕,开始启动窗口");
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
        private void FindJvm()
        {
            DateTime StartTime = DateTime.Now;
            int jvmlenght = 0;
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
                    string JVMExePAth = System.IO.Path.Combine(System.IO.Path.Combine(JVMPAth, @"bin"), @"java.exe");
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
            if (MainControls.DebugAble)
            {
                MainControls.AddConsoleLine("注册表检索完毕，共" + jvmlenght + "个JVM\n用时" + ts.ToString());
            }
        }

        private void Launch_button_click(object sender, RoutedEventArgs e)
        {
            if (McVersionChoComboBox.SelectedIndex == -1)
            {
                MessageBox.Show(Application.Current.MainWindow, "你没选版本","", MessageBoxButton.OK, MessageBoxImage.Information);
                MainControls.AddConsoleLine("[errer]启动:你没选版本");
                return;
            }
            if (JavaCombox.SelectedIndex == -1)
            {
                MessageBox.Show(Application.Current.MainWindow, "你没选JVM", "", MessageBoxButton.OK, MessageBoxImage.Information);
                MainControls.AddConsoleLine("[errer]启动:你没选JVM");
                return;
            }
            if (String.IsNullOrEmpty(MemBox.Text))
            {
                MessageBox.Show(Application.Current.MainWindow, "你没填写内存", "", MessageBoxButton.OK, MessageBoxImage.Information);
                MainControls.AddConsoleLine("[errer]启动:你没填写内存");
                return;
            }
            if (string.IsNullOrEmpty(WindowHeight.Text) || string.IsNullOrEmpty(WindowWidth.Text))
            {
                MessageBox.Show(Application.Current.MainWindow, "你把窗口大小删了", "", MessageBoxButton.OK, MessageBoxImage.Information);
                MainControls.AddConsoleLine("[errer]启动:你把窗口大小删了");
                return;
            }
            if (System.Convert.ToInt32(WindowHeight.Text)==0 || System.Convert.ToInt32(WindowWidth.Text)==0)
            {
                MessageBox.Show(Application.Current.MainWindow, "你把窗口大小设置成0了", "", MessageBoxButton.OK, MessageBoxImage.Information);
                MainControls.AddConsoleLine("[errer]启动:你把窗口大小删了");
                return;
            }
            
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
        /// <summary>
        /// Http下载文件
        /// </summary>
        
        /// <summary>
        /// 寻找mc核心文件
        /// </summary>
        /// <exception cref="MCCoresSearchExpection">找不到mc的错误</exception>
        public MCCores[] CheckMccores()
        {
            DateTime StartTime = DateTime.Now;
            if (MainControls.DebugAble) MainControls.AddConsoleLine("[debug][Info]开始Mc核心查找");
            if (!System.IO.Directory.Exists(System.IO.Path.Combine(MainControls.ApplicationPath, @".minecraft")))
            {
                throw new MCCoresSearchExpection(1);
            }
            if (!System.IO.Directory.Exists(System.IO.Path.Combine(System.IO.Path.Combine(MainControls.ApplicationPath, @".minecraft"), @"versions")))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(System.IO.Path.Combine(MainControls.ApplicationPath, @".minecraft"), @"versions"));
            }
            if (!System.IO.Directory.Exists(System.IO.Path.Combine(System.IO.Path.Combine(MainControls.ApplicationPath, @".minecraft"), @"libraries")))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(System.IO.Path.Combine(MainControls.ApplicationPath, @".minecraft"), @"libraries"));
            }
            if (!System.IO.Directory.Exists(System.IO.Path.Combine(System.IO.Path.Combine(MainControls.ApplicationPath, @".minecraft"), @"assets")))
            {
                throw new MCCoresSearchExpection(4);
            }

            System.IO.DirectoryInfo VersionsDirInfo = new DirectoryInfo(System.IO.Path.Combine(System.IO.Path.Combine(MainControls.ApplicationPath, @".minecraft"), @"versions"));
            System.IO.DirectoryInfo[] VersionsDirInfos = VersionsDirInfo.GetDirectories();
            MCCores[] Return = new MCCores[VersionsDirInfos.Length];
            for (int ie = 0; ie < Return.Length; ie++)
            {
                Return[ie] = new MCCores();
            }
            int id = 0;
            foreach (var VersionsDirInfoss in VersionsDirInfos)
            {
                Return[id].Path = VersionsDirInfoss.FullName;
                Return[id].Version = VersionsDirInfoss.Name;

                if (!System.IO.File.Exists(System.IO.Path.Combine(VersionsDirInfoss.FullName, VersionsDirInfoss.Name + @".jar")) || System.IO.File.Exists(System.IO.Path.Combine(VersionsDirInfoss.FullName, VersionsDirInfoss.Name + @".json")))
                {
                    Return[id].LaunchAble = false;
                }
                id++;
            }
            DateTime Step1Span = DateTime.Now;
            if (MainControls.DebugAble) MainControls.AddConsoleLine("[debug][Info]Mc核心查找完成,用时为" + (Step1Span - StartTime).ToString());
            for (int ie = 0; ie < Return.Length; ie++)
            {
                string MCversionJsonData = System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Path.Combine(System.IO.Path.Combine(System.IO.Path.Combine(MainControls.ApplicationPath, @".minecraft"), @"versions"), Return[ie].Version), Return[ie].Version + @".json"), Encoding.UTF8);
                CsharpJson.JsonDocument MCversionJsonDataadoc = CsharpJson.JsonDocument.FromString(MCversionJsonData);
                CsharpJson.JsonObject MCclientVersionDataobj = MCversionJsonDataadoc.Object;
                Return[ie].VersionTyte = MCclientVersionDataobj.Value("assets").ToString();
                
                int McversionTypeInt = System.Convert.ToInt32(Return[ie].VersionTyte.Split(new string[] { "." }, StringSplitOptions.None)[0] + Return[ie].VersionTyte.Split(new string[] { "." }, StringSplitOptions.None)[1]);
                if (McversionTypeInt == 117)
                {
                    Return[ie].Log4JFixLevel = 3;
                }
                else if (McversionTypeInt > 17 && McversionTypeInt < 111)
                {
                    Return[ie].Log4JFixLevel = 1;

                    MainControls.HttpDownloadFile("https://launcher.mojang.com/v1/objects/dd2b723346a8dcd48e7f4d245f6bf09e98db9696/log4j2_17-111.xml", System.IO.Path.Combine(Return[ie].Path, @"log4j2_Config.xml"));
                    Return[ie].LaunchAble = true;
                }
                else if (McversionTypeInt > 1.12 && McversionTypeInt < 1.16)
                {
                    Return[ie].Log4JFixLevel = 2;

                    MainControls.HttpDownloadFile("https://launcher.mojang.com/v1/objects/02937d122c86ce73319ef9975b58896fc1b491d1/log4j2_112-116.xml", System.IO.Path.Combine(Return[ie].Path, @"log4j2_Config.xml"));
                    Return[ie].LaunchAble = true;
                }
                else if (McversionTypeInt < 17)
                {
                    Return[ie].LaunchAble = false;
                }
                else
                {
                    Return[ie].Log4JFixLevel = 0;
                    MainControls.HttpDownloadFile("https://launcher.mojang.com/v1/objects/bd65e7d2e3c237be76cfbef4c2405033d7f91521/client-1.12.xml", System.IO.Path.Combine(Return[ie].Path, @"log4j2_Config.xml"));
                    Return[ie].LaunchAble = true;
                }
                DateTime Step2Span = DateTime.Now;
                if (MainControls.DebugAble)
                {
                    MainControls.AddConsoleLine("[debug][Info]所有Mc核心的log4j配置完成,用时为" + (Step2Span - Step1Span).ToString());
                    MainControls.AddConsoleLine("[debug][Info]开始寻找所有Mc核心的部分配置");
                }

                if (McversionTypeInt > 113)
                {
                    CsharpJson.JsonValue arguments = MCclientVersionDataobj.Value("arguments");
                    CsharpJson.JsonArray arguments_game_list = arguments.ToObject().Value("game").ToArray();
                    string Arg = string.Empty;
                    for (int i = 0, j = 0; i < arguments_game_list.Count - 2; i++)
                    {
                        if (j < 2)
                        {
                            Arg += arguments_game_list.Value(i).ToString();
                            j++;
                        }
                        else
                        {
                            Arg += " ";
                            j = 0;
                            i--;
                        }
                    }
                    Return[ie].Args = Arg;
                }
                else
                {
                    string arguments = MCclientVersionDataobj.Value("minecraftArguments").ToString();
                    Return[ie].Args = arguments;
                }
                Return[ie].MainClass = MCclientVersionDataobj.Value("mainClass").ToString();
                Return[ie].Type = MCclientVersionDataobj.Value("type").ToString();
                DateTime Step3Span = DateTime.Now;
                if (MainControls.DebugAble)
                {
                    MainControls.AddConsoleLine("[debug][Info]所有Mc核心的部分参数配置完成,用时为" + (Step3Span - Step2Span).ToString());
                    MainControls.AddConsoleLine("[debug][Info]开始阅览所有Mc核心的运行库并尝试修复");
                }
                CsharpJson.JsonArray McClientLibrary = MCclientVersionDataobj.Value("libraries").ToArray();
                McLibrary[] Mclib = new McLibrary[McClientLibrary.Count];
                for (int i = 0; i < Mclib.Length; i++) Mclib[i] = new McLibrary();
                for (int i = 0; i < McClientLibrary.Count; i++)
                {
                    try
                    {
                        Mclib[i].IsNative = (McClientLibrary.Value(i).ToObject().Value("natives").IsObject() || McClientLibrary.Value(i).ToObject().Value("name").ToString().Split(new string[] { ":" }, StringSplitOptions.None)[2] == "natives-windows" || McClientLibrary.Value(i).ToObject().Value("name").ToString().Split(new string[] { ":" }, StringSplitOptions.None)[2] == "natives-windows-x86");

                    }
                    catch (System.FormatException)
                    {
                        continue;
                    }
                    catch (System.Collections.Generic.KeyNotFoundException)
                    {
                        continue;
                    }

                    Mclib[i].name = McClientLibrary.Value(i).ToObject().Value("name").ToString();
                    if (McClientLibrary.Value(i).ToObject().Value("natives").IsObject())
                    {
                        try
                        {
                            if (McClientLibrary.Value(i).ToObject().Value("downloads").ToObject().Value("classifiers").ToObject().Value("natives-windows").IsObject())
                            {
                                Mclib[i].path = System.IO.Path.Combine(System.IO.Path.Combine(System.IO.Path.Combine(MainControls.ApplicationPath, @".minecraft"), @"libraries"), McClientLibrary.Value(i).ToObject().Value("downloads").ToObject().Value("classifiers").ToObject().Value("natives-windows").ToObject().Value("path").ToString());
                                Mclib[i].url = McClientLibrary.Value(i).ToObject().Value("downloads").ToObject().Value("classifiers").ToObject().Value("natives-windows").ToObject().Value("url").ToString();
                            }
                        }
                        catch (System.Collections.Generic.KeyNotFoundException)
                        {
                            if (McClientLibrary.Value(i).ToObject().Value("downloads").ToObject().Value("classifiers").ToObject().Value("natives-windows-64").IsObject())
                            {
                                Mclib[i].path = System.IO.Path.Combine(System.IO.Path.Combine(System.IO.Path.Combine(MainControls.ApplicationPath, @".minecraft"), @"libraries"), McClientLibrary.Value(i).ToObject().Value("downloads").ToObject().Value("classifiers").ToObject().Value("natives-windows-64").ToObject().Value("path").ToString());
                                Mclib[i].url = McClientLibrary.Value(i).ToObject().Value("downloads").ToObject().Value("classifiers").ToObject().Value("natives-windows-64").ToObject().Value("url").ToString();
                            }
                        }
                    }
                    else
                    {
                        Mclib[i].path = System.IO.Path.Combine(System.IO.Path.Combine(System.IO.Path.Combine(MainControls.ApplicationPath, @".minecraft"), @"libraries"), McClientLibrary.Value(i).ToObject().Value("downloads").ToObject().Value("artifact").ToObject().Value("path").ToString());
                        Mclib[i].url = McClientLibrary.Value(i).ToObject().Value("downloads").ToObject().Value("artifact").ToObject().Value("url").ToString();
                    }

                    if (!System.IO.File.Exists(Mclib[i].path))
                    {
                        MainControls.AddConsoleLine("[debug][Info]未找到\'"+ Mclib[i].name+"\',开始下载");
                        if (!System.IO.Directory.Exists(System.IO.Directory.GetParent(Mclib[i].path).ToString()))
                        {
                            System.IO.Directory.CreateDirectory(System.IO.Directory.GetParent(Mclib[i].path).ToString());
                        }
                        MainControls.HttpDownloadFile(Mclib[i].url, Mclib[i].path);
                    }
                }
                Return[ie].Libraries = Mclib;
                DateTime StopSpan = DateTime.Now;
                if (MainControls.DebugAble)
                {
                    MainControls.AddConsoleLine("[debug][Info]所有Mc核心的库配置完成,用时为" + (StopSpan - Step3Span).ToString());
                    MainControls.AddConsoleLine("[debug][Info]操作完成,总用时为" + (StopSpan - StartTime).ToString());
                }
            }
            return Return;
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
                JvmJavawexepath = System.IO.Path.Combine(System.IO.Path.Combine(path, @"bin"), @"javaw.exe");
            }
            public string JavaType { get; set; }
            public string JvmPath { get; set; }
            public string JvmVersion { get; set; }
            public string JvmJavaexepath { get; set; }
            public string JvmJavawexepath { get; set; }
        }
        public class McLibrary
        {
            public string path { get; set; }
            public string name { get; set; }
            public string url { get; set; }
            public bool IsNative { get; set; }
        }

        public class MCCores
        {
            public McLibrary[] Libraries { get; set; }
            public string VersionTyte { get; set; }
            public string Version { get; set; }
            public string Args { get; set; }
            public string LaunchArgs { get;set; }
            public string MainClass { set; get; }
            public string Type { set; get; }
            public string Path { set; get; }
            public bool LaunchAble { get; set; }
            /// <summary>
            /// 0:无需修复
            /// <br/>
            /// 1:https://launcher.mojang.com/v1/objects/dd2b723346a8dcd48e7f4d245f6bf09e98db9696/log4j2_17-111.xml
            /// <br/>
            /// -Dlog4j.configurationFile=log4j2_Config.xml
            /// <br/>
            /// 2:https://launcher.mojang.com/v1/objects/02937d122c86ce73319ef9975b58896fc1b491d1/log4j2_112-116.xml
            /// <br/>
            /// -Dlog4j.configurationFile=log4j2_Config.xml
            /// <br/>
            /// 3:-Dlog4j2.formatMsgNoLookups=true
            /// </summary>
            public int Log4JFixLevel { get; set; }
            void GetLaunchArgs(MCBugjumpLoginToken MCBugjumpLoginToken)
            {
                
            }
            void GetLaunchArgs(MCMicrosoftLoginToken MCMicrosoftLoginToken)
            {

            }
            void GetLaunchArgs(string uuid,string name)
            {
               LaunchArgs= LaunchArgs.Replace("${auth_player_name}",name).Replace("${version_name}",Version).Replace("${game_directory}",Path).Replace("${assets_root}",
                 System.IO.Path.Combine(System.IO.Path.Combine(MainControls.ApplicationPath, @".minecraft"), @"assets")).Replace("${assets_index_name}",VersionTyte).Replace(
                 "${auth_uuid}",uuid).Replace("${auth_access_token}","0");
            }
        }
        public class MCBugjumpLoginToken
        {
            public string UserUUID { get; set; }
            public string accessToken { get; set; }
            public string UserName { get; set; }
        }
        public class MCMicrosoftLoginToken
        {
            public string MicrosoftToken { get; set; }
            public string MicrosoftAccess_token { get; set; }
            public string MicrosoftRefresh_token { get; set; }
            public string XboxLiveToken { get; set; }
            public string XboxLiveUserHash { get; set; }
            public string XstsToken { get; set; }
            public string McAccessToken { get; set; }
            public bool HaveMcAble { get; set; }
            public string UserName { get; set; }
            public string UserUUID { get; set; }
        }
        public class MCCoresSearchExpection : ApplicationException
        {
            /// <summary>
            /// 构造错误信息
            /// </summary>
            /// <param name="ErrerType">1:未找到.minecraft文件夹<br/>2:未找到.minecraft\versions文件夹<br/>3:未找到.minecraft\libraries文件夹<br/>未找到.minecraft\assets文件夹</param>
            public MCCoresSearchExpection(int ErrerType)
            {
                Type = ErrerType;
                switch (ErrerType)
                {
                    case 1:
                        {
                            Message = "未找到.minecraft文件夹";
                        }
                        break;
                    case 2:
                        {
                            Message = "未找到.minecraft\\versions文件夹";
                        }
                        break;
                    case 3:
                        {
                            Message = "未找到.minecraft\\libraries文件夹";
                        }
                        break;
                    case 4:
                        {
                            Message = "未找到.minecraft\\assets文件夹";
                        }
                        break;
                    default:
                        {
                            Message = "内部错误";
                        }
                        break;
                }
            }
            public string Message { get; set; }
            public int Type { get; set; }
        }
    }
}
