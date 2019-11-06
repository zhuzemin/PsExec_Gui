using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PsExec_Gui.lib
{
    class Batch
    {
        public static IEnumerable<string> GetList(string path)
        {
            return Directory.GetDirectories(path).Select(x => { return new DirectoryInfo(x).Name; });

        }

    }
}
