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
        // Work with Textured Primitives
        TexturedPrimitive mLargeFlower, mSmallTarget;
        TexturedPrimitive mCollidePosition;
        bool mPrimitiveCollide = false;
        bool mPixelCollide = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public GameState()
        {
            // Create the primitives
            mLargeFlower = new TexturedPrimitive("Flower", new Vector2(40, 30), new Vector2(60, 50));
            mSmallTarget = new TexturedPrimitive("Target", new Vector2(60, 50), new Vector2(3, 7));
            mCollidePosition = new TexturedPrimitive("Soccer", Vector2.Zero, new Vector2(3, 3));
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        public void UpdateGame()
        {

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        public void DrawGame()
        {

        }
    }
}
