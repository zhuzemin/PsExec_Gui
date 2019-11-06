using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PsExec_Gui.Object;
using System.IO;
using System.Collections.ObjectModel;

namespace PsExec_Gui.lib
{
    class LV_Upload_Load
    {
        MainWindow window;
        FileUtils fileutils;
        ObservableCollection<LV_UPLOAD_Item> batchs;


        public LV_Upload_Load(MainWindow window,FileUtils fileutils, ObservableCollection<LV_UPLOAD_Item> batchs)
        {
            this.window = window;
            this.fileutils = fileutils;
            this.batchs = batchs;
        }



        public void Load(string ItemPath)
        {
            batchs.Clear();
            foreach (string file in fileutils.GetFiles(ItemPath))
            {
                LV_UPLOAD_Item lv_upload_item = new LV_UPLOAD_Item();
                lv_upload_item.Name = file.Split('\\').Last();
                lv_upload_item.Path = file;
                if (lv_upload_item.Name == "readme.txt")
                {
                    window.TB_Batch_Descriptions.Text = File.ReadAllText(lv_upload_item.Path, System.Text.Encoding.Default);
                    lv_upload_item.IsChecked = false;
                }
                else
                {
                    lv_upload_item.IsChecked = true;
                }
                batchs.Add(lv_upload_item);
            }
            window.LV_UPLOAD.ItemsSource = batchs;
            try
            {
                window.TB_Batch_ExecBat.Text = window.LV_UPLOAD.Items.Cast<LV_UPLOAD_Item>().ToList().Where(x => new[] { ".bat", ".vbs" }.Any(x.Name.Contains)).ElementAt(0).Name;
            }
            catch (Exception err)
            {
                window.TB_Batch_ExecBat.Text = "";
            }

        }
    }
}
