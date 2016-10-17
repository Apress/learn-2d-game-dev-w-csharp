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
    public class GameState
    {
        // Global constant for simple world physical properties
        static public float sGravity = 0.01f;

        private Vector2 kInitBallPosition = new Vector2(3f, 48f);
     
        // Objects in the world
        Platform mSlowStone, mBrick, mStone;
        RotateObject mBasket;
        
        public GameState()
        {
            // Create the platforms
            mBrick = new Platform("BrickPlatform", new Vector2(15, 40), new Vector2(30f, 5f));
            mBrick.Friction = 0.999f; // How rapidly object slows down: retains most speed
            mBrick.Elasticity = 0.85f; // How bouncy is this platform: 90%

            mStone = new Platform("StonePlatform", new Vector2(50, 30), new Vector2(30, 5f));
            mStone.Friction = 0.99f; // How rapidly object slows down: retains some speed
            mStone.Elasticity = 0.5f; // How bouncy is this platform: slightly more than half: 60%

            mSlowStone = new Platform("StonePlatform", new Vector2(85, 20), new Vector2(30f, 5f));
            mSlowStone.Friction = 0.9f; // How rapidly object slows down: very rapidly
            mSlowStone.Elasticity = 0.2f; // How bouncy is this platform: not very 


            // Both outside of the camera, so neither will be drawn
            mBasket = new RotateObject("BasketBall", new Vector2(-1, -1), 3f);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        public void UpdateGame()
        {
            if (InputWrapper.Buttons.A == ButtonState.Pressed)
            {
                mBasket.Position = kInitBallPosition;
                Vector2 v = new Vector2((float)(0.3f + (Game1.sRan.NextDouble()) * 0.1f), 0f);
                mBasket.Velocity = v;
            }


            if (mBasket.ObjectVisibleInCameraWindow())
            {
                mBasket.Update();
                mSlowStone.BounceObject(mBasket);
                mStone.BounceObject(mBasket);
                mBrick.BounceObject(mBasket);
            }

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void DrawGame()
        {
            mSlowStone.Draw();
            mStone.Draw();
            mBrick.Draw();

           if (mBasket.ObjectVisibleInCameraWindow())
                mBasket.Draw();

        }
    }
}
