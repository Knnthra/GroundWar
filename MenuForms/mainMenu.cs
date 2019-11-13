using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;

namespace GroundWar
{
    public partial class mainMenu : Form
    {
        HighScores scores = new HighScores();

        public mainMenu()
        {
            InitializeComponent();

            scores.getsetScore();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {   //Hides the mainmenu window, and shows the highscorelist. When the highscore list is closed it show the mainmenu again. 
            scores.CurrentScore = 0; //Prevents the 'enter name' window from showing
            scores.getsetScore();
            this.Hide();
            scores.ShowDialog();
            this.Show();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {   //Exit button, terminates the program.
            Application.Exit();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            //Start game button, starts a new game.
            GameForm newGame = new GameForm(ref scores);
            this.Hide();
            newGame.ShowDialog();
            newGame.Close();
            this.Show();
        }
        
    }
}
