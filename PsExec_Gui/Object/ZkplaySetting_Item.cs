using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsExec_Gui.Object
{
    public class List
    {
        public string bgpic { get; set; }
        public string bgcolor { get; set; }
        public List()
        {
            bgpic = "value";
            bgcolor = "value";
        }
    }

    public class ZkplaySetting_Item
    {
        public int port { get; set; }
        public int max { get; set; }
        public int mask { get; set; }
        public int isf11 { get; set; }
        public int maxtime { get; set; }
        public int istask { get; set; }
        public int vediomute { get; set; }
        public int sounddef { get; set; }
        public int volumedef { get; set; }
        public int ismouse { get; set; }
        public int islight { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h { get; set; }
        public string serverip { get; set; }
        public string myip { get; set; }
        public string servertime { get; set; }
        public int autoplay { get; set; }
        public int backtime { get; set; }
        public int iscursor { get; set; }
        public int playmode { get; set; }
        public int runsleep { get; set; }
        public List list { get; set; }
        public int picswitchtime { get; set; }
        public int welcomeswitchtime { get; set; }
        public string controlipport { get; set; }
        public string serverip2 { get; set; }
        public string comport { get; set; }
        public string kill { get; set; }
        public int picmode { get; set; }
        public int autoupdate { get; set; }
        public void Initiation()
    {
            port = 8020;
            max = 1;
            mask = 0;
            maxtime = 10;
            istask = 0;
            islight = 0;
            x=0;
            y = 0;
            w = 1024;
            h = 768;
            serverip = "";
            myip = "";
            servertime = "50000";
            autoplay = 1;
            backtime = 0;
            iscursor = 0;
            playmode = 0;
            runsleep = 1;
            list = new List();
            picswitchtime = 5000;
            welcomeswitchtime = 5000;
            controlipport = "http://127.0.0.1:80/";
            serverip2 = "";
            comport = "";
            kill = "1";
            picmode = 1;
    }
    }
}
