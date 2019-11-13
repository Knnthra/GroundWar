using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Media;

namespace GroundWar
{
    public enum MoveDirection { Left, LeftUp, LeftDown, Right, RightUp, RightDown, Up, Down, None, Space, Esc };
    public partial class GameForm : Form
    {
        #region Fields
        //Instansiates sounds
        SoundPlayer spFriendlyPU = new SoundPlayer(@"gamefiles\sounds\wrench2.wav");
        SoundPlayer spFire = new SoundPlayer(@"gamefiles\sounds\gunfire.wav");
        SoundPlayer spBossTalk = new SoundPlayer(@"gamefiles\sounds\boss_talk.wav");

        //Instansiates images
        Image gameOver = Image.FromFile(@"gamefiles\gameover.png");
        Image youWin = Image.FromFile(@"gamefiles\youwin.png");
        Image background = Image.FromFile(@"gamefiles\bg_grass.jpg");
        
        Point Background1Pos; //Position for the baggrunden

        Boss boss;
        float bossTime; //Time before the final-boss shows up
        bool bossActive; //Stores wether the boss can spawn
        float bossRdyTime; //Time before the boss is ready to move
        bool isBossDead;    //Indicates wether the boss is dead
        bool startBossDeathAnim; //should the boss' death animation play?

        Player player;
        bool isPlayerDead; //Stores wether the player is dead

        int score; //Stor the time, in seconds, it takes to complete 1 gameloop/frame.

        Random rnd = new Random();
        float enemySpawnDelay; //Time in seconds to delay next enemy spawn
        float powerSpawnDelay; //Time in seconds to delay next powerup spawn
        float fireDelay; //Time in seconds to delay next bullet

        DateTime nowTime;
        float frameTimeMili; //Stores the time since the last gameloob/frame was complete
        float factor; //Stores the time, in seconds, it takes to complete 1 gameloop/frame.
        int gameSpeed; //Pixels pr loop to move the gameobjects

        bool startFireAnim; //Stores if the fire animation should play
        bool startDeathAnim; //Stores if the on-death animation should play
        bool pressedFire; //Stores wether fire(mouseclick) is pressed
        Point shotPosition; //Stores where the mouse was clicked(shot fired)
                
        List<GameObject> gameWorld = new List<GameObject>(); //Instansiates a list with GameObjects
        List<GameObject> tmpWorld = new List<GameObject>(); //Temp list

        HighScores highScoresForm; //Stores a ref of the highscore form
        #endregion

        public GameForm(ref HighScores highForm)
        {
            InitializeComponent();

            highScoresForm = highForm;

            ResetGame();
        }

        /// <summary>
        /// Sets and resets the variables needed to restart the game
        /// </summary>
        private void ResetGame()
        {
            this.ClientSize = background.Size; 

            Background1Pos = new Point(0, 0);

            boss = new Boss(background.Size); //Instansiates the boss
            bossTime = 60 * 1000; //Time before the final-boss shows up
            bossActive = false; //Stores wether the boss can spawn
            bossRdyTime = 3000; //Time before the boss is ready to move
            isBossDead = false;  //Indicates wether the boss is dead
            startBossDeathAnim = false; //should the boss' death animation play?

            player = new Player(new Point(100, background.Height / 2));
            isPlayerDead = false;

            score = 0;

            enemySpawnDelay = 0; //Time in seconds to delay next enemy spawn
            powerSpawnDelay = 0; //Time in seconds to delay next powerup spawn
            fireDelay = 0; //Time in seconds to delay next bullet

            startFireAnim = false; //Stores wether the fire animation should play
            startDeathAnim = false; //Stores wether the on-death animation should play
            pressedFire = false; //Stores wether the fire(mouseclick) is pressed

            gameWorld.Clear();
            tmpWorld.Clear();

            gameWorld.Add(player); //Adds the player to the gameWorld list
        }

