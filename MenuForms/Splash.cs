using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GroundWar
{
    public partial class Splash : Form
    {
        Image splash = Image.FromFile(@"gamefiles\Splash.png");

        public Splash()
        {
            InitializeComponent();
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            this.ClientSize = splash.Size;          //Sets the clientsize according to the splash image size
            pb_splash.Size = this.ClientSize;       //Sets the picturebox's size according to the clientsize
            pb_splash.Image = splash;               //Loads the splash into the picturebox
            pb_splash.Location = new Point(0, 0);   //Moves the picturebox to 0,0
        }

        private void SplashTimer_Tick(object sender, EventArgs e)
        {
            SplashTimer.Enabled = false;            //Disables the timer
            this.Close();                           //Closes the splashform
        }
    }
}
