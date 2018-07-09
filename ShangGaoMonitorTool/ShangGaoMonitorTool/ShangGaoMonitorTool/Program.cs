using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShangGaoMonitorTool
{
    static class Program
    {
        static LoginForm loginForm;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            loginForm = new LoginForm();
            // new System.Threading.Thread(ShowLoginFrom).Start();
            loginForm.ShowDialog();

            if (loginForm.DialogResult == DialogResult.OK)
            {
                Application.Run(new Form1());
            }
            else
            {
                return;
            }
           
            
           
        }

        public static void ShowLoginFrom()
        {
           
            loginForm.ShowDialog();
        }
    }
}
