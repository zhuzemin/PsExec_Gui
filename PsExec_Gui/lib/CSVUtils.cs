using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PsExec_Gui.Object;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.ComponentModel;
using System.Windows;

namespace PsExec_Gui.lib
{
    class CSVUtils
    {
        //background process initialize
        private BackgroundWorker m_BackgroundWorker;// 申明后台对象
        ManualResetEvent _busy = new ManualResetEvent(true);
        bool paused = false;
        MainWindow Form;

        public CSVUtils(MainWindow window)
        {
            //background process initialize
            m_BackgroundWorker = new BackgroundWorker(); // 实例化后台对象
            m_BackgroundWorker.WorkerReportsProgress = true; // 设置可以通告进度
            m_BackgroundWorker.WorkerSupportsCancellation = true; // 设置可以取消
            m_BackgroundWorker.DoWork += new DoWorkEventHandler(DoWork);
            m_BackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(UpdateProgress);
            m_BackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CompletedWork);

            Form = window;
        }



        public List<LV_IP_Item> Read(string file)
        {
            List<LV_IP_Item> ret = new List<LV_IP_Item>();
            string[] lines = File.ReadAllLines(file, System.Text.Encoding.Default);
            foreach (string line in lines)
            {
                if (line.Trim() != "" && !line.Contains("IP"))
                {
                    string[] parts = Regex.Split(line, @",|\t");
                    LV_IP_Item item = new LV_IP_Item();
                    item.Area = parts[0];
                    item.Title = parts[1];
                    item.IP = parts[2];
                    item.Online = false;
                    item.Mac = parts[3];
                    item.User = parts[4];
                    if (parts[5] != "") item.Passwd = parts[5]; else item.Passwd = "\"\"";
                    item.PlusbeZK = parts[6];
                    item.Drive = parts[6].Split(':')[0];
                    item.Other = parts[7];
                    ret.Add(item);
                    Copy.Credential(item.IP, item.User, item.Passwd);
                }
            }
            return ret;
            }



        private void Pause(object sender, EventArgs e)
        {
            // Start the worker if it isn't running
            if (paused == false)
            {
                _busy.Reset();
                paused = true;
                //btn_pause.Content = "恢复";
            }
            else
            {
                // Unblock the worker 
                _busy.Set();
                paused = false;
                //btn_pause.Content = "暂停";
            }
        }




        void UpdateProgress(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            Form.BTN_Name_IP_ReFresh.Content = "刷新中: " + e.UserState as string;

            //ProgressBar.Value = e.ProgressPercentage;
            //label10.Text = string.Format("{0}%", progress);
        }




        void CompletedWork(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Console.WriteLine("CSV Reading Error: " + e.Error);
            }
            else if (e.Cancelled)
            {
                System.Windows.Forms.MessageBox.Show("Canceled");
            }
            else
            {
                Console.WriteLine("IP表检查完成.");
                Form.LV_IP.ItemsSource = (List<LV_IP_Item>)e.Result;
                Form.BTN_Name_IP_ReFresh.IsEnabled = true;
                Form.BTN_Name_IP_ReFresh.Content = "刷新";
            }
        }




        private void Cancel(object sender, EventArgs e)
        {
            m_BackgroundWorker.CancelAsync();
        }




        void DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            int count=1;

            List<LV_IP_Item> source = (List<LV_IP_Item>)e.Argument;
            List<LV_IP_Item> ret = new List<LV_IP_Item>();
            foreach (LV_IP_Item item in source)
            {
                _busy.WaitOne();
                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                bool BTN_Name_IP_ReFresh_IsEnabled = false;
                Form.Dispatcher.Invoke(() =>
                {
                    BTN_Name_IP_ReFresh_IsEnabled=Form.BTN_Name_IP_ReFresh.IsEnabled;
                });
                if (BTN_Name_IP_ReFresh_IsEnabled)
                {
                    break;
                }
                    item.Online = PING.PingHost(item.IP);
                    ret.Add(item);
                bw.ReportProgress((int)count/source.Count*100, count + "/" + source.Count);
                count++;
            }
            e.Result = ret;

        }



        public void Refresh(List<LV_IP_Item> CSV)
        {
            Form.BTN_Name_IP_ReFresh.IsEnabled = false;
            m_BackgroundWorker.RunWorkerAsync(CSV);
        }
    }
}
