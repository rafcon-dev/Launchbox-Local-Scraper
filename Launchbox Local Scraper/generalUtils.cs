using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Launchbox_Local_Scraper
{
    static class generalUtils
    {
        public static void createFolderIfDoesntExist(string folderPath)
        {
            try
            {
                Directory.CreateDirectory(folderPath);
            }
            catch (Exception except)
            {
                MessageBox.Show(
                    "Couldn't create folder.\n" + folderPath + "\n Check if you have permissions\n. Exception:" + except.Message.ToString());
            }
        }

        public static string removeInvalidCharsWindowsFileSystem(string str)
        {
            // Replace invalid characters with empty strings.
            try
            {
                string nstring = Regex.Replace(str, @"[^\w\.@\s\(\)\'\+\$-]", @"",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
                return nstring;
            }
            // If we timeout when replacing invalid characters, 
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

        public static string removeInvalidCharsForPaths(string str)
        {
            // Replace invalid characters with empty strings.
            try
            {
                string nstring = Regex.Replace(str, @"[^\w\.@\\\s\(\)\'\+\$-]", @"",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
                return nstring;
            }
            // If we timeout when replacing invalid characters, 
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

        public static string getFullPathFromOriginalPath( string originalPath, string fullRootPath)
        {
            if (!IsFullPath(originalPath))
            {
                return Path.GetFullPath(fullRootPath + @"\" + originalPath);
            }

            return originalPath;
        }

        public static string removeAccents(string s)
        {
            byte[] tempBytes;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(s);
            return System.Text.Encoding.UTF8.GetString(tempBytes);
        }

        private static bool IsFullPath(string path)
        {
            return !String.IsNullOrWhiteSpace(path)
                && path.IndexOfAny(System.IO.Path.GetInvalidPathChars().ToArray()) == -1
                && Path.IsPathRooted(path)
                && !Path.GetPathRoot(path).Equals(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal);
        }

        private static string replaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static string[] splitAtWords(string s)
        {
            return s.Split(" \t\r\n\x85\xA0.,;:!?()-\"".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }
        public static string eliminateFirstInstanceOfString(string s, string stringToEliminate)
        {
            return replaceFirst(s, stringToEliminate, "");
        }

        public static void IntArrayInsertionSort(List<int> list, List<int> secondListToSortByFirst) //secondlistMustHave Same size as first
        {
            int i, j;
            int N = list.Count;

            for (j = 1; j < N; j++)
            {
                for (i = j; i > 0 && list[i] < list[i - 1]; i--)
                {
                    exchange(list, i, i - 1);
                    exchange(secondListToSortByFirst, i, i - 1);
                }
            }
        }
        private static void exchange(List<int> data, int m, int n)
        {
            int temporary;

            temporary = data[m];
            data[m] = data[n];
            data[n] = temporary;
        }

        public static int getIndexOfMaxValue(List<int> list)
        {
            if (list.Count < 1)
                return -1;

            int index = 0;
            int max = list[0];

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] > max)
                {
                    max = list[i];
                    index = i;
                }
            }
            return index;
        }
    }
}
