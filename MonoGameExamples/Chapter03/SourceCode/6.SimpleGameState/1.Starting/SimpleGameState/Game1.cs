#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace BookExample
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        #region Class variables defined to be globally accessible!!
        // For drawing support
        // Convention: staticClassVariable names begin with "s"
        /// <summary>
        /// sGraphicsDevice - reference to th graphics device for current display size
        /// sSpriteBatch - reference to the SpriteBatch to draw all of the primitives
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
        const int kWindowWidth = 1000;
        const int kWindowHeight = 700;
        #endregion

        public Game1()
        {
            // Content resource loading support
            Content.RootDirectory = "Content";
            Game1.sContent = Content;

            // Create graphics device to access window size
            Game1.sGraphics = new GraphicsDeviceManager(this);

            // Set prefer window size
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
            Camera.SetCameraWindow(new Vector2(-10f, -10f), 150f);

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (InputWrapper.Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Game1.sSpriteBatch.Begin();

            Game1.sSpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
