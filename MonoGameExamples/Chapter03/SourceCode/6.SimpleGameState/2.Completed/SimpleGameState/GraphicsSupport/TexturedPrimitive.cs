using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BookExample
{
    /// <summary>
    /// TexturedPrimitive class
    /// </summary>
    public class TexturedPrimitive
    {
        // Support for drawing the image
        protected Texture2D mImage;     // The UWB-JPG.jpg image to be loaded
        protected Vector2 mPosition;    // Center position of image
        protected Vector2 mSize;        // Size of the image to be drawn

        /// <summary>
        /// Constructor of TexturePrimitive
        /// </summary>
        /// <param name="imageName">name of the image to be loaded as texture.</param>
        /// <param name="position">center position of the texture to be drawn</param>
        /// <param name="size">width/height of the texture to be drawn</param>
        public TexturedPrimitive(String imageName, Vector2 position, Vector2 size)
        {
            mImage = Game1.sContent.Load<Texture2D>(imageName);
            mPosition = position;
            mSize = size;
        }

        public TexturedPrimitive(String imageName)
        {
            mImage = Game1.sContent.Load<Texture2D>(imageName);
            mPosition = Vector2.Zero;
            mSize = Vector2.UnitX;
        }

        // Accessors
        public Vector2 Position { get { return mPosition; } set { mPosition = value; } }
        public float PositionX { get { return mPosition.X; } set { mPosition.X = value; } }
        public float PositionY { get { return mPosition.Y; } set { mPosition.Y = value; } }
        public Vector2 Size { get { return mSize; } set { mSize = value; } }
        public float Width { get { return mSize.X; } set { mSize.X = value; } }
        public float Height { get { return mSize.Y; } set { mSize.Y = value; } }
        public Vector2 MinBound { get { return mPosition - (0.5f * mSize); } }
        public Vector2 MaxBound { get { return mPosition + (0.5f * mSize); } }

        /// <summary>
        /// Allows the primitive object to update its state
        /// </summary>
        /// <param name="deltaTranslate">Amount to change the position of the primitive. 
        ///                              Value of 0 says position is not changed.</param>
        public void Update(Vector2 deltaTranslate)
        {
            mPosition += deltaTranslate;
        }

        public bool PrimitivesTouches(TexturedPrimitive otherPrim)
        {
            Vector2 v = mPosition - otherPrim.Position;
            float dist = v.Length();
            return (dist < ((mSize.X / 2f) + (otherPrim.mSize.X / 2f)));
        }

        /// <summary>
        /// Draw the primitive
        /// </summary>
        public void Draw()
        {
            // Defines location and size of the texture
            Rectangle destRect = Camera.ComputePixelRectangle(Position, Size);
            Game1.sSpriteBatch.Draw(mImage, destRect, Color.White);
        }
    }
}
