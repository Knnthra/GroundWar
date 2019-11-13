using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GroundWar
{
    class Boulder : GameObject
    {
        /// <summary>
        /// Sets the boulder's image and position
        /// </summary>
        /// <param name="spawnPosition">The position to spawn/draw the boulder</param>
        /// <param name="boulderType">The type of picture to use. "boulder" or "bouldercw" or "boulderccw"</param>
        public Boulder(Point spawnPosition, string boulderType)
        {
            if (boulderType == "boulder")
            {
                goImage = Image.FromFile(@"gamefiles\boulder.png");
            }
            else if (boulderType == "bouldercw")
            {
                goImage = Image.FromFile(@"gamefiles\bouldercw.png");
            }
            else if (boulderType == "boulderccw")
            {
                goImage = Image.FromFile(@"gamefiles\boulderccw.png");
            }
            else
            {
                goImage = Image.FromFile(@"gamefiles\boulder.png");
            }

            goPos = spawnPosition;
        }
    }
}
