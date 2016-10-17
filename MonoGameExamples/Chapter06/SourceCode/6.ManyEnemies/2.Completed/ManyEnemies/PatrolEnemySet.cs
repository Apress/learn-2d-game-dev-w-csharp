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

    public class PatrolEnemySet
    {
        private List<PatrolEnemy> mTheSet = new List<PatrolEnemy>();
        const int kDefaultNumEnemy = 15;

        public PatrolEnemySet()
        {
            CreateEnemySet(kDefaultNumEnemy);
        }

        public PatrolEnemySet(int numEnemy)
        {
            CreateEnemySet(numEnemy);
        }

        private void CreateEnemySet(int numEnemy)
        {
            for (int i = 0; i < numEnemy; i++)
            {
                PatrolEnemy enemy = new PatrolEnemy();
                mTheSet.Add(enemy);
            }
        }

        public int UpdateSet(GameObject hero)
        {
            int count = 0;
            foreach (var enemy in mTheSet)
            {
                if (enemy.UpdatePatrol(hero))
                    count++;
            }
            return count;
        }

        public void DrawSet()
        {
            foreach (var enemy in mTheSet)
                enemy.Draw();
        }
    }
}
