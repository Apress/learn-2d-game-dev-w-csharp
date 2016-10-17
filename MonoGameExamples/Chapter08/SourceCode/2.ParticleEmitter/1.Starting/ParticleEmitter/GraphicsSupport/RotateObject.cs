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
    /// Rotate object
    /// </summary>
    public class RotateObject : GameObject
    {
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="image">Image to use for this object</param>
        /// <param name="center">center position of the texture to be drawn</param>
        /// <param name="radius">radius of the rotating object</param>
        public RotateObject(String image, Vector2 center, float radius) 
                : base(image, center, new Vector2(radius*2f, radius*2f))
        {
        }

        public float Radius { get { return mSize.X / 2f; } set { mSize.X = 2f * value; mSize.Y = mSize.X; } } 

        override public void Update()
        {
            base.Update(); // moves object by velocity
            
            Vector2 v = Velocity;
            v.Y -= Game1.sGravity;
            Velocity = v;

            // now rotate the object according to the speed in x-direction
            float angularDisplace = (v.X / Radius);
            // this assumes object is rolling "on-top of" surfaces
            if (v.X > 0)
                mRotateAngle += angularDisplace;
            else
                mRotateAngle -= angularDisplace;
        }
    }
}
