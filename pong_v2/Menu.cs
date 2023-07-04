using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SpaceShooter
{
    internal class MenuItem
    {
        private Texture2D texture;
        private Vector2 position;
        private int currentState;

        public MenuItem(Texture2D texture, Vector2 position, int currentState)
        {
            this.texture = texture;
            this.position = position;
            this.currentState = currentState;
        }

        public Texture2D Texture
        { get { return texture; } }

        public Vector2 Position
        { get { return position; } }

        public int State
        { get { return currentState; } }
    }

    internal class Menu
    {
        private List<MenuItem> menu;
        private int selected = 0;
        private float currentHeight = 0;
        private bool WbuttonPressed = true;
        private bool SbuttonPressed = true;
        private bool EnterbuttonPressed = false;
        private int defaultMenuState;

        public Menu(int defaultMenuState)
        {
            menu = new List<MenuItem>();
            this.defaultMenuState = defaultMenuState;
        }

        public void AddItem(Texture2D itemTexture, int state, GameWindow window)
        {
            float X = window.ClientBounds.Width / 2 - itemTexture.Width / 2;
            float Y = window.ClientBounds.Height / 4 - 20 + currentHeight;

            currentHeight += itemTexture.Height + 20;

            MenuItem temp = new MenuItem(itemTexture, new Vector2(X, Y), state);

            menu.Add(temp);
        }

        public int Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.S))
            {
                if (!SbuttonPressed)
                {
                    SbuttonPressed = true;
                    selected++;
                    if (selected > menu.Count - 1)
                    {
                        selected = 0;
                    }
                }
            }
            else
            {
                SbuttonPressed = false;
            }
            if (keyboardState.IsKeyDown(Keys.W))
            {
                if (!WbuttonPressed)
                {
                    WbuttonPressed = true;
                    selected--;
                    if (selected < 0)
                    {
                        selected = menu.Count - 1;
                    }
                }
            }
            else
            {
                WbuttonPressed = false;
            }

            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                if (!EnterbuttonPressed)
                {
                    EnterbuttonPressed = true;
                    return menu[selected].State;
                }
            }
            else
            {
                EnterbuttonPressed = false;
            }
            return defaultMenuState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < menu.Count; i++)
            {
                if (i == selected)
                {
                    spriteBatch.Draw(menu[i].Texture, menu[i].Position, Color.SeaGreen);
                }
                else
                {
                    spriteBatch.Draw(menu[i].Texture, menu[i].Position, Color.DarkSeaGreen);
                }
            }
        }
    }
}