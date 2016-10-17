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
    public class SpinningArrowSet
    {
        // Populate the world with 4 rows by 5 columns of spinning arrows
        private const int kNumRows = 4;
        private const int kNumColumns = 5;

        private List<SpinningArrow> mTheSet = new List<SpinningArrow>();

        public SpinningArrowSet()
        {
            Vector2 min = Camera.CameraWindowLowerLeftPosition;
            Vector2 max = Camera.CameraWindowUpperRightPosition;
            Vector2 size = max - min;
            float deltaX = size.X / (float)(kNumColumns + 1);
            float deltaY = size.Y / (float)(kNumRows + 1);
            
            for (int r = 0; r < kNumRows; r++)
            {
                min.Y += deltaY;
                float useDeltaX = deltaX;
                for (int c = 0; c < kNumColumns; c++)
                {
                    Vector2 pos = new Vector2(min.X + useDeltaX, min.Y);
                    SpinningArrow arrow = new SpinningArrow(pos);
                    mTheSet.Add(arrow);
                    useDeltaX += deltaX;
                }
            }
        }

        /// <summary>
        /// Go through the list to update each arrow
        /// </summary>
        /// <param name="hero"></param>
        public void UpdateSpinningSet(TexturedPrimitive hero)
        {
            foreach (var arrow in mTheSet)
                arrow.UpdateSpinningArrow(hero);
        }

        /// <summary>
        /// Go through the list to draw each 
        /// </summary>
        public void DrawSet()
        {
            foreach (var arrow in mTheSet)
                arrow.Draw();
        }
    }
}
