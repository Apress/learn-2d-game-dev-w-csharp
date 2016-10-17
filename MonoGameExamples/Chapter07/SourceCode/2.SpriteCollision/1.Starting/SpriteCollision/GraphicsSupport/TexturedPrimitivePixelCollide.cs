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
    /// TexturedPrimitive class
    /// </summary>
    public partial class TexturedPrimitive
    {
        private Color[] mTextureColor = null;

        private void ReadColorData()
        {
            mTextureColor = new Color[mImage.Width * mImage.Height];
            mImage.GetData(mTextureColor);
        }

        private Color GetColor(int i, int j)
        {
            return mTextureColor[(j * mImage.Width) + i];
        }

        public bool PixelTouches(TexturedPrimitive otherPrim, out Vector2 collidePoint)
        {
            bool touches = PrimitivesTouches(otherPrim);
            collidePoint = Position;
            
            if (touches)
            {
                bool pixelTouch = false;

                #region Step 3a.
                Vector2 myXDir = ShowVector.RotateVectorByAngle(Vector2.UnitX, RotateAngleInRadian);
                Vector2 myYDir = ShowVector.RotateVectorByAngle(Vector2.UnitY, RotateAngleInRadian);
                Vector2 otherXDir = ShowVector.RotateVectorByAngle(Vector2.UnitX, otherPrim.RotateAngleInRadian);
                Vector2 otherYDir = ShowVector.RotateVectorByAngle(Vector2.UnitY, otherPrim.RotateAngleInRadian);
                #endregion

                #region Step 3b.
                int i=0;
                while ( (!pixelTouch) && (i<mImage.Width) )  
                {
                    int j = 0;
                    while ( (!pixelTouch) && (j<mImage.Height) ) 
                    {
                        collidePoint = IndexToCameraPosition(i, j, myXDir, myYDir);
                        Color myColor = GetColor(i, j);
                        if (myColor.A > 0)  
                        {
                            Vector2 otherIndex = otherPrim.CameraPositionToIndex(collidePoint, otherXDir, otherYDir);
                            int xMin = (int)otherIndex.X;
                            int yMin = (int)otherIndex.Y;
                            if ((xMin >= 0) && (xMin < otherPrim.mImage.Width) &&
                                 (yMin >= 0) && (yMin < otherPrim.mImage.Height)) 
                            {
                                pixelTouch = (otherPrim.GetColor(xMin, yMin).A > 0);
                            }
                        }
                        j++;
                    }
                    i++;
                }
                #endregion
                touches = pixelTouch;
            }
            return touches;
        }

        private Vector2 IndexToCameraPosition(int i, int j, Vector2 xDir, Vector2 yDir)
        {
            float x = i * Width / (float)(mImage.Width - 1);
            float y = j * Height / (float)(mImage.Height - 1);

            Vector2 r = Position 
                 + (x - (mSize.X * 0.5f)) * xDir 
                 - (y - (mSize.Y * 0.5f)) * yDir;

            return r;
        }

        private Vector2 CameraPositionToIndex(Vector2 p, Vector2 xDir, Vector2 yDir)
        {
            Vector2 delta = p - Position;
            float xOffset = Vector2.Dot(delta, xDir);
            float yOffset = Vector2.Dot(delta, yDir);
            float i = mImage.Width * (xOffset / Width);
            float j = mImage.Height * (yOffset / Height);
            i += mImage.Width / 2;
            j = (mImage.Height / 2) - j;
            return new Vector2(i, j);
        }
    }
}
