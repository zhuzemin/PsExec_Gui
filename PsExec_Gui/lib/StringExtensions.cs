using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PsExec_Gui.lib;

namespace PsExec_Gui.lib
{
    static class StringExtensions
    {
        //Split string with length
        public static List<string> GetChunks(string value, int chunkSize)
        {
            List<string> triplets = new List<string>();
            for (int i = 0; i < value.Length; i += chunkSize)
                if (i + chunkSize > value.Length)
                    triplets.Add(value.Substring(i));
                else
                    triplets.Add(value.Substring(i, chunkSize));

            return triplets;
        }



        //// str - the source string
        //// index- the start location to replace at (0-based)
        //// length - the number of characters to be removed before inserting
        //// replace - the string that is replacing characters
        public static string ReplaceAt(this string str, int index, int length, string replace)
        {
            return str.Remove(index, Math.Min(length, str.Length - index))
                    .Insert(index, replace);
        }



        public static List<List<String>> ListCompare(List<String> source, List<String> target)
        {
            List<String> NewSource = new List<String>();
            List<String> NewTarget = new List<String>();
            foreach (String u1 in source)
            {
                bool duplicatefound = false;
                foreach (String u2 in target)
                    if (u1 == u2)
                    {
                        duplicatefound = true;
                        NewTarget.Add(u2);
                        break;
                    }

                if (!duplicatefound)
                    NewSource.Add(u1);
            }
            return new List<List<String>>() { NewSource, NewTarget };
        }



    }
}
