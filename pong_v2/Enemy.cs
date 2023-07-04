using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Linq.Expressions;

namespace SpaceShooter
{
    /// <summary>
    /// Här har vi våra klassar som använder enemy som basklass
    /// 
    /// Ska nog göra så att dem är uppdelade i olika filer beroende på vad för enemy klass det är men det 
    /// kan man göra en annan dag
    /// </summary>
    internal abstract class Enemy : PhysicalObject
    {
        public Rectangle[] sourceRectangles;
        public Rectangle rectangle;
        public readonly int rectangletype = 0;
        private readonly MyTimer _shootTimer;
        public float rotation;
        protected double deathtimer = 0;

        public Enemy(Texture2D texture, float X, float Y, float speedX, float speedY) : base(texture, X, Y, speedX, speedY)
        {
            this.texture = texture;
            rectangle = new Rectangle(3, 85, 40, 30);
            sourceRectangles = new Rectangle[2];
            sourceRectangles[0] = new Rectangle(3, 85, 20, 20);
            this.vector.X = X;
            this.vector.Y = Y;
        }

        public abstract void Update(GameWindow window, GameTime gameTime);

        protected int health = 1;

        public virtual int Health
        { get { return health; } set { health = value; } }

        protected int points = 1;

        public virtual int Points
        { get { return points; } set { points = value; } }

        public virtual Rectangle Rectangle
        { get { return rectangle; } set { rectangle = value; } }

        public virtual float Rotation
        { get { return rotation; } set { rotation = value; } }

