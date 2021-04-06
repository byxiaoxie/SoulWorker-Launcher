using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using CCWin;
using Microsoft.Win32;

namespace Launcher
{
    public partial class Form1 : SkinMain
    {
        bool start = false;

        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            this.pictureBox1.Image = global::Launcher.Properties.Resources.btnClose_up;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            this.pictureBox1.Image = global::Launcher.Properties.Resources.btnClose_dn;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            this.pictureBox2.Image = global::Launcher.Properties.Resources.btnMin_up;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            this.pictureBox2.Image = global::Launcher.Properties.Resources.btnMin_dn;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            string fileName = "";
            start = true;
            this.pictureBox3.Image = global::Launcher.Properties.Resources.btnStart_disable;

            string RegValue = GetRegistryValue("SoulWorker\\DefaultIcon", "");
            if (RegValue != "")
            {
                RegValue = RegValue.Replace("Launcher.exe", "");
                fileName = RegValue + "SoulWorker100.exe";
            }
            else {
                fileName = "SoulWorker100.exe";
            }
            
            if (File.Exists(fileName))
            {
                string ServerIP = getIP("sw.byxiaoxie.com");
                //RunCmd(fileName, "IP:" + ServerIP + " PORT:10000 MultiExecuteClient:yes SkipWMModule:yes");
                RunCmd_Test(fileName, ServerIP);
                Application.Exit();
            }
            else {
                MessageBox.Show("没有找到游戏客户端.", "Launcher");
                start = false;
                this.pictureBox3.Image = global::Launcher.Properties.Resources.btnStart_dn;
            }
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            if(!start)
                this.pictureBox3.Image = global::Launcher.Properties.Resources.btnStart_up;
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            if (!start)
                this.pictureBox3.Image = global::Launcher.Properties.Resources.btnStart_dn;
        }

        /// <summary>
        /// 域名转IP
        /// </summary>
        /// <param name="domain">域名</param>
        public static string getIP(string domain)
        {
            domain = domain.Replace("http://", "").Replace("https://", "");
            IPHostEntry host = Dns.GetHostEntry(domain);
            IPEndPoint ip = new IPEndPoint(host.AddressList[0], 0);
            return ip.Address.ToString();
        }

        /// <summary>
        /// 运行cmd命令
        /// 不显示命令窗口
        /// </summary>
        /// <param name="cmdExe">指定应用程序的完整路径</param>
        /// <param name="cmdStr">执行命令行参数</param>
        static bool RunCmd(string cmdExe, string cmdStr)
        {
            bool result = false;
            try
            {
                using (Process myPro = new Process())
                {
                    myPro.StartInfo.FileName = "cmd.exe";
                    myPro.StartInfo.UseShellExecute = false;
                    myPro.StartInfo.RedirectStandardInput = true;
                    myPro.StartInfo.RedirectStandardOutput = true;
                    myPro.StartInfo.RedirectStandardError = true;
                    myPro.StartInfo.CreateNoWindow = true;
                    myPro.Start();
                    string str = string.Format(@"""{0}"" {1} {2}", cmdExe, cmdStr, "&exit");

                    myPro.StandardInput.WriteLine(str);
                    myPro.StandardInput.AutoFlush = true;
                    myPro.WaitForExit();

                    result = true;
                }
            }
            catch
            {

            }
            return result;
        }

        static bool RunCmd_Test(string fileName, string ServerIP)
        {
            bool result = false;
            try
            {
                //创建一个进程
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;//是否使用操作系统shell启动
                p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
                p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
                p.StartInfo.CreateNoWindow = true;//不显示程序窗口
                p.Start();//启动程序

                p.StandardInput.WriteLine("cd /d "+ fileName.Replace("SoulWorker100.exe", ""));
                string strCMD = "start " + fileName + " IP:" + ServerIP + " PORT:10000 MultiExecuteClient:yes SkipWMModule:yes";
                //向cmd窗口发送输入信息
                p.StandardInput.WriteLine(strCMD + "&exit");

                p.StandardInput.AutoFlush = true;

                //获取cmd窗口的输出信息
                //string output = p.StandardOutput.ReadToEnd();
                //等待程序执行完退出进程
                p.WaitForExit();
                p.Close();
                result = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message + "\r\n跟踪;" + ex.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// 读取注册表值
        /// </summary>
        /// <param name="path">指定注册表的完整路径</param>
        /// <param name="paramName">注册表名称</param>
        protected string GetRegistryValue(string path, string paramName)
        {
            string value = string.Empty;
            RegistryKey root = Registry.ClassesRoot;
            RegistryKey rk = root.OpenSubKey(path);
            if (rk != null)
            {
                value = (string)rk.GetValue(paramName, null);
            }
            return value;
        }
    }
}
