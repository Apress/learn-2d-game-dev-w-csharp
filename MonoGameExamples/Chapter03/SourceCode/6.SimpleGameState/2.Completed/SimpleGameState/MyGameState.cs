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
    /// This is is the GameState, all game variables are stored here
    /// </summary>
    public class MyGame
    {
        // Hero stuff ...
        TexturedPrimitive mHero;
        Vector2 kHeroSize = new Vector2(15, 15);
        Vector2 kHeroPosition = Vector2.Zero;

        // Basketballs ...
        List<BasketBall> mBBallList;
        TimeSpan mCreationTimeStamp;
        int mTotalBBallCreated = 0;
        const int kBballMSecInterval = 500; // this is 0.5 seconds

        // Game state
        int mScore = 0;
        int mBBallMissed=0, mBBallHit=0;
        const int kBballTouchScore = 1;
        const int kBballMissedScore = -2;
        const int kWinScore = 10;
        const int kLossScore = -10;
        TexturedPrimitive mFinal = null;

        /// <summary>
        /// Constructor of game state, allocate memory and initialize 
        /// </summary>
        public MyGame()
        {
            // hero ...
            mHero = new TexturedPrimitive("Me", kHeroPosition, kHeroSize);

            // Basketballs
            mCreationTimeStamp = new TimeSpan(0);
            mBBallList = new List<BasketBall>();
        }

        /// <summary>
        /// Update game sate:
        ///     1. Tell all Gameobjects to update themselves
        ///     2. Cause (call) all intractable objects to interact
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateGame(GameTime gameTime)
        {
            #region Step a.
            if (null != mFinal) // done!!
                return;
            #endregion

            #region Step b. go through all game object and tell each to update themselves
            // hero movement: right thumb stick
            mHero.Update(InputWrapper.ThumbSticks.Right);

            // Basketball ...
            for (int b = mBBallList.Count-1; b >= 0; b--) 
            {
                if (mBBallList[b].UpdateAndExplode()) 
                {
                    mBBallList.RemoveAt(b);
                    mBBallMissed++;
                    mScore += kBballMissedScore;
                }
            }
            #endregion

            #region Step c. Call GameObject interaction functions to cause interaction between game objects
            /// Notice we could have integrated the following loop into the above for-loop.
            /// In this implementation we separated out the following loop to highlight the 
            /// fact that, Hero->Ball interaction is inter-gameObject interaction and
            /// this happened separately from individual updates of game objects.
            for (int b = mBBallList.Count - 1; b >= 0; b--)
            {
                if (mHero.PrimitivesTouches(mBBallList[b]))
                {
                    mBBallList.RemoveAt(b);
                    mBBallHit++;
                    mScore += kBballTouchScore;
                }
            }
            #endregion

            #region Step d. final checking of game winning
            // Check for new basketball condition
            TimeSpan timePassed = gameTime.TotalGameTime;
            timePassed = timePassed.Subtract(mCreationTimeStamp);
            if (timePassed.TotalMilliseconds > kBballMSecInterval)
            {
                mCreationTimeStamp = gameTime.TotalGameTime;
                BasketBall b = new BasketBall();
                mTotalBBallCreated++;
                mBBallList.Add(b);
            }
            #endregion

            #region Step e.
            // Check for winning condition ...
            if (mScore > kWinScore)
                mFinal = new TexturedPrimitive("Winner", new Vector2(75, 50), new Vector2(30, 20));
            else if (mScore < kLossScore)
                mFinal = new TexturedPrimitive("Loser", new Vector2(75, 50), new Vector2(30, 20));
            #endregion

        }

        /// <summary>
        /// Iterate through all visible game object and draw them
        /// </summary>
        public void DrawGame()
        {
            mHero.Draw();
            
            foreach (BasketBall b in mBBallList)
                b.Draw();

            if (null != mFinal)
                mFinal.Draw();

            // Drawn last to always show up on top
            FontSupport.PrintStatus("Status: " +
                    "Score=" + mScore +
                    " Basketball: Generated( " + mTotalBBallCreated + 
                      ") Collected(" + mBBallHit + ") Missed(" + mBBallMissed + ")", null);
        }
    }
}
