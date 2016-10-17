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
        const int kSpriteSpeedFactor = 10;    // value of 1 maps to updates of 10 ticks
        SpritePrimitive mHero, mAnotherHero;
        SpritePrimitive mCurrent;

        public GameState()
        {
            mHero = new SpritePrimitive("SimpleSpriteSheet",
                new Vector2(20, 30), new Vector2(10, 10),
                4,  // number of rows
                2,  // number of columns
                0); // padding between images

            mAnotherHero = new SpritePrimitive("SimpleSpriteSheet",
                            new Vector2(80, 30), new Vector2(10, 10),
                            4,  // number of rows
                            2,  // number of columns
                            0); // padding between images

            // Start Hero by walking left and AnotherHero by walking towards right
            mHero.SetSpriteAnimation(0, 0, 0, 3, 10); // slowly
            mAnotherHero.SetSpriteAnimation(1, 0, 1, 3, 5); // twice as fast

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

            CollisionUpdate();
            UserControlUpdate();
        }

        private void CollisionUpdate()
        {

        }

        private void UserControlUpdate()
        {
            #region Selecting Hero

            #endregion

            #region Specifying rotation on hero

            #endregion

            #region Specifying rotation on flower
            
            #endregion

            #region Sprite Sheet Update
            if (InputWrapper.ThumbSticks.Left.X == 0)
            {
                mHero.SpriteEndColumn = 0;  // stops the animation
            }
            else
            {
                float useX = InputWrapper.ThumbSticks.Left.X;
                mHero.SpriteEndColumn = 3;
                if (useX < 0)
                {
                    mHero.SpriteBeginRow = 1;
                    mHero.SpriteEndRow = 1;
                    useX *= -1f;
                }
                else
                {
                    mHero.SpriteBeginRow = 0;
                    mHero.SpriteEndRow = 0;
                }
                mHero.SpriteAnimationTicks = (int)((1f - useX) * kSpriteSpeedFactor);
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
