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
        const int kMinToEmit = 5;
            // smallest number of particle emitted per cycle

        protected Vector2 mEmitPosition;
            // Emitter position

        protected int mNumRemains;
            // number of particles left to be emitted

        private ParticleCreator mParticleCreator;

        public delegate ParticlePrimitive ParticleCreator(Vector2 pos);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pos">Location of the Emitter</param>
        /// <param name="n">Number of particles to emit</param>
        public ParticleEmitter(ParticleCreator creator, Vector2 pos, int n)
        {
            mParticleCreator = creator;
            mNumRemains = n;
            mEmitPosition = pos;
        }

        /// <summary>
        /// Emits around 20% of what is left to be emitted.
        /// </summary>
        /// <param name="allParticles">Emits particles into this collection</param>
        public void EmitParticles(List<ParticlePrimitive> allParticles)
        {
            int numToEmit = 0;
            if (mNumRemains < kMinToEmit)
                numToEmit = mNumRemains;    // if only a few left, emits all of them
            else
                numToEmit = (int)Game1.RandomNumber(0.2f * mNumRemains);
                                        // other wise, emits about 20% of what's left
            mNumRemains -= numToEmit;
                        // left for future emitting.

            for (int i = 0; i < numToEmit; i++)
            {
                ParticlePrimitive p = mParticleCreator.Invoke(mEmitPosition);
                allParticles.Add(p);
            }
        }

        public bool Expired { get { return (mNumRemains <= 0); } }

    }
}
