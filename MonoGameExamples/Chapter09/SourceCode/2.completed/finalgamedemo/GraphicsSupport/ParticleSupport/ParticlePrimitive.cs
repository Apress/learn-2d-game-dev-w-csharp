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
    public class ParticlePrimitive : GameObject
    {
        /// <summary>
        /// Preset randomness to avoid visible patterns
        /// </summary>
        protected float kLifeSpanRandomness = 0.4f;
        protected float kSizeChangeRandomness = 0.5f;
        protected float kSizeRandomness = 0.3f;
        protected float kSpeedRandomness = 0.1f;

        /// <summary>
        /// Attributes for each particle
        /// </summary>
        protected int mLifeSpan;
            // number of updates before a particle disappear
        protected float mSizeChangeRate;
            /// How fast does the particle chagnes size

        /// <summary>
        /// Constructor:
        /// </summary>
        public ParticlePrimitive(Vector2 position, float size, int lifeSpan) :
            base("BUBBLE_1", position, new Vector2(size, size))
        {
            mLifeSpan = (int) (lifeSpan * 
                        Game1.RandomNumber(-kLifeSpanRandomness, kLifeSpanRandomness));

            mVelocityDir.X = Game1.RandomNumber(-0.5f, 0.5f);
            mVelocityDir.Y = Game1.RandomNumber(-0.5f, 0.5f);
            mVelocityDir.Normalize();
            mSpeed = Game1.RandomNumber(kSpeedRandomness);

            mSizeChangeRate = Game1.RandomNumber(kSizeChangeRandomness);

            mSize.X *= Game1.RandomNumber(1f-kSizeRandomness, 1+kSizeRandomness);
            mSize.Y = mSize.X;
        }
                
        /// <summary>
        /// Should the particle be deleted?
        /// </summary>
        public bool Expired { get { return (mLifeSpan < 0); } }
        

        /// <summary>
        /// Overrides the GameObject::Update, move the particle 
        /// by velocity (by calling GameObject::Update()), and then
        /// changes the apparance of the particle according to the 
        /// random parameters
        /// </summary>
        public override void Update()
        {
            base.Update();
            
            mLifeSpan--;   // continue to approach expiration

            // change its size
            mSize.X += mSizeChangeRate;
            mSize.Y += mSizeChangeRate;

            // change the tintcolor randomly
            Byte[] b = new Byte[3];
            Game1.sRan.NextBytes(b);
            mTintColor.R += b[0];
            mTintColor.G += b[1];
            mTintColor.B += b[2];
        }
    }
}
