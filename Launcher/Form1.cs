using System;
using System.Collections;
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
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
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
            CheckUpdate();
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

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            if (!start)
                this.pictureBox3.Image = global::Launcher.Properties.Resources.btnStart_up;
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            if (!start)
                this.pictureBox3.Image = global::Launcher.Properties.Resources.btnStart_dn;
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
                string ServerIP = getIP("byxiaoxie.com");
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

        private void CheckUpdate()
        {
            if (progressBarEx1.Value == 0)
            {
                //测试用记得删除 Start
                try
                {
                    uint crc = crc32.Crc32.GetFileCRC32(@"D:\Personal\Desktop\SoulWorker\SoulWorker\SoulWorker100.exe");
                    string crchex = crc.ToString("X8");
                    MessageBox.Show(crchex); //test
                    return; 
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                //测试用记得删除 End

                pictureBox3.Enabled = false;
                pictureBox3.Image = Properties.Resources.btnStart_disable;
                label1.Text = "Checking for updates...";
                string htmldata = getHtml("http://127.0.0.1:666/sw.json");
                DataTable jsondata = JsonToDataTable(htmldata);
                if (jsondata.Rows[0]["version"].ToString() != "1.1")
                {
                    Sleep(3000);
                    string MyDir = System.AppDomain.CurrentDomain.BaseDirectory;
                    DownloadFile(jsondata.Rows[0]["download"].ToString(), MyDir + "Test.zip", progressBarEx1, label1);
                }
                else 
                {
                    progressBarEx1.Value = 100;
                    label1.Text = "Latest Version...";
                    pictureBox3.Enabled = true;
                    pictureBox3.Image = Properties.Resources.btnStart_dn;
                }
                
            }
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

        /// <summary>
        /// 延迟
        /// </summary>
        /// <param name="m">毫秒</param>
        public static void Sleep(int m)
        {
            DateTime current = DateTime.Now;
            while (current.AddMilliseconds(m) > DateTime.Now)
            {
                Application.DoEvents();
            }
            return;
        }

        /// <summary>        
        /// 下载文件        
        /// </summary>        
        /// <param name="URL">下载文件地址</param>       
        /// <param name="Filename">存放位置</param>        
        /// <param name="Prog">进度条</param>        
        /// 
        public void DownloadFile(string URL, string filename, ProgressBarEx prog, Label label1)
        {
            float percent = 0;
            try
            {
                HttpWebRequest Myrq = (HttpWebRequest)HttpWebRequest.Create(URL);
                HttpWebResponse myrp = (HttpWebResponse)Myrq.GetResponse();
                long totalBytes = myrp.ContentLength;
                if (prog != null)
                {
                    prog.Maximum = (int)totalBytes;
                }
                Stream st = myrp.GetResponseStream();
                Stream so = new FileStream(filename, FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int)by.Length);
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    Application.DoEvents();
                    so.Write(by, 0, osize);
                    if (prog != null)
                    {
                        prog.Value = (int)totalDownloadedByte;
                    }
                    osize = st.Read(by, 0, (int)by.Length);

                    percent = (float)totalDownloadedByte / (float)totalBytes * 100;
                    label1.Text = "Download:" + Math.Round(percent, 2).ToString() + "%   ";
                    Application.DoEvents();
                }
                so.Close();
                st.Close();
                if (!WinrarCheck())
                {
                    MessageBox.Show("WinRAR未安装,无法执行解压!", "Launcher");
                    return;
                }
                else {
                    string MyDir = System.AppDomain.CurrentDomain.BaseDirectory;
                    string retDir = UnRAR(MyDir, "", "Test.zip");
                    if (retDir == "") 
                    {
                        MessageBox.Show("解压失败.", "Launcher");
                        return;
                    }
                }
                if (progressBarEx1.Value == progressBarEx1.Maximum)
                {
                    label1.Text = "Download complete...";
                    pictureBox3.Enabled = true;
                    pictureBox3.Image = Properties.Resources.btnStart_dn;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Winrar是否存在
        /// </summary>
        /// <returns></returns>
        static public bool WinrarCheck()
        {
            RegistryKey the_Reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe");
            return !string.IsNullOrEmpty(the_Reg.GetValue("").ToString());
        }

        /// <summary>
        /// 调用Winrar解压
        /// </summary>
        /// <param name="UnPatch">解压路径</param>
        /// <param name="Patch">压缩包路径</param>
        /// <param name="Name">压缩包名称</param>
        /// <returns></returns>
        public string UnRAR(string UnPatch, string Patch, string Name)
        {
            string the_rar;
            RegistryKey the_Reg;
            object the_Obj;
            string the_Info;

            try
            {
                the_Reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe");
                the_Obj = the_Reg.GetValue("");
                the_rar = the_Obj.ToString();
                the_Reg.Close();

                if (Directory.Exists(UnPatch) == false)
                {
                    Directory.CreateDirectory(UnPatch);
                }
                the_Info = "x " + Name + " " + UnPatch + " -y" + " -ibck";

                ProcessStartInfo the_StartInfo = new ProcessStartInfo();
                the_StartInfo.FileName = the_rar;
                the_StartInfo.Arguments = the_Info;
                the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                the_StartInfo.WorkingDirectory = Patch;

                Process the_Process = new Process();
                the_Process.StartInfo = the_StartInfo;
                the_Process.Start();
                the_Process.WaitForExit();
                the_Process.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return UnPatch;
        }

        /// <summary>
        /// 获取网站Json用
        /// </summary>
        /// <param name="html">URL</param>
        /// <returns></returns>
        public string getHtml(string html)
        {
            string pageHtml = "";
            WebClient MyWebClient = new WebClient();
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;
            Byte[] pageData = MyWebClient.DownloadData(html);
            MemoryStream ms = new MemoryStream(pageData);
            using (StreamReader sr = new StreamReader(ms, Encoding.GetEncoding("GB2312")))
            {
                pageHtml = sr.ReadLine();
            }
            return pageHtml;
        }

        /// <summary>
        /// Json 转换 DataTable
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable JsonToDataTable(string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        //Columns
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        //Rows
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                }
            }
            catch
            {
            }
            result = dataTable;
            return result;
        }
    }
}
