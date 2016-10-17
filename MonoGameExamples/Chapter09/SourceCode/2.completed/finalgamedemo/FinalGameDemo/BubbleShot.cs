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
    /// BubbleShot class: GameObject with behavior
    /// </summary>
    public class BubbleShot : GameObject
    {
        private const float kBubbleShotWidth = 7f;
        private const float kBubbleShotHeight = 7f;
        private const float kBubbleShotSpeed = 1.8f;  

        /// <summary>
        /// Constructor of GameObject
        /// </summary>
        public BubbleShot(Vector2 position, int facing)
            : base("BUBBLE_1", position, new Vector2(kBubbleShotWidth, kBubbleShotHeight), null)
        {
            Speed = kBubbleShotSpeed;
            mVelocityDir = new Vector2(facing, 0);
        }

        public bool IsOnScreen()
        {
            // take advantage of the camera window bound check
            Camera.CameraWindowCollisionStatus status = Camera.CollidedWithCameraWindow(this);
            return (Camera.CameraWindowCollisionStatus.InsideWindow == status);
        }

    }
}
