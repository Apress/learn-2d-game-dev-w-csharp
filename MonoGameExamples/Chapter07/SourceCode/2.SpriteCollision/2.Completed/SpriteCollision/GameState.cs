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

        SpritePrimitive mHero;  // Hero sprite

        const int kNumPlanes = 4;
        TexturedPrimitive[] mPlane; // The Planes
        TexturedPrimitive mFlower;  // The large background
        TexturedPrimitive mCurrentPrim; // Refer to either plane or flower

        // Support for displaying of collision
        TexturedPrimitive mHeroTarget; // Where latest hero pixel collision happened
        bool mHeroPixelCollision; // If there is a pixel collision for the hero
        bool mHeroBoundCollision; // If there is a image bound collision for the hero

        public GameState()
        {
            // Set up the flower ...
            mFlower = new TexturedPrimitive("Flower", new Vector2(50, 35), new Vector2(60, 60));

            // Planes
            mPlane = new TexturedPrimitive[kNumPlanes];
            mPlane[0] = new TexturedPrimitive("PatrolEnemy", new Vector2(10, 15), new Vector2(5, 10));
            mPlane[1] = new TexturedPrimitive("PatrolEnemy", new Vector2(90, 15), new Vector2(5, 10));
            mPlane[2] = new TexturedPrimitive("PatrolEnemy", new Vector2(90, 55), new Vector2(5, 10));
            mPlane[3] = new TexturedPrimitive("PatrolEnemy", new Vector2(10, 55), new Vector2(5, 10));

            mHeroTarget = new TexturedPrimitive("Target", new Vector2(0, 0), new Vector2(3, 3));
            mCurrentPrim = mPlane[0];
            mHeroBoundCollision = false;
            mHeroPixelCollision = false;

            mHero = new SpritePrimitive("SimpleSpriteSheet", new Vector2(20, 30), new Vector2(10, 10),
                            4,  // Number of rows
                            2,  // Number of columns
                            0); // Padding between images

           
            // Start Hero by walking left and AnotherHero by walking towards right
            mHero.SetSpriteAnimation(0, 0, 0, 3, 10); // Slowly
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        public void UpdateGame()
        {
            mHero.Position += InputWrapper.ThumbSticks.Left; 
            mHero.Update();

            CollisionUpdate();
            UserControlUpdate();
        }

        private void CollisionUpdate()
        {
            Vector2 pixelCollisionPosition = Vector2.Zero;

            #region Collide the hero with the flower
            mHeroBoundCollision = mHero.PrimitivesTouches(mFlower);
            mHeroPixelCollision = mHeroBoundCollision;
            if (mHeroBoundCollision)
            {
                mHeroPixelCollision = mHero.PixelTouches(mFlower, out pixelCollisionPosition);
                if (mHeroPixelCollision)
                    mHeroTarget.Position = pixelCollisionPosition;
            }
            #endregion

            #region Collide the hero with planes
            int i = 0;
            while ((!mHeroPixelCollision) && (i < kNumPlanes))
            {
                mHeroBoundCollision = mPlane[i].PrimitivesTouches(mHero);
                mHeroPixelCollision = mHeroBoundCollision;
                if (mHeroBoundCollision)
                {
                    mHeroPixelCollision = mPlane[i].PixelTouches(mHero, out pixelCollisionPosition);
                    if (mHeroPixelCollision)
                        mHeroTarget.Position = pixelCollisionPosition;
                }
                i++;
            }
            #endregion
        }

        private void UserControlUpdate()
        {
            #region Selecting Hero
            if (InputWrapper.Buttons.A == ButtonState.Pressed)
                mCurrentPrim = mFlower;
            if (InputWrapper.Buttons.B == ButtonState.Pressed)
                mCurrentPrim = mPlane[0];
            mCurrentPrim.Position += InputWrapper.ThumbSticks.Right;
            #endregion

            #region Specifying hero rotation
            if (InputWrapper.Buttons.X == ButtonState.Pressed)
                mHero.RotateAngleInRadian += MathHelper.ToRadians(1);
            if (InputWrapper.Buttons.Y == ButtonState.Pressed)
                mHero.RotateAngleInRadian += MathHelper.ToRadians(-1);
            #endregion

            #region Specifying flower rotation
            mCurrentPrim.RotateAngleInRadian += MathHelper.ToRadians(
                InputWrapper.Triggers.Left);
            mCurrentPrim.RotateAngleInRadian -= MathHelper.ToRadians(
                            InputWrapper.Triggers.Right);
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
            mFlower.Draw();
            foreach (var p in mPlane)
                p.Draw();
            mHero.Draw();

            if (mHeroPixelCollision)
                mHeroTarget.Draw();

            FontSupport.PrintStatus("Collisions Bound(" +
                                mHeroBoundCollision + ") Pixel(" +
                                mHeroPixelCollision + ")", null);
        }
    }
}
