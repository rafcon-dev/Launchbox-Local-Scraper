namespace Launchbox_Local_Scraper
{
    partial class LaunchboxLocalScraper
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LaunchboxLocalScraper));
            this.buttonRun = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxLBInstallationPath = new System.Windows.Forms.TextBox();
            this.listBoxGames = new System.Windows.Forms.ListBox();
            this.labelOriginalVideosPath = new System.Windows.Forms.Label();
            this.textBoxVidsPath = new System.Windows.Forms.TextBox();
            this.buttonCorrectVideoFoldersName = new System.Windows.Forms.Button();
            this.buttonBrowserFolder = new System.Windows.Forms.Button();
            this.videosPathBrowseButton = new System.Windows.Forms.Button();
            this.checkBoxSkipPlats = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoRename = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.comboBoxPlatforms = new System.Windows.Forms.ComboBox();
            this.buttonGetSystems = new System.Windows.Forms.Button();
            this.checkBoxRunForOnePlatform = new System.Windows.Forms.CheckBox();
            this.groupBoxOneSystem = new System.Windows.Forms.GroupBox();
            this.checkBoxDoThemes = new System.Windows.Forms.CheckBox();
            this.checkBoxDoSnaps = new System.Windows.Forms.CheckBox();
            this.buttonGuide = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBoxOneSystem.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonRun
            // 
            this.buttonRun.BackColor = System.Drawing.Color.GreenYellow;
            this.buttonRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRun.Location = new System.Drawing.Point(366, 159);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(176, 56);
            this.buttonRun.TabIndex = 10;
            this.buttonRun.Text = "Run";
            this.toolTip1.SetToolTip(this.buttonRun, "For each game in launchbox, find the corresponding video form \"Videos Path\"");
            this.buttonRun.UseVisualStyleBackColor = false;
            this.buttonRun.Click += new System.EventHandler(this.ButtonRun_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Launchbox Path";
            this.label1.Click += new System.EventHandler(this.Label1_Click);
            // 
            // textBoxLBInstallationPath
            // 
            this.textBoxLBInstallationPath.Location = new System.Drawing.Point(120, 16);
            this.textBoxLBInstallationPath.Name = "textBoxLBInstallationPath";
            this.textBoxLBInstallationPath.Size = new System.Drawing.Size(344, 20);
            this.textBoxLBInstallationPath.TabIndex = 1;
            this.toolTip1.SetToolTip(this.textBoxLBInstallationPath, "Path of launchbox videos");
            // 
            // listBoxGames
            // 
            this.listBoxGames.FormattingEnabled = true;
            this.listBoxGames.Location = new System.Drawing.Point(6, 119);
            this.listBoxGames.Name = "listBoxGames";
            this.listBoxGames.Size = new System.Drawing.Size(221, 277);
            this.listBoxGames.TabIndex = 6;
            // 
            // labelOriginalVideosPath
            // 
            this.labelOriginalVideosPath.AutoSize = true;
            this.labelOriginalVideosPath.Location = new System.Drawing.Point(16, 56);
            this.labelOriginalVideosPath.MaximumSize = new System.Drawing.Size(110, 0);
            this.labelOriginalVideosPath.Name = "labelOriginalVideosPath";
            this.labelOriginalVideosPath.Size = new System.Drawing.Size(102, 26);
            this.labelOriginalVideosPath.TabIndex = 3;
            this.labelOriginalVideosPath.Text = "All platforms original videos path";
            // 
            // textBoxVidsPath
            // 
            this.textBoxVidsPath.Location = new System.Drawing.Point(120, 56);
            this.textBoxVidsPath.Name = "textBoxVidsPath";
            this.textBoxVidsPath.Size = new System.Drawing.Size(344, 20);
            this.textBoxVidsPath.TabIndex = 4;
            this.toolTip1.SetToolTip(this.textBoxVidsPath, "Path of all the videos");
            // 
            // buttonCorrectVideoFoldersName
            // 
            this.buttonCorrectVideoFoldersName.BackColor = System.Drawing.Color.LightBlue;
            this.buttonCorrectVideoFoldersName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCorrectVideoFoldersName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCorrectVideoFoldersName.Location = new System.Drawing.Point(233, 233);
            this.buttonCorrectVideoFoldersName.Name = "buttonCorrectVideoFoldersName";
            this.buttonCorrectVideoFoldersName.Size = new System.Drawing.Size(312, 24);
            this.buttonCorrectVideoFoldersName.TabIndex = 11;
            this.buttonCorrectVideoFoldersName.Text = "Correct Names of Video Folders";
            this.toolTip1.SetToolTip(this.buttonCorrectVideoFoldersName, "Matches the original videos folder names with those of launchbox");
            this.buttonCorrectVideoFoldersName.UseVisualStyleBackColor = false;
            this.buttonCorrectVideoFoldersName.Click += new System.EventHandler(this.ButtonCorrectVideoFoldersName_Click);
            // 
            // buttonBrowserFolder
            // 
            this.buttonBrowserFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBrowserFolder.Location = new System.Drawing.Point(480, 8);
            this.buttonBrowserFolder.Name = "buttonBrowserFolder";
            this.buttonBrowserFolder.Size = new System.Drawing.Size(64, 28);
            this.buttonBrowserFolder.TabIndex = 2;
            this.buttonBrowserFolder.Text = "Browse";
            this.buttonBrowserFolder.UseVisualStyleBackColor = true;
            this.buttonBrowserFolder.Click += new System.EventHandler(this.ButtonBrowserFolder_Click);
            // 
            // videosPathBrowseButton
            // 
            this.videosPathBrowseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.videosPathBrowseButton.Location = new System.Drawing.Point(480, 48);
            this.videosPathBrowseButton.Name = "videosPathBrowseButton";
            this.videosPathBrowseButton.Size = new System.Drawing.Size(64, 28);
            this.videosPathBrowseButton.TabIndex = 5;
            this.videosPathBrowseButton.Text = "Browse";
            this.videosPathBrowseButton.UseVisualStyleBackColor = true;
            this.videosPathBrowseButton.Click += new System.EventHandler(this.VideosPathBrowseButton_Click);
            // 
            // checkBoxSkipPlats
            // 
            this.checkBoxSkipPlats.AutoSize = true;
            this.checkBoxSkipPlats.Location = new System.Drawing.Point(8, 88);
            this.checkBoxSkipPlats.Name = "checkBoxSkipPlats";
            this.checkBoxSkipPlats.Size = new System.Drawing.Size(232, 17);
            this.checkBoxSkipPlats.TabIndex = 1;
            this.checkBoxSkipPlats.Text = "Ignore platform if no original files are present";
            this.toolTip1.SetToolTip(this.checkBoxSkipPlats, "If we don\'t have videos for a platform, ignore it");
            this.checkBoxSkipPlats.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoRename
            // 
            this.checkBoxAutoRename.AutoSize = true;
            this.checkBoxAutoRename.Location = new System.Drawing.Point(8, 112);
            this.checkBoxAutoRename.Name = "checkBoxAutoRename";
            this.checkBoxAutoRename.Size = new System.Drawing.Size(123, 17);
            this.checkBoxAutoRename.TabIndex = 2;
            this.checkBoxAutoRename.Text = "Rename original files";
            this.toolTip1.SetToolTip(this.checkBoxAutoRename, "Renames the original videos with the correct name");
            this.checkBoxAutoRename.UseVisualStyleBackColor = true;
            // 
            // comboBoxPlatforms
            // 
            this.comboBoxPlatforms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPlatforms.FormattingEnabled = true;
            this.comboBoxPlatforms.Location = new System.Drawing.Point(8, 32);
            this.comboBoxPlatforms.Name = "comboBoxPlatforms";
            this.comboBoxPlatforms.Size = new System.Drawing.Size(184, 21);
            this.comboBoxPlatforms.TabIndex = 1;
            // 
            // buttonGetSystems
            // 
            this.buttonGetSystems.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGetSystems.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGetSystems.Location = new System.Drawing.Point(208, 32);
            this.buttonGetSystems.Name = "buttonGetSystems";
            this.buttonGetSystems.Size = new System.Drawing.Size(80, 23);
            this.buttonGetSystems.TabIndex = 2;
            this.buttonGetSystems.Text = "Get Platforms";
            this.buttonGetSystems.UseVisualStyleBackColor = true;
            this.buttonGetSystems.Click += new System.EventHandler(this.Button1_Click);
            // 
            // checkBoxRunForOnePlatform
            // 
            this.checkBoxRunForOnePlatform.AutoSize = true;
            this.checkBoxRunForOnePlatform.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxRunForOnePlatform.Location = new System.Drawing.Point(8, 8);
            this.checkBoxRunForOnePlatform.Name = "checkBoxRunForOnePlatform";
            this.checkBoxRunForOnePlatform.Size = new System.Drawing.Size(243, 24);
            this.checkBoxRunForOnePlatform.TabIndex = 0;
            this.checkBoxRunForOnePlatform.Text = "Run For One Platform Only";
            this.checkBoxRunForOnePlatform.UseVisualStyleBackColor = true;
            this.checkBoxRunForOnePlatform.CheckedChanged += new System.EventHandler(this.CheckBoxRunForOnePlatform_CheckedChanged);
            // 
            // groupBoxOneSystem
            // 
            this.groupBoxOneSystem.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBoxOneSystem.Controls.Add(this.checkBoxRunForOnePlatform);
            this.groupBoxOneSystem.Controls.Add(this.buttonGetSystems);
            this.groupBoxOneSystem.Controls.Add(this.comboBoxPlatforms);
            this.groupBoxOneSystem.Location = new System.Drawing.Point(8, 16);
            this.groupBoxOneSystem.Name = "groupBoxOneSystem";
            this.groupBoxOneSystem.Size = new System.Drawing.Size(296, 64);
            this.groupBoxOneSystem.TabIndex = 0;
            this.groupBoxOneSystem.TabStop = false;
            // 
            // checkBoxDoThemes
            // 
            this.checkBoxDoThemes.AutoSize = true;
            this.checkBoxDoThemes.Location = new System.Drawing.Point(238, 198);
            this.checkBoxDoThemes.Name = "checkBoxDoThemes";
            this.checkBoxDoThemes.Size = new System.Drawing.Size(105, 17);
            this.checkBoxDoThemes.TabIndex = 9;
            this.checkBoxDoThemes.Text = "Run For Themes";
            this.checkBoxDoThemes.UseVisualStyleBackColor = true;
            // 
            // checkBoxDoSnaps
            // 
            this.checkBoxDoSnaps.AutoSize = true;
            this.checkBoxDoSnaps.Location = new System.Drawing.Point(237, 175);
            this.checkBoxDoSnaps.Name = "checkBoxDoSnaps";
            this.checkBoxDoSnaps.Size = new System.Drawing.Size(124, 17);
            this.checkBoxDoSnaps.TabIndex = 8;
            this.checkBoxDoSnaps.Text = "Run for Video Snaps";
            this.checkBoxDoSnaps.UseVisualStyleBackColor = true;
            this.checkBoxDoSnaps.CheckedChanged += new System.EventHandler(this.CheckBoxDoSnaps_CheckedChanged);
            // 
            // buttonGuide
            // 
            this.buttonGuide.BackColor = System.Drawing.Color.Gold;
            this.buttonGuide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGuide.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGuide.Location = new System.Drawing.Point(238, 97);
            this.buttonGuide.Name = "buttonGuide";
            this.buttonGuide.Size = new System.Drawing.Size(304, 56);
            this.buttonGuide.TabIndex = 7;
            this.buttonGuide.Text = "How do I use this program??";
            this.buttonGuide.UseVisualStyleBackColor = false;
            this.buttonGuide.Click += new System.EventHandler(this.ButtonGuide_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBoxOneSystem);
            this.groupBox1.Controls.Add(this.checkBoxSkipPlats);
            this.groupBox1.Controls.Add(this.checkBoxAutoRename);
            this.groupBox1.Location = new System.Drawing.Point(230, 263);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(312, 136);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Other Options";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Missing files before scraping";
            // 
            // LaunchboxLocalScraper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(554, 403);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonCorrectVideoFoldersName);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonGuide);
            this.Controls.Add(this.checkBoxDoSnaps);
            this.Controls.Add(this.checkBoxDoThemes);
            this.Controls.Add(this.videosPathBrowseButton);
            this.Controls.Add(this.buttonBrowserFolder);
            this.Controls.Add(this.textBoxVidsPath);
            this.Controls.Add(this.labelOriginalVideosPath);
            this.Controls.Add(this.listBoxGames);
            this.Controls.Add(this.textBoxLBInstallationPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonRun);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "LaunchboxLocalScraper";
            this.Text = "Starplayer\'s Local Scraper for Launchbox | v1.0 release";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LaunchboxLocalScraper_FormClosing);
            this.Load += new System.EventHandler(this.LaunchboxLocalScraper_Load);
            this.groupBoxOneSystem.ResumeLayout(false);
            this.groupBoxOneSystem.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxLBInstallationPath;
        private System.Windows.Forms.ListBox listBoxGames;
        private System.Windows.Forms.Label labelOriginalVideosPath;
        private System.Windows.Forms.TextBox textBoxVidsPath;
        private System.Windows.Forms.Button buttonCorrectVideoFoldersName;
        private System.Windows.Forms.Button buttonBrowserFolder;
        private System.Windows.Forms.Button videosPathBrowseButton;
        private System.Windows.Forms.CheckBox checkBoxSkipPlats;
        private System.Windows.Forms.CheckBox checkBoxAutoRename;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox comboBoxPlatforms;
        private System.Windows.Forms.Button buttonGetSystems;
        private System.Windows.Forms.CheckBox checkBoxRunForOnePlatform;
        private System.Windows.Forms.GroupBox groupBoxOneSystem;
        private System.Windows.Forms.CheckBox checkBoxDoThemes;
        private System.Windows.Forms.CheckBox checkBoxDoSnaps;
        private System.Windows.Forms.Button buttonGuide;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
    }
}

