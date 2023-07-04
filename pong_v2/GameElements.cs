using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;

namespace SpaceShooter
{
    internal static class GameElements
    {
        /// <summary>
        /// Här är det lite stökigt men det blev så
        /// </summary>
        public static highState currentHighScoreState;

        public static PauseMenu currentPauseState;

        public static State currentState;

        public static List<Enemy> enemies;

        private static readonly Level1 level1 = new Level1(1);

        private static readonly Level2 level2 = new Level2(2);

        private static Background background;

        private static Texture2D[] backgroundTextures;

        private static List<Boss1_bullet> bossBulletList;

        private static SoundEffect bossdie1sound;

        private static bool bossEnabled = false;

        private static int bossHealth;

        private static List<Enemy> bossList;

        private static Texture2D bossSprite;

        private static Texture2D planetsprite;

        private static Texture2D galaxysprite;

        private static BouncingBullet bouncingBullet;

        private static Bullet bullet;

        private static float bulletSpeed_RemainingTime = 0;

        private static float bulletSpeedAmount = 200;

        private static int currentBackgroundIndex = 0;

        private static int CurrentHighScore;

        private static int currentuser;

        private static Database db = new Database();

        private static float distance = 0;

        private static double buttonTimeElapsed = 0;

        private static float DS_remainingTime = 0;

        public static float bombCountDown = 0;

        private static Texture2D enemieSprite;

        private static SoundEffect enemydie1sound;

        private static string enemytype = "";

        private static int hasdied;

        private static HighScore highscore;

        private static int killed_Enemies;

        private static int levelnum = 1;

        private static Menu menu;

        private static List<Obstacle> obstacles;

        private static Texture2D obstacleSprite;

        private static Menu pauseMenu;

        private static Player player;

        private static List<Powerups> powerUps;

        private static Texture2D powerupSprite;

        private static PrintText printText;

        private static Rocket rockets;

        private static List<ShooterBullet> shooterBulletList;

        private static Texture2D shooterBulletSprite;

        private static double leveltransition = 0;

        public enum highState
        { PrintHighScore, EnterHighScore };

        public enum PauseMenu
        { Menu, Run, Quit }

        //States
        public enum State
        { Menu, Run, HighScore, Quit, HighScoreUpdate, About, Stats, Users, PauseMenu };

        /// <summary>
        /// Skapar dessa saker när man startar applikationen
        /// </summary>
        public static void Initialize()
        {
            highscore = new HighScore(15);
            powerUps = new List<Powerups>();
            enemies = new List<Enemy>();
            bossList = new List<Enemy>();
            shooterBulletList = new List<ShooterBullet>();
            bossBulletList = new List<Boss1_bullet>();
            obstacles = new List<Obstacle>();
        }

        /// <summary>
        /// Detta laddas in när vi startar applikationen
        /// </summary>
        public static void LoadContent(ContentManager content, GameWindow window)
        {

            //skapa spelaren & sånt
            player = new Player(content.Load<Texture2D>("images/player/playership"), window.ClientBounds.Width / 2, window.ClientBounds.Height - 30, 4.5f, 5.5f, content.Load<Texture2D>("images/player/bullet"), content.Load<Texture2D>("images/player/rocket/rocket"), content.Load<Texture2D>("images/player/bouncingbullet"), content.Load<Texture2D>("images/player/bomb"));
            bullet = new Bullet(content.Load<Texture2D>("images/player/bullet"), 400, 300, 0, 12);
            rockets = new Rocket(content.Load<Texture2D>("images/player/rocket/rocket"), 400, 300);
            bouncingBullet = new BouncingBullet(content.Load<Texture2D>("images/player/bouncingbullet"), 400, 300, new Vector2(0, 0), new Vector2(0, 0), 3f, 0f);

            //Sprites
            enemieSprite = content.Load<Texture2D>("images/enemies/enemies");
            bossSprite = content.Load<Texture2D>("images/enemies/bosses/boss1");
            shooterBulletSprite = content.Load<Texture2D>("images/enemies/bullet1");
            powerupSprite = content.Load<Texture2D>("images/powerups/sprite");
            obstacleSprite = content.Load<Texture2D>("images/obstacles/basic_obstacles");
            planetsprite = content.Load<Texture2D>("images/Planets/gas");
            galaxysprite = content.Load<Texture2D>("images/Planets/galaxy");

            backgroundTextures = new Texture2D[]
            {
                content.Load<Texture2D>("images/background4"),
                content.Load<Texture2D>("images/background5"),
                content.Load<Texture2D>("images/background"),
                content.Load<Texture2D>("images/background2"),
            };
            background = new Background(backgroundTextures[currentBackgroundIndex], window);

            //menu items
            printText = new PrintText(content.Load<SpriteFont>("highFont"));
            menu = new Menu((int)State.Menu);
            menu.AddItem(content.Load<Texture2D>("images/menu/start"), (int)State.Run, window);
            menu.AddItem(content.Load<Texture2D>("images/menu/highscore"), (int)State.HighScore, window);
            menu.AddItem(content.Load<Texture2D>("images/menu/About"), (int)State.About, window);
            menu.AddItem(content.Load<Texture2D>("images/menu/statistic"), (int)State.Stats, window);
            menu.AddItem(content.Load<Texture2D>("images/menu/spelare"), (int)State.Users, window);
            menu.AddItem(content.Load<Texture2D>("images/menu/exit"), (int)State.Quit, window);

            //menu pause
            pauseMenu = new Menu((int)State.PauseMenu);
            pauseMenu.AddItem(content.Load<Texture2D>("images/menu/start"), (int)PauseMenu.Run, window);
            pauseMenu.AddItem(content.Load<Texture2D>("images/menu/mainmenu"), (int)PauseMenu.Menu, window);

            //sounds
            enemydie1sound = content.Load<SoundEffect>("sounds/enemies/explosion");
            bossdie1sound = content.Load<SoundEffect>("sounds/enemies/explosionboss1");
        }

