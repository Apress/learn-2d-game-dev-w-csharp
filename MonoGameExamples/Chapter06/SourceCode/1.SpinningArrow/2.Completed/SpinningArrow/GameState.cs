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
        PlayerControlHero mHero;
        SpinningArrow mArrow;
        
        public GameState()
        {
            mArrow = new SpinningArrow(new Vector2(50, 40));
            mHero = new PlayerControlHero(new Vector2(5, 5));
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        public void UpdateGame()
        {
            mHero.UpdateHero();
            mArrow.UpdateSpinningArrow(mHero);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void DrawGame()
        {
            mHero.Draw();
            mArrow.Draw();

            FontSupport.PrintStatus("Arrow State: " + mArrow.ArrowState.ToString(), null);
        }
    }
}
