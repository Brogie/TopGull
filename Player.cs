using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1
{
    public class Player
    {
        Texture2D playerImage;
        Vector2 playerPosition, tempCurrentFrame;

        KeyboardState keyState;
        float moveSpeed = 200;

        Animation playerAnimation = new Animation();

        public void Initialize()
        {
            playerPosition = new Vector2(10, 10);
            playerAnimation.Initialize(playerPosition, new Vector2(4, 2));
            tempCurrentFrame = Vector2.Zero;
        }

        public void LoadContent(ContentManager Content)
        {
            playerImage = Content.Load<Texture2D>("seagullplaceholder");
            playerAnimation.AnimationImage = playerImage;
        }

        public void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();

            playerPosition = playerAnimation.Position;

            if (keyState.IsKeyDown(Keys.Down))
            {
                moveSpeed = 400;
                if (keyState.IsKeyDown(Keys.Right))
                {
                    moveSpeed = 200;
                    playerPosition.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (keyState.IsKeyDown(Keys.Left))
                {
                    moveSpeed = 200;
                    playerPosition.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                playerPosition.Y += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (keyState.IsKeyDown(Keys.Up))
            {
                moveSpeed = 400;
                if (keyState.IsKeyDown(Keys.Right))
                {
                    moveSpeed = 200;
                    playerPosition.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (keyState.IsKeyDown(Keys.Left))
                {
                    moveSpeed = 200;
                    playerPosition.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                playerPosition.Y -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (keyState.IsKeyDown(Keys.Right))
            {
                moveSpeed = 300;
                playerPosition.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                tempCurrentFrame.Y = 0;
            }
            else if (keyState.IsKeyDown(Keys.Left))
            {
                moveSpeed = 300;
                playerPosition.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                tempCurrentFrame.Y = 1;
            }
            else
                playerAnimation.Active = true;

            tempCurrentFrame.X = playerAnimation.CurrentFrame.X;

            playerAnimation.Position = playerPosition;
            playerAnimation.CurrentFrame = tempCurrentFrame;

            playerAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            playerAnimation.Draw(spriteBatch);
        }
    }
}
