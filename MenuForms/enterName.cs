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
    public partial class enterName : Form
    {
        //String variable to store the entered name.
        private string result;

        //String that returns the entered name.
        public string Result
        {
            get
            {
                
                return result;
            }
        }
        
        public enterName()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {   
            //Saves the input from the textbox in the string result, and closes the window
            result = txtName.Text;
            Close();
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            //if the pressed key is enter, call the "button"'s click method
            if (e.KeyCode == Keys.Enter)
            {
                pictureBox2_Click(sender, e);
            }
        }
    }
}
