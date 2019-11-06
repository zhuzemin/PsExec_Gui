using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace PsExec_Gui.lib
{

    class WOL
    {
        public FileUtils fileutils;
        public string SoftPath { get; set; }
        public string SoftName { get; set; }
        public string SoftFolder { get; set; }

        public WOL(FileUtils fileutils)
        {
            this.fileutils = fileutils;
            SoftName = "WolCmd.exe";
            string[] files = fileutils.searchFile(fileutils.path, SoftName);
            SoftPath = files.OrderByDescending(path => File.GetLastWriteTime(path)).FirstOrDefault();
            SoftFolder = System.IO.Path.GetDirectoryName(SoftPath);
        }
    }
}

