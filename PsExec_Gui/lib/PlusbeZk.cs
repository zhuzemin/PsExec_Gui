using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PsExec_Gui.lib;
using PsExec_Gui.Object;
using System.Collections.ObjectModel;

namespace PsExec_Gui.lib
{
    class PlusbeZk
    {
        MainWindow Form;
        FileUtils fileutils;
        ObservableCollection<LV_UPLOAD_Item> batchs;

        public PlusbeZk(MainWindow window, FileUtils fileutils, ObservableCollection<LV_UPLOAD_Item> batchs)
        {
            Form = window;
            this.fileutils = fileutils;
            this.batchs = batchs;
        }



        public void Close()
        {
            Form.TB_Batch_Other.Text = "PlusbeZk.exe";
            Form.PsExec_Exec("TASKKILL");
        }



        public void Open()
        {
            if(Form.TB_Batch_Other.Text=="")Form.TB_Batch_Other.Text= Form.TB_Batch_Destination.Text +Form.TB_Batch_ExecBat.Text;
            Schtasks schtasks = new Schtasks(Form,fileutils,batchs);
            schtasks.Exec();
        }
    }
}
