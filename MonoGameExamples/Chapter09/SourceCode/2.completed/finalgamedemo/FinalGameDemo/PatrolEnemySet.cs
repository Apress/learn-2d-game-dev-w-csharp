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
    /// Namespace wide type
    /// </summary>
    public enum EnemyType
    {
        BlowFish = 0,
        JellyFish = 1,
        FightingFish = 2
    }

    public class PatrolEnemySet
    {
        #region Particles
        private const float kCollideParticleSize = 3f;
        private const int kCollideParticleLife = 80;
        private static ParticleSystem sCollisionEffect = new ParticleSystem();
        // to support particle system
        static private ParticlePrimitive CreateRedParticle(Vector2 pos)
        {
            return new ParticlePrimitive(pos, kCollideParticleSize, kCollideParticleLife);
        }
        static private ParticlePrimitive CreateDarkParticle(Vector2 pos)
        {
            return new DarkParticlePrimitive(pos, kCollideParticleSize, kCollideParticleLife);
        }
        #endregion

        private List<PatrolEnemy> mTheSet = new List<PatrolEnemy>();
        private float mAddEnemyDistance = 100f;

        //Constants
        private const int kNumEnemies = 5;

        public PatrolEnemySet()
        {
            // Create many ...
            for (int i = 0; i < kNumEnemies; i++)
            {
                PatrolEnemy e = SpawnRandomPatrolEnemy();
                mTheSet.Add(e);
            }
        }

        public PatrolEnemy SpawnRandomPatrolEnemy()
        {
            int randNum = (int)(Game1.sRan.NextDouble() * 3);
            PatrolEnemy enemy = null;
            switch (randNum)
            {
                case (int)EnemyType.BlowFish:
                    enemy = new BlowFish();
                    break;
                case (int)EnemyType.JellyFish:
                    enemy = new JellyFish();
                    break;
                case (int)EnemyType.FightingFish:
                    enemy = new FightingFish();
                    break;
                default:
                    break;

            }
            return enemy;
        }

        public int UpdateSet(Hero hero)
        {
            int count = 0;
            Vector2 touchPos;

            //Add an enemy at 100m and every 50 after
            //Should an additional enemy be added?
            if (hero.PositionX / 20 > mAddEnemyDistance)
            {
                PatrolEnemy e = SpawnRandomPatrolEnemy();
                mTheSet.Add(e);
                mAddEnemyDistance += 50;
            }

            // destroy and respawn, update and collide with bubbles
            for (int i = mTheSet.Count - 1; i >= 0; i--)
            {
                if (mTheSet[i].DestoryFlag)
                {
                    mTheSet.Remove(mTheSet[i]);
                    mTheSet.Add(SpawnRandomPatrolEnemy());
                    continue;
                }

                if (mTheSet[i].UpdatePatrol(hero, out touchPos))
                {
                    sCollisionEffect.AddEmitterAt(CreateRedParticle, touchPos);
                    count++;
                }

                List<BubbleShot> allBubbleShots = hero.AllBubbleShots();
                int numBubbleShots = allBubbleShots.Count;
                for (int j = numBubbleShots - 1; j >= 0; j--)
                {
                    if (allBubbleShots[j].PixelTouches(mTheSet[i], out touchPos))
                    {
                        mTheSet[i].SetToStuntState();
                        allBubbleShots.RemoveAt(j);
                        sCollisionEffect.AddEmitterAt(CreateRedParticle, touchPos);
                    }
                }
            }

            sCollisionEffect.UpdateParticles();
            RespawnEnemies();
            return count;
        }

        // Respawn enemies that are off the left side of the camera
        public void RespawnEnemies()
        {
            for (int i = mTheSet.Count - 1; i >= 0; i--)
            {
                if (mTheSet[i].PositionX < Camera.CameraWindowLowerLeftPosition.X - mTheSet[i].Width)
                {
                    mTheSet.Remove(mTheSet[i]);
                    mTheSet.Add(SpawnRandomPatrolEnemy());
                }
            }
        }

        public void DrawSet()
        {
            foreach (var e in mTheSet)
                e.Draw();

            sCollisionEffect.DrawParticleSystem();
        }
    }
}
