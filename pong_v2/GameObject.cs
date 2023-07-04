using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceShooter
{
    internal class GameObject
    {
        protected Texture2D texture;
        protected Vector2 vector;
        //timer

        //Gameobject
        public GameObject(Texture2D texture, float X, float Y)
        {
            this.texture = texture;
            this.vector.X = X;
            this.vector.Y = Y;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, Color.White);
        }

        public float X
        { get { return this.vector.X; } set { vector.X = value; } }

        public float Y
        { get { return this.vector.Y; } set { vector.Y = value; } }

        public Vector2 Position
        { get { return this.vector; } set { vector = value; } }

        public Vector2 SpeedX
        { get { return this.SpeedX; } }

        public Vector2 SpeedY
        { get { return this.SpeedY; } }

        public float Height
        { get { return texture.Height; } }
    }

    internal abstract class MovingObject : GameObject
    {
        protected Vector2 speed;

        public MovingObject(Texture2D texture, float X, float Y, float speedX, float speedY) : base(texture, X, Y)
        {
            this.speed.X = speedX;
            this.speed.Y = speedY;
        }
    }

    internal class PhysicalObject : MovingObject
    {
        protected bool isAlive = true;
        protected bool isoutside = false;

        public PhysicalObject(Texture2D texture, float X, float Y, float speedX, float speedY) : base(texture, X, Y, speedX, speedY)
        {
        }

        public bool CheckCollision(PhysicalObject other, Rectangle Rectangle, Rectangle rectangle2)
        {
            Rectangle myRect = new Rectangle(Convert.ToInt32(X), Convert.ToInt32(Y), Convert.ToInt32(rectangle2.Width), Convert.ToInt32(rectangle2.Height));

            Rectangle otherRect = new Rectangle(Convert.ToInt32(other.X), Convert.ToInt32(other.Y), Convert.ToInt32(Rectangle.Width), Convert.ToInt32(Rectangle.Height));
            return myRect.Intersects(otherRect);
        }

        public bool IsAlive
        { get { return isAlive; } set { isAlive = value; } }

        public bool Isoutside
        { get { return isoutside; } set { isoutside = value; } }
    }
}