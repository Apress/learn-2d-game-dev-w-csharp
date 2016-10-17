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
    /// Hero class: SpritePrimitive with behavior
    /// </summary>
    public class Hero : SpritePrimitive
    {
        #region Particle Stuff
        private const float kCollideParticleSize = 4f;
        private const int kCollideParticleLife = 20;

        ParticleSystem mCollisionEffect = new ParticleSystem();
        // to support particle system
        private ParticlePrimitive CreateParticle(Vector2 pos)
        {
            return new ParticlePrimitive(pos, kCollideParticleSize, kCollideParticleLife);
        }
        #endregion
        
        //Constants
        private const float kHeroWidth = 20f;
        private const float kTimeBetweenBubbleShot = 1.5f;     // number of seconds between shots
        private const float kBubbleShotOffset = 0.35f * kHeroWidth;
        private const float kStunTimer = 1.5f;
        private const int kMaxHeroSize = 2;
        private float mTimeSinceLastShot = 0;
        private float mStunTimer;
        private int mHeroCurrentSize;
        
        public int HeroSize
        {
            get { return mHeroCurrentSize; }
            set
            {
                mHeroCurrentSize = value;
                this.Size = new Vector2(kHeroWidth + kHeroWidth * (mHeroCurrentSize - 1) / 3, kHeroWidth + kHeroWidth * (mHeroCurrentSize - 1) / 3);
            }
        }
        
        private enum HeroState
        {
            Playing,
            Stunned,
            Unstunnable,
            Lost
        }
        private HeroState mCurrentHeroState;

        private List<BubbleShot> mBubbleShots;
        public List<BubbleShot> AllBubbleShots() { return mBubbleShots; }

        /// <summary>
        /// Constructor 
        /// </summary>
        public Hero(Vector2 position)
            : base("HERO_1", position, new Vector2(kHeroWidth, kHeroWidth), 2, 2, 0)
        {
            mHeroCurrentSize = 1;
            mStunTimer = 0;
            mCurrentHeroState = HeroState.Playing;
            mBubbleShots = new List<BubbleShot>();
            mTimeSinceLastShot = kTimeBetweenBubbleShot;

            SetSpriteAnimation(0, 0, 1, 1, 10);
            SpriteCurrentRow = 1;          
        }

        /// <summary>
        /// Update the game object change hero location
        /// </summary>
        public void Update(GameTime gameTime, Vector2 delta, bool shootBubbleShot)
        {
            switch(mCurrentHeroState)
            {
                case HeroState.Playing:
                    UpdatePlayingState(gameTime, delta, shootBubbleShot);
                    break;
                case HeroState.Stunned:
                    UpdateStunnedState(gameTime);
                    break;
                case HeroState.Unstunnable:
                    UpdateUnstunnableState(gameTime);
                    UpdatePlayingState(gameTime, delta, shootBubbleShot);
                    break;
                case HeroState.Lost:
                    mCurrentHeroState = HeroState.Lost;
                    break;
                default:
                    break;
            }
        }

        public void UpdatePlayingState(GameTime gameTime, Vector2 delta, bool shootBubbleShot)
        {
            base.Update();
            // take advantage of the camera window bound check
            BoundObjectToCameraWindow();
            
            // Player control
            mPosition += delta;

            // Sprite facing direction
            if (delta.X > 0)
                SpriteCurrentRow = 1;
            else if (delta.X < 0)
                SpriteCurrentRow = 0;

            // BubbleShot direction
            int bubbleShotDir = 1;
            if (SpriteCurrentRow == 0)
                bubbleShotDir = -1;

            mCollisionEffect.UpdateParticles();

            float deltaTime = gameTime.ElapsedGameTime.Milliseconds;
            mTimeSinceLastShot += deltaTime / 1000;
            
            // Can the hero shoot a BubbleShot?
            if (mTimeSinceLastShot >= kTimeBetweenBubbleShot)
            {
                if (shootBubbleShot)
                {
                    BubbleShot j = new BubbleShot(new Vector2(Position.X + kBubbleShotOffset * bubbleShotDir, Position.Y), bubbleShotDir);
                    mBubbleShots.Add(j);
                    mTimeSinceLastShot = 0;
                    AudioSupport.PlayACue("Bubble");
                }
            }

            // now update all the BubbleShots out there ...
            int count = mBubbleShots.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                if (!mBubbleShots[i].IsOnScreen())
                {
                    // outside now!
                    mBubbleShots.RemoveAt(i);
                }
                else
                    mBubbleShots[i].Update();
            }
        }

        public void UpdateStunnedState(GameTime gameTime)
        {
            float deltaTime = gameTime.ElapsedGameTime.Milliseconds;
            mStunTimer += deltaTime / 1000;
            if (mStunTimer >= kStunTimer)
            {
                mStunTimer = 0;
                mCurrentHeroState = HeroState.Unstunnable;
            }
        }

        public void UpdateUnstunnableState(GameTime gameTime)
        {
            float deltaTime = gameTime.ElapsedGameTime.Milliseconds;
            mStunTimer += deltaTime / 1000;
            if (mStunTimer >= kStunTimer)
            {
                mStunTimer = 0;
                mCurrentHeroState = HeroState.Playing;
            }
        }

        public override void Draw()
        {
            base.Draw();
            foreach (var j in mBubbleShots)
                j.Draw();
            mCollisionEffect.DrawParticleSystem();
        }

        public void AdjustSize(int incAdjustment)
        {
            if (incAdjustment + HeroSize > kMaxHeroSize)
                return;
            HeroSize += incAdjustment;
            MathHelper.Clamp(HeroSize, 0, 3);
            if (HeroSize <= 0)
            {
                mCurrentHeroState = HeroState.Lost;
            }
        }

        public void StunHero()
        {
            if (mCurrentHeroState != HeroState.Unstunnable && mCurrentHeroState != HeroState.Stunned)
            {
                mCurrentHeroState = HeroState.Stunned;
                AudioSupport.PlayACue("Stun");
                AdjustSize(-1);
            }
        }

        public bool HasLost()
        {
            if (mCurrentHeroState == HeroState.Lost)
                return true;
            else
                return false;
        }

        public void Feed()
        {
            AdjustSize(1);
            AudioSupport.PlayACue("Chomp");
        }
    }
}
