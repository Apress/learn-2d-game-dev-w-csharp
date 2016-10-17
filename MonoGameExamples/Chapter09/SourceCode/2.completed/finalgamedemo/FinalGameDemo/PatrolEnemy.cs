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
    public class PatrolEnemy : SpritePrimitive
    {
        protected enum PatrolState
        {
            PatrolState,
            ChaseHero,
            StuntState
        }

        protected enum PatrolType
        {
            FreeRoam,
            LeftRight,
            UpDown
        }

        // Constants  ...
        private const float kPatrolSpeed = 0.2f;
        private const float kCloseEnough = 20f; // this is the distance to trigger next patrol target
        private const float kDistToBeginChase = 40f; // this is the distance to trigger patrol chasing of hero
        private const int kStateTimer = 60 * 5; // assuming 60 updates per second, this is about 5 seconds
        private const int kStunCycle = kStateTimer / 2; // half of regular state timer
        private const float kChaseSpeed = 0.3f;
        protected const float kEnemyWidth = 10f;
        protected const int kInitFishSize = 1;

        private Color kPatrolTint = Color.White;
        private Color kChaseTint = Color.OrangeRed;
        private Color kStuntTint = Color.LightCyan;

        private Vector2 mTargetPosition;            // Target position we are moving towards
        private PatrolState mCurrentState;          // Current State
        protected PatrolType mCurrentPatrolType;     // Current Patrol Type
        protected EnemyType mCurrentEnemyType;
        protected bool mAllowRotate;

        private int mStateTimer;    // interestingly, with "gradual" velocity changing, we cannot
                                    // guarantee that we will ever rich the mTargetPosition 
                                    // (we may ended up orbiting the target), so we set a timer
                                    // when timer is up, we transit

        private bool mDestoryFlag;
        public bool DestoryFlag { get { return mDestoryFlag; } }
        
        protected int mFishSize;
        public int FishSize
        {
            get { return mFishSize; }
            set
            {
                mFishSize = value;
                this.Size = new Vector2(mFishSize * kEnemyWidth + kEnemyWidth, mFishSize * kEnemyWidth + kEnemyWidth);
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public PatrolEnemy(String image, Vector2 position, Vector2 size, int rowCounts, int columnCount, int padding) :
            base(image, position, size, rowCounts, columnCount, padding)
        {
            // causes update state to always change into a new state
            mTargetPosition = Position = Vector2.Zero;
            Velocity = Vector2.UnitY;
            mTintColor = kPatrolTint;
            mCurrentPatrolType = PatrolType.FreeRoam;
            Position = RandomPosition(true);
            mDestoryFlag = false;
            mAllowRotate = false;
            SetSpriteAnimation(0, 0, 1, 1, 10);
            FishSize = kInitFishSize;
            mCurrentEnemyType = EnemyType.BlowFish;
        }

        /// <summary>
        /// Updates states 
        /// </summary>        
        public bool UpdatePatrol(Hero hero, out Vector2 caughtPos)
        {
            
            bool caught = false;
            caughtPos = Vector2.Zero;

            mStateTimer--;

            // perform operation common to all states ...
            if (mCurrentState != PatrolState.StuntState)
            {
                base.Update();
                Vector2 toHero = hero.Position - Position;
                toHero.Normalize();
                Vector2 toTarget = mTargetPosition - Position;
                float distToTarget = toTarget.Length();
                toTarget /= distToTarget; // this is the same as normalization
                ComputeNewDirection(toTarget, toHero);

                switch (mCurrentState)
                {
                    case PatrolState.PatrolState:
                        UpdatePatrolState(hero, distToTarget);
                        break;

                    case PatrolState.ChaseHero:
                        caught = UpdateChaseHeroState(hero, distToTarget, out caughtPos);
                        break;
                }
            }
            else
            {
                UpdateStuntState(hero);
            }
            return caught;
        }

        public void SetToStuntState()
        {
            mTintColor = kStuntTint;
            mStateTimer = kStunCycle;
            mCurrentState = PatrolState.StuntState;
            AudioSupport.PlayACue("Stun");
        }

        #region support Hero detection
        /// <summary>
        /// Check if Hero is close enough, if so, transit into ChaseHero State
        /// </summary>
        /// <param name="hero">The hero to check for</param>
        private void DetectHero(GameObject hero)
        {
            Vector2 toHero = hero.Position - Position;
            if (toHero.Length() < kDistToBeginChase)
            {
                SetToChaseState(hero);
            }
        }

        public void SetToChaseState(GameObject hero)
        {
            mStateTimer = (int)(kStateTimer * 1.5f); // 1.5 times as much time for chasing
            Speed = kChaseSpeed;
            mCurrentState = PatrolState.ChaseHero;
            mTargetPosition = hero.Position;
            mTintColor = kChaseTint;
        }

        /// <summary>
        /// Update chasing of hero state
        /// </summary>
        /// <param name="hero">The hero</param>
        /// <param name="distToHero">how far away is the hero</param>
        /// <returns></returns>
        private bool UpdateChaseHeroState(Hero hero, float distToHero, out Vector2 pos)
        {
            bool caught = false;
            caught = PixelTouches(hero, out pos);
            mTargetPosition = hero.Position;

            if (caught)
            {
                switch (mCurrentEnemyType)
                {
                    case EnemyType.BlowFish:
                        hero.AdjustSize(-1);
                        this.FishSize--;
                        this.mDestoryFlag = true;
                        break;
                    case EnemyType.JellyFish:
                        hero.StunHero();
                        break;
                    case EnemyType.FightingFish:
                        if (hero.HeroSize > this.FishSize)
                        {
                            this.FishSize--;
                            this.mDestoryFlag = true;
                            hero.Feed();
                            
                        }
                        else if (hero.HeroSize <= this.FishSize)
                        {
                            this.FishSize--;
                            this.mDestoryFlag = true;
                            hero.AdjustSize(-1);
                        }
                        break;
                    default:
                        break;
 
                }

            }
            else if (mStateTimer < 0)
                RandomNextTarget();

            return caught;
        }


        private void UpdateStuntState(Hero hero)
        {
            if (mStateTimer < 0)
            {
                SetToChaseState(hero);
            }
        }

        #endregion

        #region Simple Patrol state Transition
        /// <summary>
        /// Update patrol state, if reach destination, get a random next target
        /// if hero is close, transit into ChaseHero state
        /// </summary>
        /// <param name="hero">the hero to chase</param>
        /// <param name="distToTarget">distance to the current partrol target</param>
        private void UpdatePatrolState(GameObject hero, float distToTarget)
        {
            if ((mStateTimer < 0) || (distToTarget < kCloseEnough))
            {
                switch (mCurrentPatrolType)
                {
                    case PatrolType.FreeRoam:
                        RandomNextTarget();
                        break;
                    case PatrolType.LeftRight:
                        GenerateLeftRightTarget();
                        break;
                    case PatrolType.UpDown:
                        GenerateUpDownTarget();
                        break;
                }
            }
            DetectHero(hero); // check if we should transit to ChaseHero
        }
        #endregion

        /// <summary>
        /// Compute new speed and reset state timer
        /// </summary>
        private void ComputeNewSpeedAndResetTimer()
        {
            Speed = kPatrolSpeed * (0.8f + (float)(0.4 * Game1.sRan.NextDouble())); // speed: ranges between 80% to 120
            mStateTimer = (int)(kStateTimer * (0.8f + (float)(0.6 * Game1.sRan.NextDouble())));
        }


        /// <summary>
        /// Compute new direction based on current relation to the destination
        /// to gain "gradual turning effect" or facing direction
        /// </summary>
        /// <param name="toTarget">vector towards the target</param>
        private void ComputeNewDirection(Vector2 toTarget, Vector2 toHero)
        {
            if (mAllowRotate)
            {
                // figure out if we should continue to adjust our direction ...
                double cosTheta = Vector2.Dot(toTarget, FrontDirection);
                float theta = (float)Math.Acos(cosTheta);
                if (theta > float.Epsilon)
                {
                    Vector3 frontDir3 = new Vector3(FrontDirection, 0f);
                    Vector3 toTarget3 = new Vector3(toTarget, 0f);
                    Vector3 zDir = Vector3.Cross(frontDir3, toTarget3);
                    RotateAngleInRadian -= Math.Sign(zDir.Z) * 0.03f * theta; // rotate 5% at a time towards final direction
                    VelocityDirection = FrontDirection;
                }
            }
            else
            {
                VelocityDirection = toTarget;
                if (VelocityDirection.X > 0)
                    SpriteCurrentRow = 1;
                else if (VelocityDirection.X < 0)
                    SpriteCurrentRow = 0;
            }
        }

        /// <summary>
        /// Randomly generate a next target position for patrolling
        /// </summary>
        private void RandomNextTarget()
        {
            mStateTimer = kStateTimer;
            mCurrentState = PatrolState.PatrolState;
            mTintColor = kPatrolTint;
            // Generate a random begin state
            double initState = Game1.sRan.NextDouble();
            if (initState < 0.25)
                mTargetPosition = RandomBottomRightPosition();
            else if (initState < 0.5)
                mTargetPosition = RandomTopRightPosition();
            else if (initState < 0.75)
                mTargetPosition = RandomTopLeftPosition();
            else
                mTargetPosition = RandomBottomLeftPosition();

            ComputeNewSpeedAndResetTimer();
        }

        private void GenerateUpDownTarget()
        {
            mStateTimer = kStateTimer;
            mCurrentState = PatrolState.PatrolState;
            mTintColor = kPatrolTint;
            float posY;
            float distToTopOfScreen = Camera.CameraWindowUpperLeftPosition.Y - PositionY;
            float distToBottomOfScreen = PositionY - Camera.CameraWindowLowerLeftPosition.Y;
            if (distToTopOfScreen >= distToBottomOfScreen)
            {
                posY = (float)Game1.sRan.NextDouble() * distToTopOfScreen / 2 * 0.80f + PositionY + distToTopOfScreen / 2; 
            }
            else
            {
                posY = (float)Game1.sRan.NextDouble() * -distToBottomOfScreen / 2 * 0.80f + PositionY - distToBottomOfScreen / 2; 
            }

            mTargetPosition = new Vector2(PositionX, posY);
            ComputeNewSpeedAndResetTimer();
        }

        private void GenerateLeftRightTarget()
        {
            mStateTimer = kStateTimer;
            mCurrentState = PatrolState.PatrolState;
            mTintColor = kPatrolTint;

            float posX;
            if (Velocity.X <= 0)
            {
                posX = (float)Game1.sRan.NextDouble() * Camera.Width /2 + PositionX;
            }
            else
            {
                posX = (float)Game1.sRan.NextDouble() * -Camera.Width /2 + PositionX;
            }
 
            mTargetPosition = new Vector2(posX, PositionY);

            ComputeNewSpeedAndResetTimer();
        }

        #region compute random position in one of the 4 regions

        private const float sBorderRange = 0.55f;
        /// <summary>
        /// Random position based on offset.
        /// </summary>
        /// <param name="xOffset">0 gives a random position in the left, 0.5 gives a random position on the right</param>
        /// <param name="yOffset">0 gives a random positino in the bottom, 0.5 gives a random position on the top side</param>
        /// <returns></returns>
        private Vector2 ComputePoint(double xOffset, double yOffset)
        {
            Vector2 min = new Vector2(PositionX - Camera.Width/2, Camera.CameraWindowLowerLeftPosition.Y);
            Vector2 max = new Vector2(PositionX + Camera.Width / 2, Camera.CameraWindowUpperLeftPosition.Y);
            Vector2 size = max - min;
            float x = min.X + size.X * (float)(xOffset + (sBorderRange * Game1.sRan.NextDouble()));
            float y = min.Y + size.Y * (float)(yOffset + (sBorderRange * Game1.sRan.NextDouble()));
            return new Vector2(x, y);
        }

        const float kMinOffset = -0.05f;
        private Vector2 RandomBottomRightPosition()
        {
            return ComputePoint(0.5, kMinOffset);
        }

        private Vector2 RandomBottomLeftPosition()
        {
            return ComputePoint(kMinOffset, kMinOffset);
        }

        private Vector2 RandomTopRightPosition()
        {
            return ComputePoint(0.5, 0.5);
        }

        private Vector2 RandomTopLeftPosition()
        {
            return ComputePoint(kMinOffset, 0.5);
        }
        #endregion

        public Vector2 RandomPosition(bool offCamera)
        {
            Vector2 position;
            float posX = (float)Game1.sRan.NextDouble() * Camera.Width * 0.80f + Camera.Width * 0.10f;
            float posY = (float)Game1.sRan.NextDouble() * Camera.Height * 0.80f + Camera.Height * 0.10f;
            
            if(offCamera)
                posX += Camera.CameraWindowUpperRightPosition.X;

            position = new Vector2(posX, posY);
            return position;
        }
    }
}
