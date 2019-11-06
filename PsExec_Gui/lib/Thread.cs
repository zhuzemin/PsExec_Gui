using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using PsExec_Gui.Object;
using System.Threading;
using System.ComponentModel;

namespace PsExec_Gui.lib
{
    class Thread
    {
        //background process initialize
        private BackgroundWorker m_BackgroundWorker;// 申明后台对象
        ManualResetEvent _busy = new ManualResetEvent(true);
        MainWindow Form;
        ThreadArgument ThreadArg = new ThreadArgument();

        public Thread(MainWindow window)
        {
            //background process initialize
            m_BackgroundWorker = new BackgroundWorker(); // 实例化后台对象
            m_BackgroundWorker.WorkerReportsProgress = true; // 设置可以通告进度
            m_BackgroundWorker.WorkerSupportsCancellation = true; // 设置可以取消
            m_BackgroundWorker.DoWork += new DoWorkEventHandler(DoWork);
            //m_BackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(UpdateProgress);
            m_BackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CompletedWork);

            Form = window;
        }



        void Copy(LV_IP_Item target)
        {
            foreach (LV_UPLOAD_Item file in Form.LV_UPLOAD.Items)
            {
                if (file.IsChecked)
                {
                    string destination = "";
                    Form.Dispatcher.Invoke(() =>
                    {
                        destination = "\""+Form.TB_Batch_Destination.Text+ "\"";
                    });
                    PsExec_Gui.lib.Copy.Exec(file.Path, target.IP, destination);
                }
            }
        }



        void Batch(string command)
        { 
        Console.WriteLine("执行: "+command);
        string output = PsExec_Gui.lib.CMD.ExecuteCommand(command);
        //Console.WriteLine("结果: "+output);
        }



        void DoWork(object sender, DoWorkEventArgs e)
        {
            ThreadArgument ThreadArg = (ThreadArgument)e.Argument;
            Copy(ThreadArg.target);
            if (ThreadArg.command.Contains("SYNC"))
            {
                SYNC();
                ThreadArg.command = ThreadArg.command.Replace("SYNC_","");
            }
            Form.Dispatcher.Invoke(() =>
            {
                Form.BTN_EternalBlue.IsEnabled = false;
            });
            if (ThreadArg.command.Contains("http"))
            {
                HTTP(ThreadArg.command);
            }
            else
            {
                Batch(ThreadArg.command);
            }
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
                Form.BTN_EternalBlue.IsEnabled = true;
                Form.ProgressBar.Value++;

            }

        }



        public void Exec(LV_IP_Item target,string command)
        {
            ThreadArg.target = target;
            ThreadArg.command = command;
            m_BackgroundWorker.RunWorkerAsync(ThreadArg);
        }



        void HTTP(String url)
        {
            Console.WriteLine("执行: "+url);
            HttpUtils httputils = new HttpUtils();
            httputils.HttpGet(url);
        }



        void SYNC()
        {
            bool BTN_EternalBlue_IsEnabled=false;
            do
            {
            Form.Dispatcher.Invoke(() =>
            {
                BTN_EternalBlue_IsEnabled = Form.BTN_EternalBlue.IsEnabled;
            });
                System.Threading.Thread.Sleep(1000);
            } while (BTN_EternalBlue_IsEnabled == false);
        }
    }



    class ThreadArgument
    {
        public LV_IP_Item target { get; set; }
        public string command { get; set; }
    }



    }
