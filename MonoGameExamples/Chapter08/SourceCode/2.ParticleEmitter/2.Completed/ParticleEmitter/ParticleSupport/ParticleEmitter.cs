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
    public class ParticleEmitter
    {
        // Smallest number of particle emitted per cycle
        const int kMinToEmit = 5;

        // Emitter position
        protected Vector2 mEmitPosition;

        // Number of particles left to be emitted
        protected int mNumRemains;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pos">Location of the Emitter</param>
        /// <param name="n">Number of particles to emit</param>
        public ParticleEmitter(Vector2 pos, int n)
        {
            mNumRemains = n;
            mEmitPosition = pos;
        }

        public bool Expired { get { return (mNumRemains <= 0); } }

        /// <summary>
        /// Emits around 20% of what is left to be emitted.
        /// </summary>
        /// <param name="allParticles">Emits particles into this collection</param>
        public void EmitParticles(List<ParticlePrimitive> allParticles)
        {
            int numToEmit = 0;
            if (mNumRemains < kMinToEmit)
            {
                // If only a few are left, emits all of them
                numToEmit = mNumRemains;
            }
            else
            {
                // Other wise, emits about 20% of what's left
                numToEmit = (int)Game1.RandomNumber(0.2f * mNumRemains);
            }
            // Left for future emitting.                            
            mNumRemains -= numToEmit;
                        
            for (int i = 0; i < numToEmit; i++)
            {
                ParticlePrimitive p;
                // 40% chance emitting simple particle, 60% chance emitting the new reddish particle
                if (Game1.RandomNumber(1.0f) > 0.6f)
                    p = new ParticlePrimitive(mEmitPosition, 2f, 30);
                else
                    p = new ReddishParticlePrimitive(mEmitPosition, 2f, 80);
                allParticles.Add(p);
            }
        }
    }
}
