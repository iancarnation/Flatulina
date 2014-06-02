#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Flatulina
{
    public class Player
    {
        public Texture2D sprite;
        public int width, height;
        public Vector2 pos, vel;
        public float maxSpeed, accel, friction;

        public Player()
        {
            sprite = null;
            width = 0;
            height = 0;
            pos = new Vector2(0f, 0f);
            maxSpeed = 0;
            accel = 0;
            friction = 0;

        }
        public Player(Texture2D newSprite, int newWidth, int newHeight, Vector2 newPos)
        {
            sprite = newSprite;
            width = newWidth;
            height = newHeight;
            pos = newPos;
            maxSpeed = 5f;
            accel = 10f;
            friction = 0.2f;
        }
        ~Player() { }


        public void MoveSprite(Vector2 newPos)
        {
            pos = newPos;
        }

        // Movement (goes in update loop) //
        public void CheckInput(float deltaTime)
        {
            KeyboardState state = Keyboard.GetState();

            pos = vel + pos;


            if (state.IsKeyDown(Keys.Left))
            {
                // not sure of better way yet //
                vel.X -= accel * deltaTime;
                if (vel.X < -maxSpeed)
                    vel.X = -maxSpeed;

            }
            else if (state.IsKeyDown(Keys.Right))
            {
                // not sure of better way yet //
                vel.X += accel * deltaTime;
                if (vel.X > maxSpeed)
                    vel.X = maxSpeed;
            }
            // slow down/apply friction when not in motion //
            else if (vel != Vector2.Zero)
            {
                Vector2 i = vel;
                vel.X = (i.X -= friction * i.X);
                vel.Y = (i.Y -= friction * i.Y);
            }
        }
    }
}