        /// <summary>
        /// Calls the methodes in our gameloop
        /// Also determines the gamespeed, based on the time it took to complete the last loop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameLoop_Tick(object sender, EventArgs e)
        {
            #region DetermineGameSpeed
            frameTimeMili = DateTime.Now.Subtract(nowTime).Milliseconds; //Stores the time since the last gameloob/frame was complete
            nowTime = DateTime.Now;                                      //Stores the current time

            float fps = 1000 / frameTimeMili;           //Calculate fps
            factor = frameTimeMili / 1000;              //Calculates the time, in seconds, it takes to complete 1 gameloop/frame. Used to calculate gameSpeed aswell as other speeds
            gameSpeed = (int)(100 * factor);            //gameSpeed is used to determine how many pixels a gameobject needs to move each loop/frame to maintain a constant speed, regardless of cpu-power or loop-interval
            #endregion            

            Input();
            if (bossActive == true) //Another gamelogic if the boss is active
            {
                GameLogicBoss();
            }
            else                    //Otherwise, normal gamelogic
            {
                GameLogic();
            }
            Animation();
            Physics();
            Render();
        }


        /// <summary>
        /// Handles the keyboard input
        /// </summary>
        private void Input()
        {
            #region GetKeyboardState
            MoveDirection pressedKey = MoveDirection.None;

            if (Keyboard.IsKeyDown(Keys.A))
            {
                pressedKey = MoveDirection.Left;
            }
            if (Keyboard.IsKeyDown(Keys.D))
            {
                pressedKey = MoveDirection.Right;
            }
            if (Keyboard.IsKeyDown(Keys.W))
            {
                pressedKey = MoveDirection.Up;
                if (Keyboard.IsKeyDown(Keys.D))
                {
                    pressedKey = MoveDirection.RightUp;
                }
                if (Keyboard.IsKeyDown(Keys.A))
                {
                    pressedKey = MoveDirection.LeftUp;
                }
            }
            if (Keyboard.IsKeyDown(Keys.S))
            {
                pressedKey = MoveDirection.Down;
                if (Keyboard.IsKeyDown(Keys.D))
                {
                    pressedKey = MoveDirection.RightDown;
                }
                if (Keyboard.IsKeyDown(Keys.A))
                {
                    pressedKey = MoveDirection.LeftDown;
                }
            }
            if (Keyboard.IsKeyDown(Keys.Space))
            {
                pressedKey = MoveDirection.Space;
            }
            if (Keyboard.IsKeyDown(Keys.Escape))
            {
                this.Close();
            }
            #endregion

            if (pressedKey != MoveDirection.None && isPlayerDead == false)     //If pressedKey is something other that none
            {
                player.HandleInput(pressedKey, gameSpeed, background.Size);    //Call player.HandleInput()
            }
            if ((pressedKey == MoveDirection.Space) && (isPlayerDead == true || isBossDead == true) && (highScoresForm.EnterNameForm.Visible == false))   //If the player or boss is dead and pressedKey is space
            {
                ResetGame();                                                   //Restart the game
            }
        }

