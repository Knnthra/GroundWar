using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace GroundWar
{
    class Boss : GameObject
    {
        Image deathAnim = Image.FromFile(@"gamefiles\boss_death.gif");
        float angle;           //Stores the angle used to determine the boss' y-position
        float countFrame = 0;  //Holds decimal-number as we count towards the next frame
        int currentFrame = 0;  //Stores the current frame of the animation

        /// <summary>
        /// Sets the boss' health, position and image
        /// </summary>
        /// <param name="BackgroundSize">The size of the background image</param>
        public Boss(Size BackgroundSize)
        {
            BgSize = BackgroundSize;    //Stores the background size in the gameobjects own variable
            
            goImage = Image.FromFile(@"gamefiles\boss.png");  //Sets the boss' image
            health = 500;

            //Sets the gameobjects position, relative to the background size
            goPos = new Point(BackgroundSize.Width + 200, BackgroundSize.Height / 2);
        }

        /// <summary>
        /// Animates the boss' death sequence
        /// </summary>
        /// <param name="startBossDeathAnim"></param>
        /// <param name="factor">Time in seconds it took to complete last frame/loop</param>
        public void Animate(ref bool startBossDeathAnim, float factor)
        {
            if (startBossDeathAnim == true)
            {
                FrameDimension dimension = new FrameDimension(deathAnim.FrameDimensionsList[0]);
                int frames = deathAnim.GetFrameCount(dimension);      //Gets the amount of frames in the .gif

                deathAnim.SelectActiveFrame(dimension, currentFrame); //Activates a certain frame(currentFrame)
                countFrame += (factor * frames) / 2;                  //Counts towards the next frame. Half speed
                currentFrame = (int)countFrame;                       //Convert the frame number to int

                if (currentFrame >= frames)                           //If the currentFrame exceeds the amount of frames in the .gif
                {
                    startBossDeathAnim = false;                           //Turns the startDeathAnim false, to prevent the animation from loopingss
                }
                goImage = deathAnim;        //Switches the boss image with the explosion image
            }
        }

        /// <summary>
        /// Moves the boss' y position based upon Sin(Angle)
        /// </summary>
        /// <param name="factor">Time in seconds it took to complete last frame/loop</param>
        /// <param name="degreeSpeed">Degree's, per second, to move the boss' y-position</param>
        public void MoveY(float factor, int degreeSpeed)
        {
            angle += (factor * (float)(Math.PI / 180) * degreeSpeed); // Calculate angle in degrees. Which means it moves 1 degree each second * degreeSpeed
            goPos.Y = (int)(Math.Sin(angle) * BgSize.Height/2) + BgSize.Height/2; //Move enemy in y axis based upon Sin(Angle) 
        }
    }
}