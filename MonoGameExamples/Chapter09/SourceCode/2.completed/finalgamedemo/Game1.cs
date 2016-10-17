using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BookExample
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region class variables defined to be globally accessible!!
        // for drawing support
        // Convention: staticClassVariable names begin with "s"
        /// <summary>
        /// sGraphicsDevice - reference to th graphics device for current display size
        /// sSpriteBatch - reference to the SpriteBatch to draw all of the primitivers
        /// sContent - reference to the ContentManager to load the textures
        /// </summary>
        static public SpriteBatch sSpriteBatch;  // Drawing support
        static public ContentManager sContent;   // Loading textures
        static public GraphicsDeviceManager sGraphics; // Current display size
        static public Random sRan; // for generating random numbers
        #endregion

        #region Preferred Window Size
        // Prefer window size
        // Convention: "k" to begin constant variable names
        const int kWindowWidth = 1200;
        const int kWindowHeight = 720;
        #endregion

        // preferred screen size is 1200x720
        // Width of 500 means, height will be 300
        public const float kWorldWidth = 500f;
        public const float kWorldHeight = 300f;
        public const float kWorldOverShotBuffer = 10f;
        public const float kAbsoluteWorldMinX = -kWorldOverShotBuffer;
        public const float kAbsoluteWorldMaxX = kWorldWidth + kWorldOverShotBuffer;
        public const float kAbsoluteWorldMinY = -kWorldOverShotBuffer;
        public const float kAbsoluteWorldMaxY = kWorldHeight + kWorldOverShotBuffer;


        #region Randomness support
        static public float RandomNumber(float n) {
            return (float) (sRan.NextDouble() * n);
        }
        static public float RandomNumber(float min, float max)
        {
            return min + ((max - min) * (float)(sRan.NextDouble()));
        }
        #endregion

        public static float sGravity = 0.01f;

        GameState mMyGame;
        
        public Game1()
        {
            // Content resource loading support
            Content.RootDirectory = "Content";
            Game1.sContent = Content;

            // Create graphics device to access window size
            Game1.sGraphics = new GraphicsDeviceManager(this);
            // set prefer window size
            Game1.sGraphics.PreferredBackBufferWidth = kWindowWidth;
            Game1.sGraphics.PreferredBackBufferHeight = kWindowHeight;
           
            Game1.sRan = new Random();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            Game1.sSpriteBatch = new SpriteBatch(GraphicsDevice);
            
            // Define Camera Window Bounds
            // Camera view
            Camera.SetCameraWindow(Vector2.Zero, 300);  // To begin 

            mMyGame = new GameState();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            mMyGame.UpdateGame(gameTime);

            //
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            // Clear to background color
            Color bg = new Color(0, 125, 255);
            GraphicsDevice.Clear(bg);

            Game1.sSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, Matrix.Identity);
            //Game1.sSpriteBatch.Begin( // Initialize drawing support
            
            mMyGame.DrawGame();


            Game1.sSpriteBatch.End(); // inform graphics system we are done drawing

            base.Draw(gameTime);
        }
    }
}
