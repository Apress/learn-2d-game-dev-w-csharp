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
    public class RedishParticlePrimitive : ParticlePrimitive
    {
        /// <summary>
        /// Constructor:
        /// </summary>
        public RedishParticlePrimitive(Vector2 position, float size, int lifeSpan) :
            base(position, size, lifeSpan)
        {
            mVelocityDir.Y = 5f * Math.Abs(mVelocityDir.Y);
            mVelocityDir.Normalize();
            mSpeed *= 4.25f;
            mSizeChangeRate *= 1.5f;
            mSize.X *= 0.7f;
            mSize.Y = mSize.X;

            mTintColor = Color.DarkRed;
        }
                
        /// <summary>
        /// Do the same as a regular particle, 
        /// execpt change the tint color to more dark redish
        /// </summary>
        public override void Update()
        {
            Color s = mTintColor;
            base.Update();

            if (s.R < 255)
                s.R += 1;
            if (s.G != 0)
                s.G -= 1;
            if (s.B != 0)
                s.B -= 1;
            mTintColor = s;
        }
    }
}
