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
                string outString = Regex.Replace(fileString, "cameraMinZoom=.*", "cameraMinZoom=" + textBoxMin.Text.ToString(), RegexOptions.IgnoreCase);
                outString = Regex.Replace(outString, "cameraMaxZoom=.*", "cameraMaxZoom=" + textBoxMax.Text.ToString(), RegexOptions.IgnoreCase);
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
            path = Environment.CurrentDirectory;
            path += "\\games\\config.properties";
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
            MessageBox.Show("使用说明：文件放至游戏根目录（拖一个快捷方式到桌面）\n\r打开登录器更新完成后点击修改。\n\r软件由烤肉肉制作。\n\r欢迎加入泡泡飞舞公会。（五卡玛一条）\n\rv1.0.0.0");
        }

    }
}
