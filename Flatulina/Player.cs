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
        public bool hasJumped;
        public float jumpVel;

        // jet fart
        public float jetVel;

        // gravity
        public bool useGravity;
        public float gravityAccel, dampen, terminalVel;

        // player states
        public enum player_state { ON_GROUND,JUMPING,FALLING,JET };
        public player_state playerState;


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
            gravityAccel = 20f;
            terminalVel = 30f;
            dampen = 0.08f;
            jumpVel = 12f;
            hasJumped = true;

            playerState = player_state.ON_GROUND;
        }
        ~Player() { }



        public void MoveSprite(Vector2 newPos)
        {
            pos = newPos;
        }
        
        // check keyboard input
        public void CheckInput(float deltaTime)
        {
            KeyboardState state = Keyboard.GetState();
            
            // apply velocity to position
            pos.X = vel.X + pos.X;


            // CHECK ARROWS AND APPLY VELOCITY
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

            // check space (ONLY ON_GROUND)
            if (playerState == player_state.ON_GROUND)
            {
                vel.Y = 0;
                if (state.IsKeyDown(Keys.Space) && playerState == player_state.ON_GROUND)
                {
                    playerState = player_state.JUMPING;
                }
            }
            
        }
        // check floor position
        public void CheckFloor()
        {
            if (pos.Y + height < 300)
                playerState = player_state.FALLING;
            else
            {
                pos.Y = 300 - height;
                playerState = player_state.ON_GROUND;
                hasJumped = false;
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
        public void Jump(float deltaTime)
        {
            pos.Y = vel.Y + pos.Y;

            if (!hasJumped)
            {
                hasJumped = true;
                vel.Y = -jumpVel;
            }

            //pos.Y -= jumpVel * deltaTime;
            //vel.Y = -jumpVel * deltaTime;
            if (vel.Y != 0)
            {
                Vector2 i = vel;
                vel.Y = (i.Y -= dampen * i.Y);
                Console.WriteLine(vel.Y);
                if (vel.Y > -1f && vel.Y < 1f)
                {
                    vel.Y = 0;
                    playerState = player_state.FALLING;
                }
            }
            //else
            //{
               // playerState = player_state.FALLING;
            //}
            //if(vel.Y > 50) 
                //playerState = player_state.FALLING;
        }
        public void JetFart(float deltaTime)
        {
            pos.Y = vel.Y + pos.Y;

            if (!hasJumped)
            {
                hasJumped = true;
                vel.Y = -jetVel;
            }

            //pos.Y -= jumpVel * deltaTime;
            //vel.Y = -jumpVel * deltaTime;
            if (vel.Y != 0)
            {
                Vector2 i = vel;
                vel.Y = (i.Y -= dampen * i.Y);
                Console.WriteLine(vel.Y);
                if (vel.Y > -1f && vel.Y < 1f)
                {
                    vel.Y = 0;
                    playerState = player_state.FALLING;
                }
            }
            //else
            //{
            // playerState = player_state.FALLING;
            //}
            //if(vel.Y > 50) 
            //playerState = player_state.FALLING;
        }

        public void Update(float deltaTime)
        {
            
            
            switch (playerState)
            {
                case player_state.ON_GROUND:
                    CheckFloor();
                    hasJumped = false;
                    break;
                case player_state.JUMPING:
                    Jump(deltaTime);
                    break;
                case player_state.FALLING:
                    CheckFloor();
                    Gravity(deltaTime);
                    break;
                default:
                    break;
            }
            Console.WriteLine(playerState);
            CheckInput(deltaTime);
        }  
    }
}
