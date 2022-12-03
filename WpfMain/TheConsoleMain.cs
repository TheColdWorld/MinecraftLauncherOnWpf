using System;
using System.Collections.Generic;
using System.Linq;
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
                            { MainControls.DebugAble = true; }
                            break;
                    }
                }
            }
            if (!MainControls.ConsoleMode)
            {
                Console.Title = "启动主程序中";
                if (!MainControls.DebugAble)
                {
                    IntPtr intptr = MainControls.FindWindow("ConsoleWindowClass", "启动主程序中");
                    MainControls.ShowWindow(intptr, 0);
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
        public static bool ConsoleMode { get; set; }
        public static MainWindow MainWindow;
        public static bool DebugAble { get; set; }
        public static App MainApplication;
    }
}
