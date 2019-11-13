using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GroundWar
{
    public partial class HighScores : Form
    {
        enterName myNewForm = new enterName();
        public enterName EnterNameForm
        {
            get { return myNewForm; }
        }

        //Variable to store the current score.
        int currentScore = 0;
        public int CurrentScore
        {
            set { currentScore = value; }
        }        

        public HighScores()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            getsetScore();
        }

        /// <summary>
        /// Sets the currentScore to the score passed from the game, calls the getsetScore(). Then either returns true or false
        /// </summary>
        /// <param name="score">The gameform's score</param>
        /// <returns>Wether the score is higher or equal to the current lowest</returns>
        public void isNewHigh(int score)
        {
            if (score >= int.Parse(lblScore5.Text.Split(':')[0]))
            {
                currentScore = score;
                getsetScore();
            }
        }

        public void getsetScore()
        {
            if (!File.Exists("scores.txt")) //If the scores.txt doesnt exist
            {
                //Create a default
                File.WriteAllText("scores.txt", "0000: AAAAA\r\n0000: AAAAA\r\n0000: AAAAA\r\n0000: AAAAA\r\n0000: AAAAA\r\n");
            }

            //string array for reading the saved highscore from Scores.txt
            string[] readLines = File.ReadAllLines("scores.txt");

            //string for saving the player name after "Game Over"
            string currentName = "";

            //Creates an array for the score labels
            Label[] labelCollection = new Label[5];
            labelCollection[0] = lblScore1;
            labelCollection[1] = lblScore2;
            labelCollection[2] = lblScore3;
            labelCollection[3] = lblScore4;
            labelCollection[4] = lblScore5;

            //Creates a list for the highscores     
            List<string> listHighScores = new List<string>();

            //Writes the readLines to the labels
            int j = 0;
            foreach (Label x in labelCollection)
            {
                x.Text = readLines[j];
                listHighScores.Add(readLines[j]);
                j++;
            }

            //Skal komme efter 'game-over'
            if (currentScore != 0)
            {
                //Gets the the textBox string from the enterName form.
                myNewForm.ShowDialog();
                currentName = myNewForm.Result;

                if (currentName != "") //If the entered name is different from ""
                {
                    //Adds the current score and the entered name to the highscore list.
                    listHighScores.Add(currentScore.ToString() + ": " + currentName);

                    //Sorts the highscores
                    listHighScores.Sort(new HighScoreComparer());

                    //Loop for writing text to the labels
                    int a = 0;
                    foreach (Label i in labelCollection)
                    {
                        i.Text = listHighScores[a];
                        a++;
                    }
                }
            }

            //Loop for adding the scores and names to the scores.txt
            string TmpDatat = "";
            foreach (Label x in labelCollection)
            {
                TmpDatat = TmpDatat + x.Text + "\r\n";

            }//Writes the new sorted highscore list to the scores.txt
            File.WriteAllText(@"scores.txt", TmpDatat + "\r\n");
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            //Highscore list back to mainMenu button. Closes the highscore list.
            this.Close();
        }

        private void HighScores_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}

    

    




