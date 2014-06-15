﻿#region Using Statements
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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Flatulina_Game : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch _spriteBatch;

        SpriteFont Font1;
        Texture2D pixel;

        bool debugBoxesOn;

        // Represents player
        Player player;
        //Player enemy;

        // Environment stuff
        public List<EnvironmentSolid> collisionSolids;
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
        //float playerMoveSpeed;

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

            debugBoxesOn = true;

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

            Font1 = Content.Load<SpriteFont>("Graphics\\Courier New");
            //Vector2 fontPosition = new Vector2(graphics.GraphicsDevice.Viewport.TitleSafeArea.X + 10, graphics.GraphicsDevice.Viewport.TitleSafeArea.Y + 10);

            // for drawing debug rectangles
            pixel = new Texture2D(graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White }); // so that we can draw whatever color we want on top of it

            // Load player resources
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            // Vector2 enemyPosition = new Vector2(500, 100);

            player.Initialize(this, Content.Load<Texture2D>("Graphics\\tempCherub"), playerPosition);
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
            // vvvvvvvvvvvvv New vvvvvvvvvvvvvvvvvvvvv
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update the player
            player.Update(gameTime, currentKeyboardState, currentGamePadState);

            // Save the previous state of the keyboard and game pad so we can determine single key/button presses
            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;

            // Read the current state of the keyboard and gamepad and store it
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            if (currentKeyboardState.IsKeyDown(Keys.B))
            {
                debugBoxesOn = !debugBoxesOn;
            }

            base.Update(gameTime);

            // vvvvvvvvvvvv Original vvvvvvvvvvvvvvvvvv
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            //// Save the previous state of the keyboard and game pad so we can determine single key/button presses
            //previousGamePadState = currentGamePadState;
            //previousKeyboardState = currentKeyboardState;

            //// Read the current state of the keyboard and gamepad and store it
            //currentKeyboardState = Keyboard.GetState();
            //currentGamePadState = GamePad.GetState(PlayerIndex.One);

            
            //UpdateCollision();
            //UpdatePlayer(gameTime);

            // debug options
            // toggle bounding boxes
            //if (currentKeyboardState.IsKeyDown(Keys.B))
            //{
            //    debugBoxesOn = !debugBoxesOn;
            //}
            
            //base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 

        //private void UpdatePlayer(GameTime gameTime)
        //{
        //    float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        //    bool moveRequest = false;

        //    // Get Thumbstick Controls
        //    player.Position += player.Velocity * deltaTime;                     

        //    // Use the keyboard / dpad
        //    if (currentKeyboardState.IsKeyDown(Keys.Left) || currentKeyboardState.IsKeyDown(Keys.A) || currentGamePadState.DPad.Left == ButtonState.Pressed)
        //    {
        //        player.Velocity.X -= player.accX;
        //        moveRequest = true;
        //    }

        //    if (currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D) || currentGamePadState.DPad.Right == ButtonState.Pressed)
        //    {
        //        player.Velocity.X += player.accX;
        //        moveRequest = true;
        //    }

        //    // JUMPING
        //    if (currentKeyboardState.IsKeyDown(Keys.Space) && player.onGround /*&& !player.jumping*/ && !player.jumpKeyDown)
        //    {
        //        player.jumping = true;
        //        player.jumpKeyDown = true;
        //        player.Velocity.Y = -player.jumpVelocityY;
        //    }
        //    // jump key released
        //    if (!currentKeyboardState.IsKeyDown(Keys.Space))
        //    {
        //        player.jumpKeyDown = false;
        //    }

        //    // JET FART
        //    if (currentKeyboardState.IsKeyDown(Keys.Z) /*&& !player.jet && !player.jetKeyDown*/ && player.fuel > 0)
        //    {
        //        player.fuel -= 1;
        //        //player.jet = true;
        //        //player.jetKeyDown = true;
        //        player.Velocity += player.jetForce;
        //    }

        //    if (player.Velocity.X > player.maxVelocity.X) player.Velocity.X = player.maxVelocity.X;
        //    if (player.Velocity.X < -player.maxVelocity.X) player.Velocity.X = -player.maxVelocity.X;
        //    if (player.Velocity.Y < -player.maxVelocity.Y) player.Velocity.Y = -player.maxVelocity.Y;

        //    if (!moveRequest)
        //    {
        //        if (player.Velocity.X < 0) player.Velocity.X += player.decX;
        //        if (player.Velocity.X > 0) player.Velocity.X -= player.decX;
        //        // Deceleration may produce a speed that is greater than zero but
        //        // smaller than the smallest unit of deceleration. These lines ensure
        //        // that the player does not keep travelling at slow speed forever after
        //        // decelerating.
        //        if (player.Velocity.X > 0 && player.Velocity.X < player.decX) player.Velocity.X = 0;
        //        if (player.Velocity.X < 0 && player.Velocity.X > -player.decX) player.Velocity.X = 0;
        //    }

        //    // GRAVITY //
        //    player.Velocity.Y += player.gravityAccel;

        //    // UPDATE FUEL HUD
        //    player.fuelFill.Width = player.fuel * 2;


        //    player.UpdateBoundingBoxes();
        //}

        //private void UpdateCollision()
        //{
        //    bool hitSomething = false;

        //    player.CollisionTop.DebugRectColor = Color.Red;
        //    player.CollisionBottom.DebugRectColor = Color.Red;
        //    player.CollisionLeft.DebugRectColor = Color.Red;
        //    player.CollisionRight.DebugRectColor = Color.Red;

        //    // for each of the collision solids in the environment..
        //    for (int i = 0; i < collisionSolids.Count; i++)
        //    {
        //        //Console.WriteLine("Player" + player.BoundingBox.Position);
        //        //Console.WriteLine("Solid" + collisionSolids[i].Position);

        //        // check to see if player's general bounding box is colliding
        //        if (player.BoundingBox.Intersects(collisionSolids[i].BoundingBox))
        //        {
        //            Console.WriteLine("Bounding Box Intersection");
        //            player.BoundingBox.DebugRectColor = Color.Yellow;
        //            hitSomething = true;

        //            // run the player's collision area checks and adjust position accordingly
        //            player.HandleCollisionWithSolid(collisionSolids[i].BoundingBox);
        //        }
                
        //        if (!hitSomething)    
        //            player.BoundingBox.DebugRectColor = Color.Red;
        //    }
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

            // draw velocity
            string playerVel = player.Velocity.ToString();
            Vector2 velFontPosition = new Vector2(graphics.GraphicsDevice.Viewport.TitleSafeArea.X + 10, graphics.GraphicsDevice.Viewport.TitleSafeArea.Y + 40);
            _spriteBatch.DrawString(Font1, playerVel, velFontPosition, Color.White);

            // Draw Player
            player.Draw(_spriteBatch);

            // HUD
            _spriteBatch.Draw(pixel, player.fuelFill, Color.Red);
            DrawBorder(player.fuelOutline, 2, Color.White); 

            // draw player debug rect
            if (debugBoxesOn)
            {
                DrawBorder(player.BoundingBox.DebugRect, 2, player.BoundingBox.DebugRectColor);
                DrawBorder(player.CollisionTop.DebugRect, 1, player.CollisionTop.DebugRectColor);
                DrawBorder(player.CollisionBottom.DebugRect, 1, player.CollisionBottom.DebugRectColor);
                DrawBorder(player.CollisionLeft.DebugRect, 1, player.CollisionLeft.DebugRectColor);
                DrawBorder(player.CollisionRight.DebugRect, 1, player.CollisionRight.DebugRectColor);
            }

            //enemy.Draw(_spriteBatch);

            for (int i = 0; i < collisionSolids.Count; i++)
            {
                collisionSolids[i].Draw(_spriteBatch);

                // draw debug rectangles
                if (debugBoxesOn)
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
