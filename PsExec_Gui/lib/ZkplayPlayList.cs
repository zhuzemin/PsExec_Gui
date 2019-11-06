using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using PsExec_Gui.Object;
using System.Windows;

namespace PsExec_Gui.lib
{
    class ZkplayPlayList
    {
        public Config config { get; set; }
        public string ConfigPath { get; set; }
        public MainWindow window;


        public ZkplayPlayList(MainWindow window, string ip, string ZkplayPath=null)
        {
            this.window = window;
            if (ZkplayPath == null) ZkplayPath = window.TB_Batch_Destination.Text.Replace("UploadFiles", "Data");
            else ZkplayPath = ZkplayPath + @"\Data\";
            ConfigPath = @"\\" + ip +@"\"+ ZkplayPath.Replace(":","$") + @"Config.txt";
        }

        public void GetConfig()
        {
            if (File.Exists(ConfigPath))
            {
                Console.WriteLine("获取播放列表: " + ConfigPath);
                config = JsonConvert.DeserializeObject<Config>(System.Text.RegularExpressions.Regex.Unescape(File.ReadAllText(ConfigPath)));
            }
            else
            {
                Console.WriteLine("错误: 路径有误.");

            }
        }

        public void Add(string[] source)
        {
            if (source.Length>0)
            {
                List<LV_UPLOAD_Item> LV_UPLOAD_List = new List<LV_UPLOAD_Item>();
            foreach (string file in source)
            {
                LV_UPLOAD_Item item = new LV_UPLOAD_Item();
                item.Name = System.IO.Path.GetFileName(file);
                item.Path = file;
                item.IsChecked = true;
                LV_UPLOAD_List.Add(item);
                ZkplayPlayList_Item zkplist_item = new ZkplayPlayList_Item();
                zkplist_item.Initiation(item.Name);
                config.list.Insert(0, zkplist_item);
            }
                window.LV_UPLOAD.ItemsSource = LV_UPLOAD_List;
                Save();

            }
        }



        public void Del(int index)
        {
            config.list.RemoveAt(index);
            Save();
        }



        public void Save()
        {
            using (StreamWriter streamWriter = File.CreateText(ConfigPath))
            {
                streamWriter.Write(JsonConvert.SerializeObject(config));
            }
            window.LV_ZkplayPlayList.Items.Refresh();
        }
    }



    public class Config
    {
        public int NowVersion { get; set; }
        public List<ZkplayPlayList_Item> list { get; set; }
    }
}
