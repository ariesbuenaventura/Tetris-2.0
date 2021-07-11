namespace Game
{
    partial class frmMain
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
            this.tmrGame = new System.Windows.Forms.Timer(this.components);
            this.lblScore = new System.Windows.Forms.Label();
            this.lblLines = new System.Windows.Forms.Label();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.lblLevel = new System.Windows.Forms.Label();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mnuGame = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGamePlay = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGameSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGameSettingsEasy = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGameSettingsHard = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuGameSettingsSound = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuGameExit = new System.Windows.Forms.ToolStripMenuItem();
            this.picField = new System.Windows.Forms.PictureBox();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrGame
            // 
            this.tmrGame.Interval = 1000;
            this.tmrGame.Tick += new System.EventHandler(this.tmrGame_Tick);
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScore.Location = new System.Drawing.Point(262, 160);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(95, 16);
            this.lblScore.TabIndex = 3;
            this.lblScore.Text = "Score : 000000";
            this.lblScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLines
            // 
            this.lblLines.AutoSize = true;
            this.lblLines.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLines.Location = new System.Drawing.Point(265, 181);
            this.lblLines.Name = "lblLines";
            this.lblLines.Size = new System.Drawing.Size(91, 16);
            this.lblLines.TabIndex = 4;
            this.lblLines.Text = "Lines : 000000";
            this.lblLines.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpeed.Location = new System.Drawing.Point(271, 209);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(72, 16);
            this.lblSpeed.TabIndex = 5;
            this.lblSpeed.Text = "Speed : 01";
            this.lblSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLevel.Location = new System.Drawing.Point(279, 230);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(64, 16);
            this.lblLevel.TabIndex = 6;
            this.lblLevel.Text = "Level : 01";
            this.lblLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuGame,
            this.mnuAbout});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(370, 24);
            this.mnuMain.TabIndex = 8;
            this.mnuMain.Text = "menuStrip1";
            // 
            // mnuGame
            // 
            this.mnuGame.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuGamePlay,
            this.menuGameSettings,
            this.toolStripMenuItem2,
            this.mnuGameExit});
            this.mnuGame.Name = "mnuGame";
            this.mnuGame.Size = new System.Drawing.Size(50, 20);
            this.mnuGame.Text = "&Game";
            // 
            // mnuGamePlay
            // 
            this.mnuGamePlay.Name = "mnuGamePlay";
            this.mnuGamePlay.Size = new System.Drawing.Size(125, 22);
            this.mnuGamePlay.Text = "&Start";
            this.mnuGamePlay.Click += new System.EventHandler(this.mnuGamePlay_Click);
            // 
            // menuGameSettings
            // 
            this.menuGameSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuGameSettingsEasy,
            this.mnuGameSettingsHard,
            this.toolStripMenuItem1,
            this.mnuGameSettingsSound});
            this.menuGameSettings.Name = "menuGameSettings";
            this.menuGameSettings.Size = new System.Drawing.Size(125, 22);
            this.menuGameSettings.Text = "Settings...";
            // 
            // mnuGameSettingsEasy
            // 
            this.mnuGameSettingsEasy.Checked = true;
            this.mnuGameSettingsEasy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuGameSettingsEasy.Name = "mnuGameSettingsEasy";
            this.mnuGameSettingsEasy.Size = new System.Drawing.Size(108, 22);
            this.mnuGameSettingsEasy.Text = "Easy";
            this.mnuGameSettingsEasy.Click += new System.EventHandler(this.mnuGameSettingsEasy_Click);
            // 
            // mnuGameSettingsHard
            // 
            this.mnuGameSettingsHard.Name = "mnuGameSettingsHard";
            this.mnuGameSettingsHard.Size = new System.Drawing.Size(108, 22);
            this.mnuGameSettingsHard.Text = "Hard";
            this.mnuGameSettingsHard.Click += new System.EventHandler(this.mnuGameSettingsHard_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(105, 6);
            // 
            // mnuGameSettingsSound
            // 
            this.mnuGameSettingsSound.Checked = true;
            this.mnuGameSettingsSound.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuGameSettingsSound.Name = "mnuGameSettingsSound";
            this.mnuGameSettingsSound.Size = new System.Drawing.Size(108, 22);
            this.mnuGameSettingsSound.Text = "Sound";
            this.mnuGameSettingsSound.Click += new System.EventHandler(this.mnuGameSettingsSound_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(122, 6);
            // 
            // mnuGameExit
            // 
            this.mnuGameExit.Name = "mnuGameExit";
            this.mnuGameExit.Size = new System.Drawing.Size(152, 22);
            this.mnuGameExit.Text = "E&xit";
            this.mnuGameExit.Click += new System.EventHandler(this.mnuGameExit_Click);
            // 
            // picField
            // 
            this.picField.BackColor = System.Drawing.Color.Black;
            this.picField.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picField.Image = global::Game.Properties.Resources.background;
            this.picField.Location = new System.Drawing.Point(0, 27);
            this.picField.Name = "picField";
            this.picField.Size = new System.Drawing.Size(240, 408);
            this.picField.TabIndex = 3;
            this.picField.TabStop = false;
            this.picField.Paint += new System.Windows.Forms.PaintEventHandler(this.picField_Paint);
            // 
            // picPreview
            // 
            this.picPreview.BackColor = System.Drawing.Color.Black;
            this.picPreview.Location = new System.Drawing.Point(249, 27);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(120, 120);
            this.picPreview.TabIndex = 2;
            this.picPreview.TabStop = false;
            this.picPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.picPreview_Paint);
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(52, 20);
            this.mnuAbout.Text = "&About";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(370, 436);
            this.Controls.Add(this.picField);
            this.Controls.Add(this.lblLevel);
            this.Controls.Add(this.lblSpeed);
            this.Controls.Add(this.lblLines);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.picPreview);
            this.Controls.Add(this.mnuMain);
            this.KeyPreview = true;
            this.MainMenuStrip = this.mnuMain;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tetris 2.0";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrGame;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Label lblLines;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.PictureBox picField;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuGame;
        private System.Windows.Forms.ToolStripMenuItem mnuGamePlay;
        private System.Windows.Forms.ToolStripMenuItem menuGameSettings;
        private System.Windows.Forms.ToolStripMenuItem mnuGameSettingsEasy;
        private System.Windows.Forms.ToolStripMenuItem mnuGameSettingsHard;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuGameSettingsSound;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem mnuGameExit;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
    }
}

