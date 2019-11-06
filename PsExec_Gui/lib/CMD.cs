using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PsExec_Gui.lib
{
    class CMD
    {
        public static string ExecuteCommand(string command)
        {
            Process netStat = new Process();
            netStat.StartInfo.UseShellExecute = false;
            netStat.StartInfo.CreateNoWindow = true;
            netStat.StartInfo.FileName = "cmd.exe";
            netStat.StartInfo.Arguments = "/c "+ command;
            netStat.StartInfo.RedirectStandardOutput = true;
            netStat.Start();
            string output = netStat.StandardOutput.ReadToEnd();
            return output;
        }
    }
}
