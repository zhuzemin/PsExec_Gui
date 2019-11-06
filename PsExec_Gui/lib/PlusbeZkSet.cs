using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace PsExec_Gui.lib
{
    class PlusbeZkSet
    {
        public FileUtils fileutils;
        public string SoftPath { get; set; }
        public string SoftName { get; set; }
        public string SoftFolder { get; set; }
        public string SetConfigPath { get; set; }
        XmlDocument xml = new XmlDocument();
        XmlNode ParentNode;

        public PlusbeZkSet(FileUtils fileutils)
        {
            this.fileutils = fileutils;
            SoftName = "PlusbeZkSet.exe";
            string[] files = fileutils.searchFile(fileutils.path, SoftName);
            SoftPath = files.OrderByDescending(path => File.GetLastWriteTime(path)).FirstOrDefault();
            SoftFolder = System.IO.Path.GetDirectoryName(SoftPath);
            SetConfigPath = SoftFolder + @"\AppData\SetConfig.xml";
            XmlNodeList nodeList;
            xml.Load(SetConfigPath);
            nodeList = xml.GetElementsByTagName("SetConfig");
            ParentNode = nodeList[0];
        }



        public void Setting()
        {
            ParentNode.SelectSingleNode("myIp").InnerText = "IP将自动填写";
            xml.Save(SetConfigPath);
            string batch = "start /wait \"\" "+ SoftPath;
            CMD.ExecuteCommand(batch);
            System.IO.File.Copy(SetConfigPath, fileutils.BatInnerPath+ @"\PlusbeZkSet\SetConfig.xml", true);
        }
    }
}
