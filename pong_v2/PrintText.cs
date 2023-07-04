using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter
{
    internal class PrintText
    {
        private SpriteFont font;

        public PrintText(SpriteFont font)
        {
            this.font = font;
        }

        public void Print(string text, SpriteBatch spriteBatch, int X, int Y, Color color)
        {
            spriteBatch.DrawString(font, text, new Vector2(X, Y), color);
        }
    }
}