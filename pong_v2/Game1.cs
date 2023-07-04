using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private HighScore highscore;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 400;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 400;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            GameElements.currentState = GameElements.State.Users;
            GameElements.Initialize();
            highscore = new HighScore(10);

            base.Initialize();
        }

        protected override void UnloadContent()
        {
        }

        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("highFont");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GameElements.LoadContent(Content, Window);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            switch (GameElements.currentState)
            {
                case GameElements.State.Run:
                    GameElements.currentState = GameElements.RunUpdate(Window, gameTime);
                    break;

                case GameElements.State.Users:
                    GameElements.currentState = GameElements.UserUpdate(gameTime);
                    break;

                case GameElements.State.PauseMenu:
                    GameElements.currentState = GameElements.PauseMenuUpdate(gameTime);
                    break;

                case GameElements.State.HighScore:
                    GameElements.currentState = GameElements.HighScoreUpdate(gameTime);
                    break;

                case GameElements.State.About:
                    GameElements.currentState = GameElements.About(gameTime);
                    break;

                case GameElements.State.Stats:
                    GameElements.currentState = GameElements.Stats(gameTime);
                    break;

                case GameElements.State.Quit:
                    this.Exit();
                    break;

                default:
                    GameElements.currentState = GameElements.MenuUpdate(gameTime, Window);
                    break;
            }
            GameTime = gameTime;
            base.Update(gameTime);
        }

        public static GameTime GameTime { get; private set; }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            switch (GameElements.currentState)
            {
                case GameElements.State.Run:
                    GameElements.RunDraw(spriteBatch, Window);
                    break;

                case GameElements.State.Users:
                    GameElements.UserDraw(spriteBatch, Window);
                    break;

                case GameElements.State.PauseMenu:
                    GameElements.PauseMenuDraw(spriteBatch, Window);
                    break;

                case GameElements.State.HighScore:
                    GameElements.HighScoreDraw(spriteBatch);
                    break;

                case GameElements.State.About:
                    GameElements.AboutDraw(spriteBatch);
                    break;

                case GameElements.State.Stats:
                    GameElements.StatsDraw(spriteBatch);
                    break;

                case GameElements.State.Quit:
                    this.Exit();
                    break;

                default:
                    GameElements.MenuDraw(spriteBatch);
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}