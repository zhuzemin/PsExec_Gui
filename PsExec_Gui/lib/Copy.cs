using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PsExec_Gui.lib
{
    class Copy
    {
        public static bool Credential(string ip,string user, string passwd)
        {
            string batch = @"cmdkey /add:"+ip+" /user:"+user+" /pass:"+passwd;
            string output =CMD.ExecuteCommand(batch);
            if (output.Contains("successfully"))
            {
                //Console.WriteLine(ip + "认证成功.");
                return true;
            }
            else
            {
                Console.WriteLine(output);
                return false;
            }
        }



        public static bool Exec(string file,string ip, string destination)
        {
            string batch = "xcopy \""+file+"\" \\\\" + ip + @"\"+ destination.Replace(":","$")+"*.* /Y /Z";
            Console.WriteLine("复制: "+batch);
            string output = CMD.ExecuteCommand(batch);
            if (Int32.Parse(Regex.Match(output, @"(\d*) File\(s\) copied").Groups[1].Value)>0)
            {
                Console.WriteLine("复制成功: "+ip+","+file.Split('\\').Last()  );
                return true;
            }
            else
            {
                Console.WriteLine("复制失败: "+ip + "," + file.Split('\\').Last()  );
                return false;
            }
        }
    }
}
