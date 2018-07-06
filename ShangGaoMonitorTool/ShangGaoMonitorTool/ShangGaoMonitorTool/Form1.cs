using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ShangGaoMonitorTool
{
    public partial class Form1 : Form
    {
        //private Queue<double> dataQueue = new Queue<double>(7);
        private Queue<double> dataQueue = new Queue<double>(7);

        private Queue<string> dtq = new Queue<string>(7);

        private static int filenums = 0;

        private int curValue = 0;

        private int num = 1;//每次删除增加几个点

        private static int datenum = -1; //日期初始值

        private static int localtodayfilenum = 1; // 当日本地报文的数量

        private static int sftptodayfilenum  =  2; //  省前置机当日报文数量


        static System.Timers.Timer timer;
        //string[] dt = new string[7];
        DateTime date = DateTime.Now.Date;


        public Form1()
        {
            InitializeComponent();
            initShowPie();
            CheckForIllegalCrossThreadCalls = false;  //组件之间可以调用
        }


        /// <summary>
        /// 初始化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInit_Click(object sender, EventArgs e)
        {
            InitChart();
        }
        ///// <summary>
        ///// 开始事件
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btnStart_Click(object sender, EventArgs e)
        //{
        //    this.timer1.Start();
        //}
        ///// <summary>
        ///// 停止事件
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btnStop_Click(object sender, EventArgs e)
        //{
        //    this.timer1.Stop();
        //}

        /// <summary>
        /// 初始化图表
        /// </summary>
        private void InitChart()
        {
            

            //dt[0] = date.AddDays(-1).ToString("M");
            //dt[1] = date.AddDays(0).ToString("M");
            //dt[2] = date.AddDays(1).ToString("M");
            //dt[3] = date.AddDays(2).ToString("M");
            //dt[4] = date.AddDays(3).ToString("M");
            //dt[5] = date.AddDays(4).ToString("M");
            //dt[6] = date.AddDays(5).ToString("M");
            //定义图表区域
            this.chart1.ChartAreas.Clear();
            ChartArea chartArea1 = new ChartArea("C1");
            this.chart1.ChartAreas.Add(chartArea1);
            //定义存储和显示点的容器
            this.chart1.Series.Clear();
            Series series1 = new Series("数量（件）");
            series1.ChartArea = "C1";
            this.chart1.Series.Add(series1);
            //设置图表显示样式
            this.chart1.ChartAreas[0].AxisY.Minimum = 0;
            this.chart1.ChartAreas[0].AxisY.Maximum = 100;
            //this.chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
            this.chart1.ChartAreas[0].AxisX.Minimum = 1; //坐标最小值 
            this.chart1.ChartAreas[0].AxisX.Maximum = 7;//坐标最大值
            //this.chart1.ChartAreas[0].AxisX.Interval = 1;//坐标大刻度间隔
            //this.chart1.ChartAreas[0].AxisX.MinorTickMark.Interval = 1;//小刻度间隔，就像厘米下的毫米一样
            this.chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            //this.chart1.ChartAreas[0].AxisX.LabelStyle.Format = DateTime.Now.GetDateTimeFormats('f')[0].ToString();  //GetDateTimeFormats('M')[0].ToString();//
            // this.chart1.ChartAreas[0].AxisX.LabelStyle.Format = DateTime.Now.GetDateTimeFormats('M')[0].ToString();
            //this.chart1.AlignDataPointsByAxisLabel.ActiveControl= DateTime.Now.GetDateTimeFormats('M')[0].ToString();
           
            //for (int i = 0; i < dt.Length; i++)
            //{
            //    this.chart1.ChartAreas[0].AxisX.LabelStyle.Format = dt[i];  // XValueMember 
            //                                                                //this.chart1.ChartAreas[0].AxisX. = dt[i];  // XValueMember 

            //}


            //11月5日; // 11月5日 DateTime.Now.AddDays(1).ToShortDateString();
            // this.chart1.ChartAreas[0].AxisX.LabelStyle.Angle = "一个";
            // DateTime.Now.GetDateTimeFormats('M').ToString()   DateTime.Now.AddDays(1).ToShortDateString()

            //设置标题
            this.chart1.Titles.Clear();
            this.chart1.Titles.Add("");
            this.chart1.Titles[0].Text = "";
            this.chart1.Titles[0].ForeColor = Color.RoyalBlue;
            this.chart1.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            //设置图表显示样式
            this.chart1.Series[0].Color = Color.Red;
          
            this.chart1.Titles[0].Text = string.Format("");
            this.chart1.Series[0].ChartType = SeriesChartType.Line;
            this.chart1.Series[0].BorderWidth = 2;
            this.chart1.Series[0].MarkerColor = Color.Red;       //标记点中心颜色
            this.chart1.Series[0].MarkerSize = 5;                 //标记点大小
            this.chart1.Series[0].MarkerStyle = MarkerStyle.Circle;  //标记点类型
            this.chart1.Series[0].Label = "#VAL";            //设置显示X Y的值
            this.chart1.Series[0].ToolTip = "#VALX\r#VAL件";     //鼠标移动到对应点显示数值


            this.chart1.Series[0].Points.Clear();
        }

        //饼状图
        public void initShowPie()
        {

            //Title title1 = new Title();
            // title1.Text = "总合格率（%）";
            //title1.ForeColor = Color.Blue;
            //this.chart3.Titles.Add(title1);


            this.chart3.Series.Clear();
            Series series1 = new Series();
            series1.ChartType = SeriesChartType.Pie;
            this.chart3.Series.Add(series1);

            int totalnum = localtodayfilenum + sftptodayfilenum;

            int[] percents = new int[2] { (100*localtodayfilenum / totalnum), (100* sftptodayfilenum / totalnum) };
            string[] pointsText = new string[2] { "本地", "省里" };
            Color[] myColor = new Color[2] {  Color.FromArgb(250, 205, 137),
                                                Color.FromArgb(255, 124, 255)};
            DataPoint[] AllPoints = new DataPoint[2];
            for (int i = 0; i < AllPoints.Length; i++)
            {
                AllPoints[i] = new DataPoint(i + 1, percents[i]);
                AllPoints[i].LegendText = pointsText[i];
                AllPoints[i].Color = myColor[i];
                AllPoints[i].ToolTip = percents[i].ToString() + "%";
                AllPoints[i].Label = percents[i].ToString() + "%";
                AllPoints[i].LabelForeColor = Color.Black;
                this.chart3.Series[0].Points.Add(AllPoints[i]);
            }
            this.chart3.Series[0]["PieLabelStyle"] = "Outside";
            this.chart3.EndInit();

        }
        //更新队列中的值
        private  void UpdateQueueValue()
        {

            if (dataQueue.Count >= 7)
            {
                //先出列
                for (int i = 0; i < num; i++)
                {
                    dataQueue.Dequeue();
                    dtq.Dequeue();
                }
            }
           
            //Random r = new Random();
            for (int i = 0; i < num; i++)
            {
                //对curValue只取[0,360]之间的值
               // curValue = curValue % 360;
                //对得到的正玄值，放大50倍，并上移50
               // dataQueue.Enqueue((filenums * Math.Sin(curValue * Math.PI / 180)) + filenums);
               // curValue = curValue + 10;
                dataQueue.Enqueue(filenums); // dataQueue.Enqueue(r.Next(0, 100));
                Thread.Sleep(2000);
                dtq.Enqueue(date.AddDays(datenum++).ToString("M"));
            }
          
        }



        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           // UpdateQueueValue();
            InitChart();
            UpdateQueueValue();
            this.chart1.Series[0].Points.Clear();
            for (int i = 0; i < dataQueue.Count; i++)
            {
                //this.chart1.Series[0].Points.AddXY((i + 1), dataQueue.ElementAt(i)); //dt
                this.chart1.Series[0].Points.AddXY(dtq.ElementAt(i), dataQueue.ElementAt(i));

            }

            schedule_Timer();
            
        }

        string specifiedHour = System.Configuration.ConfigurationManager.AppSettings["specifiedHour"].ToString();
        string specifiedMinute = System.Configuration.ConfigurationManager.AppSettings["specifiedMinute"].ToString();
        public void schedule_Timer()
        {
            Console.WriteLine("### Timer Started ###");

            DateTime nowTime = DateTime.Now;
            DateTime scheduledTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, Int32.Parse(specifiedHour), Int32.Parse(specifiedMinute), 0, 0); //指定预设时间  HH,MM,SS [8am and 42 minutes]
            if (nowTime > scheduledTime)
            {
                scheduledTime = scheduledTime.AddDays(1);

            }

            double tickTime = (double)(scheduledTime - DateTime.Now).TotalMilliseconds;  //每天定时触发
            timer = new System.Timers.Timer(tickTime);  //测试时间 60000
            timer.Enabled = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
         
            timer.Start();
        }
        string fileBacksPath = System.Configuration.ConfigurationManager.AppSettings["fileBacksPath"].ToString();
        string fileUpPath = System.Configuration.ConfigurationManager.AppSettings["fileUpPath"].ToString();
        public void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
           
           
            removeUnUploadFile(fileBacksPath, fileUpPath);
            //InitChart();
            UpdateQueueValue();
            this.chart1.Series[0].Points.Clear();
            for (int i = 0; i < dataQueue.Count; i++)
            {
                //this.chart1.Series[0].Points.AddXY((i + 1), dataQueue.ElementAt(i));
                this.chart1.Series[0].Points.AddXY(dtq.ElementAt(i), dataQueue.ElementAt(i));

            }
            schedule_Timer();
        }


        string provinceIP = System.Configuration.ConfigurationManager.AppSettings["provinceIP"].ToString();
        string provincePort = System.Configuration.ConfigurationManager.AppSettings["provincePort"].ToString();
        string username = System.Configuration.ConfigurationManager.AppSettings["username"].ToString();
        string password = System.Configuration.ConfigurationManager.AppSettings["password"].ToString();
        string provinceBackFilePath = System.Configuration.ConfigurationManager.AppSettings["provinceBackFilePath"].ToString();
        /// <summary>
        /// 移动未上传成功的文件
        /// </summary>
        /// <param name="fileBacksPath"></param>
        /// <param name="fileUpPath"></param>
        public  void removeUnUploadFile(string fileBacksPath, string fileUpPath)
        {
            //Console.WriteLine(DateTime.Today); //2018/7/3/0000
           

            FileInfo[] fileInfos = new DirectoryInfo(fileBacksPath).GetFiles("*.xml");

            FileInfo[] localTodayFileInfos = getLocalTodatyFile(fileInfos);

            if (localTodayFileInfos!=null)
            {
                List<string> newfileInfos = fileFilter(provinceIP, provincePort, username, password, localTodayFileInfos, provinceBackFilePath);
                filenums = newfileInfos.Count();
                if (newfileInfos.Count() > 0)
                {
                    try
                    {
                        foreach (var item in localTodayFileInfos)
                        {
                            foreach (var file in newfileInfos)
                            {
                                //如果文件名相同的就移出去，(由上报工具)重新上报
                                if (stringEx(file).Equals(stringEx(item.ToString())))
                                {
                                    File.Move(item.FullName, fileUpPath + item.Name);
                                }
                            }
                        } 

                    }
                    catch (Exception ex)
                    {

                        throw new Exception(string.Format("文件移出失败，原因：{0}", ex.Message));
                    }

                }

            }
            else
            {
                //表示今天一个报文没有生成
                filenums = 0;
            }

            //本地当日报文

            //var todayLocalFiles = from s in fileInfos
            //                      where s.LastWriteTime > DateTime.Today
            //                      select s;


           
               // filenums = fileInfos.Count();
                //如果这个文件夹下存在文件
                //if (newfileInfos.Count() > 0)
                //{
                //    foreach (var item in fileInfos)
                //    {
                //        //Console.WriteLine("文件全路径：" + item.FullName);
                //        //Console.WriteLine("文件：" + item.Name);
                //        //Console.WriteLine("文件修改时间：" + item.LastWriteTime);
                //        //如果文件的创建时间大于当天子时时间，说明是今天创建的，那么就移动到Up文件夹下，由上报工具重新上传
                //        if (DateTime.Today < item.LastWriteTime)
                //        {
                //            //把这些文件移动到Up文件夹下
                            


                //        }
                //        else
                //        {
                //            // 表示这个文件不是本地当日生存报文，的到的总数量减1
                //            filenums--;
                //        }
                //    }
                //}
     


        }


        /// <summary>
        /// 对比本地backs文件下xml报文和省前置机上RepMsg文件夹下的报文如果有不存在的就移动出来   
        /// </summary>
        public List<string> fileFilter(string ip, string port, string username, string password, FileInfo[] fileInfos, string filepath)
        {
            //连接SFTP
            SFTPHandler sftp = new SFTPHandler(ip, port, username, password);
            if (!sftp.Connected)
            {
                sftp.Connect();
            }
            //获取SFTP当天的报文  
            ArrayList fileList =  sftp.GetFileListByCreateTime(filepath,".xml");
            List<string> sftpfileList = new List<string>();
            foreach (var file in fileList)
            {
                sftpfileList.Add(stringEx(file.ToString()));
            }
            //把本地读取的报文文件放入ArryList集合中
            List<string> localFileList = new List<string>();
            foreach (var file in fileInfos)
            {
                localFileList.Add(stringEx(file.ToString()));
            }

            if (sftpfileList.Count() > 0)
            {
                //取本地当日文件集合和SFTP上面当日文件集合的差集

                localFileList = localFileList.Except(sftpfileList).ToList();

            }

            sftp.Disconnect();
            return localFileList;

        }



        public FileInfo[] getLocalTodatyFile(FileInfo[] fileInfos)
        {
            FileInfo[] localTodayFile = null; 
            int star = fileInfos.Length;
            int index = 0;
            for (int i = 0; i < fileInfos.Length; i++)
            {
                if (DateTime.Today > fileInfos[i].LastWriteTime)
                {
                    star--;
                   // localTodayFile[i] = fileInfos[i];
                }
            }
            localTodayFile = star == 0 ? null : new FileInfo[star]; //为了确实有效数组的长度

            if (localTodayFile != null)
            {
                for (int i = 0; i < fileInfos.Length; i++)
                {
                    if (DateTime.Today < fileInfos[i].LastWriteTime)
                    {
                        
                         localTodayFile[index] = fileInfos[i];
                         index++;
                    }
                }

                return localTodayFile;
            }

            return null;
        }


        /// <summary>
        /// 去除报文的名称的英文和. 
        /// </summary>
        /// <param name="str"></param>
        public string stringEx(string str)
        {
            Regex reg = new Regex(@"[a-zA-Z|.]");

            if (reg.IsMatch(str))
            {
                str = Regex.Replace(str, @"[a-zA-Z|.]", "");
            }
            return str;
        }

        //创建代理。
        private ProgressBar myProcessBar = null;//弹出的子窗体(用于显示进度条)
        private delegate bool IncreaseHandle(int nValue, string vinfo);//代理创建
        private IncreaseHandle myIncrease = null;//声明代理，用于后面的实例化代里
        private int vMax = 100;//用于实例化进度条，可以根据自己的需要，自己改变

        private void ThreadFun()
        {

            //获取本地当日报文的数量
            FileInfo[] fileInfos = new DirectoryInfo(fileBacksPath).GetFiles("*.xml");

            FileInfo[] localTodayFileInfos = getLocalTodatyFile(fileInfos);

            localtodayfilenum = localTodayFileInfos == null?0: localTodayFileInfos.Count();
            //如果是周末或0:00分，就设为初始值
            if (localtodayfilenum == 0) { localtodayfilenum = 1; }

            // 获取sftp当日报文的数量  localTodayFileInfos, provinceBackFilePath
            //连接SFTP
            SFTPHandler sftp = new SFTPHandler(provinceIP, provincePort, username, password);
            if (!sftp.Connected)
            {
                sftp.Connect();
            }
            //获取SFTP当天的报文  
            ArrayList fileList = sftp.GetFileListByCreateTime(provinceBackFilePath, ".xml");
            sftptodayfilenum = fileList.Count;
            //如果是周末或0:00分，就设为初始值
            if (sftptodayfilenum == 0) { sftptodayfilenum = 2; }
            sftp.Disconnect();

            vMax = (localtodayfilenum - sftptodayfilenum)<=0?1: (localtodayfilenum - sftptodayfilenum); //如果vMax为0就设置为1，不然进度条会卡死

            initShowPie();

            removeUnUploadFile(fileBacksPath, fileUpPath);


            MethodInvoker mi = new MethodInvoker(ShowProcessBar);
            this.BeginInvoke(mi);
            Thread.Sleep(100);
            object objReturn = null;
            for (int i = 0; i < vMax; i++)
            {
                objReturn = this.Invoke(this.myIncrease, new object[] { 1, i.ToString() + "\r\n" });
                Thread.Sleep(1000);
            }
        }
        private void ShowProcessBar()
        {
            myProcessBar = new ProgressBar(vMax);
            myIncrease = new IncreaseHandle(myProcessBar.Increase);
            myProcessBar.ShowDialog();
            myProcessBar = null;
        }


        private void button1_Click(object sender, EventArgs e)
        {

            Thread thdSub = new Thread(new ThreadStart(ThreadFun));
            thdSub.Start();
        }
    }
}
