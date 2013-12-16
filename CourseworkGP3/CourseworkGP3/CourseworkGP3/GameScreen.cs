using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace CourseworkGP3
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// 
    /// </summary>
    /// 
    // This class takes in variables that are used to create the screen for the start of the game.
    public class GameScreen : Microsoft.Xna.Framework.DrawableGameComponent
    {
        List<GameComponent> components = new List<GameComponent>();
        protected Game game;
        protected SpriteBatch spriteBatch;

        public List<GameComponent> Components
        {
            get { return components; }
        }

        public Game1 Game1
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public GameScreen(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            // TODO: Construct any child components here
            this.game = game;
            this.spriteBatch = spriteBatch;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
            foreach (GameComponent component in components)
            {
                if (component is DrawableGameComponent && ((DrawableGameComponent)component).Visible)
                    ((DrawableGameComponent)component).Draw(gameTime);
            }
        }
        // Show the screen when it is the active screen
        public virtual void Show()
        {
            this.Visible = true;
            this.Enabled = true;
            foreach (GameComponent component in components)
            {
                component.Enabled = true;
                if (component is DrawableGameComponent)
                    ((DrawableGameComponent)component).Visible = true;
            }
        }
        // Hides the screen when it is no longer the active screen
        public virtual void Hide()
        {
            this.Visible = false;
            this.Enabled = false;
            foreach (GameComponent component in components)
            {
                component.Enabled = false;
                if (component is DrawableGameComponent)
                    ((DrawableGameComponent)component).Visible = false;
            }
        }
    }
}

