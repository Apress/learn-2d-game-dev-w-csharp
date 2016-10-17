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
using XNABook_Example;

namespace BookExample
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameState
    {
        public enum GameStates
        {
            StartScreen,
            Playing,
            Dead
        }

        private int mDistantTraveled = 0;
        private GameStates mCurrentGameState;
        private TexturedPrimitive mSplashScreen;
        private TexturedPrimitive mGameOverScreen;
        private EnvironmentGenerator mEnvironment;
        private Hero mHero;

        public GameState()
        {
            mCurrentGameState = GameStates.StartScreen;
            AudioSupport.PlayBackgroundAudio("Mind_Meld", 0.5f);
            InitializeStartMenu();
        }

        public void InitializeStartMenu()
        {
            float centerX = Camera.CameraWindowUpperRightPosition.X - Camera.Width/2;
            float centerY = Camera.CameraWindowUpperRightPosition.Y- Camera.Height/2;

            mSplashScreen = new TexturedPrimitive("SPLASHSCREEN_1", new Vector2(centerX, centerY), new Vector2(Camera.Width, Camera.Height));
            String msg = "Press the 'K' key to start.";
            mSplashScreen.Label = msg;
            mSplashScreen.LabelColor = Color.Black;
            
        }

        public void InitializeGamePlay()
        {
            mHero = new Hero(new Vector2(20f, 30f));
            mEnvironment = new EnvironmentGenerator();
        }

        public void InitializeGameOverScreen()
        {
            float centerX = Camera.CameraWindowUpperRightPosition.X - Camera.Width/2;
            float centerY = Camera.CameraWindowUpperRightPosition.Y- Camera.Height/2;

            mGameOverScreen = new TexturedPrimitive("GAMEOVERSCREEN_1", new Vector2(centerX, centerY), new Vector2(Camera.Width, Camera.Height));
            String msg = mDistantTraveled +  "m traveled. Press the 'K' key to try agian.";
            mGameOverScreen.Label = msg;
            mGameOverScreen.LabelColor = Color.Black;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        public void UpdateGame(GameTime gameTime)
        {
            switch(mCurrentGameState)
            {
                case GameStates.StartScreen:
                    UpdateStartScreen();
                    break;
                case GameStates.Playing:
                    UpdateGamePlay(gameTime);
                    break;
                case GameStates.Dead:
                    UpdateGameOverScreen();
                    break;
            }
            
        }

        public void UpdateGamePlay(GameTime gameTime)
        {
            mDistantTraveled = (int)mHero.PositionX / 20;
            if (mHero.HasLost())
            {
                mCurrentGameState = GameStates.Dead;
                AudioSupport.PlayACue("Break");
                InitializeGameOverScreen();
                return;
            }

            bool shootBubbleShot = (InputWrapper.Buttons.A == ButtonState.Pressed);

            mHero.Update(gameTime, InputWrapper.ThumbSticks.Left, shootBubbleShot);
            mEnvironment.Update(mHero);
           
            #region hero moving the camera window

            float kBuffer = mHero.Width * 5f;
            float kHalfCameraSize = Camera.Width * 0.5f;
            Vector2 delta = Vector2.Zero;
            Vector2 cameraLL = Camera.CameraWindowLowerLeftPosition;
            Vector2 cameraUR = Camera.CameraWindowUpperRightPosition;
            const float kChaseRate = 0.05f;

            if (mHero.PositionX > (cameraUR.X - kHalfCameraSize))
            {   
                delta.X = (mHero.PositionX + kHalfCameraSize - cameraUR.X) * kChaseRate;
            }

            Camera.MoveCameraBy(delta);
            #endregion 
        }

        public void ClearGame()
        {
            mDistantTraveled = 0;
            mHero = null;
            mEnvironment = null;
        }

        public void UpdateStartScreen()
        {
            if (InputWrapper.Buttons.A == ButtonState.Pressed)
            {
                mSplashScreen = null;
                mCurrentGameState = GameStates.Playing;
                InitializeGamePlay();
            }
        }

        public void UpdateGameOverScreen()
        {
            if (InputWrapper.Buttons.A == ButtonState.Pressed)
            {
                mGameOverScreen = null;
                mCurrentGameState = GameStates.Playing;
                InitializeGamePlay();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void DrawGame()
        {
            switch (mCurrentGameState)
            {
                case GameStates.StartScreen:
                    if(mSplashScreen != null) 
                        mSplashScreen.Draw();
                    break;
                case GameStates.Playing:
                    mEnvironment.Draw();
                    mHero.Draw();
                    FontSupport.PrintStatus("Distance: " + mDistantTraveled + "  Size: " + mHero.HeroSize, null);
                    break;
                case GameStates.Dead:
                    if (mGameOverScreen != null)
                        mGameOverScreen.Draw();
                    break;
            }
                        
        }
    }
}
