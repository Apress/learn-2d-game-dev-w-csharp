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
            TopLeftRegion,         
            TopRightRegion,
            BottomLeftRegion,
            BottomRightRegion
        }

        private Vector2 mTargetPosition;    // Target position we are moving towards
        private PatrolState mCurrentState;  // Current State
        
        // Constants  ...
        private const float kPatrolSpeed = 0.3f;
        private const float kCloseEnough = 5f;
        
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
            mStateTimer = kStateTimer;

            #region  Generate a random state to begin
            double initState = Game1.sRan.NextDouble();
            if (initState < 0.25)
            {
                UpdateBottomLeftState(0f);
            }
            else if (initState < 0.5)
            {
                UpdateBottomRightState(0f);
            }
            else if (initState < 0.75)
            {
                UpdateTopLeftState(0f);
            }
            else
            {
                UpdateTopRightState(0f);
            }
            Position = mTargetPosition;
            #endregion
        }

        /// <summary>
        /// Updates states 
        /// </summary>
        public void UpdatePatrol()
        {
            // Perform operation common to all states ...
            base.Update();
            mStateTimer--;

            Vector2 toTarget = mTargetPosition - Position;
            float distToTarget = toTarget.Length();
            toTarget /= distToTarget; // This is the same as normalization
            ComputeNewDirection(toTarget);

            switch (mCurrentState)
            {
                case PatrolState.BottomLeftRegion:
                    UpdateBottomLeftState(distToTarget);
                    break;

                case PatrolState.BottomRightRegion:
                    UpdateBottomRightState(distToTarget);
                    break;

                case PatrolState.TopRightRegion:
                    UpdateTopRightState(distToTarget);
                    break;

                case PatrolState.TopLeftRegion:
                    UpdateTopLeftState(distToTarget);
                    break;
            }
        }

        /// <summary>
        /// When gets close enough, transit to bottom right
        /// </summary>        
        private void UpdateBottomLeftState(float distToTarget)
        {
            if ((mStateTimer < 0) || (distToTarget < kCloseEnough))
            {
                mCurrentState = PatrolState.BottomRightRegion;
                mTargetPosition = RandomBottomRightPosition();
                ComputeNewSpeedAndResetTimer();
            }
        }

        /// <summary>
        /// When gets close enough, transit to top right
        /// </summary>        
        private void UpdateBottomRightState(float distToTarget)
        {
            if ((mStateTimer < 0) || (distToTarget < kCloseEnough))
            {
                mCurrentState = PatrolState.TopRightRegion;
                mTargetPosition = RandomTopRightPosition();
                ComputeNewSpeedAndResetTimer();
            }
        }

        /// <summary>
        /// When gets close enough, transit to top left
        /// </summary>
        private void UpdateTopRightState(float distToTarget)
        {
            if ((mStateTimer < 0) || (distToTarget < kCloseEnough))
            {
                mCurrentState = PatrolState.TopLeftRegion;
                mTargetPosition = RandomTopLeftPosition();
                ComputeNewSpeedAndResetTimer();
            }
        }


        /// <summary>
        /// When gets close enough, transit to top left
        /// </summary>
        private void UpdateTopLeftState(float distToTarget)
        {
            if ((mStateTimer < 0) || (distToTarget < kCloseEnough))
            {
                mCurrentState = PatrolState.BottomLeftRegion;
                mTargetPosition = RandomBottomLeftPosition();
                ComputeNewSpeedAndResetTimer();
            }
        }

        /// <summary>
        /// Compute speed and reset state timer
        /// </summary>
        private void ComputeNewSpeedAndResetTimer()
        {
            Speed = kPatrolSpeed * (0.8f + (float)(0.4 * Game1.sRan.NextDouble())); // Speed: ranges between 80% to 120
            mStateTimer = (int) (kStateTimer * (0.8f + (float)(0.6 * Game1.sRan.NextDouble())));
        }

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
                RotateAngleInRadian -= Math.Sign(zDir.Z) * 0.03f * theta; // Rotate 3% at a time towards final direction
                VelocityDirection = FrontDirection;
            }
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
