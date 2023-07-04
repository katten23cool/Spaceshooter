using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceShooter
{
    internal class Player : PhysicalObject
    {
        private int points = 0;
        private List<Bullet> bullets;
        private readonly Texture2D bulletTexture;
        private double timeSinceLastBullet = 0;

        private readonly List<Rocket> rockets;
        private readonly Texture2D rocketTexture;
        private double timeSinceLastRocket = 0;

        private readonly List<BouncingBullet> bouncingBullets;
        private readonly Texture2D bouncingBulletTexture;
        private double timeSinceBouncingBullet = 0;

        private readonly List<Bomb> bombs;
        private readonly Texture2D bombTexture;
        private double timeSinceLastBomb = -111110;

        private readonly Rectangle[] sourceRectangles;
        private Rectangle rectangle;

        private int bulletsFired = 0;
        private float elapsedTime = 0f;
        private float timerDuration = 1f;
        private double bulletsFiredPerSecond = 0;
        private int amountRockets;

        public Player(Texture2D texture, float X, float Y, float speedX, float speedY, Texture2D bulletTexture, Texture2D rocketTexture, Texture2D bouncingBulletTexture, Texture2D bombTexture) : base(texture, X, Y, speedX, speedY)
        {
            bullets = new List<Bullet>();
            this.bulletTexture = bulletTexture;

            bombs = new List<Bomb>();
            this.bombTexture = bombTexture;

            rockets = new List<Rocket>();
            this.rocketTexture = rocketTexture;

            bouncingBullets = new List<BouncingBullet>();
            this.bouncingBulletTexture = bouncingBulletTexture;

            sourceRectangles = new Rectangle[1];
            this.texture = texture;
            amountRockets = 10;
        }

        protected int health = 5;

        public int Health
        { get { return health; } set { health = value; } }

        public Rectangle Rectangle
        { get { return rectangle; } set { rectangle = value; } }

        public double BPS
        { get { return bulletsFiredPerSecond; } set { bulletsFiredPerSecond = value; } }

        public void Update(GameWindow window, GameTime gameTime, float DS_remainingTime, float bulletSpeedAmount)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (vector.X <= window.ClientBounds.Width - rectangle.Width && vector.X >= 0 && vector.Y <= window.ClientBounds.Width - rectangle.Width && vector.Y >= 0)
            {
                sourceRectangles[0] = new Rectangle(87, 9, 39, 49);

                rectangle = new Rectangle(87, 9, 39, 50);

                if (keyboardState.IsKeyDown(Keys.W))
                {
                    sourceRectangles[0] = new Rectangle(87, 9, 39, 49);
                    vector.Y -= speed.Y;
                    if (keyboardState.IsKeyDown(Keys.D))
                    {
                        sourceRectangles[0] = new Rectangle(132, 9, 38, 49);
                        rectangle = new Rectangle(49, 9, 39, 50);
                        vector.X += speed.X;
                    }
                    if (keyboardState.IsKeyDown(Keys.A))
                    {
                        sourceRectangles[0] = new Rectangle(49, 9, 38, 49);
                        rectangle = new Rectangle(49, 9, 39, 50);
                        vector.X -= speed.X;
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.S))
                {
                    sourceRectangles[0] = new Rectangle(87, 9, 39, 49);
                    vector.Y += speed.Y;
                    if (keyboardState.IsKeyDown(Keys.D))
                    {
                        sourceRectangles[0] = new Rectangle(132, 9, 38, 49);
                        rectangle = new Rectangle(49, 9, 39, 50);
                        vector.X += speed.X;
                    }
                    if (keyboardState.IsKeyDown(Keys.A))
                    {
                        sourceRectangles[0] = new Rectangle(49, 9, 38, 49);
                        rectangle = new Rectangle(49, 9, 39, 50);
                        vector.X -= speed.X;
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.D))
                {
                    sourceRectangles[0] = new Rectangle(132, 9, 38, 49);
                    rectangle = new Rectangle(49, 9, 39, 50);
                    vector.X += speed.X;
                }
                else if (keyboardState.IsKeyDown(Keys.A))
                {
                    sourceRectangles[0] = new Rectangle(49, 9, 38, 49);
                    rectangle = new Rectangle(49, 9, 39, 50);
                    vector.X -= speed.X;
                }
            }
            if (vector.X < 0)
            {
                vector.X = 0;
            }
            if (vector.X > window.ClientBounds.Width - rectangle.Width)
            {
                vector.X = window.ClientBounds.Width - rectangle.Width;
            }
            if (vector.Y < 0)
            {
                vector.Y = 0;
            }
            if (vector.Y > window.ClientBounds.Height - rectangle.Height)
            {
                vector.Y = window.ClientBounds.Height - rectangle.Height;
            }
            if (keyboardState.IsKeyDown(Keys.R)) //up for down
            {
                if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastRocket + 500 && amountRockets > 0) //bullet spawn time aka shoot faster
                {
                    Rocket temp;

                    temp = new Rocket(rocketTexture, vector.X - 2 + rectangle.Width / 2, vector.Y);
                    rockets.Add(temp);
                    timeSinceLastRocket = gameTime.TotalGameTime.TotalMilliseconds;
                    amountRockets--;
                }
            }
            if (keyboardState.IsKeyDown(Keys.B))
            {
                if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceBouncingBullet + 1000)
                {
                    BouncingBullet temp;

                    temp = new BouncingBullet(bouncingBulletTexture, vector.X - 2 + rectangle.Width / 2, vector.Y, vector - new Vector2(8, 0), new Vector2(2.5f, 0.5f), 3f, 1f);
                    bouncingBullets.Add(temp);
                    timeSinceBouncingBullet = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            if (keyboardState.IsKeyDown(Keys.Space)) // i might change this if you cange which gun you use
            {
                if (keyboardState.IsKeyDown(Keys.Left) && gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + bulletSpeedAmount)
                {
                    Bullet temp;

                    if (DS_remainingTime > 0)
                    {
                        temp = new Bullet(bulletTexture, vector.X - 2 + rectangle.Width / 2, vector.Y, 12,0);
                        bullets.Add(temp);
                        bulletsFired ++;
                    }
                    else
                    {
                        temp = new Bullet(bulletTexture, vector.X - 2 + rectangle.Width / 2, vector.Y, 12, 0);
                        bullets.Add(temp);
                        bulletsFired++;
                    }
                    timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
                }
                else if (keyboardState.IsKeyDown(Keys.Right) && gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + bulletSpeedAmount)
                {
                    Bullet temp;

                    if (DS_remainingTime > 0)
                    {
                        temp = new Bullet(bulletTexture, vector.X - 2 + rectangle.Width / 2, vector.Y, -12, 0);
                        bullets.Add(temp);
                        bulletsFired ++;
                    }
                    else
                    {

                        temp = new Bullet(bulletTexture, vector.X - 2 + rectangle.Width / 2, vector.Y, -12, 0);
                        bullets.Add(temp);
                        bulletsFired++;
                    }
                    timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
                }
                else if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + bulletSpeedAmount)
                {
                    Bullet temp;

                    if (DS_remainingTime > 0)
                    {
                        temp = new Bullet(bulletTexture, vector.X + rectangle.Width / 2 + 8, vector.Y + 40, 0, 12);
                        bullets.Add(temp);
                        temp = new Bullet(bulletTexture, vector.X + rectangle.Width / 2 - 12, vector.Y + 40, 0, 12);
                        bullets.Add(temp);
                        bulletsFired += 2;
                    }
                    else
                    {

                        temp = new Bullet(bulletTexture, vector.X - 2 + rectangle.Width / 2, vector.Y, 0, 12);
                        bullets.Add(temp);
                        bulletsFired++;
                    }
                    timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            if (keyboardState.IsKeyDown(Keys.T))
            {
                if (GameElements.bombCountDown <= 0) //bullet spawn time aka shoot faster
                {
                    Bomb temp;

                    temp = new Bomb(bombTexture, vector.X + rectangle.Width / 2, vector.Y + rectangle.Height / 2, gameTime);
                    bombs.Add(temp);

                    GameElements.bombCountDown = 20000;
                }
            }
            //bullets per second
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // If one second has passed, reset the bulletsFired counter and the timer
            if (elapsedTime >= timerDuration)
            {
                bulletsFiredPerSecond = bulletsFired * 60;
                bulletsFired = 0;
                elapsedTime = 0f;
            }

            //bullets
            foreach (Bullet b in bullets.ToList())
            {
                b.Update(window);

                if (!b.IsAlive)
                {
                    bullets.Remove(b);
                }
            }
            foreach (Rocket r in rockets.ToList())
            {
                r.Update(window, gameTime);

                GameElements.UpdatePlayerRocket(gameTime, new Vector2(1, 1), r);

                if (r.Remove == true)
                {
                    try
                    {
                        rockets.Remove(r);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("removing rocket: " + ex.Message);
                    }
                }
            }
            foreach (BouncingBullet bb in bouncingBullets.ToList())
            {
                bb.Update(window, gameTime);
                if (!bb.IsAlive)
                {
                    bouncingBullets.Remove(bb);
                }
            }
            foreach (Bomb bb in bombs.ToList())
            {
                if (!bb.IsAlive)
                {
                    bombs.Remove(bb);
                }
                bb.Update(window, gameTime);
            }
        }

        public void Reset(float X, float Y, float speedX, float speedY)
        {
            vector.X = X;
            vector.Y = Y;
            speed.X = speedX;
            speed.Y = speedY;

            bullets.Clear();
            rockets.Clear();
            bombs.Clear();
            BouncingBullets.Clear();
            timeSinceLastBullet = 0;
            timeSinceLastBomb = 0;

            points = 0;
            isAlive = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet b in bullets)
            {
                b.Draw(spriteBatch);
            }
            foreach (Rocket r in rockets)
            {
                r.Draw(spriteBatch);
            }
            foreach (BouncingBullet bb in bouncingBullets)
            {
                bb.Draw(spriteBatch);
            }
            foreach (Bomb bb in bombs.ToList())
            {
                bb.Draw(spriteBatch);
            }
            spriteBatch.Draw(texture, vector, sourceRectangles[0], Color.White);
        }

        public int Points
        { get { return points; } set { points = value; } }

        public List<Bullet> Bullets
        { get { return bullets; } }

        public List<Rocket> Rockets
        { get { return rockets; } }

        public List<BouncingBullet> BouncingBullets
        { get { return bouncingBullets; } }

        public List<Bomb> Bombs
        { get { return bombs; } }

        public int AmountRockets
        { get { return amountRockets; } set { amountRockets = value; } }
    }

    internal class Bullet : PhysicalObject
    {
        private Rectangle rectangle;

        public Bullet(Texture2D texture, float X, float Y, int speedX, int SpeedY) : base(texture, X, Y, 0, 12f)
        {
            rectangle = texture.Bounds;
            this.speed.X = speedX;
            this.speed.Y = SpeedY;
        }

        public void Update(GameWindow window)
        {
            vector.Y -= speed.Y;
            vector.X -= speed.X;
            if (vector.Y < 0 || vector.Y > window.ClientBounds.Height)
            {
                isAlive = false;
            }
            if(vector.X < 0 || vector.X > window.ClientBounds.Width)
            {
                isAlive = false;
            }
        }

        protected int health = 1; //piercing?

        public virtual int Health
        { get { return health; } set { health = value; } }

        protected int damage = 1;

        public virtual int Damage
        { get { return damage; } set { damage = value; } }

        public Rectangle Rectangle
        { get { return rectangle; } set { rectangle = value; } }
    }

    internal class Rocket : PhysicalObject
    {
        private Rectangle rectangle;
        private float rotation;
        private Vector2 origin;

        private Rectangle[] sourceRectangles;

        private float remainingTime = 0.5f;
        private int rectangletype = 0;

        private bool remove = false;

        public Rocket(Texture2D texture, float X, float Y) : base(texture, X, Y, 1f, 4f)
        {
            sourceRectangles = new Rectangle[28];
            rectangle = new Rectangle(0, 0, 60, 17);

            sourceRectangles[0] = new Rectangle(876, 0, 60, 17);
            sourceRectangles[1] = new Rectangle(932, 17, 60, 17);
            /*
            sourceRectangles[2] = new Rectangle(74, 17, 29, 37);
            sourceRectangles[3] = new Rectangle(108, 0, 29, 37);
            sourceRectangles[4] = new Rectangle(143, 0, 31, 37);
            sourceRectangles[5] = new Rectangle(179, 0, 30, 37);
            sourceRectangles[6] = new Rectangle(214, 0, 30, 37);
            sourceRectangles[7] = new Rectangle(249, 0, 29, 37);
            sourceRectangles[8] = new Rectangle(284, 0, 30, 38);
            sourceRectangles[9] = new Rectangle(319, 0, 29, 37);
            sourceRectangles[10] = new Rectangle(354, 0, 29, 37);
            sourceRectangles[11] = new Rectangle(389, 0, 29, 37);
            sourceRectangles[12] = new Rectangle(424, 0, 29, 37);
            sourceRectangles[13] = new Rectangle(458, 0, 29, 37);
            sourceRectangles[14] = new Rectangle(493, 0, 29, 37);
            sourceRectangles[15] = new Rectangle(527, 0, 22, 37);
            sourceRectangles[16] = new Rectangle(560, 0, 22, 37);
            sourceRectangles[17] = new Rectangle(591, 0, 22, 37);
            sourceRectangles[18] = new Rectangle(621, 0, 22, 37);
            sourceRectangles[19] = new Rectangle(652, 0, 22, 37);
            sourceRectangles[20] = new Rectangle(682, 0, 22, 37);
            sourceRectangles[21] = new Rectangle(710, 0, 22, 37);
            sourceRectangles[22] = new Rectangle(737, 17, 15, 17);
            sourceRectangles[23] = new Rectangle(767, 17, 15, 17);
            sourceRectangles[24] = new Rectangle(791, 17, 15, 17);
            sourceRectangles[25] = new Rectangle(817, 17, 15, 17);
            sourceRectangles[26] = new Rectangle(840, 17, 15, 17);
            */
        }

        public void Update(GameWindow window, GameTime gameTime)
        {
            origin = new Vector2(sourceRectangles[rectangletype].Width / 2, sourceRectangles[rectangletype].Height / 2);
            if (vector.Y < 0 || vector.Y > window.ClientBounds.Height)
            {
                isAlive = false;
            }

            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            remainingTime -= elapsedSeconds;
            if (isAlive == false)
            {
                remove = true;
                /*
                rotation = 0;
                if (remainingTime <= 0)
                {
                    remainingTime = 0.05f;
                    rectangletype++;
                }
                if (rectangletype > 26)
                {
                    rectangletype = 0;
                    remove = true;
                }
                */
            }
            else
            {
                vector += new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * 10f;
                if (remainingTime <= 0)
                {
                    remainingTime = 0.5f;
                    rectangletype++;
                }
                if (rectangletype > 1)
                {
                    rectangletype = 0;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, sourceRectangles[rectangletype], Color.White, Rotation, origin, 1f, SpriteEffects.None, 0f);
        }

        protected int damage = 10;

        public virtual int Damage
        { get { return damage; } set { damage = value; } }

        public Rectangle Rectangle
        { get { return rectangle; } set { rectangle = value; } }

        public float Rotation
        { get { return rotation; } set { rotation = value; } }

        public bool Remove
        { get { return remove; } set { remove = value; } }

        public float Remaintime
        { get { return remainingTime; } set { remainingTime = value; } }
    }

    internal class BouncingBullet : PhysicalObject
    {
        public Rectangle rectangle;
        public new Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Speed { get; set; }
        public float Radius { get; set; }
        public int AmountBounces = 0;

        public BouncingBullet(Texture2D texture, float X, float Y, Vector2 position, Vector2 velocity, float speed, float radius) : base(texture, X, Y, 3f, 3f)
        {
            Position = position;
            Velocity = velocity;
            Speed = speed;
            Radius = radius;
            rectangle = texture.Bounds;
        }

        public void Update(GameWindow window, GameTime gameTime)
        {
            // Move the bullet according to its velocity and speed
            if (Position.X > window.ClientBounds.Width || Position.X < 0)
            {
                Velocity *= new Vector2(-1, 1);
                AmountBounces++;
            }
            if (Position.Y < 0)
            {
                isAlive = false;
            }
            vector = Position;
            Position -= Velocity * Speed; //* (float)gameTime.ElapsedGameTime.TotalSeconds

            if (AmountBounces > 3)
            {
                isAlive = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, new Vector2(Radius, Radius), 1f, SpriteEffects.None, 0f);
        }

        public Rectangle Rectangle
        { get { return rectangle; } set { rectangle = value; } }
    }

    internal class Bomb : PhysicalObject
    {
        private Rectangle rectangle;
        private Vector2 origin;

        private Rectangle[] sourceRectangles;

        public double remainingTime = 5f;
        private int rectangletype = 0;

        public bool exploding = false;
        public bool exploded = false;
        private double timeSinceLastFrame = 0f;
        public int Damage = 50;

        private int currentimage = 0;
        private int size = 2;

        public Bomb(Texture2D texture, float X, float Y, GameTime gameTime) : base(texture, X, Y, 1f, 4f)
        {
            sourceRectangles = new Rectangle[25];
            rectangle = new Rectangle(0, 0, 30 * 4, 38 * 4);

            sourceRectangles[0] = new Rectangle(843, 4, 11, 13);
            sourceRectangles[1] = new Rectangle(75, 0, 28, 38);
            sourceRectangles[2] = new Rectangle(108, 0, 30, 37);
            sourceRectangles[3] = new Rectangle(143, 0, 30, 38);
            sourceRectangles[4] = new Rectangle(179, 0, 30, 38);
            sourceRectangles[5] = new Rectangle(214, 0, 30, 38);
            sourceRectangles[6] = new Rectangle(249, 0, 30, 38);
            sourceRectangles[7] = new Rectangle(284, 0, 30, 38);
            sourceRectangles[8] = new Rectangle(319, 0, 30, 38);
            sourceRectangles[9] = new Rectangle(354, 0, 30, 38);
            sourceRectangles[10] = new Rectangle(389, 0, 30, 38);
            sourceRectangles[11] = new Rectangle(424, 0, 29, 38);
            sourceRectangles[12] = new Rectangle(459, 0, 28, 38);
            sourceRectangles[13] = new Rectangle(527, 0, 28, 38);
            sourceRectangles[14] = new Rectangle(560, 0, 26, 38);
            sourceRectangles[15] = new Rectangle(591, 0, 25, 38);
            sourceRectangles[16] = new Rectangle(621, 0, 26, 38);
            sourceRectangles[17] = new Rectangle(652, 0, 25, 38);
            sourceRectangles[18] = new Rectangle(682, 0, 23, 38);
            sourceRectangles[19] = new Rectangle(710, 0, 22, 38);
            sourceRectangles[20] = new Rectangle(737, 0, 22, 38);
            sourceRectangles[21] = new Rectangle(764, 0, 22, 38);
            sourceRectangles[22] = new Rectangle(791, 0, 21, 38);
            sourceRectangles[23] = new Rectangle(817, 0, 18, 38);
            sourceRectangles[24] = new Rectangle(861, 0, 13, 38);
        }

        public void Update(GameWindow window, GameTime gameTime)
        {
            float elapsedMilliSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            remainingTime -= elapsedMilliSeconds;
            if (remainingTime < 0)
            {
                remainingTime = 0;
            }
            if (remainingTime <= 0 && !exploding)
            {
                GameElements.bombCountDown = 20000;
                exploded = true;
                exploding = true;
                vector.X -= rectangle.Width / 2;
                vector.Y -= rectangle.Height / 2;
            }
            if (exploding && isAlive)
            {
                size = 4;

                if (rectangletype >= 1)
                {
                    exploded = false;
                }
                if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastFrame + 80)
                {
                    rectangletype += 1;
                    timeSinceLastFrame = gameTime.TotalGameTime.TotalMilliseconds;
                }
                if (rectangletype > 23)
                {
                    isAlive = false;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, sourceRectangles[rectangletype], Color.White, 0f, origin, size, SpriteEffects.None, 0f);
        }

        public double RemainingTime
        { get { return remainingTime; } set { remainingTime = value; } }

        public Rectangle Rectangle
        { get { return rectangle; } set { rectangle = value; } }
    }
}