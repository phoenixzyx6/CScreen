using Accord.Video.FFMPEG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CScreen
{
    public partial class Form1 : Form
    {
        bool run = true;

        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread tcpServer = new Thread(new ThreadStart(CS));
            tcpServer.IsBackground = true;
            tcpServer.Start();
            button1.Enabled = false;
            button2.Enabled = true;
        }

        public static void CS()
        {
            string Path = ConfigurationManager.AppSettings["Path"];
            string STime = ConfigurationManager.AppSettings["STime"];
            string DTime = ConfigurationManager.AppSettings["DTime"];
            string CTime = ConfigurationManager.AppSettings["CTime"];
            while (true)
            {
                /*初始化屏幕截图工具，尺寸设置为全屏录制*/
                Accord.Video.ScreenCaptureStream screenShot = new Accord.Video.ScreenCaptureStream(new Rectangle(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));
                /*初始化视频写入工具*/
                VideoFileWriter videoWriter = new VideoFileWriter();
                /*创建视频文件，并设置相关尺寸、帧率、视频格式、码率（影响清晰度）*/
                videoWriter.Open(Path + DateTime.Now.ToString("yyMMddHHmmss") + ".avi", Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, 1000/ Convert.ToInt32(CTime), VideoCodec.MSMPEG4v3, 4000 * 1024);
                /*截屏的频率，单位毫秒*/
                screenShot.FrameInterval = Convert.ToInt32(CTime);
                /*截屏成功后写入视频*/
                screenShot.NewFrame += (s, e1) =>
                {
                    videoWriter.WriteVideoFrame(e1.Frame);
                };
                screenShot.Start();

                Thread.Sleep(Convert.ToInt32(STime));

                /*停止截屏*/
                screenShot.Stop();
                /*停止视频写入*/
                videoWriter.Close();

                //文件夹路径
                DirectoryInfo dyInfo = new DirectoryInfo(Path);
                //获取文件夹下所有的文件
                foreach (FileInfo feInfo in dyInfo.GetFiles())
                {
                    //判断文件日期是否小于今天，是则删除
                    if (feInfo.CreationTime < DateTime.Today.AddDays(-Convert.ToInt32(DTime)))
                        feInfo.Delete();
                }

                screenShot = null;
                videoWriter = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            run = false;
            
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
