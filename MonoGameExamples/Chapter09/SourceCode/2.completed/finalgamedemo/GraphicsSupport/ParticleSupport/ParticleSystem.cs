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
    public class ParticleSystem
    {
        private List<ParticlePrimitive> mAllParticles;
        private List<ParticleEmitter> mAllEmitters;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public ParticleSystem()
        {
            mAllParticles = new List<ParticlePrimitive>();
            mAllEmitters = new List<ParticleEmitter>();
        }

        /// <summary>
        /// Add a new emitter into the system, each emitter will emit
        /// between 50 to 100 particles.
        /// </summary>
        /// <param name="pos">Postion to emit the particles from</param>
        public void AddEmitterAt(ParticleEmitter.ParticleCreator creator, Vector2 pos)
        {
            ParticleEmitter e = new ParticleEmitter(creator, pos, (int) Game1.RandomNumber(50, 100));
            mAllEmitters.Add(e);
        }

        /// <summary>
        /// Updates the system: emitter will emit, 
        /// all particles wil update themselves.
        /// </summary>
        public void UpdateParticles()
        {
            // First emit!
            int count = mAllEmitters.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                mAllEmitters[i].EmitParticles(mAllParticles);
                if (mAllEmitters[i].Expired)
                    mAllEmitters.RemoveAt(i);
            }

            // now particles ...
            count = mAllParticles.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                mAllParticles[i].Update();
                if (mAllParticles[i].Expired)
                    mAllParticles.RemoveAt(i);  // remove expired ones
            }
        }

        /// <summary>
        /// 1. Switch blending mode to "Additive" 
        /// 2. Go through and draw all particles
        /// 4. Set mode back to "blending"
        /// </summary>
        public void DrawParticleSystem()
        {
            // 1. Switch blend mode to "Additive"
            Game1.sSpriteBatch.End();
            Game1.sSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            // 2. Draw all particles
            foreach (var p in mAllParticles)
                p.Draw();

            // 3. Switch blend mode back to AlphaBlend
            Game1.sSpriteBatch.End();
            Game1.sSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        }

    }
}
