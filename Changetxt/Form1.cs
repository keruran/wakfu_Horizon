using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
namespace Changetxt
{
    public partial class Form1 : Form
    {
        //调用大漠
        //CDmSoft dm = new CDmSoft();
        #region
        //创建NotifyIcon对象 
        NotifyIcon notifyicon = new NotifyIcon();
        //创建托盘图标对象 
      //  Icon ico = new Icon("bbq.ico");
        //创建托盘菜单对象 
        ContextMenu notifyContextMenu = new ContextMenu();
        #endregion
        #region 隐藏任务栏图标、显示托盘图标
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            //判断是否选择的是最小化按钮 
            if (WindowState == FormWindowState.Minimized)
            {
                //托盘显示图标等于托盘图标对象 
                //注意notifyIcon1是控件的名字而不是对象的名字 
             //   notifyIcon1.Icon = ico;
                //隐藏任务栏区图标 
                this.ShowInTaskbar = false;
                //图标显示在托盘区 
                notifyicon.Visible = true;
            }
        }
        #endregion
        #region 还原窗体
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            //判断是否已经最小化于托盘 
            if (WindowState == FormWindowState.Minimized)
            {
                //还原窗体显示 
                WindowState = FormWindowState.Normal;
                //激活窗体并给予它焦点 
                this.Activate();
                //任务栏区显示图标 
               // this.ShowInTaskbar = true;
                //托盘区图标隐藏 
                notifyicon.Visible = false;
            }
        }
        #endregion

        public string pathshijv, pathlang, pathstart;
        public Form1()
        {
            InitializeComponent();
        }
        //延时方法
        [DllImport("kernel32.dll")]
        static extern uint GetTickCount();
        static void Delay(uint ms)
        {
            uint start = GetTickCount();
            while (GetTickCount() - start < ms)
            {
                Application.DoEvents();
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            #region 修改语言
                if (File.Exists(pathlang))
                {
                    //打开
                    FileStream fs1 = new FileStream(pathlang, FileMode.Open);
                    byte[] bytes1 = new byte[fs1.Length];
                    fs1.Read(bytes1, 0, bytes1.Length);
                    string fileString = System.Text.Encoding.UTF8.GetString(bytes1);
                    string outString;
                    outString = Regex.Replace(fileString, "\"fr\", .{4}", "\"fr\", \"zh\"");
                    fs1.Close();
                    //写入
                    StreamWriter sw1 = new StreamWriter(pathlang);
                    sw1.Write(outString);
                    sw1.Close();
                    //备个份
                    StreamWriter swbak1 = new StreamWriter(pathlang + ".bak");
                    swbak1.Write(fileString);
                    swbak1.Close();
                    notifyIcon1.ShowBalloonTip(1, "", "汉化成功。",ToolTipIcon.Info);
                }
                else
                notifyIcon1.ShowBalloonTip(0, "", "未找到wakfu.ini", ToolTipIcon.Info);
            #endregion
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //还原分辨率
            if (File.Exists(pathshijv + ".bak"))
            {
                File.Delete(pathshijv);
                System.IO.File.Move(pathshijv + ".bak", pathshijv);
            }
            //还原语言
            if (File.Exists(pathlang + ".bak"))
            {
                File.Delete(pathlang);
                System.IO.File.Move(pathlang + ".bak", pathlang);
            }
            notifyIcon1.ShowBalloonTip(0, "", "还原成功。", ToolTipIcon.Info);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            pathshijv = Environment.CurrentDirectory;
            pathlang = pathshijv + "\\game\\wakfu.ici";
            pathstart = pathshijv + "\\wakfu.exe";
            pathshijv += "\\game\\config.properties";
            this.notifyIcon1.Text = "沃土视距+汉化";
        }
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的不是数字键,Backspace键,"."则取消该输入
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8 && e.KeyChar != (char)46)
            {
                e.Handled = true;
            }
        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            //启动
            #region 启动游戏
            try
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = pathstart;
                info.Arguments = "";
                info.WindowStyle = ProcessWindowStyle.Normal;
                Process pro = Process.Start(info);
                pro.WaitForExit();
            }
            catch (Exception ex)
            {
                notifyIcon1.ShowBalloonTip(1, "", ex.Message, ToolTipIcon.Info);
            }
            #endregion
            Delay(500);
            #region 还原语言
            if (File.Exists(pathlang + ".bak"))
            {
                File.Delete(pathlang);
                System.IO.File.Move(pathlang + ".bak", pathlang);
            }
            #endregion
        }

        private void button4_Click(object sender, EventArgs e)
        {
            #region  修改视距
            if (checkBox1.Checked | checkBox2.Checked)
            {
                if (File.Exists(pathshijv))
                {
                    //打开
                    FileStream fs = new FileStream(pathshijv, FileMode.Open);
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    string fileString = System.Text.Encoding.UTF8.GetString(bytes);
                    string outString;
                    if (checkBox1.Checked)
                    {
                        outString = Regex.Replace(fileString, "cameraMinZoom=.*", "cameraMinZoom=" + textBoxMin.Text.ToString(), RegexOptions.IgnoreCase);
                        outString = Regex.Replace(outString, "cameraMaxZoom=.*", "cameraMaxZoom=" + textBoxMax.Text.ToString(), RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        outString = fileString;
                    }
                    if (checkBox2.Checked)
                    {
                        outString = Regex.Replace(outString, "resolution.min.width=\\d+", "resolution.min.width=" + textBoxX.Text.ToString(), RegexOptions.IgnoreCase);
                        outString = Regex.Replace(outString, "resolution.min.height=\\d+", "resolution.min.height=" + textBoxY.Text.ToString(), RegexOptions.IgnoreCase);
                    }
                    fs.Close();
                    //写入
                    StreamWriter sw = new StreamWriter(pathshijv);
                    sw.Write(outString);
                    sw.Close();
                    if (checkBox1.Checked | checkBox2.Checked)
                    {
                        //备个份
                        StreamWriter swbak = new StreamWriter(pathshijv + ".bak");
                        swbak.Write(fileString);
                        swbak.Close();

                        notifyIcon1.ShowBalloonTip(1, "", "视距修改成功。", ToolTipIcon.Info);
                    }
                }
                else
                    notifyIcon1.ShowBalloonTip(1, "", "未找到config.properties", ToolTipIcon.Info);
            }
            #endregion
        }

        private void label3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("放游戏根目录，从左到右依次点过来。\rby烤肉肉\rv1.4.1");

        }





    }
}