        /// <summary>
        /// Handles the pre-boss game logic
        /// </summary>
        private void GameLogic()
        {
            #region HandleBG
            Background1Pos.X -= gameSpeed;              //Moves the bg according to gameSpeed
            if (Background1Pos.X < -background.Width)   //If the bg has moved outside the form, its position is reset
            {
                Background1Pos.X = 0;
            }
            #endregion

            #region HasPlayerHit
            //Checks if Player has hit an enemy
            if (pressedFire == true && isPlayerDead == false && startFireAnim == false) //If the player has clicked
            {
                startFireAnim = true; //Makes the startFireAnim true, which allows the fire-animation to play
                spFire.Play();        //Plays the fire soundfile

                foreach (GameObject x in gameWorld)
                {
                    //Creates a new rectangle located at the shotPosition, to check if a player has hit an Enemy
                    if (x.GoRect().IntersectsWith(new Rectangle(shotPosition, new Size(1,1))) && (x is Enemy))
                    {
                        x.Health -= 10;      //Subtracts 10 from the enemy's health
                        score += 5;          //Adds 5 to the players score for hitting an enemy
                        break;               //Breaks from the loop, only one hit should be possible.
                    }
                }
            }
            pressedFire = false; //Prevents the application from running the loop if player havent clicked
            #endregion

            #region SpawnObjects
            if (bossTime >= 0 || isPlayerDead == true)                                      //If it isnt time for the boss OR player is dead
            {
                //Reduce the spawndelay
                enemySpawnDelay -= frameTimeMili;
                powerSpawnDelay -= frameTimeMili;

                if ((powerSpawnDelay <= 0) && (gameWorld.Count(x => x is Powerup) < 3))     //Are there less than * powerups in the gameWorld?
                {
                    gameWorld.Add(new Powerup(background.Size));                            //Add a powerup to the gameWorld
                    powerSpawnDelay = rnd.Next(500, 2000);                                  //Randomize the spawndelay for a powerup
                }
                else if ((enemySpawnDelay <= 0) && (gameWorld.Count(x => x is Enemy) < 10)) //Are there less than * Enemy's in the gameWorld?
                {
                    gameWorld.Add(new Enemy(background.Size, gameWorld));                   //Add an Enemy to the gameWorld
                    enemySpawnDelay = rnd.Next(500, 2000);                                  //Randomize the spawndelay for an enemy
                }
            }
            #endregion

            #region WaitFor+Spawn Boss
            bossTime -= frameTimeMili; //Reduce the bossTime
            if (gameWorld.Contains(player) && gameWorld.Count() == 1 && bossTime <= 0) //If the world only contains player and its time for the boss
            {
                gameWorld.Add(boss); //Add the boss to the world
                bossActive = true;   //Sets the boss to active
            }
            #endregion

            #region (Re-)MoveObjects
            List<GameObject> enemies = new List<GameObject>(); //Creates a new list for enemies, which is used to spawn bullets later

            tmpWorld.Clear();
            gameWorld.ForEach(x => tmpWorld.Add(x)); //Copies the gameworld into a temp-world
            foreach (GameObject x in tmpWorld)
            {
                if (x is Enemy)
                {
                    if (x.HandleMovement(gameSpeed, x) || (x.Health <= 0)) //If the object is offscreen or dead
                    {
                        gameWorld.Remove(x);  //Remove the object from the gameWorld
                    }
                    else
                    {
                        enemies.Add(x);       //Adds the enemy to our list of active enemies
                    }
                }
                if (x is Bullet)
                {
                    if (x.HandleMovement(gameSpeed, x))                      //If the object is offscreen
                    {
                        gameWorld.Remove(x);                                 //Remove the object from the gameWorld
                    }
                }
                if (x is Powerup)
                {
                    if (x.HandleMovement(gameSpeed, x))                      //If the object is offscreen
                    {
                        gameWorld.Remove(x);                                 //Remove the object from the gameWorld
                    }
                }
            }
            #endregion

            #region SpawnBullet
            //Adds a bullet to the gameWorld at a random enemy position
            fireDelay -= frameTimeMili;     //reduce the firedelay
            if (fireDelay <= 0 && enemies.Count() > 0) //If fireDelay is 0 and there are more than 0 enemies in the list
            {
                gameWorld.Add(new Bullet(background.Size, enemies[rnd.Next(0,enemies.Count())].GoPos)); //Add a bullet at a random enemy position
                fireDelay = rnd.Next(125, 250); //Randomize the fireDelay
            }
            #endregion

            #region PokePlayer(Is Player Dead?)
            if (player.Health <= 0 && isPlayerDead == false) //If the player's health drops too low and the player isnt already dead
            {
                player.Health = 0;                //Prevents health from dropping below 0
                gameWorld.Remove(player);         //Remove player from gameworld
                isPlayerDead = true;
                startDeathAnim = true;

                highScoresForm.isNewHigh(score);  //Calls the isNewHigh() to see if the score is better or equal to the current lowest
            }
            #endregion
        }

