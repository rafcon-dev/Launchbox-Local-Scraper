using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Launchbox_Local_Scraper
{
    public partial class HowToUse : Form
    {
        public HowToUse()
        {
            InitializeComponent();
        }

        private void HowToUse_Load(object sender, EventArgs e)
        {
            richTextBoxHowTo.Text =
                "\nThis program is a scraper for your video files, to be used with launchbox.\n\n"+
                @"------------What it does:------------" + "\n" +
                "Imagine you have in your possesison a full collection of video snaps and theme videos for launchbox." + "\n" +
                @"Now, you might just have them all in your launchbox\videos\ folder, and everything will work." + "\n" +
                @"However, you might prefer to have only some games that you like in your launchbox installation, "+
                @"in a faster SSD, or you might have a Raspberry Pi, which has limited space." + "\n" +
                @"In those cases, you will want to only get the videos that correspond to the games you have in launchbox." + "\n" +
                @"This program does that automatically for you." + "\n" +
                "\n-----------------------------------------------------------------------------------------------------------" + "\n\n" +
                @"------------How do you use this program then??------------" + "\n" +
                @"Glad you ask!" + "\n\n" +
                @"-1: set the path of your launchbox installation. Usually C:\program Files\Launchbox for example" + "\n" +
                @"-2: set the path of the folder in which you have your full video collection. For example D:\Launchbox Videos\" + "\n" +
                "-3: Check the checkboxes \"Run for videos snaps\" and \"Run for Themes\" depending on\n"+
                "if you want to get game snaps, theme videos, or both\n"+
                @"-4: Press Run" + "\n" +
                "\n-----------------------------------------------------------------------------------------------------------" + "\n\n" +
                "------------Things to have in consideration:------------" + "\n\n" +
                "You have to have your video folders with the same name of the folders in the launchbox directory." + "\n" +
                "ie, if a folder is called \"sega saturn\" in launchbox, the correspondent in the folder that contains" +
                "all your videos can't be called only \"saturn\"." + "\n" +
                "The blue button \"Correct Names of video folders attempts to solve this, but you should check." + "\n\n" +
                "If you want, you can run the program for just one game platform." + "\n" +
                "To do that, check \"Run for One Platform Only\", press \"Get Platforms\" and select the platform you want to run for.\n" +
                "\n-----------------------------------------------------------------------------------------------------------" + "\n\n" +
                "------------\"Other Options\" explanation:------------\n" +
                "--\"Ignore platform if no original videos are present\" - check this if you don't want to be reminded that" +
                "the program didn't found any videos for a specific platform.\n\n" +
                "--\"Rename original videos\": check this if you also want the original videos in your video collection to be" +
                "permanently renamed to the name that launchbox is expecting for each game.\n\n"+
                "Any suggestions, you can find me on Launchbox's forum.\n"+
                "You're welcome. Be happy.\n"+
                "                                                                              Starplayer";
                
                ;



        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void richTextBoxHowTo_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
