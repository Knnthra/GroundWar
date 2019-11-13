using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroundWar
{   //Class for comparing and sorting the highscores.
    class HighScoreComparer : IComparer<string>
    {   
        public int Compare(string x, string y)
        {   
            //Creates 2 variables called xY and yH and converts the string to an int.
            int xH = int.Parse(x.Split(':')[0]);
            int yH = int.Parse(y.Split(':')[0]);

            //Returns 0 1 or -1 depending on xH and yH
            if (xH == yH)
            {
                return 0;
            }else if (xH > yH)
            {
                return -1;
            }else{
                return 1;
            }
        }
    }
}
