using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PsExec_Gui.Object
{
    public class ZkplayPlayList_Item
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public string File { get; set; }
        public string Word { get; set; }
        public Json Json { get; set; }
        public string Tag { get; set; }
        public int Isjz { get; set; }
        public int FileType { get; set; }
        public int Ispingbao { get; set; }
        public void Initiation(string filename)
        {
            Title = filename;
            Id = 0;
            File = "UploadFiles/" + filename;
            Word = "";
            Json = new Json();
            Tag = "0";
            Isjz = 0;
            string extension=System.IO.Path.GetExtension(filename);
            if (Regex.Match(extension, "jpg|png|bmp|gif").Success)
            {
                FileType =4;
            }
            else if (Regex.Match(extension, "avi|mp4|mpeg|mkv|wmv|ts|webm").Success)
            {
                FileType = 1;
            }
            else if (Regex.Match(extension, "doc|docx").Success)
            {
                FileType = 6;
            }
            Ispingbao = 0;
        }
    }
    public class Json
    {
        public string type { get; set; }
        public string data { get; set; }
        public Json()
        {
            type = "google";
            data = "";
        }
    }
}
