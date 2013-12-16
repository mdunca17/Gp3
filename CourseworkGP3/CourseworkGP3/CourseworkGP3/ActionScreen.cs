using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CourseworkGP3
{
    // Defines the variables needed to create the action screen.
    class ActionScreen : GameScreen
    {
         GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;
        Texture2D image;
        Rectangle imageRectangle;

        public ActionScreen(Game1 game, SpriteBatch spriteBatch, Texture2D image)
            : base(game, spriteBatch)
        {
            //Sets the size of the window of the action screen
            this.image = image;
            imageRectangle = new Rectangle(
                0,
                0,
                Game.Window.ClientBounds.Width,
                Game.Window.ClientBounds.Height);
        }

        public override void Update(GameTime gameTime)
        {
           // if esc pressed then exit.
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                game.Exit();

          
            base.Update(gameTime);
            oldKeyboardState = keyboardState;
        }

        private bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) &&
                oldKeyboardState.IsKeyDown(theKey);
        }
        

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(image, imageRectangle, Color.White);
            spriteBatch.End();
               

                //writeText(songInfo, new Vector2(50, 125), Color.AntiqueWhite);
                base.Draw(gameTime);
        }
        
    }
}