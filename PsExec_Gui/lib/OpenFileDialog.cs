using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsExec_Gui.lib
{
    class OpenFileDialog
    {
        System.Windows.Forms.OpenFileDialog openfiledialog = new System.Windows.Forms.OpenFileDialog();

        public OpenFileDialog(string trigger)
        {
            //设定Filter，过滤档案
            openfiledialog.Filter = "All files (*.*)|*.*";
            //设定起始目录为程式目录
            //openfiledialog.InitialDirectory = System.Windows.Forms.Application.StartupPath;
            openfiledialog.RestoreDirectory = true;
            //设定dialog的Title
            openfiledialog.Title = "选择文件";
                switch (trigger)
            {
                case "TB_IP":
                    openfiledialog.Filter = "Acceptable File Format (*.txt;*.csv)|*.txt;*.csv|All files (*.*)|*.*";
                    break;
                case "LV_ZkplayPlayList":
                    openfiledialog.Multiselect = true;
                    break;
            }
        }



        public string[] Browser()
        {
            if (openfiledialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return openfiledialog.FileNames;
            }
            else
            {
                return new string[] { };
            }
        }
    }
}
