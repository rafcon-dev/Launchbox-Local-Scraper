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

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private bool WeHavePlatSelected()
        {
                if(comboBoxPlatforms.Text == "")
                {
                MessageBox.Show("No platform selected. Make sure to first press the \"Get Platforms\" button!", "Error!");
                return false;
            }
            return true;
        }

        private void ButtonRun_Click(object sender, EventArgs e)
        {
            if (AreBothPathsWrong())
                return;

            listBoxGames.Items.Clear();

            bool skipPlats = checkBoxSkipPlats.Checked;
            bool doThemes = checkBoxDoThemes.Checked;
            bool doSnaps = checkBoxDoSnaps.Checked;
            string lbPath = textBoxLBInstallationPath.Text;
            string allVideosPath = textBoxVidsPath.Text;

            bool weAreOnlyDoingOnePlat = checkBoxRunForOnePlatform.Checked;

            int maxTriesPerGame = 10;

            if (weAreOnlyDoingOnePlat && WeHavePlatSelected() == false)
                return;

            ////get all platforms
            XmlDocument lBSettingsFile = GetLBSettingsFile(lbPath);

            XmlNodeList platXmlList;

            List<FileToBeCopied> filesToBeCopied = new List<FileToBeCopied>();

            if (weAreOnlyDoingOnePlat)
                platXmlList = lBSettingsFile.SelectNodes("/LaunchBox/Platform" + "[Name='" + comboBoxPlatforms.Text + "']");
            else
                platXmlList = lBSettingsFile.SelectNodes("/LaunchBox/Platform");

            GetFilesToBeCopied(platXmlList, lbPath, allVideosPath,
             maxTriesPerGame, skipPlats, doThemes, weAreOnlyDoingOnePlat, doSnaps);

            if (filesToBeCopied.Count > 0)
            {
                foreach (FileToBeCopied file in filesToBeCopied)
                    file.ProcessFile();
            }

            MessageBox.Show("All done for now!", "Nice!");
        }

        private void GetFilesToBeCopied(XmlNodeList platXmlList, string lbPath, 
            string allVideosPath, int maxTries, bool skipPlats, bool doThemes, bool weAreOnlyDoingOnePlatform, bool doSnaps)
        {
            
            XmlDocument launchboxSettingsFile = GetLBSettingsFile(lbPath);

            XmlNodeList xmListPlatforms = launchboxSettingsFile.SelectNodes("/LaunchBox/PlatformFolder");

            FilesProcessor filesProcessor = new FilesProcessor(lbPath, allVideosPath, checkBoxAutoRename.Checked,
                 xmListPlatforms, weAreOnlyDoingOnePlatform,  listBoxGames, skipPlats, maxTries, doThemes, doSnaps);

            foreach (XmlNode gamePlat in platXmlList)
            {
                filesProcessor.CurrentPlatform = gamePlat["Name"].InnerText;
                listBoxGames.Items.Add("---" + filesProcessor.CurrentPlatform + "---");

                if (filesProcessor.ProcessPlatform() == false)
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

        private XmlDocument GetLBSettingsFile(string lBPath)
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

        private XmlDocument GetPlatformDataFile(string lBPath, string plat)
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



        bool AreBothPathsWrong()
        {
            if (!Directory.Exists(textBoxLBInstallationPath.Text) || !Directory.Exists(textBoxVidsPath.Text))
            {
                MessageBox.Show("Please set all paths right first!", "Error!");
                return true;
            }
            return false;
        }

        private void ButtonCorrectVideoFoldersName_Click(object sender, EventArgs e)
        {
            if (AreBothPathsWrong())
                return;

            string lbPath = textBoxLBInstallationPath.Text;

            string allVideosPath = textBoxVidsPath.Text;

            string[] originalVideoFilesPaths = Directory.GetDirectories(allVideosPath);

            XmlDocument lBSettingsFile = GetLBSettingsFile(lbPath);

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

                    if (IsThisPlatformOriginalVideoPathCorrect(originalVideoFilesPaths, currentPlatformLBVideoPath))
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

        private bool IsThisPlatformOriginalVideoPathCorrect(string[] originalVideoFilesPaths, string platfrmLBVidPath)
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
            EnableOrDisableOnePlatformModeControls();
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

        private void ButtonBrowserFolder_Click(object sender, EventArgs e)
        {
            LaunchFolderBrowser("Select the Launchbox folder", textBoxLBInstallationPath);
        }

        private void VideosPathBrowseButton_Click(object sender, EventArgs e)
        {
            LaunchFolderBrowser("Select the original videos folder", textBoxVidsPath);
        }

        void LaunchFolderBrowser(string title, TextBox textBoxPath)
        {
            var fsd = new FolderSelect.FolderSelectDialog() { Title = title };

            if (Directory.Exists(textBoxPath.Text))
                fsd.InitialDirectory = textBoxPath.Text;
            else
                fsd.InitialDirectory = Path.GetPathRoot(System.Reflection.Assembly.GetEntryAssembly().Location); //hard drive of executable

            if (fsd.ShowDialog(IntPtr.Zero))
            {
                textBoxPath.Text = fsd.FileName;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string lbPath = textBoxLBInstallationPath.Text;

            if (!Directory.Exists(lbPath))
            {
                MessageBox.Show("Please set launchbox path right first!", "Error!");
                return;
            }

            XmlDocument lBSettingsFile = GetLBSettingsFile(lbPath);

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


        private void Button1_Click_1(object sender, EventArgs e)
        {
         
        }

        private void CheckBoxRunForOnePlatform_CheckedChanged(object sender, EventArgs e)
        {
            EnableOrDisableOnePlatformModeControls();
        }

        private void EnableOrDisableOnePlatformModeControls()
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

        private void ButtonGuide_Click(object sender, EventArgs e)
        {
            HowToUse howToUseForm = new HowToUse();
            howToUseForm.ShowDialog();
        }

        private void CheckBoxDoSnaps_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

        
