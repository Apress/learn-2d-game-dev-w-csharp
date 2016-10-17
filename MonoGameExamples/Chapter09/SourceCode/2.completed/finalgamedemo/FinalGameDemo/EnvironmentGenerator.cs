using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class EnvironmentGenerator
    {
        private List<Platform> mTheFloorSet;
        private List<Platform> mTheSeaweedTallSet;
        private List<Platform> mTheSeaweedSmallSet;
        private Platform mTheSign;
        private PatrolEnemySet mEnemies;
        private FishFood mFishFood;

        private int mOffsetCounter;
        private const int kSectionSize = 800;
        private const int kFloorAndRoofSize = 40;
        private const int kSignSize = 30;
        private const int kInitialSignPosX = 100;
        private const int kSignUnitScaler = 20;

        public EnvironmentGenerator()
        {
            InitializeEnvironment();
        }

        public void InitializeEnvironment()
        {
            Camera.SetCameraWindow(Vector2.Zero, 300);
            mOffsetCounter = -20;
            mTheFloorSet = new List<Platform>();
            mTheSeaweedTallSet = new List<Platform>();
            mTheSeaweedSmallSet = new List<Platform>();

            mEnemies = new PatrolEnemySet();

            for (int i = 0; i < kSectionSize / kFloorAndRoofSize; i++)
            {
                mOffsetCounter += kFloorAndRoofSize;
                mTheFloorSet.Add(new Platform("GROUND_1", new Vector2(mOffsetCounter, 20), new Vector2(kFloorAndRoofSize, kFloorAndRoofSize)));
            }

            mTheSign = new Platform("SIGN_1", new Vector2(kInitialSignPosX, kFloorAndRoofSize / 2 + kSignSize / 2), new Vector2(kSignSize, kSignSize));

            float randNum;
            for (int i = 0; i < 5; i++)
            {
                randNum = (float)Game1.sRan.NextDouble() * mTheFloorSet[mTheFloorSet.Count - 1].PositionX + kInitialSignPosX * 2;
                mTheSeaweedTallSet.Add(new Platform("SEAWEEDTALL_1", new Vector2(randNum, kFloorAndRoofSize), new Vector2(kFloorAndRoofSize / 1.5f, kFloorAndRoofSize * 1.5f)));
            }

            for (int i = 0; i < 5; i++)
            {
                randNum = (float)Game1.sRan.NextDouble() * mTheFloorSet[mTheFloorSet.Count - 1].PositionX;
                mTheSeaweedSmallSet.Add(new Platform("SEAWEEDSMALL_1", new Vector2(randNum, kFloorAndRoofSize / 2 - 5), new Vector2(kFloorAndRoofSize / 2, kFloorAndRoofSize / 2)));
            }

            mFishFood = new FishFood();
        }

        public void Update(Hero theHero)
        {
            mFishFood.Update(theHero, mTheFloorSet, mTheSeaweedTallSet);
            mEnemies.UpdateSet(theHero);
            
            if (Camera.CameraWindowLowerRightPosition.X > mTheFloorSet[mTheFloorSet.Count - 1].Position.X)
            {
                for (int i = 0; i < mTheFloorSet.Count; i++)
                {
                    mTheFloorSet[i].PositionX += kFloorAndRoofSize * 10;
                }

                float randNum;
                for (int i = 0; i < mTheSeaweedTallSet.Count; i++)
                {
                    if (mTheSeaweedTallSet[i].PositionX < Camera.CameraWindowLowerLeftPosition.X - mTheSeaweedTallSet[i].Width)
                    {
                        randNum = (float)Game1.sRan.NextDouble() * kSectionSize / 2 + Camera.CameraWindowLowerRightPosition.X;
                        mTheSeaweedTallSet[i].PositionX = randNum;
                    }
                }

                for (int i = 0; i < mTheSeaweedSmallSet.Count; i++)
                {
                    if (mTheSeaweedSmallSet[i].PositionX < Camera.CameraWindowLowerLeftPosition.X - mTheSeaweedTallSet[i].Width)
                    {
                        randNum = (float)Game1.sRan.NextDouble() * kSectionSize / 2 + Camera.CameraWindowLowerRightPosition.X;
                        mTheSeaweedSmallSet[i].PositionX = randNum;
                    }
                }
            }

            if ((Camera.CameraWindowLowerLeftPosition.X - mTheSign.Width) > mTheSign.PositionX)
            {
                if (mTheSign.PositionX == kInitialSignPosX)
                    mTheSign.PositionX = 0;
                mTheSign.PositionX += 500;
            }
        }

        public void Draw()
        {

            for (int i = 0; i < mTheFloorSet.Count; i++)
            {
                mTheFloorSet[i].Draw();
            }

            mTheSign.Draw();
            String msg = (mTheSign.Position.X / 20).ToString() + "m";

            FontSupport.PrintStatusAt(mTheSign.Position, msg, Color.Black);

            for (int i = 0; i < mTheSeaweedTallSet.Count; i++)
            {
                mTheSeaweedTallSet[i].Draw();
            }

            for (int i = 0; i < mTheSeaweedSmallSet.Count; i++)
            {

                mTheSeaweedSmallSet[i].Draw();
            }

            mEnemies.DrawSet();
            mFishFood.Draw();
        }

    }
}
