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
    /// GameObject class: TexturedPrimitive with behavior
    /// </summary>
    public class GameObject : TexturedPrimitive
    {
        // Initial front direction (when RotateAngle is 0)
        protected Vector2 mInitFrontDir = -Vector2.UnitX;

        // GameObject behavior: velocity
        protected Vector2 mVelocityDir; // If not zero, always normalized
        protected float mSpeed;

        /// <summary>
        /// Initialize the game object
        /// </summary>
        protected void InitGameObject() 
        {
            mVelocityDir = Vector2.Zero;
            mSpeed = 0f;
        }

        /// <summary>
        /// Constructor of GameObject
        /// </summary>
        /// <param name="imageName">name of the image to be loaded as texture.</param>
        /// <param name="position">center position of the texture to be drawn</param>
        /// <param name="size">width/height of the texture to be drawn</param>
        public GameObject(String imageName, Vector2 position, Vector2 size, String label = null) 
                : base(imageName, position, size, label)
        {
            InitGameObject();
        }

        /// <summary>
        /// Update the game object, by default, move by velocity
        /// </summary>
        virtual public void Update()
        {
            mPosition += (mVelocityDir * mSpeed);
        }

        /// <summary>
        /// Get/Set the Initial FrontDirection of the GameObject
        /// </summary>
        public Vector2 InitialFrontDirection
        {
            get { return mInitFrontDir; }
            set
            {
                float len = value.Length();
                if (len > float.Epsilon) // If the input vector is well defined
                    mInitFrontDir = value / len;
                else
                    mInitFrontDir = Vector2.UnitY;
            }
        }

        /// <summary>
        /// Get/Set the current front direction based on the
        /// rotation angle on the TexturedPrimitive with respect
        /// to the InitialFrontDirection
        /// </summary>
        public Vector2 FrontDirection {
            get
            {
                return ShowVector.RotateVectorByAngle(mInitFrontDir, RotateAngleInRadian);
            }
            set
            {
                float len = value.Length();
                if (len > float.Epsilon)
                {
                    value *= (1f / len);
                    double theta = Math.Atan2(value.Y, value.X);
                    mRotateAngle = -(float)(theta - Math.Atan2(mInitFrontDir.Y, mInitFrontDir.X));
                }
            }
        }

        /// <summary>
        /// Get/Sets the velocity of the game object
        /// </summary>
        public Vector2 Velocity
        {
            get { return mVelocityDir * Speed; }
            set
            {
                mSpeed = value.Length();
                if (mSpeed > float.Epsilon)
                    mVelocityDir = value/mSpeed;
                else
                    mVelocityDir = Vector2.Zero;
            }
        }

        /// <summary>
        /// Get/Sets the speed
        /// </summary>
        public float Speed { 
            get { return mSpeed; }
            set { mSpeed = value; }
        }

        /// <summary>
        /// Get/Sets the Direction of the velocity.
        /// Ensures vector is normalized
        /// </summary>
        public Vector2 VelocityDirection
        {
            get { return mVelocityDir; }
            set
            {
                float s = value.Length();
                if (s > float.Epsilon)
                {
                    mVelocityDir = value / s;
                }
                else
                    mVelocityDir = Vector2.Zero;
            }
        }

        /// <summary>
        /// Returns if object is visible in the camera window
        /// </summary>
        /// <returns></returns>
        public bool ObjectVisibleInCameraWindow()
        {
            Camera.CameraWindowCollisionStatus status = Camera.CollidedWithCameraWindow(this);
            return (status == Camera.CameraWindowCollisionStatus.InsideWindow);
        }
    }
}
