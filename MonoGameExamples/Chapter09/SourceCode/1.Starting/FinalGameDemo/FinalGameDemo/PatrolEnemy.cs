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
    public class PatrolEnemy : GameObject
    {
        protected enum PatrolState {
            PatrolState,
            ChaseHero
        }
        
        private Vector2 mTargetPosition;    // Target position we are moving towards
        private PatrolState mCurrentState;  // Current State

        // Constants  ...
        private const float kPatrolSpeed = 0.3f;
        private const float kCloseEnough = 5f;
        private const float kDistToBeginChase = 15f; // This is the distance to trigger patrol chasing of hero

        private const int kStateTimer = 60 * 5; // Assuming 60 updates per second, this is about 5 seconds
        private int mStateTimer;    // Interestingly, with "gradual" velocity changing, we cannot
                                    // guarantee that we will ever rich the mTargetPosition 
                                    // (we may ended up orbiting the target), so we set a timer
                                    // when timer is up, we transit
 
        /// <summary>
        /// Constructor
        /// </summary>
        public PatrolEnemy() :
            base("PatrolEnemy", Vector2.Zero, new Vector2(5f, 10f))
        {
            InitialFrontDirection = Vector2.UnitY;

            // Causes update state to always change into a new state
            mTargetPosition = Position = Vector2.Zero;

            RandomNextTarget();
            Position = mTargetPosition;
        }

        /// <summary>
        /// Updates states 
        /// </summary>        
        public bool UpdatePatrol(GameObject hero)
        {
            bool caught = false;

            // Perform operation common to all states ...
            base.Update();
            mStateTimer--;

            Vector2 toTarget = mTargetPosition - Position;
            float distToTarget = toTarget.Length();
            toTarget /= distToTarget; // This is the same as normalization
            ComputeNewDirection(toTarget);

            switch (mCurrentState)
            {
                case PatrolState.PatrolState:
                    UpdatePatrolState(hero, distToTarget);
                    break;

                case PatrolState.ChaseHero:
                    caught = UpdateChaseHeroState(hero, distToTarget);
                    break;
            }
            return caught;
        }

        #region Simple Patrol state Transition
        /// <summary>
        /// Update patrol state, if reach destination, get a random next traget
        /// if hero is close, transite into ChaseHero state
        /// </summary>
        /// <param name="hero">the hero to chase</param>
        /// <param name="distToTarget">distance to the current partrol target</param>
        private void UpdatePatrolState(GameObject hero, float distToTarget)
        {
            if ((mStateTimer < 0) || (distToTarget < kCloseEnough))
            {
                RandomNextTarget();
                ComputeNewSpeedAndResetTimer();
            }
            DetectHero(hero); // check if we should transit to ChaseHero
        }
        #endregion 

        #region Support Hero detection
        /// <summary>
        /// Check if Hero is close enough, if so, transit into ChaseHero State
        /// </summary>
        /// <param name="hero">The hero to check for</param>
        private void DetectHero(GameObject hero)
        {
            Vector2 toHero = hero.Position - Position;
            if (toHero.Length() < kDistToBeginChase)
            {
                mStateTimer = (int)(kStateTimer * 1.2f); // 1.2 times as much time for chasing
                Speed *= 2.5f;   // 2.5 times the current speed!
                mCurrentState = PatrolState.ChaseHero;
                mTargetPosition = hero.Position;
                mImage = Game1.sContent.Load<Texture2D>("AlertEnemy");
            }
        }

        /// <summary>
        /// Update chasing of hero state
        /// </summary>
        /// <param name="hero">The hero</param>
        /// <param name="distToHero">how far away is the hero</param>
        /// <returns></returns>
        private bool UpdateChaseHeroState(GameObject hero, float distToHero)
        {
            bool caught = false;
            Vector2 pos;
            caught = PixelTouches(hero, out pos);
            mTargetPosition = hero.Position;

            if (caught || (mStateTimer < 0))
            {
                RandomNextTarget();
                mImage = Game1.sContent.Load<Texture2D>("PatrolEnemy");
            }
            return caught;
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
        /// to gain "gradual turning effect"
        /// </summary>
        /// <param name="toTarget">vector towards the target</param>
        private void ComputeNewDirection(Vector2 toTarget)
        {
            // Figure out if we should continue to adjust our direction ...
            double cosTheta = Vector2.Dot(toTarget, FrontDirection);
            float theta = (float)Math.Acos(cosTheta);
            if (theta > float.Epsilon)
            {
                Vector3 frontDir3 = new Vector3(FrontDirection, 0f);
                Vector3 toTarget3 = new Vector3(toTarget, 0f);
                Vector3 zDir = Vector3.Cross(frontDir3, toTarget3);
                RotateAngleInRadian -= Math.Sign(zDir.Z) * 0.03f * theta; // Rotate 5% at a time towards final direction
                VelocityDirection = FrontDirection;
            }
        }

        /// <summary>
        /// Randomly generate a next target position for patrolling
        /// </summary>
        private void RandomNextTarget()
        {
            mStateTimer = kStateTimer;
            mCurrentState = PatrolState.PatrolState;
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

        #region Compute random position in one of the 4 regions

        private const float sBorderRange = 0.45f;

        /// <summary>
        /// Random position based on offset.
        /// </summary>
        /// <param name="xOffset">0 gives a random position in the left, 0.5 gives a random position on the right</param>
        /// <param name="yOffset">0 gives a random positino in the bottom, 0.5 gives a random position on the top side</param>
        /// <returns></returns>
        private Vector2 ComputePoint(double xOffset, double yOffset)
        {
            Vector2 min = Camera.CameraWindowLowerLeftPosition;
            Vector2 max = Camera.CameraWindowUpperRightPosition;
            Vector2 size = max - min;
            float x = min.X + size.X * (float)(xOffset + (sBorderRange * Game1.sRan.NextDouble()));
            float y = min.Y + size.Y * (float)(yOffset + (sBorderRange * Game1.sRan.NextDouble()));
            return new Vector2(x, y);
        }

        private Vector2 RandomBottomRightPosition()
        {
            return ComputePoint(0.5, 0.0);
        }

        private Vector2 RandomBottomLeftPosition()
        {
            return ComputePoint(0.0, 0.0);
        }

        private Vector2 RandomTopRightPosition()
        {
            return ComputePoint(0.5, 0.5);
        }

        private Vector2 RandomTopLeftPosition()
        {
            return ComputePoint(0.0, 0.5);
        }
        #endregion 
    }
}
