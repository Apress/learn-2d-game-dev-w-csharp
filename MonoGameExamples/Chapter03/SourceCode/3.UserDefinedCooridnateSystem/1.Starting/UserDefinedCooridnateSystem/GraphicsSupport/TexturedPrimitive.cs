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
        /// <param name="position">top left pixel position of the texture to be drawn</param>
        /// <param name="size">width/height of the texture to be drawn</param>
        public TexturedPrimitive(String imageName, Vector2 position, Vector2 size)
        {
            mImage = Game1.sContent.Load<Texture2D>(imageName);
            mPosition = position;
            mSize = size;
        }

        /// <summary>
        /// Allows the primitive object to update its state
        /// </summary>
        /// <param name="deltaTranslate">Amount to change the position of the primitive. 
        ///                              Value of 0 says position is not changed.</param>
        /// <param name="deltaScale">Amount to change of the scale the primitive. 
        ///                          Value of 0 says size is not changed.</param>
        public void Update(Vector2 deltaTranslate, Vector2 deltaScale)
        {
            mPosition += deltaTranslate;
            mSize += deltaScale;
        }

        /// <summary>
        /// Draw the primitive
        /// </summary>
        public void Draw()
        {
            // Defines location and size of the texture

        }
    }
}
