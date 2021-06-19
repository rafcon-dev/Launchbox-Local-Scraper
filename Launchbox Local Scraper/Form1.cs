using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Text.RegularExpressions;

namespace Launchbox_Local_Scraper
{
    public partial class LaunchboxLocalScraper : Form
    {
        public LaunchboxLocalScraper()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private bool weHavePlatSelected()
        {
                if(comboBoxPlatforms.Text == "")
                {
                MessageBox.Show("No platform selected. Make sure to first press the \"Get Platforms\" button!", "Error!");
                return false;
            }
            return true;
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            if (areBothPathsWrong())
                return;

            listBoxGames.Items.Clear();

            bool skipPlats = checkBoxSkipPlats.Checked;
            bool doThemes = checkBoxDoThemes.Checked;
            bool doSnaps = checkBoxDoSnaps.Checked;
            string lbPath = textBoxLBInstallationPath.Text;
            string allVideosPath = textBoxVidsPath.Text;


            //  bool isAllVideosPathOnANetwork = checkIfOriginalFilesAreOnANetwork(allVideosPath);/////////////////////////////
            bool isAllVideosPathOnANetwork = false;
            bool weAreOnlyDoingOnePlat = checkBoxRunForOnePlatform.Checked;

            int maxTriesPerGame = 10;

            if (weAreOnlyDoingOnePlat && weHavePlatSelected() == false)
                return;

            ////get all platforms
            XmlDocument lBSettingsFile = getLBSettingsFile(lbPath);

            XmlNodeList platXmlList;

            List<FileToBeCopied> filesToBeCopied = new List<FileToBeCopied>();

            if (weAreOnlyDoingOnePlat)
                platXmlList = lBSettingsFile.SelectNodes("/LaunchBox/Platform" + "[Name='" + comboBoxPlatforms.Text + "']");
            else
                platXmlList = lBSettingsFile.SelectNodes("/LaunchBox/Platform");

            getFilesToBeCopied(ref filesToBeCopied, platXmlList, lbPath, allVideosPath,
             maxTriesPerGame, skipPlats, doThemes, isAllVideosPathOnANetwork, weAreOnlyDoingOnePlat, doSnaps);

            if (filesToBeCopied.Count > 0)
            {
                foreach (FileToBeCopied file in filesToBeCopied)
                    file.processFile();
            }

            MessageBox.Show("All done for now!", "Nice!");
        }

        private void getFilesToBeCopied(ref List<FileToBeCopied> filesToBeCopied, XmlNodeList platXmlList, string lbPath, 
            string allVideosPath, int maxTries, bool skipPlats, bool doThemes, bool isAllVideosPathOnANetwork, bool weAreOnlyDoingOnePlatform, bool doSnaps)
        {
            
            XmlDocument launchboxSettingsFile = getLBSettingsFile(lbPath);

            XmlNodeList xmListPlatforms = launchboxSettingsFile.SelectNodes("/LaunchBox/PlatformFolder");

            filesProcessor filesProcessor = new filesProcessor(lbPath, allVideosPath, checkBoxAutoRename.Checked,
                launchboxSettingsFile, xmListPlatforms, weAreOnlyDoingOnePlatform,  listBoxGames, skipPlats,  isAllVideosPathOnANetwork, maxTries, doThemes, doSnaps);

            foreach (XmlNode gamePlat in platXmlList)
            {
                filesProcessor.currentPlatform = gamePlat["Name"].InnerText;
                listBoxGames.Items.Add("---" + filesProcessor.currentPlatform + "---");

                if (filesProcessor.processPlatform(ref filesToBeCopied) == false)
                    break;
            }
        }

        /// <summary>
        /// Returns false if user canceled
        /// </summary>
        /// <param name="xmlNodeGame"></param>
        /// <param name="currentPlatformLBVidPath"></param>
        /// <param name="currPlatOriginalVidPath"></param>
        /// <param name="currentPlatName"></param>
        /// <param name="isAllVideosPathOnANetwork"></param>
        /// <param name="filesToBeCopied"></param>
        /// <returns></returns>

        void copyFilesThatAreToBeCopied(List<FileToBeCopied> files)
        {
            foreach (FileToBeCopied file in files)
            {
                file.processFile();
            }
        }

        string[] getOriginalVidsPaths(string allVidsPath)
        {
            try
            {
                return Directory.GetDirectories(allVidsPath);
            }
            catch (Exception except)
            {
                MessageBox.Show(
                    "Couldn't get original video paths.\nMake sure the path is ok, and that you have persmissions.\n.Exception:" + except.Message.ToString());
            }
            return new string[] { };
        }