        /// <summary>
        /// Här skapar vi allt som ska finnas i spelet
        /// </summary>
        public static void GenerateEnemies(GameWindow window)
        {
            if (levelnum == 1)
            {
                Random random_DS = new Random();
                int newDS = random_DS.Next(1, 2000);
                if (newDS == 174)
                {
                    int rndX = random_DS.Next(-50, window.ClientBounds.Width);

                    obstacles.Add(new Galaxy(galaxysprite, rndX, -350));
                }
                if (enemies.Count < level1.Totalenemies)
                {
                    int AmountMine = 0;
                    int AmountTripod = 0;
                    int AmountShooter = 0;
                    int AmountRock = 0;

                    foreach (Enemy enemy in enemies.ToList())
                    {
                        try
                        {
                            string enemytype = TypeDescriptor.GetClassName(enemy);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error getting enemy class name: " + ex.Message);
                        }
                        if (enemytype == "SpaceShooter.Mine")
                        {
                            AmountMine++;
                        }
                        else if (enemytype == "SpaceShooter.Tripod")
                        {
                            AmountTripod++;
                        }
                        else if (enemytype == "SpaceShooter.Shooter")
                        {
                            AmountShooter++;
                        }
                        else if (enemytype == "SpaceShooter.Rock")
                        {
                            AmountRock++;
                        }
                    }
                    Random location = new Random();
                    int rng = location.Next(1, 100);
                    if (AmountMine < level1.amountmines && rng == 42)
                    {
                        int rndX = location.Next(0, window.ClientBounds.Width - 60);
                        Mine temp = new Mine(enemieSprite, rndX, -50, new Vector2(4f,1f) ,enemydie1sound, 4, 4, new Rectangle(360, 34, 44, 32), new Rectangle(668, 34, 51, 32));
                        enemies.Add(temp);
                    }
                    if (AmountTripod < level1.amounttripods && rng == 7)
                    {
                        int rndX = location.Next(0, window.ClientBounds.Width - 60);
                        Tripod temp = new Tripod(enemieSprite, rndX, -50, enemydie1sound);
                        enemies.Add(temp);
                    }
                    if (AmountShooter < level1.amountshooters && rng == 22)
                    {
                        int rndX = location.Next(0, window.ClientBounds.Width - 60);
                        Shooter temp = new Shooter(enemieSprite, rndX, -50, enemydie1sound, new MyTimer());
                        enemies.Add(temp);
                    }
                    if (AmountRock < level1.amountrocks && rng == 57)
                    {
                        int rndX = location.Next(0, window.ClientBounds.Width - 60);
                        Rock temp = new Rock(enemieSprite, rndX, -50, enemydie1sound);
                        enemies.Add(temp);
                    }
                }
                int newDSi = random_DS.Next(1, 400);
                if (newDSi == 6)
                {
                    int rndX = random_DS.Next(0, window.ClientBounds.Width - 200);
                    obstacles.Add(new Wall(obstacleSprite, rndX, -50));
                    obstacles.Add(new Wall(obstacleSprite, rndX + 162, -50));
                    obstacles.Add(new Wall(obstacleSprite, rndX + 162 * 2, -50));
                }
                if (player.Points >= 100)
                {
                    if (bossEnabled == false)
                    {
                        bossEnabled = true;

                        int rndX = window.ClientBounds.Width / 2 - 144;
                        bossList.Add(new Boss1(bossSprite, rndX, -450, bossdie1sound, new MyTimer()));
                        bossList.Add(new Boss1_top_turret(bossSprite, rndX + 145, -152, new MyTimer()));
                        bossList.Add(new Boss1_front_turret(bossSprite, rndX + 216, -76, new MyTimer()));
                        bossList.Add(new Boss1_front_turret(bossSprite, rndX + 24, -76, new MyTimer()));
                        bossHealth = 500;
                    }
                }
            }
            else if (levelnum == 2 && leveltransition <= 0)
            {
                Random random_DS = new Random();
                int newDS = random_DS.Next(1, 2000);
                if (newDS == 3)
                {
                    int rndX = random_DS.Next(-50, window.ClientBounds.Width);

                    obstacles.Add(new GasGiant(planetsprite, rndX, -350));
                }
                if (enemies.Count < level2.Totalenemies)
                {
                    int AmountMine = 0;
                    int AmountTripod = 0;
                    int AmountShooter = 0;
                    int AmountRock = 0;

                    foreach (Enemy enemy in enemies.ToList())
                    {
                        try
                        {
                            string enemytype = TypeDescriptor.GetClassName(enemy);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error getting enemy class name: " + ex.Message);
                        }
                        if (enemytype == "SpaceShooter.Mine")
                        {
                            AmountMine++;
                        }
                        else if (enemytype == "SpaceShooter.Tripod")
                        {
                            AmountTripod++;
                        }
                        else if (enemytype == "SpaceShooter.Shooter")
                        {
                            AmountShooter++;
                        }
                        else if (enemytype == "SpaceShooter.Rock")
                        {
                            AmountRock++;
                        }
                    }
                    Random location = new Random();
                    int rng = location.Next(1, 100);
                    if (AmountMine < level2.amountmines && rng == 42)
                    {
                        int rndX = location.Next(0, window.ClientBounds.Width - 60);
                        Mine temp = new Mine(enemieSprite, rndX, -50, new Vector2(5f, 1.2f), enemydie1sound, 4, 4, new Rectangle(1106, 34, 44, 32), new Rectangle(1414, 34, 51, 32));
                        enemies.Add(temp);
                    }
                    if (AmountTripod < level2.amounttripods && rng == 7)
                    {
                        int rndX = location.Next(0, window.ClientBounds.Width - 60);
                        Tripod_2 temp = new Tripod_2(enemieSprite, rndX, -50, enemydie1sound);
                        enemies.Add(temp);
                    }
                    if (AmountShooter < level2.amountshooters && rng == 22)
                    {
                        int rndX = location.Next(0, window.ClientBounds.Width - 60);
                        Shooter_2 temp = new Shooter_2(enemieSprite, rndX, -50, enemydie1sound, new MyTimer());
                        enemies.Add(temp);
                    }
                    if (AmountRock < level2.amountrocks && rng == 57)
                    {
                        int rndX = location.Next(0, window.ClientBounds.Width - 60);
                        Rock_2 temp = new Rock_2(enemieSprite, rndX, -50, enemydie1sound);
                        enemies.Add(temp);
                    }
                }
                int newDW = random_DS.Next(1, 300);
                if (newDW == 1)
                {
                    int rndX = random_DS.Next(0, window.ClientBounds.Width - 200);
                    obstacles.Add(new Wall(obstacleSprite, rndX, -50));
                    obstacles.Add(new Wall(obstacleSprite, rndX + 162, -50));
                    obstacles.Add(new Wall(obstacleSprite, rndX + 162 * 2, -50));
                }
                if (player.Points >= 2000)
                {
                    level2.bossActive = true;
                }
            }
        }

