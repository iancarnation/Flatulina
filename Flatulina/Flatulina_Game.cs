#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion



namespace Flatulina
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Flatulina_Game : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch _spriteBatch;

        //Sound Vars
        public SoundEffect effect;
        public SoundEffect backgroundMusic;
        public SoundEffect squeak;
        bool isKeyDown = false;

        SpriteFont Font1;
        Texture2D pixel;

        // Represents player
        Player player;
        //Player enemy;

        // Environment stuff
        List<EnvironmentSolid> collisionSolids;
        EnvironmentSolid floor;
        EnvironmentSolid tower1;

        // Keyboard states used to determine key presses
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        // Gamepad states used to determine button presses
        GamePadState currentGamePadState;
        GamePadState previousGamePadState;

        // Mouse states used to track mouse button presses
        MouseState currentMouseState;
        MouseState previousMouseState;


        // A movement speed for the player
        float playerMoveSpeed;

        //private Texture2D flatulina;

        public Flatulina_Game()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 728;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;

            // Initialize the player class
            player = new Player();
            //enemy = new Player();


            collisionSolids = new List<EnvironmentSolid>();
            floor = new EnvironmentSolid();
            tower1 = new EnvironmentSolid();

            collisionSolids.Add(floor);
            collisionSolids.Add(tower1);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            //Loading Sound Clips
            effect = Content.Load<SoundEffect>("Sound\\Fart");
            backgroundMusic = Content.Load<SoundEffect>("Sound\\LuteSong");
            squeak = Content.Load<SoundEffect>("Sound\\ShortSqueak");

            Font1 = Content.Load<SpriteFont>("Graphics\\Courier New");
            //Vector2 fontPosition = new Vector2(graphics.GraphicsDevice.Viewport.TitleSafeArea.X + 10, graphics.GraphicsDevice.Viewport.TitleSafeArea.Y + 10);

            // for drawing debug rectangles
            pixel = new Texture2D(graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White }); // so that we can draw whatever color we want on top of it

            // Load player resources
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            // Vector2 enemyPosition = new Vector2(500, 100);

            player.Initialize(Content.Load<Texture2D>("Graphics\\tempCherub"), playerPosition);
            //enemy.Initialize(Content.Load<Texture2D>("Graphics\\cherub-flying-arms"), enemyPosition);



            // Environment
            Vector2 floorPosition = new Vector2(0, GraphicsDevice.Viewport.TitleSafeArea.Height - 100);
            floor.Initialize(Content.Load<Texture2D>("Graphics\\floor"), floorPosition);

            Vector2 tower1Position = new Vector2(400, GraphicsDevice.Viewport.TitleSafeArea.Height - 450);
            tower1.Initialize(Content.Load<Texture2D>("Graphics\\tower"), tower1Position);

            Console.WriteLine("Loadeddddd");

            //flatulina = Content.Load<Texture2D>("Player/cherub-flying-arms");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Save the previous state of the keyboard and game pad so we can determine single key/button presses
            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;

            // Read the current state of the keyboard and gamepad and store it
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            //BackGround music//////////SOUNDS BAD RIGHT NOW WILL EDIT
            SoundEffectInstance soundEffectInstance = backgroundMusic.CreateInstance();
            soundEffectInstance.Volume = 0.02f;
            //soundEffectInstance.Play(); 

            // Update the player
            UpdateCollision();
            UpdatePlayer(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 

        private void UpdatePlayer(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            bool moveRequest = false;
            //Creating effect Instance
            SoundEffectInstance soundEffect = effect.CreateInstance();
            soundEffect.Volume = 0.01f;
            soundEffect.IsLooped = false;
            //Creating an instance of squeak
            SoundEffectInstance squeakEffect = squeak.CreateInstance();
            squeakEffect.Volume = 0.1f;
            squeakEffect.IsLooped = false;

            // Get Thumbstick Controls
            player.position.X += player.velocity.X * deltaTime;           //currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed;
            player.position.Y += player.velocity.Y * deltaTime;            //currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed;

            // Use the keyboard / dpad
            if (currentKeyboardState.IsKeyDown(Keys.Left) || currentKeyboardState.IsKeyDown(Keys.A) || currentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                player.velocity.X -= player.accX;
                moveRequest = true;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D) || currentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                player.velocity.X += player.accX;
                moveRequest = true;
            }

            // JUMPING
            if (currentKeyboardState.IsKeyDown(Keys.Space) && !player.jumping && !player.jumpKeyDown)
            {
                player.jumping = true;
                player.jumpKeyDown = true;
                player.velocity.Y = -player.jumpVelocityY;
                soundEffect.Play();
            }
            if (currentKeyboardState.IsKeyUp(Keys.Space) && player.jumpKeyDown == true && soundEffect.State == SoundState.Stopped)
            {
                squeakEffect.Play();
            }

            // jump key released
            if (!currentKeyboardState.IsKeyDown(Keys.Space))
            {
                player.jumpKeyDown = false;
            }

            // JET FART
            if (currentKeyboardState.IsKeyDown(Keys.Z) /*&& !player.jet && !player.jetKeyDown*/ && player.fuel > 0)
            {
                player.fuel -= 1;
                //player.jet = true;
                //player.jetKeyDown = true;
                player.velocity.Y = -player.jetVelocityY;

            }

            if (player.velocity.X > player.maxVelocity.X) player.velocity.X = player.maxVelocity.X;
            if (player.velocity.X < -player.maxVelocity.X) player.velocity.X = -player.maxVelocity.X;
            if (player.velocity.Y < -player.maxVelocity.Y) player.velocity.Y = -player.maxVelocity.Y;

            if (!moveRequest)
            {
                if (player.velocity.X < 0) player.velocity.X += player.decX;
                if (player.velocity.X > 0) player.velocity.X -= player.decX;
                // Deceleration may produce a speed that is greater than zero but
                // smaller than the smallest unit of deceleration. These lines ensure
                // that the player does not keep travelling at slow speed forever after
                // decelerating.
                if (player.velocity.X > 0 && player.velocity.X < player.decX) player.velocity.X = 0;
                if (player.velocity.X < 0 && player.velocity.X > -player.decX) player.velocity.X = 0;
            }

            // Make sure player does not go out of bounds
            // Temporary fix until dinal collision is added
            player.position.X = MathHelper.Clamp(player.position.X, 0, GraphicsDevice.Viewport.Width - player.Width * player.scale);
            if (player.position.Y > 540)
            {
                player.position.Y = MathHelper.Clamp(player.position.Y, 0, 540/*GraphicsDevice.Viewport.Height - (player.Height + 350) * player.scale*/);
                player.jumping = false;
                player.jet = false;
            }

            // GRAVITY //
            player.velocity.Y += player.gravityAccel;

            // UPDATE FUEL HUD
            player.fuelFill.Width = player.fuel * 2;


            player.UpdateBoundingBoxes();
        }

        private void UpdateCollision()
        {
            player.CollisionTop.DebugRectColor = Color.Red;
            player.CollisionBottom.DebugRectColor = Color.Red;
            player.CollisionLeft.DebugRectColor = Color.Red;
            player.CollisionRight.DebugRectColor = Color.Red;

            // for each of the collision solids in the environment..
            for (int i = 0; i < collisionSolids.Count; i++)
            {
                //Console.WriteLine("Player" + player.BoundingBox.Position);
                //Console.WriteLine("Solid" + collisionSolids[i].Position);

                // check to see if player's general bounding box is colliding
                if (player.BoundingBox.Intersects(collisionSolids[i].BoundingBox))
                {
                    Console.WriteLine("Bounding Box Intersection");
                    player.BoundingBox.DebugRectColor = Color.Yellow;

                    // run the player's collision area checks and adjust position accordingly
                    player.HandleCollisionWithSolid(collisionSolids[i].BoundingBox);
                }
                else
                    player.BoundingBox.DebugRectColor = Color.Red;
            }
        }

        // private void UpdateCollisionOld()
        //{
        //if (player.HitBox.Intersects(enemy.HitBox))
        //    player.color = Color.Red;
        //else
        //    player.color = Color.White;

        //// Solid Environment objects
        //if (player.HitBox.Intersects(floor.HitBox))
        //{
        //    player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, floor.Position.Y - player.Height * player.scale);
        //}





        // vvvvvvvvvvvvvvvvvvvv Old Collision Attempt vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
        //    // source: http://gamedev.stackexchange.com/questions/14486/2d-platformer-aabb-collision-problems/14491#14491

        //    // This loop repeats until player has been fully pushed outside of all collision objects
        //    while (StillCollidingWithEnvironment(player))
        //    {
        //        float xDistanceToResolve = XDistanceToMoveToResolveCollisions(player);
        //        float yDistanceToResolve = YDistanceToMoveToResolveCollisions(player);
        //        bool xIsColliding = (xDistanceToResolve != 0.0f);

        //        /* if we aren't colliding on x (not possible for normal solid collision
        //           shapes, but can happen for unidirectional collision objects, such as
        //         * platforms which can be jumped up throug, but support the player from
        //         * above), or if a correction along y would simply require a smaller move
        //         * than one along x, then resolve our collision by moving along y.
        //         * */

        //        if (!xIsColliding || Math.Abs(yDistanceToResolve) < Math.Abs(xDistanceToResolve))
        //        {
        //            player.position.Y += yDistanceToResolve;
        //            break;
        //        }
        //        else // otherwise, resolve the collision by moving along x
        //            player.position.X += xDistanceToResolve; break;
        //    }


        //}

        //bool StillCollidingWithEnvironment(Player player)
        //{
        //    // loop over every collision object in the world (don't test player against itself)
        //    for (int i = 0; i < collisionSolids.Count; i++)
        //    {
        //        // if the player overlaps any environment solids, then it's colliding
        //        if (player.BoundingBox.Intersects(collisionSolids[i].HitBox))
        //        {
        //            // find one of the player's hitbox corners that is colliding
        //            for (int c = 0; c < 4; c++)
        //            {
        //                if (collisionSolids[i].HitBox.Contains(player.Corners[c]))
        //                {
        //                    player.collidingCorner = player.Corners[c];
        //                    break;
        //                }
        //            }

        //            return true;
        //        }
        //    }

        //    return false;
        //}

        //float XDistanceToMoveToResolveCollisions(Player player)
        //{
        //    // check how far we'd have to move left or right to stop colliding with anything
        //    // return whichever move is smaller
        //    float moveOutLeft = FindDistanceToEmptySpaceAlongNegativeX(player.collidingCorner);
        //    float moveOutRight = FindDistanceToEmptySpaceAlongX(player.collidingCorner);
        //    float bestMoveOut = MathHelper.Min(Math.Abs(moveOutLeft), Math.Abs(moveOutRight));

        //    // if we need to move left, return negative x
        //    if (bestMoveOut == Math.Abs(moveOutLeft))
        //        return - bestMoveOut;

        //    return bestMoveOut;
        //}

        //float FindDistanceToEmptySpaceAlongX(Vector2 collidingCorner)
        //{
        //    Vector2 cursor = collidingCorner;
        //    //bool colliding = true;
        //    // until we stop colliding....
        //    //while (colliding)
        //    //{
        //        //colliding = false;
        //        // loop over all collision objects
        //        for (int i = 0; i < collisionSolids.Count; i++)
        //        {
        //            // and if we hit an object..
        //            if (collisionSolids[i].HitBox.Contains(cursor.X, cursor.Y))
        //            {
        //                // move outside of the object, and repeat
        //                cursor.X = collisionSolids[i].HitBox.Right;
        //                //colliding = true;

        //                // break back to the while loop, to re-test collisions with new cursor position
        //                break;
        //            }
        //        }
        //    //}
        //    // return how far we had to move, to reach empty space
        //    return cursor.X - collidingCorner.X;
        //}

        //float FindDistanceToEmptySpaceAlongNegativeX(Vector2 collidingCorner)
        //{
        //    Vector2 cursor = collidingCorner;
        //    //bool colliding = true;
        //    // until we stop colliding....
        //    //while (colliding)
        //    //{
        //       // colliding = false;
        //        // loop over all collision objects
        //        for (int i = 0; i < collisionSolids.Count; i++)
        //        {
        //            // and if we hit an object..
        //            if (collisionSolids[i].HitBox.Contains(cursor.X, cursor.Y))
        //            {
        //                // move outside of the object, and repeat
        //                cursor.X = collisionSolids[i].HitBox.Left;
        //               // colliding = true;

        //                // break back to the while loop, to re-test collisions with new cursor position
        //                break;
        //            }
        //        }
        //    //}
        //    // return how far we had to move, to reach empty space
        //    return cursor.X - collidingCorner.X;
        //}

        //float YDistanceToMoveToResolveCollisions(Player player)
        //{
        //    // check how far we'd have to move up or down to stop colliding with anything
        //    // return whichever move is smaller
        //    float moveOutUp = FindDistanceToEmptySpaceAlongNegativeY(player.collidingCorner);
        //    float moveOutDown = FindDistanceToEmptySpaceAlongY(player.collidingCorner);
        //    float bestMoveOut = MathHelper.Min(Math.Abs(moveOutUp), Math.Abs(moveOutDown));

        //    // if the best move is up, need to return a negative Y value
        //    if (bestMoveOut == Math.Abs(moveOutUp))
        //        return - bestMoveOut;

        //    return bestMoveOut;
        //}

        //float FindDistanceToEmptySpaceAlongY(Vector2 collidingCorner)
        //{
        //    Vector2 cursor = collidingCorner;
        //    //bool colliding = true;
        //    // until we stop colliding....
        //   // while (colliding)
        //    //{
        //        //colliding = false;
        //        // loop over all collision objects
        //        for (int i = 0; i < collisionSolids.Count; i++)
        //        {
        //            // and if we hit an object..
        //            if (collisionSolids[i].HitBox.Contains(cursor.X, cursor.Y))
        //            {
        //                // move outside of the object, and repeat
        //                cursor.Y = collisionSolids[i].HitBox.Bottom;
        //                //colliding = true;

        //                // break back to the while loop, to re-test collisions with new cursor position
        //                break;
        //            }
        //        }
        //    //}
        //    // return how far we had to move, to reach empty space
        //    return cursor.Y - collidingCorner.Y;
        //}

        //float FindDistanceToEmptySpaceAlongNegativeY(Vector2 collidingCorner)
        //{
        //    Vector2 cursor = collidingCorner;
        //    //bool colliding = true;
        //    // until we stop colliding....
        //   // while (colliding)
        //   // {
        //       // colliding = false;
        //        // loop over all collision objects
        //        for (int i = 0; i < collisionSolids.Count; i++)
        //        {
        //            // and if we hit an object..
        //            if (collisionSolids[i].HitBox.Contains(cursor.X, cursor.Y))
        //            {
        //                // move outside of the object, and repeat
        //                cursor.Y = collisionSolids[i].HitBox.Top;
        //                //colliding = true;

        //                // break back to the while loop, to re-test collisions with new cursor position
        //                break;
        //            }
        //        }
        //    //}
        //    // return how far we had to move, to reach empty space
        //    return cursor.Y - collidingCorner.Y;

        // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        //}



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            // draw mouse coordinates
            currentMouseState = Mouse.GetState();
            string mouseCoord = currentMouseState.X.ToString() + "  " + currentMouseState.Y.ToString();
            Vector2 fontPosition = new Vector2(graphics.GraphicsDevice.Viewport.TitleSafeArea.X + 10, graphics.GraphicsDevice.Viewport.TitleSafeArea.Y + 10);
            _spriteBatch.DrawString(Font1, mouseCoord, fontPosition, Color.White);

            // Draw Player
            player.Draw(_spriteBatch);

            // HUD
            _spriteBatch.Draw(pixel, player.fuelFill, Color.Red);
            DrawBorder(player.fuelOutline, 2, Color.White);

            // draw player debug rect
            DrawBorder(player.BoundingBox.DebugRect, 2, player.BoundingBox.DebugRectColor);
            DrawBorder(player.CollisionTop.DebugRect, 1, player.CollisionTop.DebugRectColor);
            DrawBorder(player.CollisionBottom.DebugRect, 1, player.CollisionBottom.DebugRectColor);
            DrawBorder(player.CollisionLeft.DebugRect, 1, player.CollisionLeft.DebugRectColor);
            DrawBorder(player.CollisionRight.DebugRect, 1, player.CollisionRight.DebugRectColor);

            //enemy.Draw(_spriteBatch);

            for (int i = 0; i < collisionSolids.Count; i++)
            {
                collisionSolids[i].Draw(_spriteBatch);

                // draw debug rectangles
                DrawBorder(collisionSolids[i].BoundingBox.DebugRect, 2, collisionSolids[i].BoundingBox.DebugRectColor);
            }

            //_spriteBatch.Draw(flatulina, new Rectangle(50, 50, 400, 353), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Will draw a border (hollow rectangle) of the given 'thicknessOfBorder' (in pixels)
        /// of the specified color.
        ///
        /// By Sean Colombo, from http://bluelinegamestudios.com/blog
        /// </summary>
        /// <param name="rectangleToDraw"></param>
        /// <param name="thicknessOfBorder"></param>
        public void DrawBorder(Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
        {
            // Draw top line
            _spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

            // Draw left line
            _spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

            // Draw right line
            _spriteBatch.Draw(pixel, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
                                            rectangleToDraw.Y,
                                            thicknessOfBorder,
                                            rectangleToDraw.Height), borderColor);
            // Draw bottom line
            _spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X,
                                            rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                                            rectangleToDraw.Width,
                                            thicknessOfBorder), borderColor);
        }
    }
}
