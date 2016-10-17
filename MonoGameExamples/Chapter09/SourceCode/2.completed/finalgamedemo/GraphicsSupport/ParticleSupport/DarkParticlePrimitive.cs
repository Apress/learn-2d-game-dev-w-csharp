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
    /// 
    /// </summary>
    public class DarkParticlePrimitive : ParticlePrimitive
    {
        /// <summary>
        /// Constructor:
        /// </summary>
        public DarkParticlePrimitive(Vector2 position, float size, int lifeSpan) :
            base(position, size, lifeSpan)
        {
            mSpeed *= 2.25f;

            mTintColor = Color.DarkGreen;
        }
                
        /// <summary>
        /// Do the same as a regular particle, 
        /// execpt change the tint color to more dark redish
        /// </summary>
        public override void Update()
        {
            base.Update();         
            mTintColor = Color.DarkGreen;
        }
    }
}
