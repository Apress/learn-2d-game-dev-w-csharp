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
    public class FishFood : SpritePrimitive
    {
        private const float kFoodSize = 8;
        private bool mCanMove;

        public FishFood() :
            base("WORM_1", Vector2.Zero, new Vector2(kFoodSize, kFoodSize), 2, 1, 0)
        {
            Position = RandomPosition(true);
            SetSpriteAnimation(0, 0, 1, 1, 10);
            mCanMove = true;
            Speed = 0.2f;
        }

        public void Update(Hero hero, List<Platform> floor, List<Platform> seaweed)
        {
            
            if (Camera.CameraWindowUpperRightPosition.X > PositionX && mCanMove)
            {
                VelocityDirection = new Vector2(0, -1);
                Speed = 0.2f;
                base.Update();
            }

            if (Camera.CameraWindowUpperLeftPosition.X > PositionX)
            {
                Position = RandomPosition(true);
                mCanMove = true;
            }

            Vector2 vec;
            if (hero.PixelTouches(this, out vec))
            {
                Stop();
                hero.Feed();
                Position = RandomPosition(true);
                mCanMove = true;
            }
            for (int i = 0; i < floor.Count; i++)
            {
                if (floor[i].PixelTouches(this, out vec))
                {
                    Stop();
                }
            }
            for (int i = 0; i < seaweed.Count; i++)
            {
                if (seaweed[i].PixelTouches(this, out vec))
                {
                    Stop();
                }
            }
        }

        private Vector2 RandomPosition(bool offCamera)
        {
            Vector2 position;
            float posX = (float)Game1.sRan.NextDouble() * Camera.Width * 0.80f + Camera.Width * 0.10f;
            float posY = Camera.CameraWindowUpperRightPosition.Y;

            if (offCamera)
                posX += Camera.CameraWindowUpperRightPosition.X + Camera.Width*2;

            position = new Vector2(posX, posY);
            return position;
        }


        private void Stop()
        {
            mCanMove = false;
            Velocity = Vector2.Zero;
            Speed = 0;
        }
    }
}