        public virtual double Deathtimer
        { get { return deathtimer; } set { deathtimer = value; } }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, sourceRectangles[rectangletype], Color.White);
        }

        public virtual bool CanShoot()
        {
            return _shootTimer.IsTimeUp();
        }

        public virtual void ResetShootTimer()
        {
            _shootTimer.Reset(100);
        }
    }

    internal class Mine : Enemy //all enemies is suppose to be like this one
    {
        private new Rectangle[] sourceRectangles;
        private new Rectangle rectangle;
        private new Rectangle deathRectangle;
        private SoundEffect enemydie1sound;
        private new readonly int points;
        private new double deathtimer = 0;

        public Mine(Texture2D texture, float X, float Y, Vector2 Speedvector, SoundEffect enemydie1sound, int points, int health, Rectangle rectangle, Rectangle deathRectangle) : base(texture, X, Y, 5f, 1f)
        {
            sourceRectangles = new Rectangle[2];
            this.texture = texture;
            this.rectangle = rectangle;
            this.enemydie1sound = enemydie1sound;
            this.points = points;
            this.health = health;
            this.deathRectangle = deathRectangle;
            this.speed.X = Speedvector.X;
            this.speed.Y = Speedvector.Y;
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            vector.X += speed.X;
            if (vector.X > window.ClientBounds.Width - rectangle.Width || vector.X < 0)
            {
                speed.X *= -1;
            }
            vector.Y += speed.Y;
            sourceRectangles[0] = rectangle;
            if (IsAlive == false)
            {
                SoundEffectInstance mySoundEffectInstance = enemydie1sound.CreateInstance();
                mySoundEffectInstance.Volume = 0.3f;
                mySoundEffectInstance.Pitch = 0.2f;
                mySoundEffectInstance.Play();

                sourceRectangles[0] = deathRectangle;
                speed.X = 0;
                speed.Y = 2f;
            }
            if (vector.Y > window.ClientBounds.Height + 50)
            {
                isoutside = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, sourceRectangles[rectangletype], Color.White);
        }

        public override Rectangle Rectangle
        { get { return rectangle; } set { rectangle = value; } }

        public override double Deathtimer
        { get { return deathtimer; } set { deathtimer = value; } }

        public override int Points
        { get { return points; } }
    }

    internal class Tripod : Enemy
    {
        private readonly SoundEffect enemydie1sound;
        private new readonly int points = 3;
        private new double deathtimer = 0;

        public Tripod(Texture2D texture, float X, float Y, SoundEffect enemydie1sound) : base(texture, X, Y, 0f, 3f)
        {
            sourceRectangles = new Rectangle[2];
            this.texture = texture;
            rectangle = new Rectangle(32, 34, 48, 42);
            this.enemydie1sound = enemydie1sound;
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            sourceRectangles[0] = rectangle;
            vector.Y += speed.Y;
            if (IsAlive == false)
            {
                SoundEffectInstance mySoundEffectInstance = enemydie1sound.CreateInstance();
                mySoundEffectInstance.Volume = 0.3f;
                mySoundEffectInstance.Pitch = 0.2f;
                mySoundEffectInstance.Play();

                sourceRectangles[0] = new Rectangle(668, 34, 51, 32);
                speed.Y = 2f;
            }
            if (vector.Y > window.ClientBounds.Height + 50)
            {
                isoutside = true;
            }
        }

        public override double Deathtimer
        { get { return deathtimer; } set { deathtimer = value; } }

        public override int Points
        { get { return points; } }
    }

    internal class Rock : Enemy
    {
        private new int health = 4;
        private new int points = 6;
        private SoundEffect enemydie1sound;
        private new double deathtimer = 0;

        public Rock(Texture2D texture, float X, float Y, SoundEffect enemydie1sound) : base(texture, X, Y, 0f, 2f)
        {
            sourceRectangles = new Rectangle[2];
            this.texture = texture;
            rectangle = new Rectangle(142, 34, 47, 32);
            this.enemydie1sound = enemydie1sound;
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            sourceRectangles[0] = rectangle;
            vector.Y += speed.Y;
            if (IsAlive == false)
            {
                SoundEffectInstance mySoundEffectInstance = enemydie1sound.CreateInstance();
                mySoundEffectInstance.Volume = 0.3f;
                mySoundEffectInstance.Pitch = 0.2f;
                mySoundEffectInstance.Play();

                sourceRectangles[0] = new Rectangle(668, 34, 51, 32);
                speed.Y = 2f;
            }
            if (vector.Y > window.ClientBounds.Height + 50)
            {
                isoutside = true;
            }
        }

        public override int Health
        { get { return health; } set { health = value; } }

        public override double Deathtimer
        { get { return deathtimer; } set { deathtimer = value; } }

        public override int Points
        { get { return points; } }
    }

    internal class Shooter : Enemy
    {
        private SoundEffect enemydie1sound;
        private MyTimer _shootTimer;
        private new readonly int points = 4;
        private new double deathtimer = 0;

        public Shooter(Texture2D texture, float X, float Y, SoundEffect enemydie1sound, MyTimer shootTimer) : base(texture, X, Y, 4f, 0.2f)
        {
            sourceRectangles = new Rectangle[2];
            this.texture = texture;
            rectangle = new Rectangle(568, 34, 32, 32);
            this.enemydie1sound = enemydie1sound;
            _shootTimer = shootTimer;
            _shootTimer.Reset(5000);
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            sourceRectangles[0] = rectangle;
            vector.Y += speed.Y;
            vector.X += speed.X;
            if (vector.X > window.ClientBounds.Width - rectangle.Width || vector.X < 0)
            {
                speed.X *= -1;
            }
            if (IsAlive == false)
            {
                SoundEffectInstance mySoundEffectInstance = enemydie1sound.CreateInstance();
                mySoundEffectInstance.Volume = 0.3f;
                mySoundEffectInstance.Pitch = 0.2f;
                mySoundEffectInstance.Play();

                sourceRectangles[0] = new Rectangle(668, 34, 51, 32);
                speed.X = 0;
                speed.Y = 2f;
            }
            if (vector.Y > window.ClientBounds.Height + 50)
            {
                isoutside = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, sourceRectangles[rectangletype], Color.White);
        }

        public override bool CanShoot()
        {
            return _shootTimer.IsTimeUp();
        }

        public override void ResetShootTimer()
        {
            _shootTimer.Reset(3000);
        }

        private new int health = 1;

        public override int Health
        { get { return health; } set { health = value; } }

        public override double Deathtimer
        { get { return deathtimer; } set { deathtimer = value; } }

        public override int Points
        { get { return points; } }
    }

    /// <summary>
    /// Level 2
    /// </summary>

    internal class Tripod_2 : Enemy
    {
        private readonly SoundEffect enemydie1sound;
        private new readonly int points = 3;
        private new double deathtimer = 0;

        public Tripod_2(Texture2D texture, float X, float Y, SoundEffect enemydie1sound) : base(texture, X, Y, 0f, 4f)
        {
            sourceRectangles = new Rectangle[2];
            this.texture = texture;
            rectangle = new Rectangle(779, 34, 48, 42);
            this.enemydie1sound = enemydie1sound;
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            sourceRectangles[0] = rectangle;
            vector.Y += speed.Y;
            if (IsAlive == false)
            {
                SoundEffectInstance mySoundEffectInstance = enemydie1sound.CreateInstance();
                mySoundEffectInstance.Volume = 0.3f;
                mySoundEffectInstance.Pitch = 0.2f;
                mySoundEffectInstance.Play();

                sourceRectangles[0] = new Rectangle(1414, 34, 51, 32);
                speed.Y = 2f;
            }
            if (vector.Y > window.ClientBounds.Height + 50)
            {
                isoutside = true;
            }
        }

        private new int health = 2;

        public override int Health
        { get { return health; } set { health = value; } }

        public override double Deathtimer
        { get { return deathtimer; } set { deathtimer = value; } }

        public override int Points
        { get { return points; } }
    }

    internal class Rock_2 : Enemy
    {
        private readonly SoundEffect enemydie1sound;
        private new readonly int points = 10;
        private new double deathtimer = 0;

        public Rock_2(Texture2D texture, float X, float Y, SoundEffect enemydie1sound) : base(texture, X, Y, 0f, 3f)
        {
            sourceRectangles = new Rectangle[2];
            this.texture = texture;
            rectangle = new Rectangle(888, 34, 48, 42);
            this.enemydie1sound = enemydie1sound;
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            sourceRectangles[0] = rectangle;
            vector.Y += speed.Y;
            if (IsAlive == false)
            {
                SoundEffectInstance mySoundEffectInstance = enemydie1sound.CreateInstance();
                mySoundEffectInstance.Volume = 0.3f;
                mySoundEffectInstance.Pitch = 0.2f;
                mySoundEffectInstance.Play();

                sourceRectangles[0] = new Rectangle(1414, 34, 51, 32);
                speed.Y = 2f;
            }
            if (vector.Y > window.ClientBounds.Height + 50)
            {
                isoutside = true;
            }
        }

        private new int health = 6;

        public override int Health
        { get { return health; } set { health = value; } }

        public override double Deathtimer
        { get { return deathtimer; } set { deathtimer = value; } }

        public override int Points
        { get { return points; } }
    }

    internal class Shooter_2 : Enemy
    {
        private SoundEffect enemydie1sound;
        private MyTimer _shootTimer;
        private new readonly int points = 4;
        private new double deathtimer = 0;

        public Shooter_2(Texture2D texture, float X, float Y, SoundEffect enemydie1sound, MyTimer shootTimer) : base(texture, X, Y, 4f, 0.2f)
        {
            sourceRectangles = new Rectangle[2];
            this.texture = texture;
            rectangle = new Rectangle(1314, 34, 32, 32);
            this.enemydie1sound = enemydie1sound;
            _shootTimer = shootTimer;
            _shootTimer.Reset(5000);
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            sourceRectangles[0] = rectangle;
            vector.Y += speed.Y;
            vector.X += speed.X;
            if (vector.X > window.ClientBounds.Width - rectangle.Width || vector.X < 0)
            {
                speed.X *= -1;
            }
            if (IsAlive == false)
            {
                SoundEffectInstance mySoundEffectInstance = enemydie1sound.CreateInstance();
                mySoundEffectInstance.Volume = 0.3f;
                mySoundEffectInstance.Pitch = 0.2f;
                mySoundEffectInstance.Play();

                sourceRectangles[0] = new Rectangle(1414, 34, 51, 32);
                speed.X = 0;
                speed.Y = 2f;
            }
            if (vector.Y > window.ClientBounds.Height + 50)
            {
                isoutside = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, sourceRectangles[rectangletype], Color.White);
        }

        public override bool CanShoot()
        {
            return _shootTimer.IsTimeUp();
        }

        public override void ResetShootTimer()
        {
            _shootTimer.Reset(2000);
        }

        private new int health = 3;

        public override int Health
        { get { return health; } set { health = value; } }

        public override double Deathtimer
        { get { return deathtimer; } set { deathtimer = value; } }

        public override int Points
        { get { return points; } }
    }

    /// <summary>
    /// Saker som tillhör bossen
    /// </summary>
    internal class Boss1 : Enemy
    {
        private SoundEffect bossdie1sound;
        private new int health = 299;
        private new int points = 100;
        private MyTimer _shootTimer;

        public Boss1(Texture2D texture, float X, float Y, SoundEffect bossdie1sound, MyTimer deadTimer) : base(texture, X, Y, 3.5f, 0.5f)
        {
            sourceRectangles = new Rectangle[2];
            this.texture = texture;
            rectangle = new Rectangle(0, 0, 288, 385);
            sourceRectangles[0] = new Rectangle(0, 0, 288, 385);
            this.bossdie1sound = bossdie1sound;
            _shootTimer = deadTimer;
            _shootTimer.Reset(3000);
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            Bossspeed.Boss1SpeedY = speed.Y;
            Bossspeed.Boss1Speed = speed.X;
            vector.X += speed.X;
            vector.Y += speed.Y;
            if (vector.X > window.ClientBounds.Width - rectangle.Width || vector.X < 0)
            {
                speed.X *= -1;
            }

            if (vector.Y + rectangle.Height / 2 < 0)
            {
                speed.Y = 0.1f;
            }
            if (vector.Y + rectangle.Height / 2 > 0)
            {
                speed.Y = -0.1f;
            }

            if (!IsAlive)
            {
                SoundEffectInstance mySoundEffectInstance = bossdie1sound.CreateInstance();
                mySoundEffectInstance.Volume = 0.3f;
                mySoundEffectInstance.Pitch = 0.2f;

                sourceRectangles[0] = new Rectangle(510, 0, 288, 385);
                speed.X = 0;
            }
            if (health <= 0)
            {
                isAlive = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, sourceRectangles[rectangletype], Color.White);
        }

        public override bool CanShoot()
        {
            return _shootTimer.IsTimeUp();
        }

        public override void ResetShootTimer()
        {
            _shootTimer.Reset(3000);
        }

        public override int Health
        { get { return health; } set { health = value; } }

        public override int Points
        { get { return points; } }
    }

    internal class Boss1_top_turret : Enemy
    {
        private new float rotation;
        private Vector2 origin;
        private MyTimer _shootTimer;

        public Boss1_top_turret(Texture2D texture, float X, float Y, MyTimer shootTimer) : base(texture, X, Y, 1f, 0.5f)
        {
            sourceRectangles = new Rectangle[2];
            this.texture = texture;
            rectangle = new Rectangle(0, 0, 30, 30);
            sourceRectangles[0] = new Rectangle(312, 151, 30, 30);
            _shootTimer = shootTimer;
            _shootTimer.Reset(1000);
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            origin = new Vector2(sourceRectangles[rectangletype].Width / 2, sourceRectangles[rectangletype].Height / 2);
            vector.X += Bossspeed.Boss1Speed;
            vector.Y += Bossspeed.Boss1SpeedY;

            if (health <= 0)
            {
                isAlive = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, sourceRectangles[rectangletype], Color.White, Rotation, origin, 1f, SpriteEffects.None, 1f);
        } //(float)Math.PI / -2.0f

        public override bool CanShoot()
        {
            return _shootTimer.IsTimeUp();
        }

        public override void ResetShootTimer()
        {
            _shootTimer.Reset(1000);
        }

        public override float Rotation
        { get { return rotation; } set { rotation = value; } }
    }

    internal class Boss1_front_turret : Enemy
    {
        private Vector2 origin;
        public new Rectangle rectangle = new Rectangle(0, 0, 48, 52);
        private new int health = 100;
        private new int rectangletype = 0;
        private int timeSinceLastBullet = 0;
        private int timeSincelastshot = 0;
        private MyTimer _shootTimer;
        private bool canshoot = true;
        private int timesShot = 0;

        public Boss1_front_turret(Texture2D texture, float X, float Y, MyTimer shootTimer) : base(texture, X, Y, 1f, 0.5f)
        {
            sourceRectangles = new Rectangle[4];
            this.texture = texture;
            sourceRectangles[0] = new Rectangle(455, 9, 48, 52);
            sourceRectangles[1] = new Rectangle(380, 9, 48, 52);
            sourceRectangles[2] = new Rectangle(302, 9, 48, 52);
            sourceRectangles[3] = new Rectangle(457, 72, 48, 52);
            _shootTimer = shootTimer;
            _shootTimer.Reset(400);
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            vector.X += Bossspeed.Boss1Speed;
            vector.Y += Bossspeed.Boss1SpeedY;

            if (!isAlive)
            {
                rectangletype = 3;
                canshoot = false;
            }
            if (timesShot > 35)
            {
                timesShot = 0;
                canshoot = false;
                _shootTimer.Reset(4000);
                timeSincelastshot = (int)gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + 200 && canshoot)
            {
                if (rectangletype < 2)
                {
                    rectangletype++;
                }
                else
                {
                    rectangletype = 0;
                }

                timeSinceLastBullet = (int)gameTime.TotalGameTime.TotalMilliseconds;
                timesShot++;
            }
            if (gameTime.TotalGameTime.TotalMilliseconds > timeSincelastshot + 4000)
            {
                canshoot = true;
            }

            if (health <= 0)
            {
                isAlive = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, sourceRectangles[rectangletype], Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
        }

        public override bool CanShoot()
        {
            return _shootTimer.IsTimeUp();
        }

        public override void ResetShootTimer()
        {
            _shootTimer.Reset(400);
        }

        public override int Health
        { get { return health; } set { health = value; } }
    }

    internal class Boss1_bullet : PhysicalObject
    {
        private Vector2 origin;
        private Rectangle rectangle;
        private Vector2 velocity;
        private Rectangle[] sourceRectangles;

        public Boss1_bullet(Texture2D texture, float X, float Y, Vector2 velocity) : base(texture, X, Y, 0, 5f)
        {
            sourceRectangles = new Rectangle[1];
            rectangle = new Rectangle(0, 0, 6, 15);
            isAlive = true;
            this.velocity = velocity;
        }

        public void Update(GameWindow window)
        {
            sourceRectangles[0] = new Rectangle(336, 94, 12, 30);
            vector.X += velocity.X;
            vector.Y += velocity.Y;
            if (vector.Y > window.ClientBounds.Height)
            {
                isAlive = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, sourceRectangles[0], Color.White, 0f, origin, 0.5f, SpriteEffects.None, 1f);
        }

        public Rectangle Rectangle
        { get { return rectangle; } set { rectangle = value; } }
    }

    public static class Bossspeed
    {
        public static float Boss1Speed = 0f;
        public static float Boss1SpeedY = 0f;
    }

    /// <summary>
    /// Bullets
    /// </summary>
    internal class ShooterBullet : PhysicalObject
    {
        private Rectangle rectangle;
        private Vector2 velocity;
        private Rectangle[] sourceRectangles;

        public ShooterBullet(Texture2D texture, float X, float Y, Vector2 velocity) : base(texture, X, Y, 0, 2.5f)
        {
            sourceRectangles = new Rectangle[1];
            rectangle = texture.Bounds;
            isAlive = true;
            this.velocity = velocity;
        }

        public void Update(GameWindow window)
        {
            sourceRectangles[0] = new Rectangle(0, 0, 5, 5);
            vector.X += velocity.X;
            vector.Y += velocity.Y;
            if (vector.Y > window.ClientBounds.Height + 50)//window heigy
            {
                isoutside = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, sourceRectangles[0], Color.White, 1f, new Vector2(sourceRectangles[0].Width / 2, sourceRectangles[0].Height / 2), 1f, SpriteEffects.None, 1f);
        }

        public Rectangle Rectangle
        { get { return rectangle; } set { rectangle = value; } }
    }

}