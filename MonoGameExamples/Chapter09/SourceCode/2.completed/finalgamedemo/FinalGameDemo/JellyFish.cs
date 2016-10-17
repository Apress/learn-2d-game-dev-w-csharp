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
    public class JellyFish : PatrolEnemy
    {
        public JellyFish() :
            base("ENEMY_3", Vector2.Zero, new Vector2(kInitFishSize * kEnemyWidth + kEnemyWidth, kInitFishSize * kEnemyWidth + kEnemyWidth), 2, 1, 0)
        {
            mAllowRotate = true;
            mInitFrontDir = Vector2.UnitY;
            mCurrentPatrolType = PatrolType.FreeRoam;
            FishSize = kInitFishSize;
            mCurrentEnemyType = EnemyType.JellyFish;
        }
    }
}
