using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GroundWar
{
    class Enemy : GameObject
    {

        /// <summary>
        /// Determines which type of enemy to be, sets the image and position accordingly. (Making sure that a fast enemy doesnt spawn behind a slow)
        /// </summary>
        /// <param name="BackgroundSize">The size of the background image</param>
        /// <param name="gameworld">The list containing all the active gameobjects</param>
        public Enemy(Size BackgroundSize, List<GameObject> gameworld)
        {
            BgSize = BackgroundSize; //Stores the size of the bg

            if (rnd.Next(0, 100) <= 50) //% Chance of getting a blue enemy
            {
                enemyNormal = true;     //Indicates the gameobject is a normal-type enemy
                goImage = Image.FromFile(@"gamefiles\tank_all_blue.gif"); //Sets the image accordingly
                health = 20;
                goPos.X = BgSize.Width + 300;                 //Sets the x position 'far'outside the form, to get bullets from 'not-yet-seen' enemies
                goPos.Y = rnd.Next(0, BgSize.Height);      //Sets a random y position.

                #region PreventOverlapping
            reevaluate:
                //This loop ensures that a fast enemy doesnt spawn in a slow enemy's lane. Preventing them from overlapping
                //Foreach slow-enemy
                foreach (GameObject x in gameworld.FindAll(x => x is Enemy && x.EnemyNormal == false))
                {               
                    //Creates a rectangle 600 pixels wide, in front of the tank and checks if it intersects with the slow enemy.
                    if (x.GoRect().IntersectsWith(new Rectangle(goPos.X - 600, goPos.Y - (goImage.Height / 2), 600, goImage.Height)))
                    {
                        goPos.Y = rnd.Next(0, BgSize.Height);      //Sets a new random y position.
                        goto reevaluate;                              //Need to recheck the new y-position we got
                    }
                }
                #endregion
            }
            else                        //Otherwise we're getting a pink/slow tank
            {
                enemyNormal = false;
                goImage = Image.FromFile(@"gamefiles\tank_all_pink.gif");
                health = 50;
                goPos.X = BgSize.Width + 300;                 //Sets the x position 'far'outside the form, to get bullets from 'not-yet-seen' enemies
                goPos.Y = rnd.Next(0, BgSize.Height);      //Sets a random y position.
            }
        }
    }
}
