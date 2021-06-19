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
    class filesProcessor
    {
        string lbPath;
        string allVideosPath;

        string currentPlat;
        bool weAreDoingArcade = false;
        bool autoRenameOriginal;
        bool weAreOnlyDoingOnePlatform;
        bool skipPlats;
        bool isAllVideosPathOnANetwork;
        bool doThemes;
        bool doSnaps;

        bool doAudio;
        bool doImages;
        string allAudioPath;
        string allImagesPath;

        int maxTries;

        XmlDocument launchboxSettingsFile;
        XmlNodeList xmListPlatforms;

        ListBox listboxGames;

       // string currentPlatformLBVidPath;
        //string currPlatOriginalVidPath;

        public filesProcessor(string launchboxPath, string allVideosPath, bool autoRenameOriginal, XmlDocument launchboxSettingsFile, XmlNodeList xmListPlatforms,
            bool weAreOnlyDoingOnePlatform , ListBox listboxGames, bool skipPlats, bool isAllVideosPathOnANetwork, int maxTries, bool doThemes, bool doSnaps,
            bool doAudio, bool doImages, string allAudioPath, string allImagesPath)
        {
            this.lbPath = launchboxPath;
            this.allVideosPath = allVideosPath;
            this.autoRenameOriginal = autoRenameOriginal;
            this.launchboxSettingsFile = launchboxSettingsFile;
            this.xmListPlatforms =  xmListPlatforms;
            this.weAreOnlyDoingOnePlatform = weAreOnlyDoingOnePlatform;
            this.listboxGames = listboxGames;
            this.skipPlats = skipPlats;
            this.isAllVideosPathOnANetwork = isAllVideosPathOnANetwork;
            this.maxTries = maxTries;
            this.doSnaps = doSnaps;
            this.doThemes = doThemes;
            this.doAudio = doAudio;
            this.doImages = doImages;
            this.allAudioPath = allAudioPath;
            this.allImagesPath = allImagesPath;
        }

        public string currentPlatform
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

        bool askUserIfWeShouldContinue()
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

        private void fillListBoxWithMissingGames(List<XmlNode> xmlGamesMissingFiles, ListBox listBoxGames)
        {
            foreach (XmlNode xmlNodeGame in xmlGamesMissingFiles)
            {
                    listBoxGames.Items.Add(xmlNodeGame["Title"].InnerText);
            }
        }

        public bool processPlatform(ref List<FileToBeCopied> filesToBeCopied)
        {
            bool keepGoing = true;

            string currentPlatformLBVidPath = getLaunchBoxMediaPlatformPath(xmListPlatforms, "Video");

            string currPlatOriginalVidPath = getCurrentPlatformOriginalVideosPath(weAreOnlyDoingOnePlatform);

            string currentPlatformLBAudioPath = getLaunchBoxMediaPlatformPath(xmListPlatforms, "Music");

            string currPlatOriginalAudioPath = getCurrentPlatformOriginaAudiolPath(weAreOnlyDoingOnePlatform);

            string currentPlatformLBImagesPath = getLaunchBoxMediaPlatformPath(xmListPlatforms, "Images");  

            string currPlatOriginalImagesPath = getCurrentPlatformOriginaImagesPath(weAreOnlyDoingOnePlatform);

            XmlNodeList xmlGames = getPlatformDataFile().SelectNodes("/LaunchBox/Game");

            //string folderPlatName = new DirectoryInfo(currPlatOriginalVidPath).Name;

            if(keepGoing && doSnaps)
            {
                listboxGames.Items.Add("----Snaps----");
                keepGoing = (doNormalVideos(xmlGames, ref filesToBeCopied, currentPlatformLBVidPath, currPlatOriginalVidPath));
            }

            if (keepGoing && doThemes)
            {
                listboxGames.Items.Add("----Themes----");
                keepGoing = doThemesVideos(xmlGames, ref filesToBeCopied, currentPlatformLBVidPath, currPlatOriginalVidPath);
            }

            if (keepGoing && doAudio)
            {
                listboxGames.Items.Add("----Audio----");
                keepGoing = doAudioFiles(xmlGames, ref filesToBeCopied, currentPlatformLBAudioPath, currPlatOriginalAudioPath);
            }

            if (keepGoing && doImages)
            {
                listboxGames.Items.Add("----Images----");
                keepGoing = doThemesVideos(xmlGames, ref filesToBeCopied, currentPlatformLBImagesPath, currPlatOriginalImagesPath);
            }

            return keepGoing;//was true
        }
        
        private bool doAudioFiles(XmlNodeList xmlGames, ref List<FileToBeCopied> filesToBeCopied,
            string currentPlatformLBAudioPath, string currPlatOriginalAudioPath)
        {
            List<XmlNode> xmlGamesMissingFiles = new List<XmlNode>();
            int nOfMissing = getGamesWithMissingAudioFiles(xmlGames, ref xmlGamesMissingFiles, currentPlatformLBAudioPath);
            if (nOfMissing == 0) return true;

            fillListBoxWithMissingGames(xmlGamesMissingFiles, listboxGames);

            if (!Directory.Exists(currPlatOriginalAudioPath))
                return (askUserIfWeShouldContinue());

            string[] candidateFilesPaths = getCandidateFilesPaths(currPlatOriginalAudioPath);

            foreach (XmlNode xmlNodeGame in xmlGamesMissingFiles)
            {
                if (findBestFile(xmlNodeGame, currentPlatformLBAudioPath, currPlatOriginalAudioPath,
                isAllVideosPathOnANetwork, ref filesToBeCopied, ref candidateFilesPaths, false)
                == false)
                    return false;
            }
            return true;
        }

        private bool doImageFiles(XmlNodeList xmlGames, ref List<FileToBeCopied> filesToBeCopied,
    string currentPlatformLBImagesdPath, string currPlatOriginalImagesPath)
        {
            List<XmlNode> xmlGamesMissingFiles = new List<XmlNode>();
            int nOfMissing = getGamesWithMissingImageFiles(xmlGames, ref xmlGamesMissingFiles, currentPlatformLBImagesdPath);
            if (nOfMissing == 0) return true;
            fillListBoxWithMissingGames(xmlGamesMissingFiles, listboxGames);

            if (!Directory.Exists(currPlatOriginalImagesPath))
                return (askUserIfWeShouldContinue());

            string[] candidateFilesPaths = getCandidateFilesPaths(currPlatOriginalImagesPath);

            foreach (XmlNode xmlNodeGame in xmlGamesMissingFiles)
            {
                if (findBestFile(xmlNodeGame, currentPlatformLBImagesdPath, currPlatOriginalImagesPath,
                isAllVideosPathOnANetwork, ref filesToBeCopied, ref candidateFilesPaths, false)
                == false)
                    return false;
            }
            return true;
        }

        private bool doNormalVideos(XmlNodeList xmlGames, ref List<FileToBeCopied>  filesToBeCopied,
            string currentPlatformLBVidPath, string currPlatOriginalVidPath)
        {
            List<XmlNode> xmlGamesMissingFiles = new List<XmlNode>();
            int nOfMissing = getGamesWithMissingVideoFiles(xmlGames, ref xmlGamesMissingFiles, currentPlatformLBVidPath);
            if (nOfMissing == 0) return true;
            fillListBoxWithMissingGames(xmlGamesMissingFiles, listboxGames);

            if (!Directory.Exists(currPlatOriginalVidPath))
                return (askUserIfWeShouldContinue());

            string[] candidateFilesPaths = getCandidateFilesPaths(currPlatOriginalVidPath);

                foreach (XmlNode xmlNodeGame in xmlGamesMissingFiles)
            {
                    if (findBestFile(xmlNodeGame, currentPlatformLBVidPath, currPlatOriginalVidPath,
                    isAllVideosPathOnANetwork, ref filesToBeCopied, ref candidateFilesPaths, false)
                    == false)
                        return false;
            }
            return true;
        }

        private bool doThemesVideos(XmlNodeList xmlGames, ref List<FileToBeCopied> filesToBeCopied,
            string currentPlatformLBVidPath, string currPlatOriginalVidPath)
        {
            string currPlatLbThemeFolder = Path.GetFullPath(currentPlatformLBVidPath + @"\" + "theme");
            string currPlatOriginalThemeFolder = Path.GetFullPath(currPlatOriginalVidPath + @"\" + "theme");

            List<XmlNode> xmlGamesMissingFiles = new List<XmlNode>();
            int nOfMissing = getGamesWithMissingThemeFiles(xmlGames, ref xmlGamesMissingFiles, currPlatLbThemeFolder);
            if (nOfMissing == 0) return true;
            fillListBoxWithMissingGames(xmlGamesMissingFiles, listboxGames);

            if (!Directory.Exists(currPlatOriginalThemeFolder))
                return (askUserIfWeShouldContinue());

            createFolderIfDoesntExist(currPlatLbThemeFolder);

            string[] candidateFilesPaths = getCandidateFilesPaths(currPlatOriginalThemeFolder);

            foreach (XmlNode xmlNodeGame in xmlGames)
            {
                if (!checkIfMediaFileExists(xmlNodeGame, currPlatLbThemeFolder))
                {
                    if (findBestFile(xmlNodeGame, currPlatLbThemeFolder, currPlatOriginalThemeFolder,
                    isAllVideosPathOnANetwork, ref filesToBeCopied, ref candidateFilesPaths, true)
                    == false)
                        return false;
                }
            }
            return true;
        }

        private int getGamesWithMissingThemeFiles(XmlNodeList xnList, ref List<XmlNode> xmlGamesMissingFiles, string lBThemeVideoPath)
        {
            int nOfMissing = 0;

            foreach (XmlNode xmlNodeGame in xnList)
            {
                if (checkIfThemeVideoFileExists(xmlNodeGame, lBThemeVideoPath))
                    continue;
                else
                {
                    xmlGamesMissingFiles.Add(xmlNodeGame);
                    nOfMissing++;
                }
            }
            return nOfMissing;
        }

        bool checkIfThemeVideoFileExists(XmlNode game, string gamePlatVidPath)
        {
                   
            string fileName;

            if (weAreDoingArcade)
                fileName = getRomFileName(game);
            else
                fileName = generalUtils.removeInvalidCharsWindowsFileSystem(game["Title"].InnerText);

            return fileExistsIgnoreExtension(fileName, gamePlatVidPath);
      
    }

        private List<string> getFileNamesWithoutExtensionsFromFullPaths(string[] fullPaths)
        {
            List<string> candidateFilesNotFullPath = new List<string>();
            foreach (string fullPath in fullPaths)
            {
                candidateFilesNotFullPath.Add(Path.GetFileNameWithoutExtension(fullPath));
            }
            return candidateFilesNotFullPath;
        }

        private List<string> getExtensionsFromFullPaths(string[] fullPaths)
        {
            List<string> extensions = new List<string>();
            foreach (string fullPath in fullPaths)
            {
                extensions.Add(Path.GetExtension(fullPath));
            }
            return extensions;
        }


        List<int> getLimitedArrayOfIndexesOrderedByLeveshteinDistance(List<int> dists, int num)
        {
            List<int> distsCopy = dists.ToList();

            List<int> listOfIndexes = new List<int>();
            //initialize 0,1,2,3,4,5,6...
            for (int i = 0; i < dists.Count; i++)
                listOfIndexes.Add(i);

            return getNumLowestValues(distsCopy, listOfIndexes, num);
        }

        private static List<int> getNumLowestValues(List<int> list, List<int> secondListToSortByFirst, int numOfValues)//secondlistMustHave Same size as first
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

        List<int> getArrayOfIndexesOrderedByLeveshteinDistance(List<int> dists)
        {
            List<int> distsCopy = dists.ToList();

            List<int> listOfIndexes = new List<int>();
            //initialize 0,1,2,3,4,5,6...
            for (int i = 0; i < dists.Count; i++)
                listOfIndexes.Add(i);

            generalUtils.IntArrayInsertionSort(distsCopy, listOfIndexes);
            return listOfIndexes;
        }

        private DialogResult askUserIfVidIsOK(string nameOfCandidateFile, string gameName, int remainingTries, bool isTheme)
        {
            DialogIsVideoOk dialog = new DialogIsVideoOk(nameOfCandidateFile, gameName, remainingTries, currentPlat, isTheme);

            // return dialog.ShowDialog(this);

            return dialog.ShowDialog();////////////////////////////////////////////////////////////////////////////
        }
        private List<string> getCandidateFilesNames(string[] candidateFilesPaths)
        {
            List<string> fileNames = getFileNamesWithoutExtensionsFromFullPaths(candidateFilesPaths);

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

        private bool findObviousArcadeFiles(XmlNode xmlNodeGame, string gameName, ref string[] candidateFilesPaths, List<string> candidateFilesExtensions, string romFileName, string currentPlatformLBVidPath, string currPlatOriginalVidPath)
        {
            List<string> fileNames = getFileNamesWithoutExtensionsFromFullPaths(candidateFilesPaths);
            

            for (int i = 0; i < fileNames.Count(); i++)
            {
                if (string.Equals(fileNames[i], romFileName, StringComparison.OrdinalIgnoreCase))
                {
                    FileToBeCopied file = new FileToBeCopied(currentPlatformLBVidPath, currPlatOriginalVidPath, gameName,
                        candidateFilesExtensions[i], candidateFilesPaths[i], autoRenameOriginal, weAreDoingArcade, romFileName);

                    candidateFilesPaths[i] = file.getNewOriginalFilePath();
                    file.processFile();
                    return true;
                }
            }
            return false;
        }

        private bool findFilesByAskingUser(string gameName, ref string[] candidateFilesPaths,
            List<string> candidateFilesExtensions, string romName, bool isTheme, string currentPlatformLBVidPath, string currPlatOriginalVidPath)
        {
            bool keepGoing = true; 
            List<string> candidateFilesFullNamesOfGames = getCandidateFilesNames(candidateFilesPaths);

            List<int> leveshteinDistances = stringDistances.leveshteinDistancesDiscardingCommonBetweenListAndString(candidateFilesFullNamesOfGames, gameName);

            List<int> preferedVidCandidatesByOrder = getLimitedArrayOfIndexesOrderedByLeveshteinDistance(leveshteinDistances, maxTries);

            for (int i = 0; i < preferedVidCandidatesByOrder.Count && i < maxTries; i++)
            {
                DialogResult result = askUserIfVidIsOK(candidateFilesFullNamesOfGames[preferedVidCandidatesByOrder[i]],
                    gameName, maxTries - 1 - i, isTheme);

                if (result == DialogResult.Yes)
                {
                    FileToBeCopied file = new FileToBeCopied(currentPlatformLBVidPath, currPlatOriginalVidPath, gameName,
                        candidateFilesExtensions[preferedVidCandidatesByOrder[i]],
                        candidateFilesPaths[preferedVidCandidatesByOrder[i]],
                        autoRenameOriginal, weAreDoingArcade, romName);

                    candidateFilesPaths[preferedVidCandidatesByOrder[i]] = file.getNewOriginalFilePath();  //candidateFilePathsHaveBeenChangedWithTheNewGame                  

                    file.processFile();

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

        public bool findBestFile
    (XmlNode game, string currentPlatformLBVidPath, string currPlatOriginalVidPath,
    bool isAllVideosPathOnANetwork, ref List<FileToBeCopied> filesToBeCopied, ref string[] candidateFilesPaths, bool isTheme)
        {
            string gameName = game["Title"].InnerText;

            //List<string> candidateFilesNames = getFileNamesWithoutExtensionsFromFullPaths(candidateFilesPaths);

            List<string> candidateFilesExtensions = getExtensionsFromFullPaths(candidateFilesPaths);

            string romFileName = getRomFileName(game);

            if (weAreDoingArcade) //do the obvious first, files that have same name as roms...
            {
                if (findObviousArcadeFiles(game, gameName, ref candidateFilesPaths, candidateFilesExtensions, romFileName, currentPlatformLBVidPath, currPlatOriginalVidPath))
                    return true;
            }

            return findFilesByAskingUser(gameName, ref candidateFilesPaths, candidateFilesExtensions, romFileName, isTheme, currentPlatformLBVidPath, currPlatOriginalVidPath);
        }

        public XmlDocument getPlatformDataFile()
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

        public string getCurrentPlatformOriginalVideosPath(bool weAreOnlyDoingOnePlatform)
        {
            if (weAreOnlyDoingOnePlatform)
                return allVideosPath;
            else
                return Path.GetFullPath(this.allVideosPath + @"\" + this.currentPlat);
        }

        public string getCurrentPlatformOriginaAudiolPath(bool weAreOnlyDoingOnePlatform)
        {
            if (weAreOnlyDoingOnePlatform)
                return allAudioPath;
            else
                return Path.GetFullPath(this.allAudioPath + @"\" + this.currentPlat);
        }

        public string getCurrentPlatformOriginaImagesPath(bool weAreOnlyDoingOnePlatform)
        {
            if (weAreOnlyDoingOnePlatform)
                return allImagesPath;
            else
                return Path.GetFullPath(this.allImagesPath + @"\" + this.currentPlat);
        }


        
            public int getGamesWithMissingAudioFiles(XmlNodeList xnList, ref List<XmlNode> xmlGamesMissingFiles, string lBAudioPath)
        {
            int nOfMissing = 0;

            foreach (XmlNode xmlNodeGame in xnList)
            {
                if (checkIfAudioExistsInLBVidFolderAndDatabase(xmlNodeGame, lBAudioPath))
                    continue;
                else
                {
                    xmlGamesMissingFiles.Add(xmlNodeGame);
                    nOfMissing++;
                }
            }
            return nOfMissing;
        }

        public int getGamesWithMissingImageFiles(XmlNodeList xnList, ref List<XmlNode> xmlGamesMissingFiles, string lBImagesPath)
        {
            int nOfMissing = 0;

            foreach (XmlNode xmlNodeGame in xnList)
            {
                if (checkIfVidExistsInLBVidFolderAndDatabase(xmlNodeGame, lBImagesPath))
                    continue;
                else
                {
                    xmlGamesMissingFiles.Add(xmlNodeGame);
                    nOfMissing++;
                }
            }
            return nOfMissing;
        }


        public int getGamesWithMissingVideoFiles(XmlNodeList xnList, ref List<XmlNode> xmlGamesMissingFiles, string lBVideoPath)
        {
            int nOfMissing = 0;

            foreach (XmlNode xmlNodeGame in xnList)
            {
                if (checkIfVidExistsInLBVidFolderAndDatabase(xmlNodeGame, lBVideoPath))
                    continue;
                else
                {
                    xmlGamesMissingFiles.Add(xmlNodeGame);
                    nOfMissing++;
                }
            }
            return nOfMissing;
        }

        public bool checkIfVidExistsInLBVidFolderAndDatabase(XmlNode game, string gamePlatVidPath)
        {
            return (game["MissingVideo"].InnerText == "false" || checkIfMediaFileExists(game, gamePlatVidPath));
            
        }

        public bool checkIfAudioExistsInLBVidFolderAndDatabase(XmlNode game, string gamePlatAudioPath)
        {
            return (game["MissingMusic"].InnerText == "false" || checkIfMediaFileExists(game, gamePlatAudioPath));
           
        }

        public bool checkIfImagesExistsInLBVidFolderAndDatabase(XmlNode game, string gamePlatVidPath)/////////////////////////////////////SHITSHOWSIC
        {
            return (game["MissingVideo"].InnerText == "false" || checkIfMediaFileExists(game, gamePlatVidPath));
            
        }

        public bool checkIfMediaFileExists(XmlNode game, string gamePlatMediaPath)
        {
            string fileName;

            if (weAreDoingArcade)
                fileName = getRomFileName(game);
            else
                fileName = generalUtils.removeInvalidCharsWindowsFileSystem(game["Title"].InnerText);

            return fileExistsIgnoreExtension(fileName, gamePlatMediaPath);
        }

        private string getRomFileName(XmlNode game)
        {
            return Path.GetFileNameWithoutExtension(generalUtils.removeInvalidCharsForPaths(game["ApplicationPath"].InnerText));
        }


        private bool fileExistsIgnoreExtension(string fileNameWithoutExtension, string directory)
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

        //mediaType can be either "Video", or "Music", or "Images"
        public string getLaunchBoxMediaPlatformPath(XmlNodeList xmListPlatforms, string mediaType)
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



        public string[] getCandidateFilesPaths(string originalMediaPaths)
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
