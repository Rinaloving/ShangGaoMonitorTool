using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShangGaoMonitorTool
{
    public partial class ProgressBar : Form
    {
        public ProgressBar(int vMax)
        {
            InitializeComponent();
            this.progressBar1.Maximum = vMax;
        }

        private void ProgressBar_Load(object sender, EventArgs e)
        {

        }

        public bool Increase(int nValue, string nInfo)
        {
            if (nValue > 0)
            {
                if (progressBar1.Value + nValue < progressBar1.Maximum)
                {
                    progressBar1.Value += nValue;
                    this.label1.Text = "正在移动报文数量：" + nInfo;
                    Application.DoEvents();
                    progressBar1.Update();
                    progressBar1.Refresh();
                    this.label1.Update();
                    this.label1.Refresh();
                    return true;
                }
                else
                {
                    progressBar1.Value = progressBar1.Maximum;
                    this.label1.Text = nInfo;
                    this.Close();//执行完之后，自动关闭子窗体
                    return false;
                }
            }
            return false;
        }



    }
}
