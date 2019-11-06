using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Shell32;
using System.Security.Permissions;
using System.Globalization;

namespace PsExec_Gui.lib
{
    public class FileUtils
    {
        public string path { get; set; }
        public string target { get; set; }
        public string ShutdownSoftFolderName { get; set; }
        public string PlusbeZKFolderName { get; set; }
        public string OfficeFolderName { get; set; }
        public string BatInnerPath { get; set; }
        public string BatCustomPath { get; set; }
        public string EquationExploitPath { get; set; }
        public string EquationExploitDllsPath { get; set; }
        public FileUtils()
        {
            path = Directory.GetCurrentDirectory();
            BatInnerPath = path + @"\BAT_Inner\";
            BatCustomPath = path + @"\BAT_Custom\";
            EquationExploitPath = path + @"\Tools\EquationExploit\";
            EquationExploitDllsPath = EquationExploitPath + @"Dlls\";
        }

        //filter files
        public IEnumerable<string> GetFiles(string path, IEnumerable<string> exclude=null, SearchOption searchOption = SearchOption.AllDirectories)
        {
            IEnumerable<FileInfo> files = new DirectoryInfo(path).EnumerateFiles("*.*", searchOption);
            if (exclude == null) exclude = Enumerable.Empty<string>();
            foreach (var filename in files)
            {
                if (!exclude.Any(x => x == filename.Name || (x.StartsWith("*") && x.Contains(filename.Extension))))
                {
                    yield return filename.FullName;
                }

            }
        }

        //filter folders
        public IEnumerable<string> FilterFolder(string path, List<string> _excludedDirectories)
        {
            var filteredDirs = Directory.GetDirectories(path).Where(d => !isExcluded(_excludedDirectories, d));
            return filteredDirs;
        }

        //method to check
        static bool isExcluded(List<string> exludedDirList, string target)
        {
            return exludedDirList.Any(d => new DirectoryInfo(target).Name.Equals(d));
        }

        //then use this
        public void copyFolder(string sourceDirectory, string targetDirectory)
        {
            Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(sourceDirectory, targetDirectory, true);
        }

        public string[] searchFile(string path, string pattern, string second_pattern = null, Boolean SearchSubDir = true)
        {
            if (second_pattern == null)
            {
                second_pattern = pattern;
            }
            String[] files;
            if (SearchSubDir)
                files = Directory.GetFiles(path, pattern, SearchOption.AllDirectories).Where(f => (new FileInfo(f).Attributes & FileAttributes.Hidden & FileAttributes.System) == 0).ToArray();
            else
                files = Directory.GetFiles(path, pattern, SearchOption.TopDirectoryOnly).Where(f => (new FileInfo(f).Attributes & FileAttributes.Hidden & FileAttributes.System) == 0).ToArray();
            List<string> TempList = new List<string>();

            foreach (string u1 in files)
            {
                if (u1.Contains(second_pattern) || second_pattern == pattern)
                {
                    TempList.Add(u1);
                }

            }
            return TempList.ToArray();
            //return files;
        }
        public string GetShortcutTargetFile(string shortcutFilename)
        {
            string pathOnly = System.IO.Path.GetDirectoryName(shortcutFilename);
            string filenameOnly = System.IO.Path.GetFileName(shortcutFilename);

            Shell shell = new Shell();
            Folder folder = shell.NameSpace(pathOnly);
            FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                Shell32.ShellLinkObject link = (Shell32.ShellLinkObject)folderItem.GetLink;
                return link.Path;
            }

            return string.Empty;
        }

        public void FileWrite(string path,string content,bool append )
        {
            if (!append) {
                // This text is added only once to the file.
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                // Create a file to write to.
                System.IO.File.WriteAllText(path, content, System.Text.Encoding.Default);
            }
            else {
                // This text is always added, making the file longer over time
                // if it is not deleted.
                System.IO.File.AppendAllText(path, content, System.Text.Encoding.Default);
            }
        }

        //find all file and folder in path
        public IEnumerable<string> Traverse(string rootDirectory)
        {
            IEnumerable<string> files = Enumerable.Empty<string>();
            IEnumerable<string> directories = Enumerable.Empty<string>();
            try
            {
                // The test for UnauthorizedAccessException.
                var permission = new FileIOPermission(FileIOPermissionAccess.PathDiscovery, rootDirectory);
                permission.Demand();

                files = Directory.GetFiles(rootDirectory);
                directories = Directory.GetDirectories(rootDirectory);
            }
            catch
            {
                // Ignore folder (access denied).
                rootDirectory = null;
            }

            if (rootDirectory != null)
                yield return rootDirectory;

            foreach (var file in files)
            {
                yield return file;
            }
            foreach (var directory in directories)
            {
                yield return directory;
            }

            // Recursive call for SelectMany.
            /*var subdirectoryItems = directories.SelectMany(Traverse);
            foreach (var result in subdirectoryItems)
            {
                yield return result;
            }*/
        }



        public void BinaryWrite(string filepath, dynamic content)
        {

            BinaryWriter bw;
            //create the file
            try
            {
                bw = new BinaryWriter(new FileStream(filepath, FileMode.Create));
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message + "\n Cannot create file.");
                return;
            }

            //writing into the file
            try
            {
                Console.WriteLine(content.GetType());
                if (content.GetType() == typeof(List<string>))
                {
                    foreach(string s in content)
                    {
                        bw.Write(byte.Parse(s, NumberStyles.HexNumber));
                    }
                }
                else
                {
                    bw.Write(content);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message + "\n Cannot write to file.");
                return;
            }
            bw.Close();
        }
    }
}
