using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace GroundWar
{
    class Powerup : GameObject
    {
        float countFrame = 0;  //Holds decimal-number as we count towards the next frame
        int currentFrame = 0;   //Stores the current frame of the gunFire gif
        public bool triggered; //Stores whether this powerup needs to be animated

        /// <summary>
        /// Determines what kind of powerup this is, and sets its image accordingly
        /// </summary>
        /// <param name="BackgroundSize">The size of the background image</param>
        public Powerup(Size BackgroundSize)
        {
            BgSize = BackgroundSize; //Stores the size of the bg

            goPos.X = BgSize.Width + 100; //Sets the x position outside the form.
            goPos.Y = rnd.Next(0, BgSize.Height); //Sets a random y position.

            if (rnd.Next(0, 100) <= 20) //% Chance of getting a friendly powerup
            {
                friendlyPU = true;
                goImage = Image.FromFile(@"gamefiles\wrench.gif");
            }
            else                           //Otherwise we get a unfriendly powerup
            {
                friendlyPU = false;
                goImage = Image.FromFile(@"gamefiles\bomb.gif");
            }
        }

        /// <summary>
        /// Overrides the standard GoRect(). The 'unfriendly powerup' needs to return a rectangle, smaller than its actual picture
        /// </summary>
        /// <returns>A rectangle, based on wether its a friendly or unfriendly powerup</returns>
        public override Rectangle GoRect()
        {
            if (friendlyPU == false)
            {
                return new Rectangle(goPos.X - 9, goPos.Y + 24, 15, 15);
            }
            return base.GoRect();
        }

        /// <summary>
        /// Handles the animation of the unfriendly powerup
        /// </summary>
        /// <param name="factor">Seconds it took for the last gameloop/frame to complete</param>
        /// <returns>Wether the animation is done</returns>
        public bool Animate(float factor)
        {
            FrameDimension dimension = new FrameDimension(goImage.FrameDimensionsList[0]);
            int frames = goImage.GetFrameCount(dimension); //Gets the amount of frames in the .gif

            goImage.SelectActiveFrame(dimension, currentFrame); //Activates a certain frame(currentFrame)
            countFrame += (factor * frames) * 1.5f;                //Counts towards the next frame.
            currentFrame = (int)countFrame;

            if (currentFrame >= frames) //If the currentFrame exceeds the amount of frames in the .gif
            {
                currentFrame = 0;       //Reset the animation
                countFrame = 0;
                goImage.SelectActiveFrame(dimension, currentFrame); //Activates a certain frame(currentframe=0)
                return true; //The animation is done
            }
            return false;    //The animation is not done yet
        }
    }
}
