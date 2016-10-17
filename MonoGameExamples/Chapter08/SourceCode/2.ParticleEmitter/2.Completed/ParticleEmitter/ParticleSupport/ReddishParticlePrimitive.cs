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
    public class ReddishParticlePrimitive : ParticlePrimitive
    {
        /// <summary>
        /// Constructor:
        /// </summary>
        public ReddishParticlePrimitive(Vector2 position, float size, int lifeSpan) :
            base(position, size, lifeSpan)
        {
            mVelocityDir.Y = 5f * Math.Abs(mVelocityDir.Y);
            mVelocityDir.Normalize();
            mSpeed *= 5.25f;
            mSizeChangeRate *= 1.5f;
            mSize.X *= 0.7f;
            mSize.Y = mSize.X;

            mTintColor = Color.DarkOrange;
        }
        
        /// <summary>
        /// Do the same as a regular particle, 
        /// except change the tint color to more dark reddish
        /// </summary>
        public override void Update()
        {
            base.Update();

            Color s = mTintColor;
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
