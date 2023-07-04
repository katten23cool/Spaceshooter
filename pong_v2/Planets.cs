using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter
{
    internal class GasGiant : Obstacle
    {
        private int pictureX = 0;
        private int pictureY = 0;
        private int currentimage = 0;
        private new Rectangle[] sourceRectangles;
        private new Rectangle rectangle;
        private double timeSinceLastimage = 0;
        private Vector2 origin;

        public GasGiant(Texture2D texture, float X, float Y) : base(texture, X, Y, 0f, 1f)
        {
            this.texture = texture;
            rectangle = new Rectangle(0, 0, 1000, 500);
            sourceRectangles = new Rectangle[1];
            sourceRectangles[0] = new Rectangle(0, 0, 100, 100);
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            vector.X += speed.X;
            vector.Y += speed.Y;
            sourceRectangles[0] = new Rectangle(pictureX, pictureY, 100, 100);
            if (currentimage < 800 && gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastimage + 200)
            {
                currentimage++;
                pictureX += 100;
                if (currentimage % 20 == 0)
                {
                    pictureX = 0;
                    pictureY += 100;
                }

                if (currentimage == 800)
                {
                    currentimage = 0;
                    pictureY = 0;
                    pictureX = 0;
                }
                timeSinceLastimage = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (vector.Y > window.ClientBounds.Height)
            {
                isAlive = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, sourceRectangles[0], Color.White, 0f, origin, 2f, SpriteEffects.None, 0f);
        }
    }

    internal class Galaxy : Obstacle
    {
        private int pictureX = 0;
        private int pictureY = 0;
        private int currentimage = 0;
        private new Rectangle[] sourceRectangles;
        private new Rectangle rectangle;
        private double timeSinceLastimage = 0;
        private Vector2 origin;

        public Galaxy(Texture2D texture, float X, float Y) : base(texture, X, Y, 0f, 1f)
        {
            this.texture = texture;
            rectangle = new Rectangle(0, 0, 1000, 500);
            sourceRectangles = new Rectangle[1];
            sourceRectangles[0] = new Rectangle(0, 0, 150, 150);
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            vector.X += speed.X;
            vector.Y += speed.Y;
            sourceRectangles[0] = new Rectangle(pictureX, pictureY, 150, 150);
            if (currentimage < 800 && gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastimage + 100)
            {
                currentimage++;
                pictureX += 150;
                if (currentimage % 20 == 0)
                {
                    pictureX = 0;
                    pictureY += 150;
                }

                if (currentimage == 800)
                {
                    currentimage = 0;
                    pictureY = 0;
                    pictureX = 0;
                }
                timeSinceLastimage = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (vector.Y > window.ClientBounds.Height)
            {
                isAlive = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, sourceRectangles[0], Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}