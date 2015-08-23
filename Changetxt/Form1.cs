using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Changetxt
{
    public partial class Form1 : Form
    {

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
                this.ShowInTaskbar = true;
                //托盘区图标隐藏 
                notifyicon.Visible = false;
            }
        }
        #endregion






        public string path;
        public Form1()
        {
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {


            if (File.Exists(path))

            {
                //打开
                FileStream fs = new FileStream(path, FileMode.Open);
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
                StreamWriter sw = new StreamWriter(path);
                sw.Write(outString);
                sw.Close();
                //备个份
                StreamWriter swbak = new StreamWriter(path + ".bak");
                swbak.Write(fileString);
                swbak.Close();
                button1.Visible = true;
                button2.Visible = false;

            }
            else
                MessageBox.Show("请放至游戏根目录。");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            File.Delete(path);
            System.IO.File.Move(path + ".bak", path);
            button1.Visible = false;
            button2.Visible = true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.notifyIcon1.Text = "沃土视距修改";
            path = Environment.CurrentDirectory;
            path += "\\game\\config.properties";
        }
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的不是数字键,Backspace键,"."则取消该输入
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8 && e.KeyChar != (char)46)
            {
                e.Handled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("使用说明：文件放至游戏根目录（拖一个快捷方式到桌面）\n\r打开登录器更新完成后点击修改。\n\r软件由烤肉肉制作。\n\r欢迎加入泡泡飞舞公会。（五卡玛一条）");
        }

    }
}
