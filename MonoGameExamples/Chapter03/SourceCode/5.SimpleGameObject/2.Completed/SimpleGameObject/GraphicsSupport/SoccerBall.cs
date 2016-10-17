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
    /// This is the main type for your game
    /// </summary>
    public class SoccerBall : TexturedPrimitive
    {
        private Vector2 mDeltaPosition; // change current position by this amount

        /// <summary>
        /// Constructor of SoccerBall
        /// </summary>
        /// <param name="position">center position of the ball</param>
        /// <param name="diameter">diameter of the ball</param>
        public SoccerBall(Vector2 position, float diameter) : 
            base("Soccer", position, new Vector2(diameter, diameter))
        {
            mDeltaPosition.X = (float) (Game1.sRan.NextDouble()) * 2f - 1f;
            mDeltaPosition.Y = (float) (Game1.sRan.NextDouble()) * 2f - 1f;
        }

        // Accessors
        public float Radius 
        { 
            get { return mSize.X * 0.5f; } 
            set { mSize.X = 2f * value; mSize.Y = mSize.X;} 
        }

        /// <summary>
        /// Compute the soccer's movement in the camera window
        /// </summary>
        public void Update()
        {
            Camera.CameraWindowCollisionStatus status = Camera.CollidedWithCameraWindow(this);
            switch (status) {
                case Camera.CameraWindowCollisionStatus.CollideBottom:
                case Camera.CameraWindowCollisionStatus.CollideTop:
                    mDeltaPosition.Y *= -1;
                    break;
                case Camera.CameraWindowCollisionStatus.CollideLeft:
                case Camera.CameraWindowCollisionStatus.CollideRight:
                    mDeltaPosition.X *= -1;
                    break;
            }
            Position += mDeltaPosition;
        }
    }
}
