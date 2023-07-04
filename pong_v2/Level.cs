using Microsoft.Xna.Framework;

namespace SpaceShooter
{
    internal abstract class Level
    {
        private int totalenemies = 10;

        public Level(int level)
        {
        }

        public abstract void Update(GameWindow window, GameTime gameTime);

        //variables
        public virtual int Totalenemies
        { get { return totalenemies; } set { totalenemies = value; } }
    }

    internal class Level1 : Level
    {
        public readonly int totalenemies = 20;
        public readonly int amountmines = 7;
        public readonly int amounttripods = 5;
        public readonly int amountshooters = 3;
        public readonly int amountrocks = 5;
        public readonly int amountwalls = 6;
        public bool bossActive = false;

        public Level1(int level) : base(1)
        {
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
        }

        //variables
    }

    internal class Level2 : Level
    {
        public readonly int totalenemies = 25;
        public readonly int amountmines = 3;
        public readonly int amounttripods = 2;
        public readonly int amountshooters = 3;
        public readonly int amountrocks = 5;
        public readonly int amountwalls = 6;
        public bool bossActive = false;

        public Level2(int level) : base(1)
        {
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
        }

        //variables
        public override int Totalenemies
        { get { return totalenemies; } }

        public int Amountmines
        { get { return amountmines; } }

        public int Amounttripods
        { get { return amounttripods; } }

        public int Amountshooters
        { get { return amountshooters; } }

        public int Amountrocks
        { get { return amountrocks; } }
    }
}