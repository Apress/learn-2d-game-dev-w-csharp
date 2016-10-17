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
    /// ShowVector class: shows a 2D vector
    /// </summary>
    public class ShowVector
    {
        // Support for drawing the image
        protected static Texture2D sImage = null;   // Singleton for the class
        private static float kLenToWidthRatio = 0.2f;
        
        
        /// <summary>
        /// Load the Arrow image if not already loaded
        /// </summary>
        static private void LoadImage()
        {
            if (null == sImage)
                ShowVector.sImage = Game1.sContent.Load<Texture2D>("RightArrow");
        }

        /// <summary>
        /// Draws the arrow image extending from "from" along the "dir" direction
        /// </summary>
        /// <param name="from">beginning position of the vector</param>
        /// <param name="dir">the direction of the vector</param>
        static public void DrawPointVector(Vector2 from, Vector2 dir)
        {
            LoadImage();

            float length = dir.Length();

            float theta = 0f;
            // compute the angle to rotate ...
            if (length > 0.001f)
            {
                dir /= length;
                theta = (float)Math.Acos((double)dir.X);
                if (dir.X < 0.0f)
                {
                    if (dir.Y > 0.0f)
                        theta = -theta;
                }
                else
                {
                    if (dir.Y > 0.0f)
                        theta= -theta;
                }
            }
            
            // Defines where and size of the texture to show
            Vector2 size = new Vector2(length, kLenToWidthRatio * length);
            Rectangle destRect = Camera.ComputePixelRectangle(from, size);
            
            // destRect is computed with respect to the "from" position, on the left-side of the texture
            // we only need to offse the reference in the y from top-left to middle-left
            Vector2 org = new Vector2(0f, ShowVector.sImage.Height/2f);

            Game1.sSpriteBatch.Draw(ShowVector.sImage, destRect, null, Color.White, 
                            theta, org, SpriteEffects.None, 0f);

            String msg;
            msg = "Direction=" + dir + "\nSize=" + length;
            FontSupport.PrintStatusAt(from+(0.5f*length*dir), msg, Color.Black);
        }


        /// <summary>
        /// Drawing the vector between the two input positions
        /// </summary>
        /// <param name="from">begining position for the vector</param>
        /// <param name="to">ending position of the vector</param>
        static public void DrawFromTo(Vector2 from, Vector2 to)
        {
            DrawPointVector(from, to - from);
        }

        /// <summary>
        /// Rotate the vector-v by the the specified Radians in the clockwise direction
        /// and return the result
        /// </summary>
        /// <param name="v">Vector to be rotated. </param>
        /// <param name="angleInRadian"></param>
        /// <returns></returns>
        static public Vector2 RotateVectorByAngle(Vector2 v, float angleInRadian)
        {
            float sinTheta = (float)(Math.Sin((double)angleInRadian));
            float cosTheta = (float)(Math.Cos((double)angleInRadian));
            float x, y;
            x =  cosTheta * v.X + sinTheta * v.Y;
            y = -sinTheta * v.X + cosTheta * v.Y;
            return new Vector2(x, y);
        }
    }
}