        public static void GenerateEnemyProjectiles(GameTime gameTime)
        {
            foreach (Enemy enemy in enemies.ToList())
            {
                if (enemy != null)
                {
                    enemytype = TypeDescriptor.GetClassName(enemy);
                    if (enemytype == "SpaceShooter.Shooter")
                    {
                        if (enemy.CanShoot())
                        {
                            ShooterBullet temp;
                            temp = new ShooterBullet(shooterBulletSprite, enemy.X + enemy.rectangle.Width / 2 + 8, enemy.Y + 40, new Vector2(0, 4f));
                            shooterBulletList.Add(temp);
                            enemy.ResetShootTimer();
                        }
                    }
                    else if (enemytype == "SpaceShooter.Shooter_2")
                    {
                        if (enemy.CanShoot())
                        {
                            ShooterBullet temp;
                            temp = new ShooterBullet(shooterBulletSprite, enemy.X + enemy.rectangle.Width / 2 + 8, enemy.Y + 40, new Vector2(0, 7f));
                            shooterBulletList.Add(temp);
                            enemy.ResetShootTimer();
                        }
                    }
                }
            }
        }

        public static void GeneratePowerUps(float X, float Y)
        {
            Random random_DS = new Random();
            int newDS = random_DS.Next(1, 20);
            if (newDS == 1)
            {
                powerUps.Add(new DoubleShot(powerupSprite, X, Y));
            }
            //bullet speed powerup
            if (newDS == 8)
            {
                powerUps.Add(new BulletSpeed(powerupSprite, X, Y));
            }
            //power up health
            if (newDS == 14)
            {
                powerUps.Add(new HealthPoint(powerupSprite, X, Y));
            }
            //coin
            if (newDS == 2 || newDS == 17)
            {
                powerUps.Add(new GoldCoin(powerupSprite, X, Y));
            }
            //rocketammo
            if (newDS == 10)
            {
                powerUps.Add(new RocketAmmo(powerupSprite, X, Y));
            }
        }

        public static void RunDraw(SpriteBatch spriteBatch, GameWindow window)
        {
            //draw the stuuf
            background.Draw(spriteBatch);
            foreach (Obstacle ob in obstacles.ToList())
            {
                ob.Draw(spriteBatch);
            }
            foreach (Enemy e in enemies.ToList())
            {
                e?.Draw(spriteBatch); //while not nul

                if (!e.IsAlive)
                {
                    printText.Print("+" + e.Points, spriteBatch, Convert.ToInt16(e.X - 10), Convert.ToInt16(e.Y - 30), Color.Yellow);
                }
            }
            foreach (Bomb bomb in player.Bombs.ToList())
            {
                if (!bomb.exploding)
                {
                    printText.Print(bomb.RemainingTime.ToString("F1"), spriteBatch, Convert.ToInt16(bomb.Position.X - 10), Convert.ToInt16(bomb.Position.Y - 30), Color.Red);
                }
            }
            player.Draw(spriteBatch);
            foreach (Powerups gc in powerUps)
            {
                gc.Draw(spriteBatch);
            }
            foreach (Enemy e in bossList.ToList())
            {
                e?.Draw(spriteBatch);
            }
            foreach (ShooterBullet e in shooterBulletList.ToList())
            {
                e?.Draw(spriteBatch);
            }
            foreach (Boss1_bullet e in bossBulletList.ToList())
            {
                e?.Draw(spriteBatch);
            }

            //user information
            printText.Print("Points: " + player.Points, spriteBatch, 0, 0, Color.White);
            printText.Print("HP: " + player.Health, spriteBatch, 0, 30, Color.White);
            printText.Print("Rockets: " + player.AmountRockets, spriteBatch, 0, 60, Color.White);
            if (DS_remainingTime >= 0)
            {
                float DB_print = DS_remainingTime / 1000;
                printText.Print("DB: " + DB_print.ToString("F1"), spriteBatch, 0, 90, Color.White);
            }
            if (bulletSpeed_RemainingTime >= 0)
            {
                float DB_print = bulletSpeed_RemainingTime / 1000;
                printText.Print("BS: " + DB_print.ToString("F1"), spriteBatch, 0, 120, Color.White);
            }
            if (bombCountDown >= 0)
            {
                float DB_print = bombCountDown / 1000;
                printText.Print("Bomb: " + DB_print.ToString("F1"), spriteBatch, 0, 150, Color.White);
            }
            if (bossHealth > 0)
            {
                printText.Print("Boss health: " + bossHealth, spriteBatch, 0, 180, Color.White);
            }

            int developormode = 0;
            //extra informeation stuff ig
            if (developormode == 1)
            {
                printText.Print("BPS: " + player.BPS, spriteBatch, 0, 210, Color.White);
                printText.Print("Bulletspeed: " + bulletSpeedAmount, spriteBatch, 0, 150, Color.White);
                printText.Print("Distance: " + distance.ToString("F1"), spriteBatch, 500, 0, Color.White);
            }

            if (leveltransition > 0)
            {
                printText.Print("Level: " + levelnum, spriteBatch, window.ClientBounds.Width / 2 - 20, window.ClientBounds.Height / 2, Color.White);
            }
        }

