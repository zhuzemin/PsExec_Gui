using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsExec_Gui.lib
{
    class PsShutdown
    {
        //search "Softs" floder for soft path, if exist multiple version, select the newer one
        public FileUtils fileutils;
        public string version { get; set; }
        public string softName { get; set; }
        public PsShutdown(FileUtils fileutils)
        {
            this.fileutils = fileutils;
            softName = "psshutdown.exe";
            string[] files = fileutils.searchFile(fileutils.path, softName);
            version = files.OrderByDescending(path => File.GetLastWriteTime(path)).FirstOrDefault();
        }



        public string Exec(string command)
        {
            Process netStat = new Process();
            netStat.StartInfo.UseShellExecute = false;
            netStat.StartInfo.CreateNoWindow = true;
            netStat.StartInfo.FileName = version;
            netStat.StartInfo.Arguments = command;
            netStat.StartInfo.RedirectStandardOutput = true;
            netStat.Start();
            string output = netStat.StandardOutput.ReadToEnd();
            return output;
        }
    }
}
