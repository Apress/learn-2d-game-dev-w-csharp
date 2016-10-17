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
    public class SpritePrimitive : GameObject
    {
        private int mNumRow, mNumColumn, mPaddings;
            // Dimension of the sprite sheet:
            //     NumRow x NumColumn 
            // number of images: mPaddings between images
        private int mSpriteImageWidth, mSpriteImageHeight;  
            // dimension of each image

        #region Per Animation setting
        private int mUserSpecifedTicks; 
            // number of ticks before changing to the next image
        
        private int mCurrentTick;
            // number of ticks since displaying current displayed image
        
        private int mCurrentRow, mCurrentColumn; 
            // row/col of current displayed image
        
        private int mBeginRow, mEndRow;     // Begin/end row/column for current animation
        private int mBeginCol, mEndCol;
            // does not check for Begin > End!!
        #endregion 


        /// <summary>
        /// Constructor:
        /// </summary>
        /// <param name="image">The sprite page</param>
        /// <param name="position">Center location of this primitive</param>
        /// <param name="size">Size of this primitive</param>
        /// <param name="rowCounts">numRows in the sprite page</param>
        /// <param name="columnCount">numColumns in the sprite page</param>
        /// <param name="padding">num pixel paddings (if any) between images</param>
        public SpritePrimitive(String image, Vector2 position, Vector2 size, int rowCounts, int columnCount, int padding) : 
            base(image, position, size)
        {
            mNumRow = rowCounts;
            mNumColumn = columnCount;
            mPaddings = padding;

            mSpriteImageWidth = mImage.Width / mNumRow;
            mSpriteImageHeight = mImage.Height / mNumColumn;

            // initialize per animation settings to showing top/left image constantly
            mUserSpecifedTicks = 1;
            mCurrentTick = 0;
            mCurrentRow = 0;
            mCurrentColumn = 0;
            mBeginRow = mBeginCol = mEndRow = mEndCol = 0;
        }


        public int SpriteCurrentRow { get { return mCurrentRow; } set { mCurrentRow = value; } }
        public int SpriteCurrentColumn { get { return mCurrentColumn; } set { mCurrentColumn = value; } }

        public int SpriteBeginRow { get { return mBeginRow; } set { mBeginRow = value; mCurrentRow = value; } }
        public int SpriteEndRow { get { return mEndRow; } set { mEndRow = value; } }
        public int SpriteBeginColumn { get { return mBeginCol; } set { mBeginCol = value; mCurrentColumn = value; } }
        public int SpriteEndColumn { get { return mEndCol; } set { mEndCol = value; } }
        public int SpriteAnimationTicks { get { return mUserSpecifedTicks; } set { mUserSpecifedTicks = value; } }

        public void SetSpriteAnimation(int beginRow, int beginCol, int endRow, int endCol, int tickInterval)
        {
            mUserSpecifedTicks = tickInterval;
            mBeginRow = beginRow;
            mBeginCol = beginCol;
            mEndRow = endRow;
            mEndCol = endCol;

            // initialize animation
            mCurrentRow = mBeginRow;
            mCurrentColumn = mBeginCol;
            mCurrentTick = 0;
        }

        public override void Update()
        {
            base.Update();

            // now update the sprite state
            mCurrentTick++;
            if (mCurrentTick > mUserSpecifedTicks)
            {
                // advance image
                mCurrentTick = 0;

                mCurrentColumn++;
                if (mCurrentColumn > mEndCol)
                {
                    mCurrentColumn = mBeginCol;
                    //mCurrentRow++;

                    //if (mCurrentRow > mEndRow)
                    //    // end of the sequence, re-start
                    //    mCurrentRow = mBeginRow;
                }
            }
        }

        public override void Draw()
        {
            // Defines where and size of the texture to show
            Rectangle destRect = Camera.ComputePixelRectangle(Position, Size);

            int imageTop = mCurrentRow * mSpriteImageWidth;
            int imageLeft = mCurrentColumn * mSpriteImageHeight;

            // define the rotation origin
            Vector2 org = new Vector2(mSpriteImageWidth/2, mSpriteImageHeight/2);

            // define the area to be drawn
            Rectangle srcRect = new Rectangle(
                        imageLeft + mPaddings,
                        imageTop + mPaddings,
                        mSpriteImageWidth, mSpriteImageHeight);
                
            // Draw the texture
            Game1.sSpriteBatch.Draw(mImage,
                            destRect,           // Area to be drawn in pixel space
                            srcRect,            // <<-- rect on the spriteSheet
                            mTintColor,        // 
                            mRotateAngle,       // Angle to roate (clockwise)
                            org,                // Image reference position
                            SpriteEffects.None, 0f);
            

            if (null != Label)
                FontSupport.PrintStatusAt(Position, Label, LabelColor);
        }

        #region override to support per-pixel collision
        protected override int SpriteTopPixel { get { return mCurrentRow * mSpriteImageHeight; } }
        protected override int SpriteLeftPixel { get { return mCurrentColumn * mSpriteImageWidth; } }
        protected override int SpriteImageWidth { get { return mSpriteImageWidth; } }
        protected override int SpriteImageHeight { get { return mSpriteImageHeight; } }
        #endregion 
    }
}
