using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace CScreen
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (DateTime.Now<Convert.ToDateTime("2021-01-01")) {
                Thread tcpServer = new Thread(new ThreadStart(Form1.CS))
                {
                    IsBackground = true
                };
                tcpServer.Start();
            }

            Application.Run();

        }

    }
}
