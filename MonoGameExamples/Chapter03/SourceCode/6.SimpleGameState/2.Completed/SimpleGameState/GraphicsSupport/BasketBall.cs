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
    public class BasketBall : TexturedPrimitive
    {
        private const float kIncreaseRate = 1.001f; // Change current position by this amount
        private Vector2 kInitSize = new Vector2(5, 5);
        private const float kFinalSize = 15f;

        /// <summary>
        /// Constructor of BasketBall
        /// </summary>
        public BasketBall() : base("BasketBall")
        {
            mPosition = Camera.RandomPosition();
            mSize = kInitSize;
        }

        /// <summary>
        /// Continuously increase in size until it gets too large, returns true if exploded
        /// </summary>
        public bool UpdateAndExplode()
        {
            mSize *= kIncreaseRate;
            return mSize.X > kFinalSize;
        }
    }
}