        /// <summary>
        /// Här har vi alla update sakerna
        /// använder dessa för att updatera allt i spelet
        /// Runupdate är den som är viktigast då den används för att updatera alla andra updates 
        /// </summary>
        public static State RunUpdate(GameWindow window, GameTime gameTime)
        {
            //updates
            background.Update(window);
            player.Update(window, gameTime, DS_remainingTime, bulletSpeedAmount);
            UpdatePowerUps(window, gameTime);
            UpdatePlayerBomb(window, gameTime);
            UpdateEnemies(window, gameTime);
            UpdateObstacles(window, gameTime);
            Timers(gameTime);

            //generate stuff
            GenerateEnemies(window);
            GenerateEnemyProjectiles(gameTime);
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                if (gameTime.TotalGameTime.TotalMilliseconds > buttonTimeElapsed + 200)
                {
                    buttonTimeElapsed = gameTime.TotalGameTime.TotalMilliseconds;
                    return State.PauseMenu;
                }
            }
            //player collision with BB
            foreach (BouncingBullet b in player.BouncingBullets.ToList())
            {
                if (player.CheckCollision(b, b.Rectangle, player.Rectangle))
                {
                    Vector2 normal = Vector2.Zero;
                    if (b.Position.Y < player.Y + player.Rectangle.Height)
                    {
                        normal += Vector2.UnitY;
                    }
                    else if (b.Position.Y > player.Y)
                    {
                        normal -= Vector2.UnitY;
                    }
                    else if (b.Position.X < player.X + player.Rectangle.Width)
                    {
                        normal += Vector2.UnitX;
                    }
                    else if (b.Position.X > player.X)
                    {
                        normal -= Vector2.UnitX;
                    }

                    b.Velocity = Vector2.Reflect(b.Velocity, normal);

                    b.Speed *= 0.8f;

                    player.BouncingBullets.Remove(b);
                    player.Health -= 1;
                    if (player.Health <= 0)
                    {
                        player.IsAlive = false;
                    }
                }
            }

            //player alive
            if (!player.IsAlive)
            {
                int local_killed_Enemies = 0;
                string q_select = "select * from player";
                SQLiteCommand dbCommand = new SQLiteCommand(q_select, db.dbConn);
                db.OpenConn();
                //få resultat
                SQLiteDataReader result = dbCommand.ExecuteReader();
                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        if (Convert.ToInt16(result["id"]) == currentuser)
                        {
                            CurrentHighScore = Convert.ToInt16(result["highscore"]);
                            local_killed_Enemies = Convert.ToInt16(result["killed_enemies"]);
                        }
                    }
                }
                db.CloseConn();
                int localhighscore = CurrentHighScore;
                if (CurrentHighScore < player.Points)
                {
                    localhighscore = player.Points;
                }
                local_killed_Enemies += killed_Enemies;
                //updatera
                string q_update = "UPDATE player SET highscore = " + localhighscore + ", killed_enemies = " + local_killed_Enemies + " WHERE id = " + currentuser + ";";
                SQLiteCommand dbComman = new SQLiteCommand(q_update, db.dbConn);
                db.OpenConn();
                int resul = dbComman.ExecuteNonQuery();
                db.CloseConn();

