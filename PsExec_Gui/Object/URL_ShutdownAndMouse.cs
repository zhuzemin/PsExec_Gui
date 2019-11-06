using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsExec_Gui.Object
{
    class URL_ShutdownAndMouse
    {
        public string shutdown { get; set; }
        public string restart { get; set; }

        public URL_ShutdownAndMouse(string ip)
        {
            shutdown = "http://" + ip + ":8088/?act=shutdown&object=0&states=1&r=2139686054";
            restart = "http://" + ip + ":8088/?act=restart&object=0&states=3&r=884763882";
        }

    }
}
