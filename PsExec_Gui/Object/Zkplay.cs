using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsExec_Gui.Object
{
    class Zkplay
    {
        public string restart { get; set; }
        public Zkplay(string ip)
        {
            restart = "http://" + ip + ":8020/?act=restar&sn=18F42BE047ADAF5A0F738E643E84EF27";
        }
    }
}
