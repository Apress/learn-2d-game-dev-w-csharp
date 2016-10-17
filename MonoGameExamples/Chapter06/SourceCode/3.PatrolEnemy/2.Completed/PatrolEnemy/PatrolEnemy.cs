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

        private const float kPatrolSpeed = 0.3f;
        private const float kCloseEnough = 1f;

        /// <summary>
        /// Constructor
        /// </summary>
        public PatrolEnemy() :
            base("PatrolEnemy", Vector2.Zero, new Vector2(5f, 10f))
        {
            InitialFrontDirection = Vector2.UnitY;

            // Causes update state to always change into a new state
            mTargetPosition = Position = Vector2.Zero;

            #region  Generate a random state to begin
            double initState = Game1.sRan.NextDouble();
            if (initState < 0.25)
            {
                UpdateBottomLeftState();
            }
            else if (initState < 0.5)
            {
                UpdateBottomRightState();
            }
            else if (initState < 0.75)
            {
                UpdateTopLeftState();
            }
            else
            {
                UpdateTopRightState();
            }
            Position = mTargetPosition;
            #endregion
        }

        /// <summary>
        /// Updates states 
        /// </summary>
        public void UpdatePatrol()
        {
            // Operation common to all states ...
            base.Update(); // Moves game object by velocity

            switch (mCurrentState)
            {
                case PatrolState.BottomLeftRegion:
                    UpdateBottomLeftState();
                    break;

                case PatrolState.BottomRightRegion:
                    UpdateBottomRightState();
                    break;

                case PatrolState.TopRightRegion:
                    UpdateTopRightState();
                    break;

                case PatrolState.TopLeftRegion:
                    UpdateTopLeftState();
                    break;
            }
        }

        /// <summary>
        /// When gets close enough, transit to bottom right
        /// </summary>
        private void UpdateBottomLeftState()
        {
            if ((Position - mTargetPosition).LengthSquared() < kCloseEnough)
            {
                mCurrentState = PatrolState.TopLeftRegion;
                mTargetPosition = RandomBottomRightPosition();
                ComputePositionAndVelocity();
            }
        }

        /// <summary>
        ///  when gets close enough, transit to top right
        /// </summary>
        private void UpdateBottomRightState()
        {
            if ((Position - mTargetPosition).LengthSquared() < kCloseEnough)
            {
                mCurrentState = PatrolState.BottomLeftRegion;
                mTargetPosition = RandomTopRightPosition();
                ComputePositionAndVelocity();
            }
        }

        /// <summary>
        /// when gets clsoe enough, transit to top left
        /// </summary>
        private void UpdateTopRightState()
        {
            if ((Position - mTargetPosition).LengthSquared() < kCloseEnough)
            {
                mCurrentState = PatrolState.BottomRightRegion;
                mTargetPosition = RandomTopLeftPosition();
                ComputePositionAndVelocity();
            }
        }

        /// <summary>
        /// When gets close enough, transit to top left
        /// </summary>
        private void UpdateTopLeftState()
        {
            if ((Position - mTargetPosition).LengthSquared() < kCloseEnough)
            {
                mCurrentState = PatrolState.TopRightRegion;
                mTargetPosition = RandomBottomLeftPosition();
                ComputePositionAndVelocity();
            }
        }

        /// <summary>
        /// Compute velocity based on target
        /// </summary>
        private void ComputePositionAndVelocity()
        {
            // Speed: ranges between 80% to 120
            Speed = kPatrolSpeed * (0.8f + (float)(0.4 * Game1.sRan.NextDouble()));
            // This is the vector from Center to the next Position
            Vector2 toNextPosition = mTargetPosition - Position;
            VelocityDirection = toNextPosition;
            FrontDirection = VelocityDirection;
        }

        #region compute random position in one of the 4 regions

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
