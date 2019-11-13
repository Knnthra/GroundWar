namespace GroundWar
{
    partial class mainMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainMenu));
            this.RadarBox = new System.Windows.Forms.PictureBox();
            this.GroundWarLbl = new System.Windows.Forms.PictureBox();
            this.StartGameBtn = new System.Windows.Forms.PictureBox();
            this.HighScoresBtn = new System.Windows.Forms.PictureBox();
            this.ExitBtn = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.RadarBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GroundWarLbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartGameBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HighScoresBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExitBtn)).BeginInit();
            this.SuspendLayout();
            // 
            // RadarBox
            // 
            this.RadarBox.Image = ((System.Drawing.Image)(resources.GetObject("RadarBox.Image")));
            this.RadarBox.Location = new System.Drawing.Point(342, 51);
            this.RadarBox.Name = "RadarBox";
            this.RadarBox.Size = new System.Drawing.Size(243, 228);
            this.RadarBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.RadarBox.TabIndex = 5;
            this.RadarBox.TabStop = false;
            // 
            // GroundWarLbl
            // 
            this.GroundWarLbl.Image = ((System.Drawing.Image)(resources.GetObject("GroundWarLbl.Image")));
            this.GroundWarLbl.Location = new System.Drawing.Point(58, 51);
            this.GroundWarLbl.Name = "GroundWarLbl";
            this.GroundWarLbl.Size = new System.Drawing.Size(219, 50);
            this.GroundWarLbl.TabIndex = 6;
            this.GroundWarLbl.TabStop = false;
            // 
            // StartGameBtn
            // 
            this.StartGameBtn.Image = ((System.Drawing.Image)(resources.GetObject("StartGameBtn.Image")));
            this.StartGameBtn.Location = new System.Drawing.Point(102, 127);
            this.StartGameBtn.Name = "StartGameBtn";
            this.StartGameBtn.Size = new System.Drawing.Size(129, 35);
            this.StartGameBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.StartGameBtn.TabIndex = 7;
            this.StartGameBtn.TabStop = false;
            this.StartGameBtn.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // HighScoresBtn
            // 
            this.HighScoresBtn.Image = ((System.Drawing.Image)(resources.GetObject("HighScoresBtn.Image")));
            this.HighScoresBtn.Location = new System.Drawing.Point(102, 177);
            this.HighScoresBtn.Name = "HighScoresBtn";
            this.HighScoresBtn.Size = new System.Drawing.Size(129, 50);
            this.HighScoresBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.HighScoresBtn.TabIndex = 8;
            this.HighScoresBtn.TabStop = false;
            this.HighScoresBtn.Click += new System.EventHandler(this.pictureBox4_Click);
            // 
            // ExitBtn
            // 
            this.ExitBtn.Image = ((System.Drawing.Image)(resources.GetObject("ExitBtn.Image")));
            this.ExitBtn.Location = new System.Drawing.Point(143, 243);
            this.ExitBtn.Name = "ExitBtn";
            this.ExitBtn.Size = new System.Drawing.Size(45, 36);
            this.ExitBtn.TabIndex = 9;
            this.ExitBtn.TabStop = false;
            this.ExitBtn.Click += new System.EventHandler(this.pictureBox5_Click);
            // 
            // mainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InfoText;
            this.ClientSize = new System.Drawing.Size(628, 316);
            this.Controls.Add(this.ExitBtn);
            this.Controls.Add(this.HighScoresBtn);
            this.Controls.Add(this.StartGameBtn);
            this.Controls.Add(this.GroundWarLbl);
            this.Controls.Add(this.RadarBox);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "mainMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "mainMenu";
            ((System.ComponentModel.ISupportInitialize)(this.RadarBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GroundWarLbl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartGameBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HighScoresBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExitBtn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox RadarBox;
        private System.Windows.Forms.PictureBox GroundWarLbl;
        private System.Windows.Forms.PictureBox StartGameBtn;
        private System.Windows.Forms.PictureBox HighScoresBtn;
        private System.Windows.Forms.PictureBox ExitBtn;
    }
}