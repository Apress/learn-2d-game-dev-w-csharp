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
        // Collection of particles
        private List<ParticlePrimitive> mAllParticles;

        /// <summary>
        /// Constructor
        /// </summary>
        public ParticleSystem()
        {
            mAllParticles = new List<ParticlePrimitive>();
        }

        /// <summary>
        /// Create a particle at the position
        /// </summary>
        /// <param name="pos">Position to create the particle</param>
        public void AddParticleAt(Vector2 pos)
        {
            ParticlePrimitive particle = new ParticlePrimitive(pos, 2f, 50);
            mAllParticles.Add(particle);
        }

        /// <summary>
        /// Go through and update all particles in the system, remove
        /// those that has expired
        /// </summary>
        public void UpdateParticles()
        {
            int particleCounts = mAllParticles.Count;
            for (int i = particleCounts - 1; i >= 0; i--)
            {
                mAllParticles[i].Update();
                if (mAllParticles[i].Expired)
                    mAllParticles.RemoveAt(i);  // Remove expired ones
            }
        }

        /// <summary>
        /// 1. Switch blending mode to "Additive" 
        /// 2. Go through and draw all particles
        /// 3. Set mode back to "blending"
        /// </summary>
        public void DrawParticleSystem()
        {
            // 1. Switch blend mode to "Additive"
            Game1.sSpriteBatch.End();
            Game1.sSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            // 2. Draw all particles
            foreach (var particle in mAllParticles)
                particle.Draw();

            // 3. Switch blend mode back to AlphaBlend
            Game1.sSpriteBatch.End();
            Game1.sSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        }

    }
}
