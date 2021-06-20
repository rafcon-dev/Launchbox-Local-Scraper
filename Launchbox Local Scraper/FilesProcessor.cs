using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static Launchbox_Local_Scraper.generalUtils;

namespace Launchbox_Local_Scraper
{
    class FilesProcessor
    {
        readonly string lbPath;
        readonly string allVideosPath;

        string currentPlat;
        bool weAreDoingArcade = false;
        readonly bool autoRenameOriginal;
        readonly bool weAreOnlyDoingOnePlatform;
        readonly bool skipPlats;
        readonly bool doThemes;
        readonly bool doSnaps;


        readonly int maxTries;

       
        readonly XmlNodeList xmListPlatforms;

        readonly ListBox listboxGames;

       // string currentPlatformLBVidPath;
        //string currPlatOriginalVidPath;

        public FilesProcessor(string launchboxPath, string allVideosPath, bool autoRenameOriginal, XmlNodeList xmListPlatforms,
            bool weAreOnlyDoingOnePlatform , ListBox listboxGames, bool skipPlats, int maxTries, bool doThemes, bool doSnaps)
        {
            this.lbPath = launchboxPath;
            this.allVideosPath = allVideosPath;
            this.autoRenameOriginal = autoRenameOriginal;
           
            this.xmListPlatforms =  xmListPlatforms;
            this.weAreOnlyDoingOnePlatform = weAreOnlyDoingOnePlatform;
            this.listboxGames = listboxGames;
            this.skipPlats = skipPlats;
           
            this.maxTries = maxTries;
            this.doSnaps = doSnaps;
            this.doThemes = doThemes;
 
   

        }

        public string CurrentPlatform
        {
            get
            {
                return this.currentPlat;
            }
            set
            {
                this.currentPlat = value;

                if (string.Equals(currentPlat, "ARCADE", StringComparison.OrdinalIgnoreCase))
                    weAreDoingArcade = true;
                else
                    weAreDoingArcade = false;
            }
        }

