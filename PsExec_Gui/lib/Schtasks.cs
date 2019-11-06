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
    class Schtasks
    {
        MainWindow Form;
        FileUtils fileutils;
        ObservableCollection<LV_UPLOAD_Item> batchs;
        public string filename { get; set; }

        public Schtasks(MainWindow window,FileUtils fileutils, ObservableCollection<LV_UPLOAD_Item> batchs)
        {
            Form = window;
            this.fileutils = fileutils;
            this.batchs = batchs;
            filename = "schtasks.bat";
        }



        public void CreateBatch()
        {
            string batch = string.Format(@"
set hour=%time:~0,2%
rem Remove leading space if single digit
if ""%hour:~0,1%"" == "" "" set hour=0%hour:~1,1%
set /A min=%time:~3,2%+2
   echo %min%>len
   for %%a in (len) do set /a len=%%~za - 2
rem Remove leading space
if %len% == -2 set min=0%min%
set st=%hour%:%min%
schtasks /create /tn ""{0}"" /tr ""{1}"" /sc ONCE /st %st% /f
", Form.LB_BAT_INNER.SelectedItem, Form.TB_Batch_Other.Text);
            fileutils.FileWrite(fileutils.BatInnerPath + Form.LB_BAT_INNER.SelectedItem + @"\" + filename, batch, false);
            if(batchs!=null) if (batchs.Where(x => x.Name == filename).Count() == 0) { 
            LV_UPLOAD_Item lv_upload_item = new LV_UPLOAD_Item();
            lv_upload_item.Name = filename;
            lv_upload_item.Path = fileutils.BatInnerPath + Form.LB_BAT_INNER.SelectedItem + @"\" + filename;
            lv_upload_item.IsChecked = true;
            batchs.Add(lv_upload_item);
        }
            Console.WriteLine("新建成功: "+filename);
        }



        public void Exec()
        {
            if (!Form.TB_Batch_Other.Text.Contains(":")) Form.TB_Batch_Other.Text = Form.TB_Batch_Destination.Text + Form.TB_Batch_Other.Text;

            CreateBatch();
            Form.TB_Batch_ExecBat.Text = filename;
            Form.PsExec_Exec("EXEC");
        }
    }
}
