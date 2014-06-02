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
        // general object data
        public Texture2D sprite;
        public int width, height;
        public Vector2 pos;

        // movement
        public Vector2 vel;
        public float maxSpeed, accel, friction; 
            
        // jumping
        public float jumpVel;

        // gravity
        public bool useGravity;
        public float gravityAccel, terminalVel;

        // player states
        public enum player_state {ON_GROUND,JUMPING,FALLING };

        public player_state state;

        public Player()
        {
            sprite = null;
            width = 0;
            height = 0;
            pos = new Vector2(0f, 0f);

            maxSpeed = 0;
            accel = 0;
            friction = 0;
            gravityAccel = 0;
            terminalVel = 0;

            

        }
        public Player(Texture2D newSprite, int newWidth, int newHeight, Vector2 newPos)
        {
            sprite = newSprite;
            width = newWidth;
            height = newHeight;
            pos = newPos;
            maxSpeed = 4f;
            accel = 10f;
            friction = 0.15f;
            gravityAccel = 12f;
            terminalVel = 25f;
            jumpVel = 5f;

            state = player_state.ON_GROUND;
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

            pos.X = vel.X + pos.X;

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
            }

            

        }
        public void Gravity(float deltaTime)
        {
            pos.Y = vel.Y + pos.Y;


            vel.Y = vel.Y + gravityAccel * deltaTime;
            if (vel.Y > terminalVel)
                vel.Y = terminalVel;
            if (pos.Y > 300)
                pos.Y = 300;
        }

        public void On_Ground(float deltaTime)
        {
        }

        public void Jumping(float deltaTime) 
        {
        }

        public void Falling(float deltaTime) 
        {
        }

        public void Update(float deltaTime)
        {
            CheckInput(deltaTime);
            Gravity(deltaTime);

            switch (state)
            {
                case player_state.ON_GROUND:
                    On_Ground(deltaTime);
                    break;
                case player_state.JUMPING:
                    Jumping(deltaTime);
                    break;
                case player_state.FALLING:
                    Falling(deltaTime);
                    break;
                default:
                    break;
            }
        }
    }
}
