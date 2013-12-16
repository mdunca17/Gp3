using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CourseworkGP3
{
    class StartScreen : GameScreen
    {
        MenuComponent menuComponent;
        Texture2D image;
        Rectangle imageRectangle;

        public int SelectedIndex
        {
            get { return menuComponent.SelectedIndex; }
            set { menuComponent.SelectedIndex = value; }
        }
        // This constructor takes in variables that are used to create the screen for the start of the game.
        public StartScreen(Game game,
        SpriteBatch spriteBatch,
        SpriteFont spriteFont,
        Texture2D image)
            : base(game, spriteBatch)
        {
            string[] menuItems = { "Start Game", "End Game" };
            menuComponent = new MenuComponent(game,
                spriteBatch,
                spriteFont,
                menuItems);
            Components.Add(menuComponent);
            this.image = image;
            //sets the size of the window for the game.
            imageRectangle = new Rectangle(
                0,
                0,
                Game.Window.ClientBounds.Width,
                Game.Window.ClientBounds.Height);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(image, imageRectangle, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
