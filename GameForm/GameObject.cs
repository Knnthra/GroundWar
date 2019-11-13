using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GroundWar
{
    class GameObject
    {
        #region Fields
        //Stores wether the gameobject is a normal enemy
        protected bool enemyNormal;
        public bool EnemyNormal
        {
            get { return enemyNormal; }
        }

        //Stores wether the gameobject is a friendly powerup
        protected bool friendlyPU;
        public bool FriendlyPU
        {
            get { return friendlyPU; }
        }

        //Stores the gameobject's position
        protected Point goPos;
        public Point GoPos
        {
            get { return goPos; }
        }

        //Stores the amount of extra speed(Extra, relative to the background's movement speed)
        protected int extraSpeed;

        //Stores the gameobject's health
        protected int health;
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        //Stores the gameobject's image
        protected Image goImage;
        public Image GoImage
        {
            get { return goImage; }
        }

        protected Random rnd = new Random();
        protected Size BgSize; //Stores the background's size
        #endregion

        /// <summary>
        /// Creates a rectangle based on the sprite
        /// </summary>
        /// <returns>A rectangle based on the sprite</returns>
        public virtual Rectangle GoRect()
        {
            return new Rectangle(goPos.X - (goImage.Width / 2), goPos.Y - (goImage.Height / 2), goImage.Width, goImage.Height);
        }

        /// <summary>
        /// Draws the object on its x and y coordinate. Minus the half of its size, to make its center position 0,0
        /// </summary>
        /// <param name="GameForm">The GameForm</param>
        /// <param name="dc">The Graphics controller used to render gameobjects</param>
        public virtual void Render(GameForm GameForm, Graphics dc)
        {
            dc.DrawImage(goImage, goPos.X - (goImage.Width / 2), goPos.Y - (goImage.Height / 2), goImage.Width, goImage.Height);
        }

        /// <summary>w
        /// Moves the object according to the gamespeed
        /// At the same time it returns a boolean value wether it has moved outside the screen
        /// </summary>
        /// <param name="gameSpeed">gamespeed, based on how long each gameloop takes</param>
        /// <param name="go">which gameobject to move</param>
        /// <returns>Boolean value, wether the object has moved outside the screen</returns>
        public virtual bool HandleMovement(int gameSpeed, GameObject go)
        {
            #region SetXtraSpeed
            //The extra speed each object has is calculated each time we move it, because the gameSpeed can vary
            extraSpeed = 0;
            if (go is Enemy)
            {
                if (enemyNormal == true)
                {
                    extraSpeed += gameSpeed;
                }
                else //If the gameobject is a hard enemy
                {
                    extraSpeed += gameSpeed / 3;
                }                
            }
            else if (go is Bullet)
            {
                extraSpeed += gameSpeed * 4;
            }
            #endregion

            goPos.X -= gameSpeed + extraSpeed; //Moves the object according to gamespeed and the extraspeed
            if (goPos.X + (goImage.Width / 2) < 0) //If the object moves out of the screen
            {
                return true; //Return true
            }
            else
            {
                return false; //Otherwise it havent moved outside the screen. Return false
            }
        }
    }
}
