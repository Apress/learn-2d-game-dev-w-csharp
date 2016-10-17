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
    public class SpinningArrow : GameObject
    {
        public enum SpinningArrowState {
            ArrowSpinning,
            ArrowTransition,
            ArrowPointsToHero
        };

        private SpinningArrowState mArrowState = SpinningArrowState.ArrowTransition;
                // Current state of the spinning arrow

        private float mSpinRate = 0f;
                // Current spinning rate, 

        #region Constants
        private const float kHeroTriggerDistance = 15f;     // distance from hero to trigger PointsToHero state
        
        // Constants for spinning
        private const float kMaxSpinRate = (float)Math.PI / 10f; // assuming 60 updates a second, this is 12 cycles per second
        private const float kDeltaSpin = kMaxSpinRate / 200f; // Takes 200 cycles to spin to max speed
        #endregion 


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position">center of the arrow</param>
        public SpinningArrow(Vector2 position) : 
            base("TransientArrow", position, new Vector2(10, 4))
        {
            // arrow initially facing positive X direction
            InitialFrontDirection = Vector2.UnitX;
        }

        /// <summary>
        /// Return the state of the spinning arrow
        /// </summary>
        public SpinningArrowState ArrowState { get { return mArrowState; } }

        /// <summary>
        /// Updates the FSM of the spinning arrow
        /// </summary>
        /// <param name="hero">If in proper state, the arrow will track the hero</param>
        public void UpdateSpinningArrow(TexturedPrimitive hero)
        {
            switch (mArrowState)
            {
                case SpinningArrowState.ArrowTransition:
                    UpdateTransitionState();
                    break;
                case SpinningArrowState.ArrowSpinning:
                    UpdateSpinningState(hero);
                    break;
                case SpinningArrowState.ArrowPointsToHero:
                    UpdatePointToHero(hero.Position - Position);
                    break;
            }
        }

        /// <summary>
        /// Update the rotation angle of the arrow
        /// </summary>
        private void SpinTheArrow()
        {
            RotateAngleInRadian += mSpinRate;
            if (RotateAngleInRadian > (2 * Math.PI))
                RotateAngleInRadian -= (float)(2 * Math.PI);
        }


        /// <summary>
        /// Update function for Warming up: Transition state
        /// </summary>
        private void UpdateTransitionState()
        {
            SpinTheArrow();

            if (mSpinRate < kMaxSpinRate)
                mSpinRate += kDeltaSpin;
            else
            {
                // Transition to Spin state
                mArrowState = SpinningArrowState.ArrowSpinning;
                mImage = Game1.sContent.Load<Texture2D>("RightArrow");
            }
        }

        /// <summary>
        /// Update function for steady state spinning, capable of detecting the hero
        /// </summary>
        /// <param name="hero">if hero gets too close, hero will be tracked</param>
        private void UpdateSpinningState(TexturedPrimitive hero)
        {
            SpinTheArrow();

            // Check for hero ... 
            Vector2 toHero = hero.Position - Position;
            if (toHero.Length() < kHeroTriggerDistance)
            {
                // Transition to PointsToHeroState
                mSpinRate = 0f;
                mArrowState = SpinningArrowState.ArrowPointsToHero;
                mImage = Game1.sContent.Load<Texture2D>("PointingArrow");
            }
        }

        /// <summary>
        /// Update function to track the hero. If hero gets too far, transit back to
        /// "warm-up" state.
        /// </summary>
        /// <param name="toHero">vector point to the hero.</param>
        private void UpdatePointToHero(Vector2 toHero)
        {
            float dist = toHero.Length();
            if (dist < kHeroTriggerDistance)
            {
                FrontDirection = toHero;
            }
            else
            {
                // Go back to TransitionState for spinning up the arrow
                mArrowState = SpinningArrowState.ArrowTransition;
                mImage = Game1.sContent.Load<Texture2D>("TransientArrow");
            }
        }
    }
}
