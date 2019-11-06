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
using System.Windows.Shapes;
using PsExec_Gui.Object;
using Newtonsoft.Json;
using System.IO;

namespace PsExec_Gui
{
    /// <summary>
    /// Interaction logic for Zkplay_Setting.xaml
    /// </summary>
    public partial class ZkplaySetting : Window
    {
        public ZkplaySetting_Item Setting { get; set; }
        public string SettingPath { get; set; }
        public MainWindow window;
        public bool state;


        public ZkplaySetting(MainWindow window)
        {
            InitializeComponent();



            this.window = window;
            SettingPath = window.fileutils.BatInnerPath + @"Zkplay设置\Setting.txt";
            Setting = JsonConvert.DeserializeObject<ZkplaySetting_Item>(System.Text.RegularExpressions.Regex.Unescape(File.ReadAllText(SettingPath)));



            switch (Setting.max)
            {
                case 1:
                    RB_Screen_Full.IsChecked = true;
                    break;
                case 2:
                    RB_Screen_Custom.IsChecked = true;
                    break;
            }



            TB_Margin_Left.Text = Setting.x.ToString();
            TB_Margin_Up.Text = Setting.y.ToString();
            TB_Resolution_Width.Text = Setting.w.ToString();
            TB_Resolution_Height.Text = Setting.h.ToString();



            CBB_AutoPlay.ItemsSource =new []{ "黑屏","自动播放", "欢迎词","桌面" };
            CBB_AutoPlay.SelectedIndex = Setting.autoplay;



            CBB_ComPort.ItemsSource = new []{ "","COM1","COM2","COM3","COM4","COM5"};
            CBB_ComPort.SelectedItem = Setting.comport;



            switch (Setting.istask)
            {
                case 0:
                    CB_Hide_Taskbar.IsChecked=false;
                    break;
                case 1:
                    CB_Hide_Taskbar.IsChecked =true;
                    break;
            }



            switch (Setting.iscursor)
            {
                case 0:
                    CB_Hide_Cursor.IsChecked = false;
                    break;
                case 1:
                    CB_Hide_Cursor.IsChecked = true;
                    break;
            }



            switch (Setting.mask)
            {
                case 0:
                    CB_Mask.IsChecked = false;
                    break;
                case 1:
                    CB_Mask.IsChecked = true;
                    break;
            }



            switch (Setting.ismouse)
            {
                case 0:
                    CB_Remote_Mouse.IsChecked = false;
                    break;
                case 1:
                    CB_Remote_Mouse.IsChecked = true;
                    break;
            }



            switch (Setting.islight)
            {
                case 0:
                    CB_Linkage.IsChecked = false;
                    break;
                case 1:
                    CB_Linkage.IsChecked = true;
                    break;
            }



            switch (Setting.sounddef)
            {
                case 0:
                    CB_Volume_Ctrl.IsChecked = false;
                    break;
                case 1:
                    CB_Volume_Ctrl.IsChecked = true;
                    break;
            }



            switch (Setting.autoupdate)
            {
                case 0:
                    CB_AutoUpdate.IsChecked = false;
                    break;
                case 1:
                    CB_AutoUpdate.IsChecked = true;
                    break;
            }



            switch (Setting.isf11)
            {
                case 0:
                    CB_BrowserFullScreen.IsChecked = false;
                    break;
                case 1:
                    CB_BrowserFullScreen.IsChecked = true;
                    break;
            }



            switch (Setting.vediomute)
            {
                case 0:
                    CB_VideoMute.IsChecked = false;
                    break;
                case 1:
                    CB_VideoMute.IsChecked = true;
                    break;
            }



            TB_Server_IP.Text = Setting.serverip;
            //TB_Client_IP.Text = Setting.myip;
            TB_UpdateInterval.Text = Setting.servertime;



            CBB_PicSwitchEffect.ItemsSource = new[] { "无动效模式", "动效模式" };
            CBB_PicSwitchEffect.SelectedIndex = Setting.picmode;



            TB_PicSwithInterval.Text = Setting.picswitchtime.ToString();



            TB_ScreenSaverInterval.Text = Setting.backtime.ToString();



            TB_WelcomeSwitchInterval.Text = Setting.welcomeswitchtime.ToString();



            TB_KillWhenShutdown.Text = Setting.kill;



            TB_ClientPort.Text = Setting.port.ToString();


            SD_Volume.Value = Setting.volumedef;



            CB_Startup.Visibility = Visibility.Hidden;



            state = true;
        }



