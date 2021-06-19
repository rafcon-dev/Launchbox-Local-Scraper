using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Launchbox_Local_Scraper.Properties;

namespace Launchbox_Local_Scraper
{
    static class MameUtils
    {
        static Dictionary<string, string> mameRoms = new Dictionary<string, string>();

        static MameUtils()
        {

            fillMameDictionary();
        }

        public static string getGameNameFromRomName(string romName)
        {
            try
            { 
            return mameRoms[romName];
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine
                    (@"Couldn't find game for rom called " + romName + " in mame.txt.\nMake sure mame.txt hasn't been modified!\nSkipping...");
            }
            return string.Empty;
        }

        private static void fillMameDictionary()
        {
            try
            {
                string folder = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

                // string content = File.ReadAllText(Path.GetFullPath(folder + "\\mame.txt"));
                string mameDatabase = Resources.mame;
                var regexMatches = Regex.Matches(mameDatabase, @"(\S+)\s+""(.*?)""");

                foreach (Match m in regexMatches)
                {
                    try
                    {
                        mameRoms.Add(m.Groups[1].ToString(), m.Groups[2].ToString());
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("Unable to build mame dictionary. Make sure mame.txt hasn't been modified ");
                    }
                    
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Make sure there is a file named mame.txt in the programs directory\n" + e.ToString());
            }
        }
    }
}
