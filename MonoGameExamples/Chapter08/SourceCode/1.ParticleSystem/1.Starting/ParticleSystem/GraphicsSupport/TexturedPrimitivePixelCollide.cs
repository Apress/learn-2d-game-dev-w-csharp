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

        #region Static support for sharing color data across same image
        static Dictionary<String, Color[]> sTextureData = 
                new Dictionary<string, Color[]>();

        static private Color[] LoadColorInfo(String imageName, Texture2D image)
        {
            Color[] imageData = new Color[image.Width * image.Height];
            image.GetData(imageData);
            sTextureData.Add(imageName, imageData);
            return imageData;
        }
        #endregion

        private void ReadColorData()
        {
            if (sTextureData.ContainsKey(mImageName))
                mTextureColor = sTextureData[mImageName];
            else
                mTextureColor = LoadColorInfo(mImageName, mImage);
        }

        private Color GetColor(int i, int j)
        {
            return mTextureColor[((j+SpriteTopPixel) * mImage.Width) + 
                                   i + SpriteLeftPixel];
        }

        public bool PixelTouches(TexturedPrimitive otherPrim, out Vector2 collidePoint)
        {
            bool touches = PrimitivesTouches(otherPrim);
            collidePoint = Position;

            if (touches)
            {
                bool pixelTouch = false;

                Vector2 myXDir = ShowVector.RotateVectorByAngle(Vector2.UnitX, RotateAngleInRadian);
                Vector2 myYDir = ShowVector.RotateVectorByAngle(Vector2.UnitY, RotateAngleInRadian);

                Vector2 otherXDir = ShowVector.RotateVectorByAngle(Vector2.UnitX, otherPrim.RotateAngleInRadian);
                Vector2 otherYDir = ShowVector.RotateVectorByAngle(Vector2.UnitY, otherPrim.RotateAngleInRadian);

                int i=0;
                while ( (!pixelTouch) && (i<SpriteImageWidth) ) 
                {
                    int j = 0;
                    while ( (!pixelTouch) && (j<SpriteImageHeight) )
                    {
                        collidePoint = IndexToCameraPosition(i, j, myXDir, myYDir);

                        Color myColor = GetColor(i, j);
                        if (myColor.A > 0)
                        {
                            Vector2 otherIndex = otherPrim.CameraPositionToIndex(collidePoint, otherXDir, otherYDir);
                            int xMin = (int)otherIndex.X;
                            int yMin = (int)otherIndex.Y;

                            if ((xMin >= 0) && (xMin < otherPrim.SpriteImageWidth) &&
                                (yMin >= 0) && (yMin < otherPrim.SpriteImageHeight))
                            {
                                pixelTouch = (otherPrim.GetColor(xMin, yMin).A > 0);
                            }
                        }
                        j++;
                    }
                    i++;
                }

                touches = pixelTouch;
            }
            return touches;
        }

        private Vector2 IndexToCameraPosition(int i, int j, Vector2 xDir, Vector2 yDir)
        {
            float x = i * Width / (float)(SpriteImageWidth - 1);
            float y = j * Height / (float)(SpriteImageHeight- 1);

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
            float i = SpriteImageWidth * (xOffset / Width);
            float j = SpriteImageHeight * (yOffset / Height);
            i += SpriteImageWidth / 2;
            j = (SpriteImageHeight / 2) - j;
            return new Vector2(i, j);
        }
    }
}