        private void RB_Screen_Checked(object sender, RoutedEventArgs e)
        {
            if (state)
            {
            if ((bool)RB_Screen_Full.IsChecked) Setting.max = 1;
            if ((bool)RB_Screen_Custom.IsChecked) Setting.max = 2;
            }
        }



        void TextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            if (state)
            {
            System.Windows.Controls.TextBox tb = sender as System.Windows.Controls.TextBox;
            switch (tb.Name)
            {
                case "TB_Margin_Left":
                    Setting.x = int.Parse(tb.Text);
                    break;
                case "TB_Margin_Up":
                    Setting.y= int.Parse(tb.Text);
                    break;
                case "TB_Resolution_Width":
                    Setting.w= int.Parse(tb.Text);
                    break;
                case "TB_Resolution_Height":
                    Setting.h= int.Parse(tb.Text);
                    break;
                case "TB_Server_IP":
                    Setting.serverip = tb.Text;
                    break;
                case "TB_UpdateInterval":
                    Setting.servertime = tb.Text;
                    break;
                case "TB_PicSwithInterval":
                    Setting.picswitchtime = int.Parse(tb.Text);
                    break;
                case "TB_WelcomeSwitchInterval":
                    Setting.welcomeswitchtime = int.Parse(tb.Text);
                    break;
                case "TB_KillWhenShutdown":
                    Setting.kill = tb.Text;
                    break;
                case "TB_ClientPort":
                    Setting.port = int.Parse(tb.Text);
                    break;
                case "TB_ScreenSaverInterval":
                    Setting.backtime = int.Parse(tb.Text);
                    break;
                }

            }
        }



        void CBB_AutoPlay_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (state)
            {
            Setting.autoplay = CBB_AutoPlay.SelectedIndex;
            }
        }



        void CBB_ComPort_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (state)
            {
            Setting.comport = (string)CBB_ComPort.SelectedItem;
            }
        }



        void CBB_PicSwitchEffect_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (state)
            {
            Setting.picmode = CBB_PicSwitchEffect.SelectedIndex;
            }
        }



        void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (state)
            {
            System.Windows.Controls.CheckBox cb = sender as System.Windows.Controls.CheckBox;
            switch (cb.Name)
            {
                case "CB_Hide_Taskbar":
                    switch (cb.IsChecked)
                    {
                        case true:
                            Setting.istask=1;
                            break;
                        case false:
                            Setting.istask = 0;
                            break;
                    }
                    break;
                case "CB_Hide_Cursor":
                    switch (cb.IsChecked)
                    {
                        case true:
                            Setting.iscursor = 1;
                            break;
                        case false:
                            Setting.iscursor = 0;
                            break;
                    }
                    break;
                case "CB_Remote_Mouse":
                    switch (cb.IsChecked)
                    {
                        case true:
                            Setting.ismouse = 1;
                            break;
                        case false:
                            Setting.ismouse = 0;
                            break;
                    }
                    break;
                case "CB_Mask":
                    switch (cb.IsChecked)
                    {
                        case true:
                            Setting.mask = 1;
                            break;
                        case false:
                            Setting.mask = 0;
                            break;
                    }
                    break;
                case "CB_Linkage":
                    switch (cb.IsChecked)
                    {
                        case true:
                            Setting.islight = 1;
                            break;
                        case false:
                            Setting.islight = 0;
                            break;
                    }
                    break;
                case "CB_Volume_Ctrl":
                    switch (cb.IsChecked)
                    {
                        case true:
                            Setting.sounddef = 1;
                            break;
                        case false:
                            Setting.sounddef = 0;
                            break;
                    }
                    break;
                    case "CB_VideoMute":
                        switch (cb.IsChecked)
                        {
                            case true:
                                Setting.vediomute = 1;
                                break;
                            case false:
                                Setting.vediomute = 0;
                                break;
                        }
                        break;
                    case "CB_BrowserFullScreen":
                        switch (cb.IsChecked)
                        {
                            case true:
                                Setting.isf11 = 1;
                                break;
                            case false:
                                Setting.isf11 = 0;
                                break;
                        }
                        break;
                    case "CB_AutoUpdate":
                        switch (cb.IsChecked)
                        {
                            case true:
                                Setting.autoupdate = 1;
                                break;
                            case false:
                                Setting.autoupdate = 0;
                                break;
                        }
                        break;
                }

            }
        }



        private void SD_Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SD_Volume.ToolTip = SD_Volume.Value;
            Setting.volumedef = (int)SD_Volume.Value;
        }



        private void Window_Closed(object sender, EventArgs e)
        {
            using (StreamWriter streamWriter = File.CreateText(SettingPath))
            {
                streamWriter.Write(JsonConvert.SerializeObject(Setting));
            }

        }
    }
}
