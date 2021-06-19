using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace Launchbox_Local_Scraper
{
    class FileToBeCopied
    {
       // RoboCommand backup;/// <summary>
        /// /////////////////////////////////////////////////////////////////////
        /// </summary>
        private string platLBVidFolder;
        private string platOriginalVidFolder;
        private string gameName;
        private string ext;
        private string fileToCopy;
        private bool renameOriginalVideos;
        private bool weAreDoingArcade;
        private string romName;

        public FileToBeCopied(string platformLaunchboxVideoFolder, string platformOriginalVideoFolder, string gameName, string extension,
            string fileToCopy, bool renameOriginalVideos, bool weAreDoingArcade, string romFileName)
        {
            this.platLBVidFolder = platformLaunchboxVideoFolder;
            this.platOriginalVidFolder = platformOriginalVideoFolder;
            this.gameName = gameName;
            this.ext = extension;
            this.fileToCopy = fileToCopy;
            this.renameOriginalVideos = renameOriginalVideos;
            this.weAreDoingArcade = weAreDoingArcade;
            this.romName = romFileName;
        }

        private string concatenateVideoPath(string videoFolder)
        {
            string correctFileName;

            if (weAreDoingArcade)
                correctFileName = romName;
            else
                correctFileName = generalUtils.removeInvalidCharsWindowsFileSystem(gameName);

            return Path.GetFullPath(videoFolder + @"\" + (correctFileName) + ext);
        }

        public void processFileAsyncTask()
        {
            generalUtils.createFolderIfDoesntExist(platLBVidFolder);

            try
            {
                File.Copy(fileToCopy, concatenateVideoPath(platLBVidFolder));

                if (renameOriginalVideos && ! weAreDoingArcade) //only rename videos if they havent got mame filenames, because user might make mistake...
                {
                    string newFilePath = concatenateVideoPath(platOriginalVidFolder);

                    if (!newFilePath.ToUpper().Equals(fileToCopy.ToUpper())) //if the new file name is different than the original, all in upercase because windows is stupid
                    {
                        Directory.Move(fileToCopy, newFilePath); //renames original file to correct name
                    }
                }
            }

            catch (Exception except)
            {
                MessageBox.Show(
                    "Couldn't create file.\n Check if you have permissions\n. Exception:" + except.Message.ToString());
            }
        }

        /// <summary>
        /// copies file to launchbox folder and renames original file if  renameOriginalVideos is true
        /// </summary>
        public void processFile()
        {
           // processFileAsyncTask();
            //do async because we don't want to wait for files to copy...
            Task task = new Task(new Action(processFileAsyncTask));
            task.Start();
        }

        public string getNewOriginalFilePath()
        {
            return concatenateVideoPath(platOriginalVidFolder);
        }
    }
}