        bool AskUserIfWeShouldContinue()
        {
            bool shouldWeContinue = true;

            if (!skipPlats)
            {
                DialogResult resultNoVids =
                    MessageBox.Show("Couldn't find original media for:\n" + currentPlat + ".\nSkipping this Platform...",
                    "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (resultNoVids == DialogResult.OK)
                    shouldWeContinue = true;
                else if (resultNoVids == DialogResult.Cancel)
                    shouldWeContinue = false;
            }
            return shouldWeContinue;
        }

        private void FillListBoxWithMissingGames(List<XmlNode> xmlGamesMissingFiles, ListBox listBoxGames)
        {
            foreach (XmlNode xmlNodeGame in xmlGamesMissingFiles)
            {
                    listBoxGames.Items.Add(xmlNodeGame["Title"].InnerText);
            }
        }

        public bool ProcessPlatform()
        {
            bool keepGoing = true;

            string currentPlatformLBVidPath = GetLaunchBoxMediaPlatformPath(xmListPlatforms, "Video");

            string currPlatOriginalVidPath = GetCurrentPlatformOriginalVideosPath(weAreOnlyDoingOnePlatform);


            XmlNodeList xmlGames = GetPlatformDataFile().SelectNodes("/LaunchBox/Game");

            if(keepGoing && doSnaps)
            {
                listboxGames.Items.Add("----Snaps----");
                keepGoing = (DoNormalVideos(xmlGames, currentPlatformLBVidPath, currPlatOriginalVidPath));
            }

            if (keepGoing && doThemes)
            {
                listboxGames.Items.Add("----Themes----");
                keepGoing = DoThemesVideos(xmlGames, currentPlatformLBVidPath, currPlatOriginalVidPath);
            }

            return keepGoing;//was true
        }
        
      


        private bool DoNormalVideos(XmlNodeList xmlGames,
            string currentPlatformLBVidPath, string currPlatOriginalVidPath)
        {
            List<XmlNode> xmlGamesMissingFiles = new List<XmlNode>();
            int nOfMissing = GetGamesWithMissingVideoFiles(xmlGames, ref xmlGamesMissingFiles, currentPlatformLBVidPath);
            if (nOfMissing == 0) return true;
            FillListBoxWithMissingGames(xmlGamesMissingFiles, listboxGames);

            if (!Directory.Exists(currPlatOriginalVidPath))
                return (AskUserIfWeShouldContinue());

            string[] candidateFilesPaths = GetCandidateFilesPaths(currPlatOriginalVidPath);

                foreach (XmlNode xmlNodeGame in xmlGamesMissingFiles)
            {
                    if (FindBestFile(xmlNodeGame, currentPlatformLBVidPath, currPlatOriginalVidPath,
                   ref candidateFilesPaths, false)
                    == false)
                        return false;
            }
            return true;
        }

        private bool DoThemesVideos(XmlNodeList xmlGames,
            string currentPlatformLBVidPath, string currPlatOriginalVidPath)
        {
            string currPlatLbThemeFolder = Path.GetFullPath(currentPlatformLBVidPath + @"\" + "theme");
            string currPlatOriginalThemeFolder = Path.GetFullPath(currPlatOriginalVidPath + @"\" + "theme");

            List<XmlNode> xmlGamesMissingFiles = new List<XmlNode>();
            int nOfMissing = GetGamesWithMissingThemeFiles(xmlGames, ref xmlGamesMissingFiles, currPlatLbThemeFolder);
            if (nOfMissing == 0) return true;
            FillListBoxWithMissingGames(xmlGamesMissingFiles, listboxGames);

            if (!Directory.Exists(currPlatOriginalThemeFolder))
                return (AskUserIfWeShouldContinue());

            createFolderIfDoesntExist(currPlatLbThemeFolder);

            string[] candidateFilesPaths = GetCandidateFilesPaths(currPlatOriginalThemeFolder);

            foreach (XmlNode xmlNodeGame in xmlGames)
            {
                if (!CheckIfMediaFileExists(xmlNodeGame, currPlatLbThemeFolder))
                {
                    if (FindBestFile(xmlNodeGame, currPlatLbThemeFolder, currPlatOriginalThemeFolder,
                    ref candidateFilesPaths, true)
                    == false)
                        return false;
                }
            }
            return true;
        }

        private int GetGamesWithMissingThemeFiles(XmlNodeList xnList, ref List<XmlNode> xmlGamesMissingFiles, string lBThemeVideoPath)
        {
            int nOfMissing = 0;

            foreach (XmlNode xmlNodeGame in xnList)
            {
                if (CheckIfThemeVideoFileExists(xmlNodeGame, lBThemeVideoPath))
                    continue;
                else
                {
                    xmlGamesMissingFiles.Add(xmlNodeGame);
                    nOfMissing++;
                }
            }
            return nOfMissing;
        }

        bool CheckIfThemeVideoFileExists(XmlNode game, string gamePlatVidPath)
        {
                   
            string fileName;

            if (weAreDoingArcade)
                fileName = GetRomFileName(game);
            else
                fileName = generalUtils.removeInvalidCharsWindowsFileSystem(game["Title"].InnerText);

            return FileExistsIgnoreExtension(fileName, gamePlatVidPath);
      
    }

        private List<string> GetFileNamesWithoutExtensionsFromFullPaths(string[] fullPaths)
        {
            List<string> candidateFilesNotFullPath = new List<string>();
            foreach (string fullPath in fullPaths)
            {
                candidateFilesNotFullPath.Add(Path.GetFileNameWithoutExtension(fullPath));
            }
            return candidateFilesNotFullPath;
        }

        private List<string> GetExtensionsFromFullPaths(string[] fullPaths)
        {
            List<string> extensions = new List<string>();
            foreach (string fullPath in fullPaths)
            {
                extensions.Add(Path.GetExtension(fullPath));
            }
            return extensions;
        }


        List<int> GetLimitedArrayOfIndexesOrderedByLeveshteinDistance(List<int> dists, int num)
        {
            List<int> distsCopy = dists.ToList();

            List<int> listOfIndexes = new List<int>();
            //initialize 0,1,2,3,4,5,6...
            for (int i = 0; i < dists.Count; i++)
                listOfIndexes.Add(i);

            return GetNumLowestValues(distsCopy, listOfIndexes, num);
        }

        private static List<int> GetNumLowestValues(List<int> list, List<int> secondListToSortByFirst, int numOfValues)//secondlistMustHave Same size as first
        {
            List<int> lowestValues = new List<int>();
            List<int> secondListSortedByLowest = new List<int>();

            if (list.Count < 1)
                return lowestValues;

            //initialize list
            for (int i = 0; i < numOfValues && i < list.Count; i++)
            {
                lowestValues.Add(list[i]);
                secondListSortedByLowest.Add(secondListToSortByFirst[i]);
            }


            int indexOfHighest = generalUtils.getIndexOfMaxValue(lowestValues);

            for (int i = numOfValues; i < list.Count; i++)
            {
                if (list[i] < lowestValues[indexOfHighest]) //we found a new lowest value!
                {
                    lowestValues[indexOfHighest] = list[i];
                    secondListSortedByLowest[indexOfHighest] = secondListToSortByFirst[i];
                    indexOfHighest = generalUtils.getIndexOfMaxValue(lowestValues);
                }
            }
            generalUtils.IntArrayInsertionSort(lowestValues, secondListSortedByLowest);

            return secondListSortedByLowest;
        }


        private DialogResult AskUserIfVidIsOK(string nameOfCandidateFile, string gameName, int remainingTries, bool isTheme)
        {
            DialogIsVideoOk dialog = new DialogIsVideoOk(nameOfCandidateFile, gameName, remainingTries, currentPlat, isTheme);

            // return dialog.ShowDialog(this);

            return dialog.ShowDialog();////////////////////////////////////////////////////////////////////////////
        }
        private List<string> GetCandidateFilesNames(string[] candidateFilesPaths)
        {
            List<string> fileNames = GetFileNamesWithoutExtensionsFromFullPaths(candidateFilesPaths);

            if (weAreDoingArcade == true)
            {
                List<string> mameGameNames = new List<string>();
                int i = 0;
                foreach (string fileName in fileNames)
                {
                    string mameGame = MameUtils.getGameNameFromRomName(fileName);
                    mameGameNames.Add(mameGame);

                    if(! string.IsNullOrEmpty(mameGame))
                        i++;
                }
                return mameGameNames;
            }
            else
                return fileNames;
    }

        private bool FindObviousArcadeFiles(string gameName, ref string[] candidateFilesPaths, List<string> candidateFilesExtensions, string romFileName, string currentPlatformLBVidPath, string currPlatOriginalVidPath)
        {
            List<string> fileNames = GetFileNamesWithoutExtensionsFromFullPaths(candidateFilesPaths);
            

            for (int i = 0; i < fileNames.Count(); i++)
            {
                if (string.Equals(fileNames[i], romFileName, StringComparison.OrdinalIgnoreCase))
                {
                    FileToBeCopied file = new FileToBeCopied(currentPlatformLBVidPath, currPlatOriginalVidPath, gameName,
                        candidateFilesExtensions[i], candidateFilesPaths[i], autoRenameOriginal, weAreDoingArcade, romFileName);

                    candidateFilesPaths[i] = file.GetNewOriginalFilePath();
                    file.ProcessFile();
                    return true;
                }
            }
            return false;
        }

        private bool FindFilesByAskingUser(string gameName, ref string[] candidateFilesPaths,
            List<string> candidateFilesExtensions, string romName, bool isTheme, string currentPlatformLBVidPath, string currPlatOriginalVidPath)
        {
            bool keepGoing = true; 
            List<string> candidateFilesFullNamesOfGames = GetCandidateFilesNames(candidateFilesPaths);

            List<int> leveshteinDistances = stringDistances.leveshteinDistancesDiscardingCommonBetweenListAndString(candidateFilesFullNamesOfGames, gameName);

            List<int> preferedVidCandidatesByOrder = GetLimitedArrayOfIndexesOrderedByLeveshteinDistance(leveshteinDistances, maxTries);

            for (int i = 0; i < preferedVidCandidatesByOrder.Count && i < maxTries; i++)
            {
                DialogResult result = AskUserIfVidIsOK(candidateFilesFullNamesOfGames[preferedVidCandidatesByOrder[i]],
                    gameName, maxTries - 1 - i, isTheme);

                if (result == DialogResult.Yes)
                {
                    FileToBeCopied file = new FileToBeCopied(currentPlatformLBVidPath, currPlatOriginalVidPath, gameName,
                        candidateFilesExtensions[preferedVidCandidatesByOrder[i]],
                        candidateFilesPaths[preferedVidCandidatesByOrder[i]],
                        autoRenameOriginal, weAreDoingArcade, romName);

                    candidateFilesPaths[preferedVidCandidatesByOrder[i]] = file.GetNewOriginalFilePath();  //candidateFilePathsHaveBeenChangedWithTheNewGame                  

                    file.ProcessFile();

                    break;//we're done with this game
                }
                else if (result == DialogResult.No)
                    continue;
                else if (result == DialogResult.Cancel)
                {
                    keepGoing = false;
                    break;
                }
                else if (result == DialogResult.Ignore)
                    break;
            }
            return keepGoing;
        }

        public bool FindBestFile
    (XmlNode game, string currentPlatformLBVidPath, string currPlatOriginalVidPath,
    ref string[] candidateFilesPaths, bool isTheme)
        {
            string gameName = game["Title"].InnerText;

            //List<string> candidateFilesNames = getFileNamesWithoutExtensionsFromFullPaths(candidateFilesPaths);

            List<string> candidateFilesExtensions = GetExtensionsFromFullPaths(candidateFilesPaths);

            string romFileName = GetRomFileName(game);

            if (weAreDoingArcade) //do the obvious first, files that have same name as roms...
            {
                if (FindObviousArcadeFiles(gameName, ref candidateFilesPaths, candidateFilesExtensions, romFileName, currentPlatformLBVidPath, currPlatOriginalVidPath))
                    return true;
            }

            return FindFilesByAskingUser(gameName, ref candidateFilesPaths, candidateFilesExtensions, romFileName, isTheme, currentPlatformLBVidPath, currPlatOriginalVidPath);
        }

        public XmlDocument GetPlatformDataFile()
        {
            XmlDocument platformDataFile = new XmlDocument();

            string fullPlatformPath = Path.GetFullPath(this.lbPath + @".\Data\Platforms\" + currentPlat + @".xml");

            try
            {
                platformDataFile.Load(fullPlatformPath);
            }
            catch (Exception except)
            {
                MessageBox.Show("Couldn't load Platform Data File. Make sure that Launchbox path is ok, and that you have persmissions.\n" +
                    except.Message.ToString());
            }
            return platformDataFile;
        }

        public string GetCurrentPlatformOriginalVideosPath(bool weAreOnlyDoingOnePlatform)
        {
            if (weAreOnlyDoingOnePlatform)
                return allVideosPath;
            else
                return Path.GetFullPath(this.allVideosPath + @"\" + this.currentPlat);
        }






      




        public int GetGamesWithMissingVideoFiles(XmlNodeList xnList, ref List<XmlNode> xmlGamesMissingFiles, string lBVideoPath)
        {
            int nOfMissing = 0;

            foreach (XmlNode xmlNodeGame in xnList)
            {
                if (CheckIfVidExistsInLBVidFolderAndDatabase(xmlNodeGame, lBVideoPath))
                    continue;
                else
                {
                    xmlGamesMissingFiles.Add(xmlNodeGame);
                    nOfMissing++;
                }
            }
            return nOfMissing;
        }

        public bool CheckIfVidExistsInLBVidFolderAndDatabase(XmlNode game, string gamePlatVidPath)
        {
            return (game["MissingVideo"].InnerText == "false" || CheckIfMediaFileExists(game, gamePlatVidPath));
            
        }





        public bool CheckIfMediaFileExists(XmlNode game, string gamePlatMediaPath)
        {
            string fileName;

            if (weAreDoingArcade)
                fileName = GetRomFileName(game);
            else
                fileName = generalUtils.removeInvalidCharsWindowsFileSystem(game["Title"].InnerText);

            return FileExistsIgnoreExtension(fileName, gamePlatMediaPath);
        }

        private string GetRomFileName(XmlNode game)
        {
            return Path.GetFileNameWithoutExtension(generalUtils.removeInvalidCharsForPaths(game["ApplicationPath"].InnerText));
        }


        private bool FileExistsIgnoreExtension(string fileNameWithoutExtension, string directory)
        {
            if (Directory.Exists(directory))
            {
                string[] files = Directory.GetFiles(directory);

                foreach (string file in files)
                {
                    if (fileNameWithoutExtension == Path.GetFileNameWithoutExtension(file))
                        return true;
                }
            }
            return false;
        }

        //mediaType can be either "Video"
        public string GetLaunchBoxMediaPlatformPath(XmlNodeList xmListPlatforms, string mediaType)
        {
            string currPlatlBVidPath = "not defined";
            foreach (XmlNode xmlNodePlatformFolder in xmListPlatforms)
            {
                if (xmlNodePlatformFolder["MediaType"].InnerText == mediaType)
                    if (xmlNodePlatformFolder["Platform"].InnerText == this.currentPlat)
                        currPlatlBVidPath = xmlNodePlatformFolder["FolderPath"].InnerText;
            }

            currPlatlBVidPath = generalUtils.getFullPathFromOriginalPath(currPlatlBVidPath, this.lbPath);

            return currPlatlBVidPath;
        }



        public string[] GetCandidateFilesPaths(string originalMediaPaths)
        {
            try
            {
                return Directory.GetFiles(originalMediaPaths, "*", SearchOption.TopDirectoryOnly);
            }
            catch (Exception except)
            {
                MessageBox.Show(
                    "Couldn't get original media.\nMake sure the path is ok, and that you have permissions.\n.Exception:" +
                   except.Message.ToString());
            }
            return new string[] { };
        }
    }
}
