using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Game World
        // These variables define the world 

        //callums bit
        ParticleGenerator rain;
        Player player = new Player();

        Mover gull;
        PhysicsMover enemy;
        Sprite background;

        Song titleMusic;
        //Song deathMusic;

        GamePadState gamePadState;
        //Song themeSong;
        //SoundEffect BurpSound;

        int screenWidth;
        int screenHeight;
        int round = 0;

        bool skipped;

        List<Sprite> gameSprites = new List<Sprite>();
        List<Mover> players = new List<Mover>();
        List<PhysicsMover> enemys = new List<PhysicsMover>();
        List<GamePadState> gamePads = new List<GamePadState>();

        SpriteFont messageFont;

        CButton btnPlay;



        string messageString = "Hello world";

        int player1Score, player2Score, player1Wins, player2Wins;
        int timer;

        enum GameState
        {
            MainMenu,
            Playing,
            EndRound,
        }
        GameState CurrentGameState = GameState.MainMenu;

        //void startPlayingGame()
        //{
        //    foreach (Sprite s in gameSprites)
        //        s.Reset();

        //    messageString = "Cracker Chase";
        //}

        void gameOver()
        {
            foreach (Mover m in players)
            {
                m.Reset();
            }

            foreach (PhysicsMover p in enemys)
            {
                p.Reset();
            }
            if (round == 3)
            {
                MediaPlayer.Play(titleMusic);
                round = 0;
            }


            timer = 1800;
            player1Score = 20;
            player2Score = 20;
            skipped = false;
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.graphics.IsFullScreen = true;
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
            player.Initialize();
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

            messageFont = Content.Load<SpriteFont>("MessageFont");

            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;

            IsMouseVisible = true;
            skipped = false;

            player1Wins = 0;
            player2Wins = 0;

            Texture2D gullTexture = Content.Load<Texture2D>("seagull");
            Texture2D gullTexture2 = Content.Load<Texture2D>("eagle");
            Texture2D backgroundTexture = Content.Load<Texture2D>("background");
            Texture2D enemyTexture = Content.Load<Texture2D>("cracker");

            btnPlay = new CButton(Content.Load<Texture2D>("Button"), graphics.GraphicsDevice);
            btnPlay.setPosition(new Vector2(350, 450));

            rain = new ParticleGenerator(Content.Load<Texture2D>("Rain"), graphics.GraphicsDevice.Viewport.Width, 600);
            player.LoadContent(Content);

            //setup controllers
            gamePadState = GamePad.GetState(PlayerIndex.One);
            if (gamePadState.IsConnected)
            {
                gamePads.Add(gamePadState);
            }
            gamePadState = GamePad.GetState(PlayerIndex.Two);
            if (gamePadState.IsConnected)
            {
                gamePads.Add(gamePadState);
            }

            //setup songs
            titleMusic = Content.Load<Song>("musicTitle");
           // deathMusic = Content.Load<Song>("musicDeath");
            MediaPlayer.Play(titleMusic);
            MediaPlayer.IsRepeating = true;
           // MediaPlayer. = true;
            MediaPlayer.Volume = 1f;

            background = new Sprite(screenWidth, screenHeight, backgroundTexture, screenWidth, 0, 0);
            gameSprites.Add(background);

            int enemyWidth = screenWidth / 20;

            for (int i = 0; i < 1; i++)
            {
                enemy = new PhysicsMover(screenWidth, screenHeight, enemyTexture, enemyWidth, screenWidth / 2, screenHeight / 2, 500 , 500 , 1000 , 1000 , 0.6f);
                gameSprites.Add(enemy);
                enemys.Add(enemy);
            }

            int gullWidth = screenWidth / 15;

            //player one
            gull = new Mover(screenWidth, screenHeight, gullTexture, gullWidth, (screenWidth / 4)*1, screenHeight / 2, 500, 500);
            gameSprites.Add(gull);
            players.Add(gull);
            //player two
            gull = new Mover(screenWidth, screenHeight, gullTexture2, gullWidth, (screenWidth / 4)*3, screenHeight / 2, 500, 500);
            gameSprites.Add(gull);
            players.Add(gull);


            // go to the start screen state

            gameOver();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        void updateEndRound(GameTime gameTime)
        {
            GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
            GamePad.SetVibration(PlayerIndex.Two, 0f, 0f);

            int secsLeft = timer / 60;

            if(round == 3)
            {
                if (player1Wins > player2Wins)
                    messageString = "Germany won the game! ";

                if (player1Wins < player2Wins)
                    messageString = "Russia won the game! ";
                
            }
            else
            {
                if (player1Score > player2Score)
                    messageString = "Germany won round " + round;
                if (player1Score < player2Score)
                    messageString = "Russia won round " + round;
            }

            if(!skipped)
            {
                round++;
                if (player1Score > player2Score)
                    player1Wins++;
                if (player1Score < player2Score)
                    player2Wins++;
            }

            skipped = true;

            gamePads[0] = GamePad.GetState(PlayerIndex.One);
            gamePads[1] = GamePad.GetState(PlayerIndex.Two);

            if (gamePads[0].Buttons.A == ButtonState.Pressed)
            {
                CurrentGameState = GameState.Playing;
                gameOver();
            }
                    
            if (gamePads[1].Buttons.A == ButtonState.Pressed)
            {
                CurrentGameState = GameState.Playing;
                gameOver();
            }

            if (gamePads[0].Buttons.Back == ButtonState.Pressed)
            {
                GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
                GamePad.SetVibration(PlayerIndex.Two, 0f, 0f);
                Exit();
            }

            if (gamePads[1].Buttons.Back == ButtonState.Pressed)
            {
                GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
                GamePad.SetVibration(PlayerIndex.Two, 0f, 0f);
                Exit();
            }
                    

            
        }
        void updateGamePlay(GameTime gameTime)
        {
            int selectedPlayer = -1;
            //get user input for the keyboard & controler
            gamePads[0] = GamePad.GetState(PlayerIndex.One);
            gamePads[1] = GamePad.GetState(PlayerIndex.Two);

            //Player input bit
            foreach (GamePadState g in gamePads)
            {
                selectedPlayer++;

                if (g.Buttons.Back == ButtonState.Pressed)
                {
                    GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
                    GamePad.SetVibration(PlayerIndex.Two, 0f, 0f);
                    Exit();
                }

                //Sort out the gull movement
                if (g.ThumbSticks.Left.Y > 0)
                    players[selectedPlayer].StartMovingUp();
                else
                    players[selectedPlayer].StopMovingUp();

                if (g.ThumbSticks.Left.Y < 0)
                    players[selectedPlayer].StartMovingDown();
                else
                    players[selectedPlayer].StopMovingDown();

                if (g.ThumbSticks.Left.X < 0)
                    players[selectedPlayer].StartMovingLeft();
                else
                    players[selectedPlayer].StopMovingLeft();

                if (g.ThumbSticks.Left.X > 0)
                    players[selectedPlayer].StartMovingRight();
                else
                    players[selectedPlayer].StopMovingRight();
            }
            if (selectedPlayer == players.Count - 1)
                selectedPlayer = -1;

            //Chase bit
            float distPlayer1 = -1f;
            float distPlayer2 = -1f;
            int closestPlayer = 0;
            foreach (PhysicsMover p in enemys)
            {
                //find closest player
                foreach (Mover m in players)
                {
                    if (distPlayer1 == -1f)
                        distPlayer1 = p.GetDistanceFrom(m);
                    else
                        distPlayer2 = p.GetDistanceFrom(m);
                }
                //select closest player
                if (distPlayer1 <= distPlayer2)
                    closestPlayer = 0;
                else
                    closestPlayer = 1;
                //move on the horizontal axis to the closest player
                if (p.getXPosition() >= players[closestPlayer].getXPosition())
                {
                    p.StopMovingRight();
                    p.StartMovingLeft();
                }
                else
                {
                    p.StopMovingLeft();
                    p.StartMovingRight();
                }
                //move onthe vertical axis
                if (p.getYPosition() >= players[closestPlayer].getYPosition())
                {
                    p.StopMovingDown();
                    p.StartMovingUp();
                }
                else
                {
                    p.StopMovingUp();
                    p.StartMovingDown();
                }
                //test who is on the ball
                if (players[closestPlayer].IntersectsWith(p))
                {
                    if (closestPlayer == 0)
                    {
                        GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
                        player1Score--;
                    }
                    else
                    {
                        GamePad.SetVibration(PlayerIndex.Two, 1.0f, 1.0f);
                        player2Score--;
                    }
                }
                else
                {
                    GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
                    GamePad.SetVibration(PlayerIndex.Two, 0f, 0f);
                }
            }
            foreach (Sprite s in gameSprites)
                s.Update(1.0f / 60.0f);

            timer = timer - 1;

            int secsLeft = timer / 60;
            messageString = "Germany: " + player1Score + "        Time: " + secsLeft.ToString() + "        Russia: " + player2Score;

            if ((player1Score < 0 || player2Score < 0) || timer < 0)
            {
                CurrentGameState =  GameState.EndRound;
            }
        }

        //void updateStartScreen(GameTime gameTime)
        //{
        //    KeyboardState keys = Keyboard.GetState();
        //    gamePadState = GamePad.GetState(PlayerIndex.One);

        //    if (keys.IsKeyDown(Keys.Space) | gamePadState.Buttons.Start == ButtonState.Pressed)
        //        startPlayingGame();
        //}


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    if (btnPlay.isClicked == true) CurrentGameState = GameState.Playing;
                    btnPlay.Update(mouse);
                    break;

                case GameState.Playing:
                    rain.Update(gameTime, graphics.GraphicsDevice);
                    //player.Update(gameTime);
                    drawGamePlay();
                    updateGamePlay(gameTime);
                    break;
                case GameState.EndRound:
                    drawGamePlay();
                    updateEndRound(gameTime);
                    break;


            }
            base.Update(gameTime);
        }

        void drawStartScreen()
        {
            spriteBatch.Begin();

            foreach (Sprite s in gameSprites)
                s.Draw(spriteBatch);

            float xPos = (screenWidth - messageFont.MeasureString(messageString).X) / 2;

            Vector2 statusPos = new Vector2(xPos, screenHeight / 2);

            spriteBatch.DrawString(messageFont, messageString, statusPos, Color.Red);

            spriteBatch.End();
        }

        void drawGamePlay()
        {
            spriteBatch.Begin();

            foreach (Sprite s in gameSprites)
                s.Draw(spriteBatch);

            float xPos = (screenWidth - messageFont.MeasureString(messageString).X) / 2;

            Vector2 statusPos = new Vector2(xPos, 10);

            spriteBatch.DrawString(messageFont, messageString, statusPos, Color.Red);

            spriteBatch.End();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(Content.Load<Texture2D>("MainMenu"), new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    btnPlay.cButtonDraw(spriteBatch);
                    break;

                case GameState.Playing:
                    break;
            }

            // TODO: Add your drawing code here
            rain.Draw(spriteBatch);
            player.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    //    public class Game1x : Microsoft.Xna.Framework.Game
    //    {
    //        GraphicsDeviceManager graphics;
    //        SpriteBatch spriteBatch;

    //        public Game1()
    //        {
    //            graphics = new GraphicsDeviceManager(this);
    //            Content.RootDirectory = "Content";
    //        }

    //        /// <summary>
    //        /// Allows the game to perform any initialization it needs to before starting to run.
    //        /// This is where it can query for any required services and load any non-graphic
    //        /// related content.  Calling base.Initialize will enumerate through any components
    //        /// and initialize them as well.
    //        /// </summary>
    //        protected override void Initialize()
    //        {
    //            // TODO: Add your initialization logic here

    //            base.Initialize();
    //        }

    //        /// <summary>
    //        /// LoadContent will be called once per game and is the place to load
    //        /// all of your content.
    //        /// </summary>
    //        protected override void LoadContent()
    //        {
    //            // Create a new SpriteBatch, which can be used to draw textures.
    //            spriteBatch = new SpriteBatch(GraphicsDevice);

    //            // TODO: use this.Content to load your game content here
    //        }

    //        /// <summary>
    //        /// UnloadContent will be called once per game and is the place to unload
    //        /// all content.
    //        /// </summary>
    //        protected override void UnloadContent()
    //        {
    //            // TODO: Unload any non ContentManager content here
    //        }

    //        /// <summary>
    //        /// Allows the game to run logic such as updating the world,
    //        /// checking for collisions, gathering input, and playing audio.
    //        /// </summary>
    //        /// <param name="gameTime">Provides a snapshot of timing values.</param>
    //        protected override void Update(GameTime gameTime)
    //        {
    //            // Allows the game to exit
    //            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
    //                this.Exit();

    //            // TODO: Add your update logic here

    //            base.Update(gameTime);
    //        }

    //        /// <summary>
    //        /// This is called when the game should draw itself.
    //        /// </summary>
    //        /// <param name="gameTime">Provides a snapshot of timing values.</param>
    //        protected override void Draw(GameTime gameTime)
    //        {
    //            GraphicsDevice.Clear(Color.CornflowerBlue);

    //            // TODO: Add your drawing code here

    //            base.Draw(gameTime);
    //        }
    //    }
}
