using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Flatulina
{
    // Represents a single animation sequence (group of sub textures)
    using sequence = List<string>;

    // Represents a single image on the texture atlas
    struct SubTexture
    {
        string name;
        Rectangle rect;
    };

    //struct Atlas
    //{
    //    Vector2 size;
    //    string texturePath;
    //    string animationDataPath;
    //};

    class TextureAtlas
    {
        // The image representing the collection of images used for animation
        Texture2D textureAtlas;
        // The scale used to display the atlas
        public float Scale;
        // The time since we last updated the frame
        int elapsedTime;
        // The time we display a frame until the next one
        int frameTime;
        // The index of the current frame we are displaying
        int currentFrame;
        // The color of the frame we will be displaying
        Color color;
        // The area of the atlas we want to display (frame)
        Rectangle sourceRect = new Rectangle();
        // The area where we want to display the frame in the game
        Rectangle destinationRect = new Rectangle();
        // The state of the Animation
        public bool Active;
        // determines if the animation will keep playing or deactivate after one run
        public bool Looping;
        // position on screen?
        public Vector2 Position;


        // Collection of all individual sub textures of the texture atlas
        Dictionary<string, SubTexture> subTextures;
        // Collection of separate animation sequences within texture atlas
        Dictionary<string, sequence> animations;



        public void Initialize(Texture2D texture, Vector2 position, int frameWidth, int frameHeight, 
                                int frameCount, int frametime, Color color, float scale, bool looping)
        {
            // Load Texture Atlas

            // Load Animation Info

            // vvv - From tutorial - vvv ------------------------
            //// Cache passed in values
            //this.color = color;
            //this.FrameWidth = frameWidth;
            //this.FrameHeight = frameHeight;
            //this.frameCount = frameCount;
            //this.Scale = scale;

            //Looping = looping;
            //Position = position;
            //textureAtlas = texture;

            //// Set time to 0
            //elapsedTime = 0;
            //currentFrame = 0;

            //// Set Animation to active by default
            //Active = true;

        }

        public void Update(GameTime gameTime)
        {
            // Do not update the game if we are not active
            if (Active == false) return;
            // Update elapsed time
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            // If elapsed time is larger than the frame time, we need to switch frames
            if (elapsedTime > frameTime)
            {
                // Move to next frame
                currentFrame++;

                //// If the currentFrame is equal to frameCount, reset currentFrame
                //if (currentFrame == frameCount)
                //{
                //    currentFrame = 0;
                    
                //    // If we are not looping, deactivate the animation
                //    if (Looping == false)
                //        Active = false;
                //}

                // Reset elapsed time
                elapsedTime = 0;

            }

            // Grab the correct frame in the texture Atlas 
            

            // Determine the destination Rectangle for drawing location
        }

        public void Draw()
        {
 
        }

    }
}