        /// <summary>
        /// Handles the post-boss game logic
        /// </summary>
        private void GameLogicBoss()
        {
            #region Pre-Fight
            if ((boss.GoPos.X >= background.Size.Width - boss.GoImage.Width) && isBossDead == false) //If the boss isnt in place and isnt dead
            {
                //Moves the boss inside the screen(to the left)
                boss.HandleMovement(gameSpeed, boss);

                #region SpawnBoulder
                if (!gameWorld.Exists(x => x is Boulder)) //If there doesnt exist any boulders in the gameworld
                {
                    gameWorld.Add(new Boulder(new Point(279, 147), "bouldercw")); //Top right
                    gameWorld.Add(new Boulder(new Point(326, 89), "boulder")); //Top left

                    gameWorld.Add(new Boulder(new Point(279, 383), "boulder")); //Bottom top
                    gameWorld.Add(new Boulder(new Point(279, 461), "boulderccw")); //Bottom 
                }
                #endregion

                if ((boss.GoPos.X <= background.Size.Width - boss.GoImage.Width)) //If the boss is in place
                {
                    spBossTalk.Play();                  //Play the bossTalk sound
                }
            }
            else if (bossRdyTime >= 0) //If the boss isnt ready to move
            {
                bossRdyTime -= frameTimeMili; //Reduce the time before the boss starts
            }
            #endregion
            else //The boss is in place and ready to move
            {
                #region HasPlayerHit
                if (pressedFire == true && isPlayerDead == false && startFireAnim == false && isBossDead == false) //If the player has clicked
                {
                    startFireAnim = true; //Makes the startFireAnim true, which allows the fire-animation to play
                    spFire.Play(); //Plays the fire soundfile

                    //Creates a new rectangle located at the shotPosition, to check if a player has hit an Enemy
                    if (boss.GoRect().IntersectsWith(new Rectangle(shotPosition, new Size(1,1))) && gameWorld.Contains(boss))
                    {
                        boss.Health -= 10;      //Subtracts 10 from the boss's health
                        score += 5;             //Adds 5 to the players score for hitting an enemy
                        if (boss.Health <= 0 && isBossDead == false) //If the boss' health gets too low and isnt already dead
                        {
                            if (score > 0) //If the score is positive
                            {
                                //Doubles the player's score for killing the boss
                                score = score * 2;
                            }
                            gameWorld.Remove(boss); //Remove the boss from the gameworld
                            isBossDead = true;
                            startBossDeathAnim = true;

                            highScoresForm.isNewHigh(score);  //Calls the isNewHigh() to see if the score is better or equal to the current lowest
                        }
                    }
                }
                pressedFire = false; //Prevents the application from running the loop if player havent clicked
                #endregion

                #region CalcMovSpeed
                if (isBossDead == false) //If the boss is alive
                {
                    int movSpeed = 22500 / boss.Health; //Calculates the boss' movement speed based on his health
                    if (movSpeed > 200)                 //But limits it from going too fast
                    {
                        movSpeed = 200;
                    }
                    boss.MoveY(factor, movSpeed); //Move the boss' Y position
                }
                #endregion

                #region (Re-)MoveBullets
                tmpWorld.Clear();
                gameWorld.ForEach(x => tmpWorld.Add(x)); //Copies the gameworld into a temp-world
                foreach (GameObject x in tmpWorld.FindAll(x => x is Bullet))
                {
                    if (x.HandleMovement(gameSpeed, x)) //If the bullet is offscreen
                    {
                        gameWorld.Remove(x);            //Remove it from the gameworld
                    }
                }
                #endregion

                #region SpawnBullet
                //Spawns a bullet if the delay is 0 or less and the boss is alive
                fireDelay -= frameTimeMili;         //Count down the firedelay
                if (fireDelay <= 0 && isBossDead == false)
                {
                    gameWorld.Add(new Bullet(background.Size, new Point(boss.GoPos.X - 9, boss.GoPos.Y))); //Adds a bullet to the gameWorld at the boss(main gun)
                    gameWorld.Add(new Bullet(background.Size, new Point(boss.GoPos.X + 1, boss.GoPos.Y + 49))); //Adds a bullet to the gameWorld at the boss(top gun)
                    gameWorld.Add(new Bullet(background.Size, new Point(boss.GoPos.X + 1, boss.GoPos.Y - 49))); //Adds a bullet to the gameWorld at the boss(bottom gun)
                    fireDelay = rnd.Next(25,50); //Milliseconds between bullets
                }
                #endregion
            }

            #region PokePlayer(Is Player Dead?)
            if (player.Health <= 0 && isPlayerDead == false) //If the player's health drops too low and the player isnt already dead
            {
                player.Health = 0;                //Prevents health from dropping below 0
                gameWorld.Remove(player);         //Remove player from gameworld
                isPlayerDead = true;
                startDeathAnim = true;

                highScoresForm.isNewHigh(score);  //Calls the isNewHigh() to see if the score is better or equal to the current lowest
            }
            #endregion
        }