                hasdied = 1;
                Reset(window);
                return State.HighScore;
            }
            return State.Run;
        }

        public static void UpdateEnemies(GameWindow window, GameTime gameTime)
        {
            foreach (Enemy enemy in enemies.ToList())
            {
                if (enemy.Isoutside)
                {
                    enemies.Remove(enemy);
                }
                if (gameTime.TotalGameTime.TotalMilliseconds > enemy.Deathtimer + 200 && !enemy.IsAlive)
                {
                    player.Points += enemy.Points;
                    enemies.Remove(enemy);
                    killed_Enemies++;
                    GeneratePowerUps(enemy.Position.X, enemy.Position.Y);
                }
                if (enemy != null && enemy.IsAlive == true)
                {
                    foreach (Bullet b in player.Bullets.ToList())
                    {
                        if (enemy.CheckCollision(b, bullet.Rectangle, enemy.Rectangle))
                        {
                            enemy.Health -= bullet.Damage;
                            if (enemy.Health <= 0)
                            {
                                enemy.IsAlive = false;
                            }
                            b.Health -= 1;
                            if (b.Health <= 0)
                            {
                                b.IsAlive = false;
                            }
                        }
                    }
                    foreach (Rocket r in player.Rockets.ToList())
                    {
                        if (r.IsAlive == true)
                        {
                            if (enemy.CheckCollision(r, rockets.Rectangle, enemy.Rectangle))
                            {
                                enemy.Health -= rockets.Damage;
                                if (enemy.Health <= 0)
                                {
                                    enemy.IsAlive = false;
                                }
                                r.IsAlive = false;
                            }
                        }
                    }
                    foreach (BouncingBullet b in player.BouncingBullets.ToList())
                    {
                        if (enemy.CheckCollision(b, b.Rectangle, enemy.Rectangle))
                        {
                            Vector2 normal = Vector2.Zero;
                            if (b.Position.Y < enemy.Y + enemy.Rectangle.Height)
                            {
                                normal += Vector2.UnitY;
                            }
                            else if (b.Position.Y > enemy.Y)
                            {
                                normal -= Vector2.UnitY;
                            }
                            else if (b.Position.X < enemy.X + enemy.Rectangle.Width)
                            {
                                normal += Vector2.UnitX;
                            }
                            else if (b.Position.X > enemy.X)
                            {
                                normal -= Vector2.UnitX;
                            }

                            b.Velocity = Vector2.Reflect(b.Velocity, normal);

                            b.Speed *= 0.8f;

                            b.AmountBounces++;

                            enemy.Health -= 1;
                            if (enemy.Health <= 0)
                            {
                                enemy.IsAlive = false;
                            }
                        }
                    }
                    enemy.Update(window, gameTime);
                    if (enemy.IsAlive)
                    {
                        if (enemy.CheckCollision(player, player.Rectangle, enemy.Rectangle))
                        {
                            killed_Enemies++;
                            player.Health -= enemy.Health;
                            enemies.Remove(enemy);
                            if (player.Health <= 0)
                            {
                                player.IsAlive = false;
                            }
                        }
                    }
                    else
                    {
                        enemy.Deathtimer = gameTime.TotalGameTime.TotalMilliseconds;

                        /*
                        Timer timer = new(200);
                        timer.Elapsed += (sender, e) =>
                        {
                            try
                            {
                                enemies.Remove(enemy);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error removing enemy  " + ex.Message);
                            }
                        };
                        timer.Start();
                        */
                    }
                }
            }
            foreach (ShooterBullet bullet in shooterBulletList.ToList())
            {
                bullet.Update(window);
                if (bullet.CheckCollision(player, player.Rectangle, bullet.Rectangle))
                {
                    shooterBulletList.Remove(bullet);
                    player.Health -= 1;
                    if (player.Health <= 0)
                    {
                        player.IsAlive = false;
                    }
                }
            }
            foreach (Boss1_bullet bullet in bossBulletList.ToList())
            {
                bullet.Update(window);
                if (bullet.CheckCollision(player, player.Rectangle, bullet.Rectangle))
                {
                    bossBulletList.Remove(bullet);
                    player.Health -= 1;
                    if (player.Health <= 0)
                    {
                        player.IsAlive = false;
                    }
                }
            }
            //boss
            int bosshelt = 0;
            bool bossdead = false;
            foreach (Enemy enemy in bossList.ToList()) //boss class make so better destinguich
            {
                string enemytype = TypeDescriptor.GetClassName(enemy);
                if (enemytype == "SpaceShooter.Boss1")
                {
                    if (!enemy.IsAlive)
                    {
                        bossdead = true;
                        if (enemy.CanShoot())
                        {
                            bossList.Remove(enemy);
                            levelnum = 2;
                            leveltransition = 20000;
                            ChangeBackground(window, 1);
                        }
                    }
                }
                else if (enemytype == "SpaceShooter.Boss1_top_turret")
                {
                    Vector2 toPlayer;
                    toPlayer.X = player.X + player.Rectangle.Width / 2 - enemy.Position.X + enemy.rectangle.Width / 2;
                    toPlayer.Y = player.Y + player.Rectangle.Height / 2 - enemy.Position.Y + enemy.rectangle.Height;

                    enemy.Rotation = (float)Math.Atan2(toPlayer.Y, toPlayer.X);

                    // if player is within firing range and the bullet timer
                    if (toPlayer.Length() < 500f && enemy.CanShoot() && enemy.Y + enemy.rectangle.Height > 0)
                    {
                        Vector2 bulletVelocity = Vector2.Normalize(toPlayer) * 5f;
                        shooterBulletList.Add(new ShooterBullet(shooterBulletSprite, enemy.Position.X, enemy.Position.Y, bulletVelocity));
                        enemy.ResetShootTimer();
                    }
                    if (bossdead)
                    {
                        bossList.Remove(enemy);
                    }
                }
                else if (enemytype == "SpaceShooter.Boss1_front_turret")
                {
                    if (enemy.IsAlive)
                    {
                        if (enemy.CanShoot() && enemy.Y + enemy.rectangle.Height > 0)
                        {
                            bossBulletList.Add(new Boss1_bullet(bossSprite, enemy.X + enemy.rectangle.Width / 2, enemy.Y + enemy.rectangle.Height, new Vector2(0, 5.5f)));
                            enemy.ResetShootTimer();
                        }
                    }
                    if (bossdead)
                    {
                        bossList.Remove(enemy);
                    }
                }
                foreach (Bullet b in player.Bullets.ToList())
                {
                    if (enemy.CheckCollision(b, bullet.Rectangle, enemy.Rectangle) && enemy.IsAlive)
                    {
                        enemy.Health -= bullet.Damage;
                        if (enemy.Health <= 0)
                        {
                            enemy.IsAlive = false;
                            player.Points += enemy.Points;
                        }
                        b.Health -= 1;
                        if (b.Health <= 0)
                        {
                            b.IsAlive = false;
                        }
                    }
                }
                foreach (Rocket r in player.Rockets.ToList())
                {
                    if (r.IsAlive == true)
                    {
                        if (enemy.CheckCollision(r, rockets.Rectangle, enemy.Rectangle) && enemy.IsAlive)
                        {
                            enemy.Health -= bullet.Damage;
                            if (enemy.Health <= 0)
                            {
                                enemy.IsAlive = false;
                                player.Points += enemy.Points;
                            }
                            r.IsAlive = false;
                        }
                    }
                }
                foreach (BouncingBullet b in player.BouncingBullets.ToList())
                {
                    if (enemy.CheckCollision(b, b.Rectangle, enemy.Rectangle) && enemy.IsAlive)
                    {
                        Vector2 normal = Vector2.Zero;
                        if (b.Position.Y < enemy.Y + enemy.Rectangle.Height)
                        {
                            normal += Vector2.UnitY;
                        }
                        else if (b.Position.Y > enemy.Y)
                        {
                            normal -= Vector2.UnitY;
                        }
                        else if (b.Position.X < enemy.X + enemy.Rectangle.Width)
                        {
                            normal += Vector2.UnitX;
                        }
                        else if (b.Position.X > enemy.X)
                        {
                            normal -= Vector2.UnitX;
                        }

                        b.Velocity = Vector2.Reflect(b.Velocity, normal);

                        b.Speed *= 0.8f;

                        b.AmountBounces++;

                        enemy.Health -= 1;
                        if (enemy.Health <= 0)
                        {
                            enemy.IsAlive = false;
                            player.Points += enemy.Points;
                        }
                    }
                }
                enemy.Update(window, gameTime);
                if (enemy.IsAlive)
                {
                    if (enemytype == "SpaceShooter.Boss1")
                    {
                        enemy.ResetShootTimer();
                    }
                    bosshelt += enemy.Health;
                    if (enemy.CheckCollision(player, player.Rectangle, enemy.Rectangle))
                    {
                        player.Health -= enemy.Health;
                        if (player.Health <= 0)
                        {
                            player.IsAlive = false;
                        }
                    }
                }
            }
            bossHealth = bosshelt;
        }

        public static void UpdateObstacles(GameWindow window, GameTime gameTime)
        {
            foreach (Obstacle ob in obstacles.ToList())
            {
                ob.Update(window, gameTime);

                if (player.CheckCollision(ob, ob.Rectangle, player.Rectangle))
                {
                    Rectangle overlapRectangle = Rectangle.Intersect(player.Rectangle, ob.Rectangle);

                    //checks where player is overlaping
                    bool playerIsAboveObstacle = player.Position.Y + player.Rectangle.Height <= ob.Position.Y + overlapRectangle.Height;
                    bool playerIsBelowObstacle = player.Position.Y >= ob.Position.Y + ob.Rectangle.Height - overlapRectangle.Height;
                    bool playerIsLeftOfObstacle = player.Position.X + player.Rectangle.Width <= ob.Position.X + overlapRectangle.Width;
                    bool playerIsRightOfObstacle = player.Position.X >= ob.Position.X + ob.Rectangle.Width - overlapRectangle.Width;

                    //the statesments
                    if (playerIsAboveObstacle)
                    {
                        player.Y = ob.Position.Y - player.Rectangle.Height;
                    }
                    else if (playerIsBelowObstacle)
                    {
                        player.Y = ob.Position.Y + ob.Rectangle.Height;
                        if (player.Y + player.Rectangle.Height > window.ClientBounds.Height)
                        {
                            player.IsAlive = false;
                        }
                    }
                    else if (playerIsLeftOfObstacle) // if get trouble with it change this back to if
                    {
                        player.X = ob.Position.X - player.Rectangle.Width;
                    }
                    else if (playerIsRightOfObstacle)
                    {
                        player.X = ob.Position.X + ob.Rectangle.Width;
                    }
                }
                foreach (Bullet bullet in player.Bullets.ToList())
                {
                    if (bullet.CheckCollision(ob, ob.Rectangle, bullet.Rectangle))
                    {
                        player.Bullets.Remove(bullet);
                    }
                }
                foreach (ShooterBullet bullet in shooterBulletList.ToList())
                {
                    if (bullet.CheckCollision(ob, ob.Rectangle, bullet.Rectangle))
                    {
                        shooterBulletList.Remove(bullet);
                    }
                }
                foreach (Boss1_bullet bullet in bossBulletList.ToList())
                {
                    if (bullet.CheckCollision(ob, ob.Rectangle, bullet.Rectangle))
                    {
                        bossBulletList.Remove(bullet);
                    }
                }
                foreach (BouncingBullet b in player.BouncingBullets.ToList())
                {
                    if (ob.CheckCollision(b, b.Rectangle, ob.Rectangle))
                    {
                        Vector2 normal = Vector2.Zero;
                        if (b.Position.X < ob.X + ob.Rectangle.Width)
                        {
                            normal += Vector2.UnitX;
                        }
                        else if (b.Position.X > ob.X)
                        {
                            normal -= Vector2.UnitX;
                        }
                        if (b.Position.Y < ob.Y + ob.Rectangle.Height)
                        {
                            normal += Vector2.UnitY;
                        }
                        else if (b.Position.Y > ob.Y)
                        {
                            normal -= Vector2.UnitY;
                        }

                        b.Velocity = Vector2.Reflect(b.Velocity, normal);
                        b.AmountBounces++;

                        b.Speed *= 0.8f;
                    }
                }
            }
        }

        public static void UpdatePlayerBomb(GameWindow window, GameTime gameTime)
        {
            foreach (Bomb bomb in player.Bombs.ToList())
            {
                if (bomb.exploded)
                {
                    if (bomb.CheckCollision(player, player.Rectangle, bomb.Rectangle))
                    {
                        player.IsAlive = false;
                    }
                    foreach (Bullet bullet in player.Bullets.ToList())
                    {
                        if (bullet.CheckCollision(bomb, bomb.Rectangle, bullet.Rectangle))
                        {
                            player.Bullets.Remove(bullet);
                        }
                    }
                    foreach (ShooterBullet bullet in shooterBulletList.ToList())
                    {
                        if (bullet.CheckCollision(bomb, bomb.Rectangle, bullet.Rectangle))
                        {
                            shooterBulletList.Remove(bullet);
                        }
                    }
                    foreach (Boss1_bullet bullet in bossBulletList.ToList())
                    {
                        if (bullet.CheckCollision(bomb, bomb.Rectangle, bullet.Rectangle))
                        {
                            bossBulletList.Remove(bullet);
                        }
                    }
                    foreach (BouncingBullet b in player.BouncingBullets.ToList())
                    {
                        if (bomb.CheckCollision(b, b.Rectangle, bomb.Rectangle))
                        {
                            player.BouncingBullets.Remove(b);
                        }
                    }
                    foreach (Enemy enemy in enemies.ToList())
                    {
                        if (bomb.CheckCollision(enemy, enemy.Rectangle, bomb.Rectangle))
                        {
                            player.Points += enemy.Points;
                            enemies.Remove(enemy);
                        }
                    }
                    foreach (Enemy enemy in bossList.ToList())
                    {
                        if (bomb.CheckCollision(enemy, enemy.Rectangle, bomb.Rectangle))
                        {
                            enemy.Health -= bomb.Damage;
                        }
                    }
                }
            }
        }

        public static void UpdatePlayerRocket(GameTime gameTime, Vector2 position, Rocket rocket)
        {
            Vector2 nearestEnemyPosition = Vector2.Zero;
            float nearestDistance = float.MaxValue;
            foreach (Enemy enemy in enemies.ToList())
            {
                if (enemy != null)
                {
                    float distance = Vector2.Distance(rocket.Position, enemy.Position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestEnemyPosition = enemy.Position + new Vector2(enemy.Rectangle.Width / 2, enemy.rectangle.Height / 2);
                        position = rocket.Position;
                    }
                }
            }
            if (nearestDistance == float.MaxValue)
            {
                foreach (Enemy enemy in bossList.ToList())
                {
                    if (enemy != null && enemy.IsAlive)
                    {
                        float distance = Vector2.Distance(rocket.Position, enemy.Position);
                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nearestEnemyPosition = enemy.Position + new Vector2(enemy.Rectangle.Width / 2, enemy.rectangle.Height / 2);
                            position = rocket.Position;
                        }
                    }
                }
            }
            Vector2 fector;
            fector.X = nearestEnemyPosition.X - position.X;
            fector.Y = nearestEnemyPosition.Y - position.Y;
            //roatation
            rocket.Rotation = (float)Math.Atan2(fector.Y, fector.X);
        }

        public static void UpdatePowerUps(GameWindow window, GameTime gameTime)
        {
            foreach (Powerups powerups in powerUps.ToList())
            {
                string poweruptype = TypeDescriptor.GetClassName(powerups);
                powerups.Update(window, gameTime);

                if (powerups.IsAlive)
                {
                    if (poweruptype == "SpaceShooter.DoubleShot")
                    {
                        if (powerups.CheckCollision(player, player.Rectangle, powerups.Rectangle))
                        {
                            powerUps.Remove(powerups);
                            player.Points += 5;
                            DS_remainingTime += 10000;
                        }
                    }
                    else if (poweruptype == "SpaceShooter.GoldCoin")
                    {
                        if (powerups.CheckCollision(player, player.Rectangle, powerups.Rectangle))
                        {
                            powerUps.Remove(powerups);
                            player.Points += 5;
                            DS_remainingTime += 10000;
                        }
                    }
                    else if (poweruptype == "SpaceShooter.HealthPoint")
                    {
                        if (powerups.CheckCollision(player, player.Rectangle, powerups.Rectangle))
                        {
                            powerUps.Remove(powerups);
                            player.Points += 5;
                            if (player.Health < 5)
                            {
                                player.Health++;
                            }
                        }
                    }
                    else if (poweruptype == "SpaceShooter.BulletSpeed")
                    {
                        if (powerups.CheckCollision(player, player.Rectangle, powerups.Rectangle))
                        {
                            powerUps.Remove(powerups);
                            player.Points += 5;
                            bulletSpeedAmount = 100;
                            bulletSpeed_RemainingTime += 10000;
                        }
                    }
                    else if (poweruptype == "SpaceShooter.RocketAmmo")
                    {
                        if (powerups.CheckCollision(player, player.Rectangle, powerups.Rectangle))
                        {
                            powerUps.Remove(powerups);
                            player.Points += 5;

                            if (player.AmountRockets < 10)
                            {
                                player.AmountRockets++;
                            }
                        }
                    }
                }
                else
                {
                    powerUps.Remove(powerups);
                }
            }
        }

        /// <summary>
        /// Dessa är alla states och dess draw funktioner och har tänkt att kanske flytta dem till en annan fil för att göra det lite mindre kod i GameElements
        /// </summary>
        public static void UserDraw(SpriteBatch spriteBatch, GameWindow window)
        {
            background.Draw(spriteBatch);

            printText.Print($"Valj en av de 5 spelarna och sen tryck escape och tryck \"R\" om du vill ta bort all data inom den ", spriteBatch, 10, 0, Color.White);

            string q_select = "select * from player order by id asc";
            SQLiteCommand dbCommand = new SQLiteCommand(q_select, db.dbConn);
            db.OpenConn();
            //Get result-set from db
            SQLiteDataReader result = dbCommand.ExecuteReader();
            int height = 30;
            int userid = 1;
            if (result.HasRows)
            {
                while (result.Read())
                {
                    string name = Convert.ToString(result["name"]);
                    string highscore = Convert.ToString(result["highscore"]);
                    string id = Convert.ToString(result["id"]);
                    string killed_enemies = Convert.ToString(result["killed_enemies"]);

                    if (Convert.ToInt16(result["id"]) == currentuser)
                    {
                        printText.Print($"{name}", spriteBatch, 10, height, Color.Red);
                        printText.Print(
                            $"\nHighscore {highscore}" +
                            $"\nKilled enemies {killed_enemies}", spriteBatch, window.ClientBounds.Width / 2, window.ClientBounds.Height / 3, Color.White);
                    }
                    else
                    {
                        printText.Print($"{name}", spriteBatch, 10, height, Color.White);
                    }
                    height += 30;
                    userid++;
                }
            }

            db.CloseConn();
        }

        public static State UserUpdate(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                string q_update = "UPDATE player SET highscore = 0, killed_enemies = 0 WHERE id = " + currentuser + ";";
                SQLiteCommand dbComman = new SQLiteCommand(q_update, db.dbConn);
                db.OpenConn();
                int resul = dbComman.ExecuteNonQuery();
                db.CloseConn();
            }
            else
            {
                string q_select = "select * from player";
                SQLiteCommand dbCommand = new SQLiteCommand(q_select, db.dbConn);
                db.OpenConn();
                //Get result-set from db
                SQLiteDataReader result = dbCommand.ExecuteReader();
                int id = 0;
                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        id++;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    if (gameTime.TotalGameTime.TotalMilliseconds > buttonTimeElapsed + 200)
                    {
                        currentuser++;
                        buttonTimeElapsed = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    if (gameTime.TotalGameTime.TotalMilliseconds > buttonTimeElapsed + 200)
                    {
                        currentuser--;
                        buttonTimeElapsed = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
                if (currentuser < 1)
                {
                    currentuser = 1;
                }
                if (currentuser > id)
                {
                    currentuser = id;
                }
                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        if (Convert.ToInt16(result["id"]) == currentuser)
                        {
                            CurrentHighScore = Convert.ToInt16(result["highscore"]);
                            killed_Enemies = Convert.ToInt16(result["killed_enemies"]);
                        }
                    }
                }
                db.CloseConn();
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return State.Menu;
            }
            return State.Users;
        }

        public static State Stats(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return State.Menu;
            }

            return State.Stats;
        }

        public static void StatsDraw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);

            //database
            string q_select = "select * from player";
            SQLiteCommand dbCommand = new SQLiteCommand(q_select, db.dbConn);
            db.OpenConn();
            //Get result-set from db
            SQLiteDataReader result = dbCommand.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    if (Convert.ToInt16(result["id"]) == currentuser)
                    {
                        string id = Convert.ToString(result["id"]);
                        string name = Convert.ToString(result["name"]);
                        string highscore = Convert.ToString(result["highscore"]);
                        string killed_enemies = Convert.ToString(result["killed_enemies"]);
                        printText.Print(
                              $"Id: {id}" +
                            $"\nName {name}" +
                            $"\nHighscore {highscore}" +
                            $"\nKilled enemies {killed_enemies}", spriteBatch, 10, 0, Color.White);
                    }
                }
            }
            db.CloseConn();
        }

        public static void HighScoreDraw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);

            string q_select = "select * from player order by highscore desc";
            SQLiteCommand dbCommand = new SQLiteCommand(q_select, db.dbConn);
            db.OpenConn();
            //Get result-set from db
            SQLiteDataReader result = dbCommand.ExecuteReader();
            int height = 0;
            int amount = 0;
            if (result.HasRows)
            {
                while (result.Read())
                {
                    if (amount < 10)
                    {
                        string name = Convert.ToString(result["name"]);
                        string highscore = Convert.ToString(result["highscore"]);

                        printText.Print($"{name}: {highscore}", spriteBatch, 10, height, Color.White);
                        height += 30;
                        amount++;
                    }
                }
            }
            db.CloseConn();
            //Goal
        }

        public static State HighScoreUpdate(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return State.Menu;
            }

            switch (currentHighScoreState)
            {
                case highState.EnterHighScore:
                    if (highscore.EnterUpdate(gameTime, CurrentHighScore))
                    {
                        currentHighScoreState = highState.EnterHighScore;
                    }
                    break;

                default:
                    if (hasdied == 1)
                    {
                        currentHighScoreState = highState.EnterHighScore;
                        hasdied = 0;
                    }
                    break;
            }
            return State.HighScore;
        }

        public static State About(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return State.Menu;
            }

            return State.About;
        }

        public static void AboutDraw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);

            //how to play
            printText.Print("How to play game: " +
                "\n" +
                "\nMovement:" +
                "\nForward - W" +
                "\nLeft - A" +
                "\nRight - D" +
                "\nBackwards - S" +
                "\nShoot normal - Space" +
                "\nShoot rocket - R" +
                "\nShoot BouncingBullet - B" +
                "\nPlant a bomb - T" +
                "\n\nGoal:" +
                "\nThe goal of the game is to get as much points as possible ", spriteBatch, 0, 0, Color.White);
        }

        public static void MenuDraw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            menu.Draw(spriteBatch);
        }

        public static State MenuUpdate(GameTime gameTime, GameWindow window)
        {
            Reset(window);
            return (State)menu.Update(gameTime);
        }

        public static void PauseMenuDraw(SpriteBatch spritebatch, GameWindow window)
        {
            background.Draw(spritebatch);
            RunDraw(spritebatch, window);
            pauseMenu.Draw(spritebatch);
        }

        public static State PauseMenuUpdate(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                if (gameTime.TotalGameTime.TotalMilliseconds > buttonTimeElapsed + 200)
                {
                    buttonTimeElapsed = gameTime.TotalGameTime.TotalMilliseconds;
                    return State.Run;
                }
            }
            return (State)pauseMenu.Update(gameTime);
        }

        /// <summary>
        /// När spelaren dör så används denna att reseta allt till normalt
        /// </summary>
        private static void Reset(GameWindow window)
        {
            player.Reset(window.ClientBounds.Width / 2, window.ClientBounds.Height - 30, 4.5f, 5.5f);

            player.Points = 0;
            killed_Enemies = 0;
            ChangeBackground(window, 0);

            DS_remainingTime = 0;
            bulletSpeed_RemainingTime = 0;
            bulletSpeedAmount = 200;
            player.Health = 5;
            distance = 0;
            enemies.Clear();
            powerUps.Clear();
            enemies.Clear();
            shooterBulletList.Clear();
            obstacles.Clear();
            levelnum = 1;
            player.AmountRockets = 10;
            bombCountDown = 0;

            bossBulletList.Clear();
            bossList.Clear();
            bossHealth = 0;
            bossEnabled = false;
            level1.bossActive = false;
        }

        public static void ChangeBackground(GameWindow window, int type)
        {
            // Change the current background to the next one
            background = new Background(backgroundTextures[type], window);
        }

        /// <summary>
        /// Här har jag mina timers för nedräkningar
        /// </summary>
        public static void Timers(GameTime gameTime)
        {
            //powerup timers
            float elapsedMilliSeconds = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            DS_remainingTime -= elapsedMilliSeconds;
            if (DS_remainingTime < 0)
            {
                DS_remainingTime = 0;
            }
            bulletSpeed_RemainingTime -= elapsedMilliSeconds;
            if (bulletSpeed_RemainingTime < 0)
            {
                bulletSpeed_RemainingTime = 0;
                bulletSpeedAmount = 200;
            }
            bombCountDown -= elapsedMilliSeconds;
            if (bombCountDown < 0)
            {
                bombCountDown = 0;
            }
            leveltransition -= elapsedMilliSeconds;
            if (leveltransition < 0)
            {
                leveltransition = 0;
            }
            if (player.IsAlive)
            {
                float distancew = 2 * elapsedMilliSeconds / 1000;
                distance += distancew;
            }
        }

    }
}