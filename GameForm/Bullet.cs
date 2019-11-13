using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GroundWar
{
    class Bullet : GameObject
    {

        /// <summary>
        /// Sets the bullet's image and position
        /// </summary>
        /// <param name="BackgroundSize">The size of the background image</param>
        /// <param name="tankToSpawnAt">The tank-gameobject, which the bullet will spawn at</param>
        public Bullet(Size BackgroundSize, Point tankToSpawnAt)
        {
            goImage = Image.FromFile(@"gamefiles\bullet.png");

            BgSize = BackgroundSize; //Stores the size of the bg

            goPos.X = tankToSpawnAt.X - 40; //Sets the X position equal to the tank, with an offset of 40 to appear at the front of the gun
            goPos.Y = tankToSpawnAt.Y;      //Sets the y position equal to the tank's y-position
        }
    }
}