        /// <summary>
        /// Detects and handles collisions
        /// </summary>
        private void Physics()
        {
            tmpWorld.Clear();
            gameWorld.ForEach(x => tmpWorld.Add(x)); //Copies the gameworld into a temp-world
            foreach (GameObject x in tmpWorld)
            {
                #region DetectBulletCollisionWithBoulder
                //If the boss is active, we need to check for bullets collision with boulders
                if (x is Bullet && bossActive == true)
                {
                    foreach (GameObject y in gameWorld.FindAll(y => y is Boulder)) //Foreach boulder in our gameworld
                    {
                        //If the bullet intersects with a boulder
                        if (x.GoRect().IntersectsWith(y.GoRect()))
                        {
                            gameWorld.Remove(x); //Remove the bullet from the gameworld
                        }
                    }
                }
                #endregion

                //If the current gameobject isnt the player or the boss, and the player isnt dead
                if (!(x is Player) && isPlayerDead == false && !(x is Boss))
                {
                    //Detects collision with a obj and the player
                    if (x.GoRect().IntersectsWith(player.GoRect()))
                    {
                        if (!(x is Powerup))                //If the colliding object isnt a powerup
                        {
                            player.Health -= 10;            //Subtract 10hp
                            score -= 10;                    //Subtracts 10 from the score for the player being hit
                            gameWorld.Remove(x);            //Removes the colliding object
                        }
                        else                                //If the colliding object is a powerup
                        {
                            #region PowerupCollision
                            if (x.FriendlyPU == true && !(player.Health >= 100))  //If it is a friendly powerup and player doesnt have max hp
                            {
                                player.Health += 10;            //Add 10hp
                                spFriendlyPU.Play();            //Plays the wrench soundfile
                                gameWorld.Remove(x);            //Removes the colliding object
                            }
                            else if (x.FriendlyPU == false)     //If the colliding obj is a unfriendly powerup
                            {
                                Powerup xPu = (Powerup)x;       //Converts the object to a powerup
                                if (xPu.triggered == false)     //If it havnt already been triggered
                                {
                                    player.Health -= 10;        //Subtract 10hp
                                    score -= 10;                //Subtracts 10 from the score for the player being hit
                                    xPu.triggered = true;       //'Triggeres' the powerup, indicating that it needs to be animated   
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles/Calls all the animate methods
        /// </summary>   
        private void Animation()
        {
            player.Animate(ref startFireAnim, ref startDeathAnim, factor); //Calls the player's Animate method

            boss.Animate(ref startBossDeathAnim, factor);                  //Calls the boss' Animate method

            //Animates some powerups
            foreach (Powerup x in gameWorld.FindAll(x => x is Powerup)) //Foreach powerup in our gameworld
            {
                if (x.triggered == true && x.Animate(factor))     //If it has been 'triggered' and 'Call the Animate() method, and ask if the animation is done'
                {
                    gameWorld.Remove(x); //Remove the powerup if the animation is done                  
                }
            }
        }

        /// <summary>
        /// Handles the drawing of the objects
        /// </summary>
        private void Render()
        {
            Bitmap tmpBitmap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height); //Creates a new, empty, bitmap, with equal size to the form's client-size
            Graphics gphBitmap = Graphics.FromImage(tmpBitmap); //Creates a graphics controller from the new bitmap

            //Draws the background
            int Background2PosX = Background1Pos.X + background.Size.Width; //Determines the position behind the bg
            gphBitmap.DrawImage(background, Background1Pos);                //Draws the first bg 
            gphBitmap.DrawImage(background, Background2PosX, 0);            //Draws the second bg, behind the first (to be able to simulate a continuous bg loob)

            #region DrawsLayers
            //Draws the Powerups(Is ontop of the bg)
            if (bossActive == false) //Only search for powerups and enemies if the boss isnt active
            {
                foreach (GameObject powerups in gameWorld.FindAll(x => x is Powerup))
                {
                    powerups.Render(this, gphBitmap);
                }
                //Draws the enemies(Is ontop of the powerups)
                foreach (GameObject enemies in gameWorld.FindAll(x => x is Enemy))
                {
                    enemies.Render(this, gphBitmap);
                }
            }
            else //If the boss is active
            {
                boss.Render(this, gphBitmap);
                //Draws the boulders
                foreach (GameObject x in gameWorld.FindAll(x => x is Boulder))
                {
                    x.Render(this, gphBitmap);
                }
            }
            //Draws the bullets(Is ontop of the enemy-tanks)
            foreach (GameObject bullets in gameWorld.FindAll(x => x is Bullet))
            {
                bullets.Render(this, gphBitmap);
            }
            //Draws the player(Is ontop of every object)
            if (gameWorld.Contains(player))
            {
                player.Render(this, gphBitmap);
            }
            else //Draws the player on-death animation
            {
                player.deathRender(gphBitmap);
            }
            #endregion

            HUD(gphBitmap); //Draws the hud

            Graphics dc = CreateGraphics(); //Creates a graphics controller
            dc.DrawImage(tmpBitmap, 0, 0);  //Draws the 'newly' created bitmap on the form. Thus eliminating any 'flicker' problem

            //Releases resources
            gphBitmap.Dispose();
            tmpBitmap.Dispose();
            dc.Dispose();
        }


        /// <summary>
        /// Draws the players health, score and final end-game screen.
        /// </summary>
        /// <param name="dc">The graphics controller used to draw the HUD</param>
        private void HUD(Graphics dc)
        {
            Font hudFont = new Font("Visitor TT1 BRK", 20); //Sets the Font type and size
            SolidBrush txtbrush = new SolidBrush(Color.Black); //Sets the color to black

            if (isPlayerDead == false) //If the player isnt dead
            {
                if (isBossDead == true) //If the boss is dead
                {
                    //Draws the 'You Win'-image in the middle of the screen
                    dc.DrawImage(youWin, (this.ClientSize.Width / 2) - (youWin.Width / 2), (this.ClientSize.Height / 2) - (youWin.Height / 2), youWin.Width, youWin.Height);
                }
                dc.DrawString("Health: " + player.Health.ToString(), hudFont, txtbrush, new Point(5, 5)); //Draws a black 'text-shadow'(Health)

                txtbrush = new SolidBrush(Color.White);                                                   //Changes the color to white
                dc.DrawString("Health: " + player.Health.ToString(), hudFont, txtbrush, new Point(0, 0)); //Draws the text(Health)

            }
            else //If the player is dead
            {
                //Draws the 'GameOver' image to the middle of the screen
                dc.DrawImage(gameOver, (this.ClientSize.Width/2) - (gameOver.Width/2), (this.ClientSize.Height/2) - (gameOver.Height/2), gameOver.Width, gameOver.Height);
            }

            #region DrawsScore
            txtbrush = new SolidBrush(Color.Black);                                                                    //Changes the color to Black
            dc.DrawString("Score: " + score.ToString(), hudFont, txtbrush, new Point(5, this.ClientSize.Height - 20)); //Draws a black 'text-shadow'(Score)
            txtbrush = new SolidBrush(Color.White);                                                                    //Changes the color to white
            dc.DrawString("Score: " + score.ToString(), hudFont, txtbrush, new Point(0, this.ClientSize.Height - 25)); //Draws the text(Score)
            #endregion
        }

        private void MainForm_Click(object sender, EventArgs e)
        {
            pressedFire = true;   //Makes the pressedFire true, which allows for a loop to check if anything is hit
            
            Point clientStartPos = this.PointToScreen(new Point(0,0)); //Sets the clients 0,0 position relative to the screen
            shotPosition.X = Cursor.Position.X - clientStartPos.X;     //Sets the shotPosition to the cursors position(relative to the client)
            shotPosition.Y = Cursor.Position.Y - clientStartPos.Y;
        }
    }
}