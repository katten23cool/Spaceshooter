using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter
{
    internal abstract class Powerups : PhysicalObject
    {
        public Rectangle[] sourceRectangles;
        public Rectangle rectangle;

        public Powerups(Texture2D texture, float X, float Y, float speedX, float speedY) : base(texture, X, Y, 0, 2f)
        {
            rectangle = new Rectangle(3, 85, 20, 20);
        }

        public abstract void Update(GameWindow window, GameTime gameTime);

        public virtual Rectangle Rectangle
        { get { return rectangle; } set { rectangle = value; } }
    }

    internal class GoldCoin : Powerups
    {
        private new Rectangle[] sourceRectangles;
        private new Rectangle rectangle;
        private float remainingTime = 0.2f;
        private int rectangletype = 0;

        public GoldCoin(Texture2D texture, float X, float Y) : base(texture, X, Y, 0, 2f)
        {
            sourceRectangles = new Rectangle[6];
            this.texture = texture;
            rectangle = new Rectangle(3, 85, 20, 20);
            sourceRectangles[0] = new Rectangle(3, 85, 20, 20);
            sourceRectangles[1] = new Rectangle(28, 85, 16, 25);
            sourceRectangles[2] = new Rectangle(54, 85, 10, 20);
            sourceRectangles[3] = new Rectangle(79, 85, 6, 21);
            sourceRectangles[4] = new Rectangle(100, 85, 10, 21);
            sourceRectangles[5] = new Rectangle(120, 85, 15, 21);
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            vector.Y += speed.Y;
            if (vector.Y > window.ClientBounds.Height)
            {
                isAlive = false;
            }

            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            remainingTime -= elapsedSeconds;

            if (remainingTime <= 0)
            {
                remainingTime = 0.2f;
                rectangletype++;
            }
            if (rectangletype > 5)
            {
                rectangletype = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, sourceRectangles[rectangletype], Color.White);
        }

        public override Rectangle Rectangle
        { get { return rectangle; } set { rectangle = value; } }
    }

    internal class DoubleShot : Powerups
    {
        private new Rectangle[] sourceRectangles;
        private new Rectangle rectangle;
        private readonly int rectangletype = 0;

        public DoubleShot(Texture2D texture, float X, float Y) : base(texture, X, Y, 0, 2f)
        {
            sourceRectangles = new Rectangle[1];
            this.texture = texture;
            rectangle = new Rectangle(0, 0, 23, 23);
            sourceRectangles[0] = new Rectangle(45, 119, 23, 23);
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            vector.Y += speed.Y;
            if (vector.Y > window.ClientBounds.Height)
            {
                isAlive = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, sourceRectangles[rectangletype], Color.White);
        }

        public override Rectangle Rectangle
        { get { return rectangle; } set { rectangle = value; } }
    }

    internal class HealthPoint : Powerups
    {
        private new Rectangle[] sourceRectangles;
        private new Rectangle rectangle;
        private readonly int rectangletype = 0;

        public HealthPoint(Texture2D texture, float X, float Y) : base(texture, X, Y, 0, 2f)
        {
            sourceRectangles = new Rectangle[1];
            this.texture = texture;
            rectangle = new Rectangle(0, 0, 23, 23);
            sourceRectangles[0] = new Rectangle(112, 119, 23, 23);
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            vector.Y += speed.Y;
            if (vector.Y > window.ClientBounds.Height)
            {
                isAlive = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, sourceRectangles[rectangletype], Color.White);
        }

        public override Rectangle Rectangle
        { get { return rectangle; } set { rectangle = value; } }
    }

    internal class MovementSpeed : Powerups
    {
        private new Rectangle[] sourceRectangles;
        private new Rectangle rectangle;
        private readonly int rectangletype = 0;

        public MovementSpeed(Texture2D texture, float X, float Y) : base(texture, X, Y, 0, 2f)
        {
            sourceRectangles = new Rectangle[1];
            this.texture = texture;
            rectangle = new Rectangle(45, 119, 23, 23);
            sourceRectangles[0] = new Rectangle(147, 119, 23, 23);
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            vector.Y += speed.Y;
            if (vector.Y > window.ClientBounds.Height)
            {
                isAlive = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, sourceRectangles[rectangletype], Color.White);
        }

        public override Rectangle Rectangle
        { get { return rectangle; } set { rectangle = value; } }
    }

    internal class BulletSpeed : Powerups
    {
        private new Rectangle[] sourceRectangles;
        private new Rectangle rectangle;
        private readonly int rectangletype = 0;

        public BulletSpeed(Texture2D texture, float X, float Y) : base(texture, X, Y, 0, 2f)
        {
            sourceRectangles = new Rectangle[1];
            this.texture = texture;
            rectangle = new Rectangle(45, 119, 23, 23);
            sourceRectangles[0] = new Rectangle(80, 119, 23, 23);
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            vector.Y += speed.Y;
            if (vector.Y > window.ClientBounds.Height)
            {
                isAlive = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, sourceRectangles[rectangletype], Color.White);
        }

        public override Rectangle Rectangle
        { get { return rectangle; } set { rectangle = value; } }
    }

    internal class RocketAmmo : Powerups
    {
        private new Rectangle[] sourceRectangles;
        private new Rectangle rectangle;
        private readonly int rectangletype = 0;

        public RocketAmmo(Texture2D texture, float X, float Y) : base(texture, X, Y, 0, 2f)
        {
            sourceRectangles = new Rectangle[1];
            this.texture = texture;
            rectangle = new Rectangle(45, 119, 23, 23);
            sourceRectangles[0] = new Rectangle(179, 119, 23, 23);
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            vector.Y += speed.Y;
            if (vector.Y > window.ClientBounds.Height)
            {
                isAlive = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, sourceRectangles[rectangletype], Color.White);
        }

        public override Rectangle Rectangle
        { get { return rectangle; } set { rectangle = value; } }
    }
}