        private XmlDocument getLBSettingsFile(string lBPath)
        {
            XmlDocument launchboxSettingsFile = new XmlDocument();

            string settingsFilePath = Path.GetFullPath(lBPath + @".\Data\Platforms.xml");

            try
            {
                launchboxSettingsFile.Load(settingsFilePath);
            }
            catch (Exception except)
            {
                MessageBox.Show("Couldn't load Launchbox Settings File. Make sure the path is ok, and that you have persmissions.\n" +
                    except.Message.ToString());
            }
            return launchboxSettingsFile;
        }

        private XmlDocument getPlatformDataFile(string lBPath, string plat)
        {
            XmlDocument platformDataFile = new XmlDocument();

            string fullPlatformPath = Path.GetFullPath(lBPath + @".\Data\Platforms\" + plat + @".xml");

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

        //LEVESHTEIN STUFF
      

        /// <summary>
        /// Searches a list, getting the lowest possible leveshtein distances between str and each of the strings on the list, ie.
        /// ie, for each string s, s="potato blue", string = "potato", potato is compared to "potato", "otato ", "tato b", "ato bl", etc
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="str"></param>
        /// <returns></returns>



        bool areBothPathsWrong()
        {
            if (!Directory.Exists(textBoxLBInstallationPath.Text) || !Directory.Exists(textBoxVidsPath.Text))
            {
                MessageBox.Show("Please set all paths right first!", "Error!");
                return true;
            }
            return false;
        }

        private void buttonCorrectVideoFoldersName_Click(object sender, EventArgs e)
        {
            if (areBothPathsWrong())
                return;

            string lbPath = textBoxLBInstallationPath.Text;

            string allVideosPath = textBoxVidsPath.Text;

            string[] originalVideoFilesPaths = Directory.GetDirectories(allVideosPath);

            XmlDocument lBSettingsFile = getLBSettingsFile(lbPath);

            //foreach platform, check the name of the video folder and the name of the other original folder

            string currentPlatformLBVideoPath = "not defined";

            XmlNodeList xmListPlatforms = lBSettingsFile.SelectNodes("/LaunchBox/PlatformFolder");

            foreach (XmlNode xmlNodePlatformFolder in xmListPlatforms)
            {
                if (xmlNodePlatformFolder["MediaType"].InnerText == "Video")
                {
                    currentPlatformLBVideoPath = xmlNodePlatformFolder["FolderPath"].InnerText;
                    listBoxGames.Items.Add(currentPlatformLBVideoPath);

                    currentPlatformLBVideoPath = generalUtils.getFullPathFromOriginalPath(currentPlatformLBVideoPath, lbPath);

                    string lBVidFolderName = new DirectoryInfo(currentPlatformLBVideoPath).Name;

                    if (isThisPlatformOriginalVideoPathCorrect(originalVideoFilesPaths, currentPlatformLBVideoPath))
                        continue;

                    foreach (string originalVideoFolderPath in originalVideoFilesPaths)
                    {
                        string originalVideoFolderName = new DirectoryInfo(originalVideoFolderPath).Name;

                        if (lBVidFolderName.Equals(originalVideoFolderName) == false) //if video folder names are different
                        {
                            //check if contains, ie, if is probable
                            if (originalVideoFolderName.Contains(lBVidFolderName) || 
                                lBVidFolderName.Contains(originalVideoFolderName))
                            {
                                string newOriginalVideoPath = Directory.GetParent(originalVideoFolderPath).FullName;
                                newOriginalVideoPath = Path.GetFullPath(newOriginalVideoPath + @"\" + lBVidFolderName);

                                //ask user
                                DialogResult result = MessageBox.Show(
                                    "Should the folder\n" +
                                    originalVideoFolderPath + "\n" +
                                    "be renamed to:\n" +
                                    newOriginalVideoPath + "?"
                                    , "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                                if (result == DialogResult.Yes)
                                {
                                    try
                                    {
                                        Directory.Move(originalVideoFolderPath, newOriginalVideoPath);
                                    }
                                    catch (Exception except)
                                    {
                                        MessageBox.Show("Couldn't change folder name, check that you have persmissions, or that the folder doesn't already exist.\n" +
                                            except.Message.ToString());
                                    }
                                }
                                else if (result == DialogResult.No)
                                {
                                    continue;
                                }
                                else if (result == DialogResult.Cancel)
                                {
                                    return;
                                }
                            }
                        }
                    }
                   
                }
            }
            MessageBox.Show("All done for now!");
        }

        private bool isThisPlatformOriginalVideoPathCorrect(string[] originalVideoFilesPaths, string platfrmLBVidPath)
        {
            string lBVidFolderName = new DirectoryInfo(platfrmLBVidPath).Name;

            foreach (string originalVidFolderPath in originalVideoFilesPaths)
            {
                string originalVidFolderName = new DirectoryInfo(originalVidFolderPath).Name;

                if (lBVidFolderName.Equals(originalVidFolderName) == true)
                {
                    return true;
                }
            }
            return false;
        }

        private void LaunchboxLocalScraper_Load(object sender, EventArgs e)
        {
            textBoxLBInstallationPath.Text = Properties.Settings.Default.launchboxPath;
            textBoxVidsPath.Text = Properties.Settings.Default.videosPath;

            checkBoxSkipPlats.Checked = Properties.Settings.Default.SkipPlatforms;
            checkBoxAutoRename.Checked = Properties.Settings.Default.renameOriginal;
            checkBoxRunForOnePlatform.Checked = Properties.Settings.Default.runForOnePlatform;
            checkBoxDoThemes.Checked = Properties.Settings.Default.doThemesAlso;
            checkBoxDoSnaps.Checked = Properties.Settings.Default.doVideoSnaps;
            enableOrDisableOnePlatformModeControls();
        }

        private void LaunchboxLocalScraper_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.launchboxPath = textBoxLBInstallationPath.Text;
            Properties.Settings.Default.videosPath = textBoxVidsPath.Text;

            Properties.Settings.Default.SkipPlatforms = checkBoxSkipPlats.Checked;
            Properties.Settings.Default.renameOriginal = checkBoxAutoRename.Checked;
            Properties.Settings.Default.runForOnePlatform = checkBoxRunForOnePlatform.Checked;
            Properties.Settings.Default.doThemesAlso = checkBoxDoThemes.Checked;
            Properties.Settings.Default.doVideoSnaps = checkBoxDoSnaps.Checked;

            Properties.Settings.Default.Save();
        }

        private void buttonBrowserFolder_Click(object sender, EventArgs e)
        {
            launchFolderBrowser("Select the Launchbox folder", textBoxLBInstallationPath);
        }

        private void videosPathBrowseButton_Click(object sender, EventArgs e)
        {
            launchFolderBrowser("Select the original videos folder", textBoxVidsPath);
        }

        void launchFolderBrowser(string title, TextBox textBoxPath)
        {
            var fsd = new FolderSelect.FolderSelectDialog();
            fsd.Title = title;

            if (Directory.Exists(textBoxPath.Text))
                fsd.InitialDirectory = textBoxPath.Text;
            else
                fsd.InitialDirectory = Path.GetPathRoot(System.Reflection.Assembly.GetEntryAssembly().Location); //hard drive of executable

            if (fsd.ShowDialog(IntPtr.Zero))
            {
                textBoxPath.Text = fsd.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string lbPath = textBoxLBInstallationPath.Text;

            if (!Directory.Exists(lbPath))
            {
                MessageBox.Show("Please set launchbox path right first!", "Error!");
                return;
            }

            XmlDocument lBSettingsFile = getLBSettingsFile(lbPath);

            XmlNodeList platXmlList = lBSettingsFile.SelectNodes("/LaunchBox/Platform");

            List<string> plats = new List<string>();

            foreach (XmlNode gamePlat in platXmlList)
            {
                plats.Add(gamePlat["Name"].InnerText);
            }

            plats.Sort();

            foreach (string gamePlat in plats)
            {
                comboBoxPlatforms.Items.Add(gamePlat);
            }

            if (comboBoxPlatforms.Items.Count > 0)
                comboBoxPlatforms.SelectedIndex = 0;

            //   groupBoxOneSystem.BackColor = Color.LimeGreen;
        }
        private bool checkIfOriginalFilesAreOnANetwork(string originalFilesPath)
        {
            bool isAllVideosPathOnANetwork = new Uri(originalFilesPath).IsUnc;
            if (isAllVideosPathOnANetwork)
                MessageBox.Show("Original Videos are on a network path. Files will be copied at the end.");

            return isAllVideosPathOnANetwork;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
         
        }

        private void checkBoxRunForOnePlatform_CheckedChanged(object sender, EventArgs e)
        {
            enableOrDisableOnePlatformModeControls();
        }

        private void enableOrDisableOnePlatformModeControls()
        {
            if (checkBoxRunForOnePlatform.Checked)
            {
                labelOriginalVideosPath.Text = "Original videos path for selected platform";
                comboBoxPlatforms.Enabled = true;
                buttonGetSystems.Enabled = true;
            }
            else
            {
                labelOriginalVideosPath.Text = "Original videos path for all platforms";
                comboBoxPlatforms.Enabled = false;
                buttonGetSystems.Enabled = false;
            }
        }

        private void buttonGuide_Click(object sender, EventArgs e)
        {
            HowToUse howToUseForm = new HowToUse();
            howToUseForm.ShowDialog();
        }

        private void checkBoxDoSnaps_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

        
