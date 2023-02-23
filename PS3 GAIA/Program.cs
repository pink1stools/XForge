using DarkUI.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace XForge
{
    static class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)

        {
            AttachConsole(ATTACH_PARENT_PROCESS);
            if (args.Length == 1 && File.Exists(args[0]))

            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.AddMessageFilter(new ControlScrollFilter());
                Application.Run(new Form2(args[0]));
                //Console.WriteLine("welcome!");

                //Console.ReadLine();

            }

            else

            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.AddMessageFilter(new ControlScrollFilter());
                Application.Run(new Form2());
            }
        }
    }
}
