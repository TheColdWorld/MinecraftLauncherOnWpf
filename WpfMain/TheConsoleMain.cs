using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WpfMain;

namespace WpfMain
{
    internal class TheConsoleMain
    {
        [STAThread]
        static void Main(string[] args)
        {
            MainControls.DebugAble = false;
            MainControls.ConsoleMode = false;
            if (args.Length != 0)
            {
                foreach (string str in args)
                {
                    switch (str)
                    {
                        case "-console":
                            {
                                MainControls.ConsoleMode = true;
                                TheConsoleMain.ConsoleMain(args);

                            }
                            break;
                        case "-debug":
                            { 
                                MainControls.DebugAble = true;
                                MainControls.DebugLevel = 1;
                            }
                            break;
                        case "-debugLevel:1":
                            {
                                MainControls.DebugLevel = 1;
                            }
                            break;
                        case "-debugLevel:2":
                            {
                                MainControls.DebugLevel = 2;
                            }
                            break;
                    }
                }
            }
            if (!MainControls.ConsoleMode)
            {
                Console.Title = "启动主程序中";
                MainControls.ConsoleHWND = MainControls.FindWindow("ConsoleWindowClass", "启动主程序中");
                if (!MainControls.DebugAble)
                {
                    MainControls.ShowWindow(MainControls.ConsoleHWND, 0);
                    MainControls.ConsoleShowAble = false;
                }
                MainControls.MainApplication = new App();
                MainControls.MainApplication.InitializeComponent();
                MainControls.MainApplication.Run();
            }
        }
        static void ConsoleMain(string[] args)
        {

        }
    }
    public static class MainControls
    {
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();
        public static IntPtr ConsoleHWND;
        public static bool ConsoleShowAble;
        public static bool ConsoleMode { get; set; }
        public static MainWindow MainWindow;
        public static bool DebugAble { get; set; }
        public static int DebugLevel { get; set; }
        public static App MainApplication;
        public static string ApplicationPath = System.AppDomain.CurrentDomain.BaseDirectory;
        public static void AddConsole(string Message)
        {
            Console.Write(Message);
        }
        public static void AddConsoleLine(string Message)
        {
            Console.WriteLine(Message);
        }
        public static void ShowConsole()
        {
            if (ConsoleMode) return;
            if (!ConsoleShowAble)
            {
                ShowWindow(ConsoleHWND, 1);
                ConsoleShowAble = true;
            }
            else
            {
                ShowWindow(ConsoleHWND, 0);
                ConsoleShowAble = false;
            }
        }
        public static string HttpDownloadFile(string url, string path)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
            //创建本地文件写入流
            Stream stream = new FileStream(path, FileMode.Create);
            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr, 0, (int)bArr.Length);
            while (size > 0)
            {
                stream.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, (int)bArr.Length);
            }
            stream.Close();
            responseStream.Close();
            return path;
        }
    }
}
