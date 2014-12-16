using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    class ParticleGenerator
    {
        Texture2D texture;

        float spawnWidth;
        float density;

        List<Raindrops> raindrops = new List<Raindrops>();

        float timer;

        Random rand1, rand2;

        public ParticleGenerator(Texture2D newTexture, float newSpawnWidth, float newDensity)
        {
            texture = newTexture;
            spawnWidth = newSpawnWidth;
            density = newDensity;
            rand1 = new Random();
            rand2 = new Random();
        }

        public void createParticle()
        {
            double anything = rand1.Next();

            raindrops.Add(new Raindrops(texture, new Vector2(-50 + (float)rand1.NextDouble() * spawnWidth, 0), new Vector2(1, rand2.Next(5, 8))));
        }

        public void setDensity(float inDense)
        {
            density = inDense;
        }

        public void Update(GameTime gameTime, GraphicsDevice graphics)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            while (timer > 0)
            {
                timer -= 1f / density;
                createParticle();
            }

            for (int i = 0; i < raindrops.Count; i++)
            {
                raindrops[i].Update();

                if (raindrops[i].Position.Y > graphics.Viewport.Height)
                {
                    raindrops.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Raindrops raindrop in raindrops)
                raindrop.Draw(spriteBatch);
        }
    }
}
