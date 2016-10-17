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
        const int kSpriteSpeedFactor = 10;    // Value of 1 maps to updates of 10 ticks
        SpritePrimitive mHero, mAnotherHero;
        SpritePrimitive mCurrent;
		
        public GameState()
        {
            mHero = new SpritePrimitive("SimpleSpriteSheet", 
                            new Vector2(20, 30), new Vector2(10, 10),
                            4,  // Number of rows
                            2,  // Number of columns
                            0); // Padding between images

            mAnotherHero = new SpritePrimitive("SimpleSpriteSheet", 
                            new Vector2(80, 30), new Vector2(10, 10),
                            4,  // Number of rows
                            2,  // Number of columns
                            0); // Padding between images

            // Start Hero by walking left and AnotherHero by walking towards right
            mHero.SetSpriteAnimation(0, 0, 0, 3, 10); // Slowly
            mAnotherHero.SetSpriteAnimation(1, 0, 1, 3, 5); // Twice as fast
            
            mCurrent = mAnotherHero;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        public void UpdateGame()
        {
            mHero.Update();
            mAnotherHero.Update();

            UserControlUpdate();
        }

        private void UserControlUpdate()
        {
            #region Selecting Hero
            if (InputWrapper.Buttons.A == ButtonState.Pressed)
                mCurrent = mHero;
            if (InputWrapper.Buttons.B == ButtonState.Pressed)
                mCurrent = mAnotherHero;
            mCurrent.Position += InputWrapper.ThumbSticks.Right;
            #endregion

            #region Specifying rotation
            if (InputWrapper.Buttons.X == ButtonState.Pressed)
                mCurrent.RotateAngleInRadian += MathHelper.ToRadians(1);
            if (InputWrapper.Buttons.Y == ButtonState.Pressed)
                mCurrent.RotateAngleInRadian += MathHelper.ToRadians(-1);
            #endregion

            #region Sprite Sheet Update
            if (InputWrapper.ThumbSticks.Left.X == 0)
            {
                mCurrent.SpriteEndColumn = 0;  // Stops the animation
            }
            else
            {
                float useX = InputWrapper.ThumbSticks.Left.X;
                mCurrent.SpriteEndColumn = 3;
                if (useX < 0)
                {
                    mCurrent.SpriteBeginRow = 1;
                    mCurrent.SpriteEndRow = 1;
                    useX *= -1f;
                }
                else
                {
                    mCurrent.SpriteBeginRow = 0;
                    mCurrent.SpriteEndRow = 0;
                }
                mCurrent.SpriteAnimationTicks = (int)((1f - useX) * kSpriteSpeedFactor);
            }
            #endregion
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void DrawGame()
        {
            mHero.Draw();
            mAnotherHero.Draw();
        }
    }
}
