using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace GroundWar
{
    class Player : GameObject
    {
        Image gunRedSprite = Image.FromFile(@"gamefiles\tank_gun_red.gif");
        Image gunFire = Image.FromFile(@"gamefiles\gun_fire.gif");

        Image deathAnim = Image.FromFile(@"gamefiles\player_death.gif");
        float countFrame =  0;  //Holds decimal-number as we count towards the next frame
        int currentFrame = 0;   //Stores the current frame of the gunFire gif

        /// <summary>
        /// Sets the player's health, image and position
        /// </summary>
        /// <param name="StartPos">The startposition of the player</param>
        public Player(Point StartPos)
        {
            health = 100;
            goPos = StartPos; //Sets its provided start position.
            goImage = Image.FromFile(@"gamefiles\tank_body_red.gif");
        }

        /// <summary>
        /// Changes the players x and y position, depending on which key is pressed
        /// At the same time, the methode keeps the players position from going beyond a certain limit
        /// Also handles the gunFire animation
        /// </summary>
        /// <param name="PressedKey">Which key, if any, is pressed</param>
        /// <param name="GameSpeed">The gamespeed, calculated each gameloop-tick</param>
        /// <param name="BackgroundSize">The size of the background</param>
        public void HandleInput(MoveDirection PressedKey, int GameSpeed, Size BackgroundSize)
        {
            #region MovePlayer
            if (PressedKey == MoveDirection.Up)
            {
                goPos.Y -= GameSpeed;
            }
            else if (PressedKey == MoveDirection.Down)
            {
                goPos.Y += GameSpeed;
            }
            else if (PressedKey == MoveDirection.Right)
            {
                goPos.X += GameSpeed;
            }
            else if (PressedKey == MoveDirection.RightUp)
            {
                goPos.X += GameSpeed;
                goPos.Y -= GameSpeed;
            }
            else if (PressedKey == MoveDirection.RightDown)
            {
                goPos.X += GameSpeed;
                goPos.Y += GameSpeed;
            }
            else if (PressedKey == MoveDirection.Left)
            {
                goPos.X -= GameSpeed;
            }
            else if (PressedKey == MoveDirection.LeftUp)
            {
                goPos.X -= GameSpeed;
                goPos.Y -= GameSpeed;
            }
            else if (PressedKey == MoveDirection.LeftDown)
            {
                goPos.X -= GameSpeed;
                goPos.Y += GameSpeed;
            }
            #endregion

            #region MoveLimit
            if (goPos.X > BackgroundSize.Width / 3)
            {
                goPos.X = BackgroundSize.Width / 3;
            }
            if (goPos.X < 0)
            {
                goPos.X = 0;
            }
            if (goPos.Y > BackgroundSize.Height)
            {
                goPos.Y = BackgroundSize.Height;
            }
            if (goPos.Y < 0)
            {
                goPos.Y = 0;
            }
            #endregion
        }

        /// <summary>
        /// Handles both the player's fire-animation and on-death animation
        /// </summary>
        /// <param name="startFireAnim">Wether the fire animation should play</param>
        /// <param name="startDeathAnim">Wether the death animation should play</param>
        /// <param name="factor">The time, in seconds, it took to complete the last frame/loop</param>
        public void Animate(ref bool startFireAnim, ref bool startDeathAnim, float factor)
        {
            #region DeathAnimation
            if (startDeathAnim == true) //If the death animation should play
            {
                FrameDimension dimension = new FrameDimension(deathAnim.FrameDimensionsList[0]);
                int frames = deathAnim.GetFrameCount(dimension);      //Gets the amount of frames in the .gif

                if (currentFrame >= frames)  //If the currentFrame exceeds or equals the amount of frames in the .gif
                {
                    currentFrame = frames-1; //Set the currentFrame to the next-to last frame in the .gif
                }

                deathAnim.SelectActiveFrame(dimension, currentFrame); //Activates a certain frame(currentFrame)
                countFrame += (factor * frames) * 2;                  //Counts towards the next frame. Double speed
                currentFrame = (int)countFrame;
            }
            #endregion

            #region FireAnimation
            if (startFireAnim == true)
            {
                FrameDimension dimension = new FrameDimension(gunFire.FrameDimensionsList[0]);
                int frames = gunFire.GetFrameCount(dimension); //Gets the amount of frames in the .gif

                if (currentFrame >= frames) //If the currentFrame exceeds the amount of frames in the .gif
                {
                    currentFrame = 0;       //Reset the animation
                    countFrame = 0;
                    gunFire.SelectActiveFrame(dimension, currentFrame); //Activates a certain frame(currentframe=0)
                    startFireAnim = false;                              //Turns the startFireAnim false, to prevent the animation from looping
                }

                gunFire.SelectActiveFrame(dimension, currentFrame); //Activates a certain frame(currentFrame)
                countFrame += (factor * frames) * 2;                //Counts towards the next frame. Double speed
                currentFrame = (int)countFrame;
            }
            #endregion
        }

        /// <summary>
        /// The players tank-image is abit bigger than normal, so we need to tweak its rectangle
        /// </summary>
        /// <returns>A rectangle, based on the player's sprite(the visual part of it)</returns>
        public override Rectangle GoRect()
        {
            return new Rectangle(goPos.X - (goImage.Width / 2) + 10, goPos.Y - (goImage.Height / 2), goImage.Width - 10, goImage.Height);
        }

        /// <summary>
        /// Dedicated on-death render method
        /// </summary>
        /// <param name="dc">The Graphics controller used to draw objects</param>
        public void deathRender(Graphics dc)
        {
            dc.DrawImage(deathAnim, goPos.X-(deathAnim.Width/2) + 15,  (goPos.Y - deathAnim.Height) + 45); //Draws the deathAnim .gif
        }

        /// <summary>
        /// The player class needs a very unique way of rendering(To be able to rotate the gun), which is why its overriding the Render() method
        /// </summary>
        /// <param name="tmpMainForm">The GameForm</param>
        /// <param name="dc">The Graphics controller used to draw objects</param>
        public override void Render(GameForm gameForm, Graphics dc)
        {
            Point PlayerPos_RltScreen = gameForm.PointToScreen(goPos); //Players position(relative to the screen)
            Point MousePos_RltGun = new Point((Cursor.Position.X - PlayerPos_RltScreen.X), (Cursor.Position.Y - PlayerPos_RltScreen.Y)); //Mouse position(relative to player)

            dc.DrawImage(goImage, goPos.X - (goImage.Width / 2), goPos.Y - (goImage.Height / 2)); //Draws player(body)(with its center in 0,0), on its x and y position
            
            dc.TranslateTransform(goPos.X, goPos.Y); //Moves the graphics transform to maintain rotation around the player

            float angle = gunAngleCalc(MousePos_RltGun); //Calls the gunAngleCalc() methode, and stores the result
            dc.RotateTransform(angle); //Rotates the graphics transform

            dc.DrawImage(gunRedSprite, -(gunRedSprite.Width / 2), -(gunRedSprite.Height / 2)); //Draws the player's gun withs its center in 0,0
            dc.DrawImage(gunFire, (gunRedSprite.Width / 2) - 15, -(gunFire.Height / 2)); //Draws the gunfire .gif

            dc.RotateTransform(-angle); //Rotates the graphics transform back to 0
            dc.TranslateTransform(-(goPos.X),-(goPos.Y)); //Moves the graphics transform back to 0,0            
        }

        /// <summary>
        /// Calculates the angle(using phytagoras and cosinus) needed to 'point at' the cursor.
        /// </summary>
        /// <param name="tmpMousePos_RltGun">Cursor position, relative to the player</param>
        /// <returns>An angle, based on the gun and the mouse position</returns>
        private float gunAngleCalc(Point tmpMousePos_RltGun)
        {
            Point PosZero = new Point(0, 0);

            //If the cursor is in 0,0(relative to the player), then the angle cant be calculated
            if (tmpMousePos_RltGun == PosZero)
            {
                return 0;
            }

            double angle;
            double hypotenuse;

            //Calculates the hypotenuse using Phytagoras(a^2 + b^2 = c^2)
            hypotenuse = Math.Sqrt(                                      //hypotenuse = squared(c^2)
                                   Math.Pow((tmpMousePos_RltGun.Y), 2) + //a^2  +
                                   Math.Pow((tmpMousePos_RltGun.X), 2)); //        b^2

            angle = Math.Acos((tmpMousePos_RltGun.X) / hypotenuse); //Calculates the angle in radians
            angle = angle * 180 / Math.PI;                          //Converts the radians to degree's

            //If the cursor's y position is negative('above' the player), the returned angle is also negative
            if (tmpMousePos_RltGun.Y < 0)
            {
                return -(float)angle;
            }
            else
            {
                return (float)angle;
            }
        }
    }
}
