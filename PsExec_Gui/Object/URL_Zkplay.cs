using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PsExec_Gui.Object
{
    class URL_Zkplay
    {
        public string restart { get; set; }
        public string play { get; set; }
        public string setting_set { get; set; }

        public URL_Zkplay(string ip,int index=0, ZkplaySetting_Item setting =null)
        {
            restart = "http://" + ip + ":8020/?act=restar&sn=18F42BE047ADAF5A0F738E643E84EF27";
            play = "http://" + ip + ":8020/?act=movie&object="+index.ToString()+"&states=13&sn=18F42BE047ADAF5A0F738E643E84EF27";
            setting_set = "http://" + ip + ":8020/?act=saveset&object="+ HttpUtility.UrlEncode (JsonConvert.SerializeObject(setting))+ "&states=0&sn=18F42BE047ADAF5A0F738E643E84EF27";
        }
    }
}
