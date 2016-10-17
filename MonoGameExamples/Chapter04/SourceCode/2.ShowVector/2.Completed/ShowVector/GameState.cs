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
        // Size of all the positions
        Vector2 kPointSize = new Vector2(5f, 5f);

        // Work with Textured Primitive 
        TexturedPrimitive mPa, mPb; // the locators for showing Point-A and Point-B
        TexturedPrimitive mPx; // to show same displacement can be applied to any position

        TexturedPrimitive mPy; // to show we can rotate/manipulate vectors independently 
        Vector2 mVectorAtPy = new Vector2(10, 0); // Start with Vector in the X direction;

        TexturedPrimitive mCurrentLocator;

        /// <summary>
        /// Constructor
        /// </summary>
        public GameState()
        {
            // Create the primitives
            mPa = new TexturedPrimitive("Position", new Vector2(30, 30), kPointSize, "Pa");
            mPb = new TexturedPrimitive("Position", new Vector2(60, 30), kPointSize, "Pb");
            mPx = new TexturedPrimitive("Position", new Vector2(20, 10), kPointSize, "Px");
            mPy = new TexturedPrimitive("Position", new Vector2(20, 50), kPointSize, "Py");
            mCurrentLocator = mPa;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        public void UpdateGame()
        {
            #region Step 3a. Change current selected vector
            if (InputWrapper.Buttons.A == ButtonState.Pressed)
                mCurrentLocator = mPa;
            else if (InputWrapper.Buttons.B == ButtonState.Pressed)
                mCurrentLocator = mPb;
            else if (InputWrapper.Buttons.X == ButtonState.Pressed)
                mCurrentLocator = mPx;
            else if (InputWrapper.Buttons.Y == ButtonState.Pressed)
                mCurrentLocator = mPy;
            #endregion

            #region Step 3b. Move Vector
            // Change the current locator position
            mCurrentLocator.Position += InputWrapper.ThumbSticks.Right;
            #endregion

            #region Step 3c. Rotate Vector
            // Left thumbstick-X rotates the vector at Py
            float rotateYByRadian = MathHelper.ToRadians(
                        InputWrapper.ThumbSticks.Left.X);
            #endregion

            #region Step 3d. Increase/Decrease the length of vector
            // Left thumbstick-Y increase/decrease the length of vector at Py
            float vecYLen = mVectorAtPy.Length();
            vecYLen += InputWrapper.ThumbSticks.Left.Y;
            #endregion

            #region Step 3e. Compute vector changes
            // Compute the rotated direction of vector at Py
            mVectorAtPy = ShowVector.RotateVectorByAngle(mVectorAtPy, rotateYByRadian);
            mVectorAtPy.Normalize(); // Normalize vectorATY to size of 1f
            mVectorAtPy *= vecYLen;  // Scale the vector to the new size
            #endregion
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary
        public void DrawGame()
        {
            // Drawing the vectors
            Vector2 v = mPb.Position - mPa.Position;    // Vector V is from Pa to Pb

            // Draw Vector-V at Pa, and Px
            ShowVector.DrawFromTo(mPa.Position, mPb.Position);
            ShowVector.DrawPointVector(mPx.Position, v);

            // Draw VectorAtY at Py
            ShowVector.DrawPointVector(mPy.Position, mVectorAtPy);

            mPa.Draw();
            mPb.Draw();
            mPx.Draw();
            mPy.Draw();

            // Print out text message to echo status
            FontSupport.PrintStatus("Locator Positions: A=" + mPa.Position + "  B=" + mPb.Position, null);
        }
    }
}
