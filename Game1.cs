/*****************************************************************************************
 * Assignment 05 - Team Project
 * Jason Carter and Melinda Frandsen
 * 
 * CSIS 2410 12/02/17
 * 
 * Space Wars
 * 
 * This is a space combat game with 3 levels of game play.
 * The player must shoot all enemies and not allow them to reach his position.
 * To move, use the left and right arrow keys.
 * To shoot use the spacebar.
 * **************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using Microsoft.Xna.Framework.Media;

namespace SpaceWars
{
    /// <summary>
    /// Class contains the main bulk of game play function
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont font_01, font_02, font_03;                         // fonts used for scores
        KeyboardState kb;

        Song vaderThememp3;                         // sound file plays when vader appears
        Song trapmp3;                               // sound file
        Song powerOTDSmp3;                          // sound file plays when player loses

        Player p;
        Minion m;
        BigBoss b;

        Rectangle mainFrame;                        // rectangle area of screen
        Rectangle rTorpedo;                         // rectangle object containing images

        Texture2D background;                       // background image for screen
        Texture2D torpedo;                          // torpedo image
        
        int attackSpeed = 2;                        // designates speed of attack
        int vaderSpeed = 1;                         // designates speed of Vader's attack
        int rows = 4;                               // number rows in squadron
        int cols = 8;                               // number of columns in squadron
        int rightSide;                              // right edge of screen
        int leftside = 0;                           // left edge of screen
        int deathCount = 0;                         // tally of dead from squadron
        int vaderDeathCount = 0;                    // tally of hits on vader
        int gameState = 0;                          // deterimes gamestate in switch

        bool[,] minionAlive;                        // boolean is enemy alive
        bool vaderAlive = true;                     // boolean is vader alive
        bool directionRight = true;                 // squadron movement right(true) left(false)
        bool torpedoVisible = false;                // torpedo is visible or is not
        bool isGameOver = false;                    // game over - yes, no
        bool isPlaying = true;                      // boolean to keep sound file from glitching

        // images and positons for start and end screens
        Texture2D spriteStartScreen;
        Texture2D spriteEndScreenLose;
        Texture2D spriteEndScreenWin;
        Vector2 spriteStartScreen_Pos;
        Vector2 spriteEndScreen_Pos;

        // text and positions for start screen
        String introTextString_01;
        String introTextString_02;
        String introTextString_03;
        Vector2 introTextPos_01;
        Vector2 introTextPos_02;
        Vector2 introTextPos_03;

        // text and positions for end screens
        String endTextString_01;
        String endTextString_02;
        String endTextString_03;
        Vector2 endTextPos_01;
        Vector2 endTextPos_02;
        Vector2 endTextPos_03;

        // text and positons for running score in game
        String currentScoreString_01;                // "current score: "
        String currentScoreString_02 = "000";        // actual current score as a string
        Vector2 currentScorePos_01;                  // position of "current score:"
        Vector2 currentScorePos_02;                  // position of actual current score as a string

        /// <summary>
        /// Constructor
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Score.ReadScores();

            spriteStartScreen_Pos = Vector2.Zero;
            spriteEndScreen_Pos = Vector2.Zero;

            introTextPos_01.X = 300;
            introTextPos_01.Y = 340;
            introTextString_01 =
                "CHOOSE YOUR CHARACTER:\n";
            introTextPos_02.X = 150;
            introTextPos_02.Y = 385;
            introTextString_02 =
                "PRESS 1    HAN SOLO\n" +
                "PRESS 2    PRINCESS LEIA\n" +
                "PRESS 3    LUKE SKYWALKER\n";
            introTextPos_03.X = 450;
            introTextPos_03.Y = 385;
            introTextString_03 =
                "PRESS 4    CHEWBACCA\n" +
                "PRESS 5    OBI-WAN KENOBI\n" +
                "PRESS 6    R2D2\n";

            endTextPos_01.X = 335;
            endTextPos_01.Y = 340;
            endTextString_01 = "GAME OVER";
            endTextPos_02.X = 350;
            endTextPos_02.Y = 375;
            endTextString_02 = "Top 3 Scores:";
            endTextPos_03.X = 360;
            endTextPos_03.Y = 400;
            endTextString_03 = Score.FindTopThreeScores();

            currentScorePos_01.X = 10;                   // sets "current score" X position
            currentScorePos_01.Y = 460;                  // sets "current score" Y position
            currentScorePos_02.X = 130;                  // sets current score (200 etc.) X position
            currentScorePos_02.Y = 460;                  // sets current score (200 etc.) Y position
            currentScoreString_01 = "Current Score:";   // prints to screen

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // load font for use with scores, start screen and end screen text
            font_01 = Content.Load<SpriteFont>("Fonts\\SpriteFont1");
            font_02 = Content.Load<SpriteFont>("Fonts\\SpriteFont2");
            font_03 = Content.Load<SpriteFont>("Fonts\\SpriteFont3");

            // load sound files
            vaderThememp3 = this.Content.Load<Song>("vaderThememp3");
            powerOTDSmp3 = this.Content.Load<Song>("PowerOTDS");
            trapmp3 = this.Content.Load<Song>("trap");

            // load images for start and game over screens
            spriteStartScreen = Content.Load<Texture2D>("sw1");
            spriteEndScreenLose = Content.Load<Texture2D>("loser");
            spriteEndScreenWin = Content.Load<Texture2D>("winner");

            // load image for torpedo and set rectangle
            torpedo = Content.Load<Texture2D>("torpedo");
            rTorpedo.Width = torpedo.Width;
            rTorpedo.Height = torpedo.Height;
            rTorpedo.X = 0;
            rTorpedo.Y = 0;

            // load space background image
            background = Content.Load<Texture2D>("space");

            // desgnates size of window
            mainFrame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            LoadTieFighter();
            LoadXWing();
            LoadTorpedo();
            LoadBigBoss();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (gameState)
            {
                case 0:
                    // select character
                    kb = Keyboard.GetState();
                    if (kb.IsKeyDown(Keys.NumPad1))
                    {
                        Score.currentPlayer = "Han Solo";
                        gameState = 10;
                    }
                    if (kb.IsKeyDown(Keys.NumPad2))
                    {
                        Score.currentPlayer = "Princess Leia";
                        gameState = 10;
                    }
                    if (kb.IsKeyDown(Keys.NumPad3))
                    {
                        Score.currentPlayer = "Luke Skywalker";
                        gameState = 10;
                    }
                    if (kb.IsKeyDown(Keys.NumPad4))
                    {
                        Score.currentPlayer = "Chewbacca";
                        gameState = 10;
                    }
                    if (kb.IsKeyDown(Keys.NumPad5))
                    {
                        Score.currentPlayer = "Obi-Wan Kenobi";
                        gameState = 10;
                    }
                    if (kb.IsKeyDown(Keys.NumPad6))
                    {
                        Score.currentPlayer = "R2D2";
                        gameState = 10;
                    }
                    break;
                case 86:
                    break;
                default:
                    EnemySquadronMovement();
                    PlayerControls();

                    // if torpedo visible... move torpedo
                    if (torpedoVisible == true)
                        rTorpedo.Y = rTorpedo.Y - 5;

                    // check to see if minion enemy has been hit
                    CollisionChecker();

                    if (deathCount == 32) //32
                    {
                        if (gameState == 10)
                        {
                            LoadImperialShuttle();              // load second wave of enemies
                            gameState = 11;
                            deathCount = 0;
                        }
                        else if (gameState == 11)
                        {                                       // vader enters
                            if (isPlaying == true)
                            {
                                MediaPlayer.Play(vaderThememp3);
                                isPlaying = false;
                            }
                            VaderMovement();                    
                            BigBossCollisionChecker();
                        }
                    }

                    // if torpedo goes out of scope, make invisible
                    if (rTorpedo.Y + rTorpedo.Height < 0)
                        torpedoVisible = false;

                    int count = 0;
                    for (int r = 0; r < rows; r++)          // loops through rows
                    {
                        for (int c = 0; c < cols; c++)      // loops through columns
                        {
                            if (minionAlive[r, c] == true)
                            {
                                count = count + 1;
                            }
                        }
                    }

                    // increase speed when squadron is down to 1/2 and again when 1/3
                    if (count < (rows * cols / 2))
                        attackSpeed = 4;
                    if (count < (rows * cols / 3))
                        attackSpeed = 6;

                    //check to see if minions have reached x-Wing  GAME OVER
                    for (int r = 0; r < rows; r++)          // loops through rows
                    {
                        for (int c = 0; c < cols; c++)      // loops through columns
                        {
                            if (minionAlive[r, c] == true)
                            {
                                if (m.rMinion[r, c].Y + m.rMinion[r, c].Height > p.rXWingFighter.Y)
                                {
                                    GameOverLose();         // loss by minons reaching player position
                                }
                            }
                        }
                    }

                    // converts current score int to string
                    currentScoreString_02 = Score.currentScore.ToString();
                    break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            switch (gameState)  
            {
                case 0:     // start screeen
                    spriteBatch.Draw(spriteStartScreen, spriteStartScreen_Pos, Color.White);
                    spriteBatch.DrawString(font_01, introTextString_01, introTextPos_01, Color.DarkGoldenrod);
                    spriteBatch.DrawString(font_01, introTextString_02, introTextPos_02, Color.DarkGoldenrod);
                    spriteBatch.DrawString(font_01, introTextString_03, introTextPos_03, Color.DarkGoldenrod);
                    break;
                case 86:    // end screen when player loses
                    spriteBatch.Draw(spriteEndScreenLose, spriteEndScreen_Pos, Color.White);
                    spriteBatch.DrawString(font_02, endTextString_01, endTextPos_01, Color.Gainsboro);
                    spriteBatch.DrawString(font_03, endTextString_02, endTextPos_02, Color.Gainsboro);
                    spriteBatch.DrawString(font_03, endTextString_03, endTextPos_03, Color.Gainsboro);
                    break;
                case 87:    // end screen when player wins
                    spriteBatch.Draw(spriteEndScreenWin, spriteEndScreen_Pos, Color.White);
                    spriteBatch.DrawString(font_02, endTextString_01, endTextPos_01, Color.Gainsboro);
                    spriteBatch.DrawString(font_03, endTextString_02, endTextPos_02, Color.Gainsboro);
                    spriteBatch.DrawString(font_03, endTextString_03, endTextPos_03, Color.Gainsboro);
                    break;
                default:    // regular game play
                    spriteBatch.Draw(background, mainFrame, Color.White);

                    // draws squadron
                    for (int r = 0; r < rows; r++)          // loops through rows
                    {
                        for (int c = 0; c < cols; c++)      // loops through columns
                        {
                            if (minionAlive[r, c] == true)
                            {
                                spriteBatch.Draw(m.minion, m.rMinion[r, c], Color.White);
                            }
                        }
                    }

                    // draws xWingFighter
                    spriteBatch.Draw(p.xWingFighter, p.rXWingFighter, Color.White);

                    // draws vaderFighter
                    spriteBatch.Draw(b.vaderFighter, b.rVaderFighter, Color.White);

                    // draws torpedo when fired
                    if (torpedoVisible == true)
                        spriteBatch.Draw(torpedo, rTorpedo, Color.White);

                    // draws current score on bottom left screen
                    spriteBatch.DrawString(font_01, currentScoreString_01, currentScorePos_01, Color.Gainsboro);
                    spriteBatch.DrawString(font_01, currentScoreString_02, currentScorePos_02, Color.White);
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// loads image and rectangle size into gameplay
        /// </summary>
        private void LoadTorpedo()          // assigns rectangle the same size as image
        {
            torpedo = Content.Load<Texture2D>("torpedo");
            rTorpedo.Width = torpedo.Width;
            rTorpedo.Height = torpedo.Height;
            rTorpedo.X = 0;
            rTorpedo.Y = 0;
        }

        /// <summary>
        /// loads image and rectangle size into gameplay
        /// sets position
        /// </summary>
        private void LoadXWing()          // assigns rectangle the same size as image
        {
            rightSide = graphics.GraphicsDevice.Viewport.Width;
            Texture2D t = Content.Load<Texture2D>("xWingFighter3");
            Rectangle RectangleXWingFighter = new Rectangle();
            RectangleXWingFighter.Width = t.Width;
            RectangleXWingFighter.Height = t.Height;
            RectangleXWingFighter.X = rightSide / 2 - (RectangleXWingFighter.Width / 2);
            RectangleXWingFighter.Y = 400;
            p = new Player(t, RectangleXWingFighter);
        }

        /// <summary>
        /// loads image and rectangle size into gameplay
        /// positions Vader off screen for entrance later
        /// </summary>
        private void LoadBigBoss()          // assigns rectangle the same size as image
        {
            rightSide = graphics.GraphicsDevice.Viewport.Width;
            Texture2D t = Content.Load<Texture2D>("vaderFighter");
            Rectangle RectangleVaderFighter = new Rectangle();
            RectangleVaderFighter.Width = t.Width;
            RectangleVaderFighter.Height = t.Height;
            RectangleVaderFighter.X = rightSide;
            RectangleVaderFighter.Y = 20;
            b = new BigBoss(t, RectangleVaderFighter);
        }

        /// <summary>
        /// loads image and rectangle array into gameplay
        /// </summary>
        private void LoadTieFighter()          // assigns rectangle the same size as image
        {
            Texture2D t = Content.Load<Texture2D>("tieFighter");
            Rectangle[,] RectangleMinion = new Rectangle[rows, cols];

            minionAlive = new bool[rows, cols];
            for (int r = 0; r < rows; r++)          // loops through rows
            {
                for (int c = 0; c < cols; c++)      // loops through columns
                {
                    RectangleMinion[r, c].Width = t.Width;
                    RectangleMinion[r, c].Height = t.Height;
                    RectangleMinion[r, c].X = 80 * c;
                    RectangleMinion[r, c].Y = 70 * r;
                    minionAlive[r, c] = true;
                }
                m = new Minion(t, RectangleMinion);
            }
        }

        /// <summary>
        /// loads image and rectangle array into gameplay
        /// </summary>
        private void LoadImperialShuttle()          // assigns rectangle the same size as image
        {
            Texture2D t = Content.Load<Texture2D>("shuttle");
            Rectangle[,] RectangleTieFighter = new Rectangle[rows, cols];

            minionAlive = new bool[rows, cols];
            for (int r = 0; r < rows; r++)          // loops through rows
            {
                for (int c = 0; c < cols; c++)      // loops through columns
                {
                    RectangleTieFighter[r, c].Width = t.Width;
                    RectangleTieFighter[r, c].Height = t.Height;
                    RectangleTieFighter[r, c].X = 80 * c;
                    RectangleTieFighter[r, c].Y = 70 * r;
                    minionAlive[r, c] = true;
                }
                m = new Minion(t, RectangleTieFighter);
            }
        }

        /// <summary>
        /// Designates keyboard controls for player
        /// right arrow - move right
        /// left arrow - move left
        /// spacebar - fire
        /// </summary>
        private void PlayerControls()
        {
            kb = Keyboard.GetState();

            // move X-Wing left
            if (kb.IsKeyDown(Keys.Left))
            {
                p.rXWingFighter.X = p.rXWingFighter.X - 3;
                if (p.rXWingFighter.X < 0)
                    p.rXWingFighter.X = 0;
            }

            // move X-Wing right
            if (kb.IsKeyDown(Keys.Right))
            {
                p.rXWingFighter.X = p.rXWingFighter.X + 3;
                if (p.rXWingFighter.X + p.rXWingFighter.Width > rightSide)
                    p.rXWingFighter.X = rightSide - p.rXWingFighter.Width;
            }

            // when spacebar is pressed, fires torpedo  this sets position
            if (kb.IsKeyDown(Keys.Space) && torpedoVisible.Equals(false))
            {
                //if (isPlaying == true)
                //{
                //    MediaPlayer.Play(blastermp3);
                //    isPlaying = false;
                //}
                torpedoVisible = true;
                rTorpedo.X = p.rXWingFighter.X + (p.rXWingFighter.Width / 2) - (rTorpedo.Width / 2);
                rTorpedo.Y = p.rXWingFighter.Y - rTorpedo.Height + 2;
            }
        }

        /// <summary>
        /// Checks to see if a torpedo has intersected with one of the 
        /// Minions in the squadron
        /// </summary>
        private void CollisionChecker()
        {
            if (torpedoVisible == true)
            {
                for (int r = 0; r < rows; r++)          // loops through rows
                {
                    for (int c = 0; c < cols; c++)      // loops through columns
                    {
                        if (minionAlive[r, c] == true)
                        {
                            if (rTorpedo.Intersects(m.rMinion[r, c]))
                            {
                                torpedoVisible = false;
                                minionAlive[r, c] = false;
                                deathCount++;
                                Score.ShipKilled();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks to see if a torpedo has intersected with one of the 
        /// Minions in the squadron
        /// </summary>
        private void BigBossCollisionChecker()
        {
            if (torpedoVisible == true)
            {
                if (vaderAlive == true)
                {
                    if (rTorpedo.Intersects(b.rVaderFighter))
                    {
                        torpedoVisible = false;
                        vaderDeathCount++;
                        Score.VaderKilled();

                        if (vaderDeathCount == 1)               // as vader is hit - speed changes
                        {
                            vaderSpeed = 4;
                        }
                        if (vaderDeathCount == 2)
                        {
                            vaderSpeed = 6;
                        }
                        if (vaderDeathCount == 4)
                        {
                            vaderSpeed = 4;
                        }
                        if (vaderDeathCount == 6)
                        {
                            vaderSpeed = 12;
                        }
                        if (vaderDeathCount == 8)
                        {
                            vaderSpeed = 14;
                        }

                        if (vaderDeathCount == 10)
                        {
                            vaderAlive = false;
                            GameOverWin();                      // game over due to player killing vader
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Darth Vader moves left to right and changes direction
        /// </summary>
        private void VaderMovement()
        {
            // move left and right
            if (directionRight == true)
            {
                b.rVaderFighter.X = b.rVaderFighter.X + vaderSpeed;
            }
            if (directionRight == false)
            {
                b.rVaderFighter.X = b.rVaderFighter.X - vaderSpeed;
            }

            // check to see if they've gone past the right edge
            rightSide = graphics.GraphicsDevice.Viewport.Width;
            bool changeDirection = false;

            if (b.rVaderFighter.X + b.rVaderFighter.Width > rightSide)
            {
                directionRight = false;
                changeDirection = true;
            }
            if (b.rVaderFighter.X < leftside)
            {
                directionRight = true;
                changeDirection = true;
            }

            // if the directions change the tie fighters move down
            if (b.rVaderFighter.X + b.rVaderFighter.Width < rightSide)
            {
                if (changeDirection == true)
                {
                    b.rVaderFighter.Y = b.rVaderFighter.Y + 15;
                }
            }

            //check to see if big boss has reached x-Wing  GAME OVER

            if (vaderAlive == true)
            {
                if (b.rVaderFighter.Y + b.rVaderFighter.Height > p.rXWingFighter.Y)
                {
                    GameOverLose();
                }
            }
        }

        /// <summary>
        /// allows for movement of enemy squadron 
        /// Left to Right and back
        /// When edge is reached squadron moves down
        /// </summary>
        private void EnemySquadronMovement()
        {
            // move tie fighters left and right
            for (int r = 0; r < rows; r++)          // loops through rows
            {
                for (int c = 0; c < cols; c++)      // loops through columns
                {
                    if (directionRight == true)
                        m.rMinion[r, c].X = m.rMinion[r, c].X + attackSpeed;
                    if (directionRight == false)
                        m.rMinion[r, c].X = m.rMinion[r, c].X - attackSpeed;
                }
            }

            // check to see if they've gone past the right edge
            rightSide = graphics.GraphicsDevice.Viewport.Width;
            bool changeDirection = false;
            for (int r = 0; r < rows; r++)          // loops through rows
            {
                for (int c = 0; c < cols; c++)      // loops through columns
                {
                    if (minionAlive[r, c] == true)
                    {
                        if (m.rMinion[r, c].X + m.rMinion[r, c].Width > rightSide)
                        {
                            directionRight = false;
                            changeDirection = true;
                        }
                        if (m.rMinion[r, c].X < leftside)
                        {
                            directionRight = true;
                            changeDirection = true;
                        }
                    }
                }
            }

            // if the directions change the tie fighters move down
            if (changeDirection == true)
            {
                for (int r = 0; r < rows; r++)          // loops through rows
                {
                    for (int c = 0; c < cols; c++)      // loops through columns
                    {
                        m.rMinion[r, c].Y = m.rMinion[r, c].Y + 3;
                    }
                }
            }
        }

        /// <summary>
        /// If enemies reach the player's position GameOverLose is called
        /// Adds the current player score to a List of scores
        /// Writes the scores to a text file
        /// </summary>
        private void GameOverLose()
        {
            if (isPlaying == true)
            {
                MediaPlayer.Play(powerOTDSmp3);
                isPlaying = false;
            }
            isGameOver = true;
            gameState = 86;
            Score.AddCurrentToAllScoresList();
            Score.WriteScores();
        }

        /// <summary>
        /// If player defeats all emeies GameOverWin is called
        /// Adds the current player score to a List of scores
        /// Writes the scores to a text file
        /// </summary>
        private void GameOverWin()
        {
            MediaPlayer.Stop();
            isGameOver = true;
            gameState = 87;
            Score.AddCurrentToAllScoresList();
            Score.WriteScores();
        }
    }
}
