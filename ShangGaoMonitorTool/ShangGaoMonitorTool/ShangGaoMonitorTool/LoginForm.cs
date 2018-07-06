using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ShangGaoMonitorTool
{
   
    public partial class LoginForm : Form
    {
        public static DialogResult checkResult = DialogResult.OK; //用来判断连接是否成功
        public LoginForm()
        {
            InitializeComponent();
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            if (checkResult == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;    //返回一个登录成功的对话框状态
                this.Close();    //关闭登录窗口
               
            }
            else
            {
                MessageBox.Show("无法登录！");
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if ((int)e.KeyCode == 13)
            {
                e.SuppressKeyPress = true;
            }

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((int)e.KeyCode == 13)
            {
                e.SuppressKeyPress = true;
            }

        }

       
    }
}
