using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    public class Animation
    {
        int frameCounter;
        int switchFrame;

        bool active;

        Vector2 position, amountofFrames, currentFrame;
        Texture2D image;
        Rectangle sourceRect;

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public Vector2 CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Texture2D AnimationImage
        {
            set { image = value; }
        }

        public int FrameWidth
        {
            get { return image.Width / (int)amountofFrames.X; }
        }

        public int FrameHeight
        {
            get { return image.Height / (int)amountofFrames.Y; }
        }

        public void Initialize(Vector2 position, Vector2 Frames)
        {
            active = false;
            switchFrame = 80;
            this.position = position;
            this.amountofFrames = Frames;
        }

        public void Update(GameTime gameTime)
        {
            if (active)

                frameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            else
                frameCounter = 0;
            if (frameCounter >= switchFrame)
            {
                frameCounter = 0;
                currentFrame.X += FrameWidth;
                if (currentFrame.X >= image.Width)
                    currentFrame.X = 0;
            }
            sourceRect = new Rectangle((int)currentFrame.X, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position, sourceRect, Color.White);
        }
    }
}
