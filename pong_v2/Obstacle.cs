using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter
{
    internal abstract class Obstacle : PhysicalObject
    {
        public Rectangle[] sourceRectangles;
        public Rectangle rectangle;
        public int rectangletype;

        public Obstacle(Texture2D texture, float X, float Y, float speedX, float speedY) : base(texture, X, Y, speedX, speedY)
        {
            this.texture = texture;
            rectangle = new Rectangle(3, 85, 40, 30);
            sourceRectangles = new Rectangle[2];
            sourceRectangles[0] = rectangle;
            this.vector.X = X;
            this.vector.Y = Y;
        }

        public abstract void Update(GameWindow window, GameTime gameTime);

        protected int health = 1;

        public virtual int Health
        { get { return health; } set { health = value; } }

        public virtual Texture2D Texture
        { get { return texture; } set { texture = value; } }

        public virtual Rectangle Rectangle
        { get { return rectangle; } set { rectangle = value; } }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, sourceRectangles[rectangletype], Color.White);
        }
    }

    internal class Wall : Obstacle
    {
        public Wall(Texture2D texture, float X, float Y) : base(texture, X, Y, 0f, 1f)
        {
            sourceRectangles = new Rectangle[2];
            this.texture = texture;
            rectangle = new Rectangle(0, 0, 162, 34);
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            sourceRectangles[0] = rectangle;
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
    }

    internal class WallY : Obstacle
    {
        public WallY(Texture2D texture, float X, float Y) : base(texture, X, Y, 0f, 1f)
        {
            sourceRectangles = new Rectangle[2];
            this.texture = texture;
            rectangle = new Rectangle(0, 0, 47, 106);
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            sourceRectangles[0] = rectangle;
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
    }
}