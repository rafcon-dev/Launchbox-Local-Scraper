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
    public partial class DialogIsVideoOk : Form
    {
        string videoName;
        string gameName;
        int remainingTries;

        public DialogIsVideoOk(string videoName, string gameName, int remainingTries, string platform, bool isTheme)
        {
            InitializeComponent();
            this.videoName = videoName;
            this.gameName = gameName;
            this.remainingTries = remainingTries;

            labelPlatform.Text = platform;
            labelVideoName.Text = this.videoName;
            labelGameName.Text = this.gameName;
            labelTries.Text = (this.remainingTries).ToString() + " more tries for this game.";

            if (isTheme)
                Text = "Choose Theme file...";
        }
                     
        private void labelText_Click(object sender, EventArgs e)
        {

        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void buttonSkipGame_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Ignore;
        }
    }
}
