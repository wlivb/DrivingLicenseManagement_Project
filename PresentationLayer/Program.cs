using PresentationLayer.Global_Classes;
using PresentationLayer.Login;
using System;
using System.Windows.Forms;

namespace PresentationLayer
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            while (true)
            {
                frmLogin login = new frmLogin();

                if (login.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new frmMain());

                    if (clsGlobal.CurrentUser == null)
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
}


