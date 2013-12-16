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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //Key board states
        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;

        // Menu system States
        GameScreen activescreen;
        StartScreen startScreen;
        ActionScreen actionScreen;
        EndScreen endScreen;
        

        #region User Defined Variables
        //------------------------------------------
        // Added for use with fonts
        //------------------------------------------
        SpriteFont fontToUse;
        Texture2D background;
        //--------------------------------------------------
        // Added for use with playing Audio via Media player
        //--------------------------------------------------
        private Song bkgMusic;
        //private String songInfo;

        //--------------------------------------------------
        //Set the sound effects to use
        //--------------------------------------------------
        
        private SoundEffect DyingSound;
        private SoundEffect YumSound;

        // Set the 3D model to draw.
        private Model mdlSnake;
        private Matrix[] mdlSnakeTransforms;

        //Set the hedge to draw and their position in level.
        private Model mdlHedge;
        private Model mdlHedge2;
        private Matrix[] mdlHedgeTransforms;
        private Matrix[] mdlHedgeTransforms2;
        private Vector3 hedgePosition = new Vector3(52, 0, 0);
        private Vector3 hedgepos = new Vector3(-52, 0, 0);
        private Vector3 hedgepos2 = new Vector3(-10, 0, 40);
        private Vector3 hedgepos3 = new Vector3(-10, 0, -40);

        //Floor- draws floor and sets position in the game.
        private Model mdlFloor;
        private Matrix[] mdlFloorTransforms;
        private Vector3 floorPosition = new Vector3(-60, 0, 0);

        // The aspect ratio determines how to scale 3d to 2d projection.
        private float aspectRatio;

        // Set the position of the model in world space, and set the rotation.
        private Vector3 mdlPosition = new Vector3(0, 0, 0);
        private float mdlRotation = 0.0f;
        private Vector3 mdlVelocity = Vector3.Zero;
        private Vector3 mdlRotate = Vector3.Zero;
        // create an array of enemy Birds and fruit that are eaten.
        private Model mdlFruit;
        private Matrix[] mdFruitTransforms;
        private Fruit objFruit;
        private Birds[] BirdList = new Birds[GameConstants.NumBirds];
        private Model mdlBanana;
        private Model mdlBirds;
        private Matrix[] BirdsTransforms;

        // create an array of laser bullets
        private Model mdlLaser;
        private Matrix[] mdlLaserTransforms;
        private Laser[] SnakeList = new Laser[GameConstants.NumLasers];
        // used to randomly spawn fruit
        private Random random = new Random();
        //Keyboard states
        KeyboardState oldkeyboardState;
        private KeyboardState lastState;
        
        //Game Background
        Vector2 mPosition = new Vector2(100, -150);
        Texture2D JungleTexture;

        // score of how many fruit have been ate 
        private int score;
        
        //Fruit changer number
        private int fruitchange = 2;
        // Used to stop drawing instructions after a period of the game start
        private int Itimer = 150;
        // Is used to count the score as player gets point for time they survive
        private int timer = 0;
        // Number of lives player has
        private int lives = 3;
        // Used to define the Lifebar of player
        private int LifeBar = 100;
        // speed increase
        private int SpeedTimer = 0;

        //third person camera
        private Camera TPC;
        private bool Cam = true;
        
        // Set the position of the camera in world space, for our view matrix.
        private Vector3 cameraPosition = new Vector3(0.0f, 84.0f, 60.0f);
        private Vector3 cameraPos2 = new Vector3(0.0f, 84.0f, 60.0f);
        private Matrix viewMatrix;
        private Matrix ViewM;
        private Matrix projectionMatrix;
        private Matrix projectMatrix;
        // Texture for game end screen
        public Texture2D GameOverScreen;

        private bool LiveGone;
        // Turns sound off
        private bool SoundOn = true;

        
        // rotation for the camera
        Matrix RotationMatrix;

        bool gameStarted = false;

        BasicEffect basicEffect;
        

        private void InitializeTransform()
        {
            

        }
        // Method used to define the in game camera View
        private void firstCameraTransform()
        {

            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;

            viewMatrix = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);

             projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
               MathHelper.ToRadians(45), aspectRatio, 1.0f, 350.0f);

            
        }
        // Method used to define in game Third person view.
        private void secCameraTransform()
        {

            cameraPos2 = new Vector3(0, 20, 50);
            RotationMatrix = Matrix.CreateRotationY(mdlPosition.Y);

            Vector3 transformedReference = Vector3.Transform(cameraPos2, RotationMatrix);

            cameraPos2 = transformedReference + mdlPosition;

            // aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;

            ViewM = Matrix.CreateLookAt(cameraPos2, mdlPosition, Vector3.Up);

            Viewport viewPort = graphics.GraphicsDevice.Viewport;
            float aspectRatio = (float)viewPort.Width / (float)viewPort.Height;
            projectMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45), aspectRatio, 1.0f, 350.0f);
            
        }
       // Used to define the movement of the player
        private void MoveModel()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            // Create some velocity if the right trigger is down.
            Vector3 mdlVelocityAdd = Vector3.Zero;

            // Find out what direction we should be thrusting, using rotation.
            mdlVelocityAdd.X = -(float)Math.Sin(mdlRotation);
            mdlVelocityAdd.Z = -(float)Math.Cos(mdlRotation);

            
            // Checks if Xbox controller is connected
            if (gamePadState.IsConnected)
            {
                // then it is connected check what button is pressed
                // Rotates left
                if (gamePadState.DPad.Left == ButtonState.Pressed)
                {

                    mdlRotation += 1.0f * 0.10f;


                }
                // Rotates right
                if (gamePadState.DPad.Right == ButtonState.Pressed)
                {

                    mdlRotation -= 1.0f * 0.10f;

                }
                // Moves Forward 
                if (gamePadState.DPad.Up == ButtonState.Pressed)
                {

                    mdlVelocityAdd *= 0.02f;
                    mdlVelocity += mdlVelocityAdd;

                }
                // Moves Back
                if (gamePadState.DPad.Down == ButtonState.Pressed)
                {

                    mdlVelocityAdd *= -0.02f;
                    mdlVelocity += mdlVelocityAdd;

                }
            }
            // Keyboard controls
            // Rotates left
            if (keyboardState.IsKeyDown(Keys.Left))
            {
               
                mdlRotation += 1.0f* 0.10f;
              
               
            }
            // Rotates Right
            if (keyboardState.IsKeyDown(Keys.Right))
            {
               
                mdlRotation -= 1.0f * 0.10f;
               
            }
            // Goes Forward
            if (keyboardState.IsKeyDown(Keys.Up)) 
            {
               
                mdlVelocityAdd *= 0.02f;
                mdlVelocity += mdlVelocityAdd;
                
            }
            // Goes Back
            if (keyboardState.IsKeyDown(Keys.Down))
            {
               
                mdlVelocityAdd *= -0.02f;
                mdlVelocity += mdlVelocityAdd;
                
            }
            // Turns volume off when A is pressed
            if (keyboardState.IsKeyDown(Keys.A) && (lastState.IsKeyUp(Keys.A)))
            {
                MediaPlayer.Pause();
                SoundEffect.MasterVolume = 0.0f;


            }
            // Turns volume on when S is pressed
            if (keyboardState.IsKeyDown(Keys.S) && (lastState.IsKeyUp(Keys.S)))
            {
                MediaPlayer.Resume();
                SoundEffect.MasterVolume = 1.0f;

            }
            // Turns Volume up when Page up is pressed
            if (keyboardState.IsKeyDown(Keys.PageUp) && (lastState.IsKeyUp(Keys.PageUp)))
            {
                MediaPlayer.Volume = MediaPlayer.Volume + 0.1f;
            }
            // Turns Volume Down when Page Down is pressed
            if (keyboardState.IsKeyDown(Keys.PageDown) && (lastState.IsKeyUp(Keys.PageDown)))
            {
                MediaPlayer.Volume = MediaPlayer.Volume - 0.1f;
            }
            // Resets game when R is presses
            if (keyboardState.IsKeyDown(Keys.R))
            {
                mdlVelocity = Vector3.Zero;
                mdlPosition = new Vector3(0, 0, 0);
                mdlRotation = 0.0f;
                ResetFruitandBirds();
            }
            // This is not used in game
            //are we shooting?
            if (keyboardState.IsKeyDown(Keys.Space) || lastState.IsKeyDown(Keys.Space))
            {
                //add another bullet.  Find an inactive bullet slot and use it
                //if all bullets slots are used, ignore the user input
                for (int i = 0; i < GameConstants.NumLasers; i++)
                {
                    if (!SnakeList[i].isActive)
                    {
                        Matrix tardisTransform = Matrix.CreateRotationY(mdlRotation);
                        SnakeList[i].direction = tardisTransform.Forward;
                        SnakeList[i].speed = GameConstants.LaserSpeedAdjustment;
                        SnakeList[i].position = mdlPosition + SnakeList[i].direction;
                        SnakeList[i].isActive = true;
                        
                        break; //exit the loop     
                    }
                }
            }
            //checks last Key state
            lastState = keyboardState;

        }
        // This Method Resets the fruit and birds to original settings.
        private void ResetFruitandBirds()
        {
            // x and z starting coordinates
            float xStart;
            float zStart;

            // Resets Fruit back to settings at start of game
            for (int i = 0; i < GameConstants.NumFruit; i++)
            {
                if (random.Next(2) == 0)
                {
                    xStart = (float)-GameConstants.PlayfieldSizeX;
                }
                else
                {
                    xStart = (float)GameConstants.PlayfieldSizeX;
                }
                zStart = (float)random.NextDouble() * GameConstants.PlayfieldSizeZ;
                objFruit.position = new Vector3(xStart, 0.0f, zStart);
                double angle = random.NextDouble() * 2 * Math.PI;
                objFruit.direction.X = -(float)Math.Sin(angle);
                objFruit.direction.Z = (float)Math.Cos(angle);
                objFruit.speed = GameConstants.FruitMinSpeed +
                   (float)random.NextDouble() * GameConstants.FruitMaxSpeed;
                objFruit.isActive = true;
            }
            // resets Birds to settings at start of game 
            for (int j = 0; j < GameConstants.NumBirds; j++)
            {
                if (random.Next(2) == 0)
                {
                    xStart = (float)-GameConstants.PlayfieldSizeX;
                }
                else
                {
                    xStart = (float)GameConstants.PlayfieldSizeX;
                }
                zStart = (float)random.NextDouble() * GameConstants.PlayfieldSizeZ;
                BirdList[j].position = new Vector3(xStart, 0.0f, zStart);
                double angle = random.NextDouble() * 2 * Math.PI;
                BirdList[j].direction.X = -(float)Math.Sin(angle);
                BirdList[j].direction.Z = (float)Math.Cos(angle);
                BirdList[j].speed = GameConstants.BirdsMinSpeed +
                   (float)random.NextDouble() * GameConstants.BirdsMaxSpeed;
                BirdList[j].isActive = true;
            }
            //Resets all values back to settings before game starts
            timer = 0;
            lives = 3;
            LifeBar = 100;
            score = 0;
            Itimer = 150;
            SpeedTimer = 0;
            mdlPosition = new Vector3(0, 0, 0);
        }

        private Matrix[] SetupEffectTransformDefaults(Model myModel)
        {
            Matrix[] absoluteTransforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(absoluteTransforms);

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    // Changes the camera view to in game camera
                    if (Cam == true)
                     {
                        effect.Projection = projectionMatrix;
                        effect.View = viewMatrix;
                     }
                        // Changes the camera to third person camera
                     else if (Cam == true)
                     {
                         effect.Projection = projectMatrix;
                         effect.View = ViewM;
                    }
                }
            }
            return absoluteTransforms;
        }

        public void DrawModel(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms)
        {
            //Draw the model, a model can have multiple meshes, so loop
            foreach (ModelMesh mesh in model.Meshes)
            {
                //This is where the mesh orientation is set
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * modelTransform;
                    // Changes boolean to switch between the two camera views. 
                    //Draws in game Camera
                    if (Cam == true)
                    {
                        effect.Projection = projectionMatrix;
                        effect.View = viewMatrix;
                    }
                        //Draws in third person camera
                    else if (Cam == false)
                        {
                           effect.Projection = projectMatrix;
                           effect.View = ViewM;
                        }
                    // Creates fog after timer is over 1000 to make game harder
                    if (timer > 1000)
                    {
                        effect.FogEnabled = true;
                        effect.FogColor = Color.Gray.ToVector3(); // For best results, ake this color whatever your background is.
                        effect.FogStart = 1.0f;
                        effect.FogEnd = 1.9f;
                    }
                }
                //Draw the mesh, will use the effects set above.
                mesh.Draw();
            }
        }
        // Defines the write text method used to put text on screen
        private void writeText(string msg, Vector2 msgPos, Color msgColour)
        {
            spriteBatch.Begin();
            string output = msg;
            // Find the center of the string
            Vector2 FontOrigin = fontToUse.MeasureString(output) / 2;
            Vector2 FontPos = msgPos;
            // Draw the string
            spriteBatch.DrawString(fontToUse, output, FontPos, msgColour);
            spriteBatch.End();
        }

        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }
        private void InitializeEffect()
        {

            basicEffect = new BasicEffect(graphics.GraphicsDevice);
            basicEffect.View = viewMatrix;
            basicEffect.Projection = projectionMatrix;
        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = false;
            Window.Title = "GP3";
            // Initialises these methods
            secCameraTransform();
            firstCameraTransform();
            ResetFruitandBirds();
            
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            // Loads font and main menu 
            startScreen = new StartScreen(this, spriteBatch,
               Content.Load<SpriteFont>(".\\Fonts\\DrWho"),
               Content.Load<Texture2D>(".\\Texture\\MainMenu"));
            Components.Add(startScreen);
            startScreen.Hide();
            // loads action screen
            actionScreen = new ActionScreen(this, spriteBatch,
                Content.Load<Texture2D>(".\\Texture\\mfloor2"));
            Components.Add(actionScreen);
            actionScreen.Hide();
            // loads end screen
            endScreen = new EndScreen(this, spriteBatch,
               Content.Load<SpriteFont>(".\\Fonts\\DrWho"),
               Content.Load<Texture2D>(".\\Texture\\mfloor"));
            Components.Add(endScreen);
            endScreen.Hide();
            // makes action screen = start screen when game starts
            activescreen = startScreen;
            activescreen.Show();
            background = Content.Load<Texture2D>(".\\Texture\\mFloor");

           

            
            string[] menuItems = { "Start Game", "High Scores", "End Game" };

            
            //-------------------------------------------------------------
            // added to load font
            //-------------------------------------------------------------
            fontToUse = Content.Load<SpriteFont>(".\\Fonts\\DrWho");
            //-------------------------------------------------------------
            // added to load Song
           //-------------------------------------------------------------
            bkgMusic = Content.Load<Song>(".\\Audio\\jungle");
   
            //-------------------------------------------------------------
            // added to load Model
            //-------------------------------------------------------------
            // Snake model
            mdlSnake = Content.Load<Model>(".\\Models\\Snake");
            mdlSnakeTransforms = SetupEffectTransformDefaults(mdlSnake);
            // Fruit Models
            mdlFruit = Content.Load<Model>(".\\Models\\Orange");
            mdFruitTransforms = SetupEffectTransformDefaults(mdlFruit);
            mdlBanana = Content.Load<Model>(".\\Models\\banana");
            mdFruitTransforms = SetupEffectTransformDefaults(mdlBanana);
            // Hedge models
            mdlHedge = Content.Load<Model>(".\\Models\\Hedge");
            mdlHedgeTransforms = SetupEffectTransformDefaults(mdlHedge);
            mdlHedge2 = Content.Load<Model>(".\\Models\\Hedge2");
            mdlHedgeTransforms2 = SetupEffectTransformDefaults(mdlHedge2);
            //floor 
            mdlFloor = Content.Load<Model>(".\\Models\\Grass");
            mdlFloorTransforms = SetupEffectTransformDefaults(mdlFloor);
            // Bird models
            mdlBirds = Content.Load<Model>(".\\Models\\Duck");
            BirdsTransforms = SetupEffectTransformDefaults(mdlBirds);
            // Background
            JungleTexture = Content.Load<Texture2D>(".\\Texture\\jungleBack");
            

            //-------------------------------------------------------------
            // added to load SoundFX's
            //-------------------------------------------------------------
            // sound effects
            DyingSound = Content.Load<SoundEffect>("Audio\\dying_01");
            YumSound = Content.Load<SoundEffect>("Audio\\yum_01");
           

            
             // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            keyboardState = Keyboard.GetState();
            // Allows the game to exit with xbox controller
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            // If enter is pressed make active screen equal action screen
            if (activescreen == startScreen)
            {
                if (CheckKey(Keys.Enter))
                {
                    if (startScreen.SelectedIndex == 0)
                    {
                        // Makes music not play at start screen as equals false
                        if (SoundOn == true)
                            {
                               MediaPlayer.Play(bkgMusic);
        
                               MediaPlayer.IsRepeating = true;
                             }
                        activescreen.Hide();
                        activescreen = actionScreen;
                        gameStarted = true;
                    }
                    if (startScreen.SelectedIndex == 1)
                    {
                        this.Exit();
                    }
                }

            }
            // Stops music when end screen is active
            if (activescreen == endScreen)
            {
                SoundOn = false;
            }
            // ends application when esc button is pressed
            if (activescreen == endScreen|| activescreen == startScreen|| activescreen == actionScreen)
            {

                if (CheckKey(Keys.Escape))
                {
                    Exit();
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed)
                    this.Exit();

            }
            
                //old key state
                oldkeyboardState = Keyboard.GetState();
                // Current Xbox contoller state
                GamePadState currentState = GamePad.GetState(PlayerIndex.One);
                // Sets vibration on xbox controller
                float vibrationAmount = 0.0f;

                if (activescreen == actionScreen)
                {
                    // updates logic here
                    MoveModel();
                    secCameraTransform();
                    
                    // Add velocity to the current position.
                    mdlPosition += mdlVelocity;
                    // icreases SpeedTimer
                    SpeedTimer++;
                    // Changes camera depending on what button is pressed
                    if (keyboardState.IsKeyDown(Keys.C))
                    {
                        Cam = true;
                        
                    }
                    if (keyboardState.IsKeyDown(Keys.X))
                    {
                        Cam = false;
                    }

                    

                    // Bleed off velocity over time.
                    mdlVelocity *= 0.95f;

                    float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                    for (int i = 0; i < GameConstants.NumFruit; i++)
                    {
                        objFruit.Update(timeDelta);
                    }

                    for (int i = 0; i < GameConstants.NumLasers; i++)
                    {
                        if (SnakeList[i].isActive)
                        {
                            SnakeList[i].Update(timeDelta);
                        }
                    }
                    for (int j = 0; j < GameConstants.NumBirds; j++)
                    {
                        if (BirdList[j].isActive)
                        {
                            BirdList[j].Update(timeDelta);
                        }
                    }
                    //Creates Snake bounding sphere

                    BoundingSphere SnakeBoundingSphere =
                      new BoundingSphere(mdlPosition,
                              mdlSnake.Meshes[0].BoundingSphere.Radius *
                                     GameConstants.SnakeBoundingSphereScale);

                    

                    //move fruit randomly
                    if (objFruit.isActive == false)
                    {

                        objFruit.position.X = random.Next(7) * GameConstants.PlayfieldSizeX;
                        objFruit.position.Z = random.Next(7) * GameConstants.PlayfieldSizeZ;
                    }
                    objFruit.isActive = true;


                    //Check for collisions
                    for (int j = 0; j < BirdList.Length; j++)
                    {

                        if (BirdList[j].isActive)
                        {
                            BoundingSphere BirdsBoundingSphere =
                                                  new BoundingSphere(BirdList[j].position,
                                                          mdlBirds.Meshes[0].BoundingSphere.Radius *
                                                                GameConstants.BirdsBoundingSphereScale);

                            if (BirdsBoundingSphere.Intersects(SnakeBoundingSphere))
                            {
                                LiveGone = true;
                                // if player still has lifes decrease life bar
                                if (lives > 0)
                                {
                                    LifeBar--;
                                  
                                }
                                
                                // Vibrate if bounding spheres intersect
                                if (currentState.IsConnected)
                                {
                                    

                                    vibrationAmount =
                                MathHelper.Clamp(vibrationAmount + 0.03f, 0.0f, 1.0f);
                                    GamePad.SetVibration(PlayerIndex.One,
                                        vibrationAmount, vibrationAmount);
                                }
                            }
                            LiveGone = false;
                        }
                        // increases speed of bird
                        if (SpeedTimer > 100)
                        {

                            BirdList[j].speed = GameConstants.BirdsMinSpeed + GameConstants.BirdsSpeedAdjustment;
                            BirdList[j].speed = GameConstants.BirdsMinSpeed + GameConstants.BirdsSpeedAdjustment;
                            SpeedTimer = 0;
                        }
                    }
                    
                    if (objFruit.isActive)
                    {
                        // Creates Fruit Bounding Sphere
                        BoundingSphere FruitSphereA =
                          new BoundingSphere(objFruit.position, mdlFruit.Meshes[0].BoundingSphere.Radius *
                                         GameConstants.FruitBoundingSphereScale);

                        for (int k = 0; k < SnakeList.Length; k++)
                        {
                           // checks if snake and fruit collide
                            if (FruitSphereA.Intersects(SnakeBoundingSphere))
                            {
                                // play sound effect of snake eating fruit
                                YumSound.Play();
                                objFruit.isActive = false;
                                // if player still has lifes add onto score
                                if (lives > 0)
                                {
                                    score++;
                                }
                                // add 50 on to timer if player eats fruit
                                timer = timer + 50;
                                break; //no need to check other collisions
                            }
                            
                        }

                    }

                    //Check if we are in boundaries
                    if (mdlPosition.X < -50 || mdlPosition.X > 50 || mdlPosition.Z < -40 || mdlPosition.Z > 34)
                    {
                        // takes life off life bar if they are out with boundaries
                        LifeBar--;
       
                        // vibrates xbox controller if they are outside boundaries
                         if (currentState.IsConnected)
                         {

                             vibrationAmount =
                         MathHelper.Clamp(vibrationAmount + 0.03f, 0.0f, 1.0f);
                             GamePad.SetVibration(PlayerIndex.One,
                                 vibrationAmount, vibrationAmount);
                         }
                    }
                    // used to decrement the timer that stops the drawing of instructions
                    Itimer--;
                    // keep adding on to timer if player still has lifes 
                    if (lives > 0)
                    {
                        timer++;
                    }
                    // if life bar reachs zero take life off player and play dying sound
                    //Then reset players position to middle of screen
                    if (LifeBar < 0)
                    {
                        lives--;
                        LifeBar = 100;
                        DyingSound.Play();
                        mdlPosition = new Vector3(0, 0, 0);

                    }
                    // if player loses all lifes then move to end screen
                    if (lives <= 0)
                    {
                        activescreen = endScreen;
                        activescreen.Show();
                    }
                    base.Update(gameTime);
                }
            oldKeyboardState = keyboardState;
       }

        private bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) &&
                oldKeyboardState.IsKeyDown(theKey);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {


            GraphicsDevice.Clear(Color.CornflowerBlue);

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rs;
            // draws main menu
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            spriteBatch.End();

            

            // draws these if active screen is action screen
            if (activescreen == actionScreen)
            {
                
                //Background
                spriteBatch.Begin();
                spriteBatch.Draw(JungleTexture, new Vector2(0,-300), Color.White);
                spriteBatch.End();

                // floor model
                Matrix floorTransform = Matrix.CreateRotationZ(0.0f) * Matrix.CreateScale((GameConstants.FruitScalar) * 150) * Matrix.CreateTranslation(floorPosition);
                DrawModel(mdlFloor, floorTransform, mdlFloorTransforms);
                // Hedge model on left
                Matrix hedgeTransform = Matrix.CreateScale((GameConstants.FruitScalar) * 15) * Matrix.CreateTranslation(hedgePosition);
                DrawModel(mdlHedge, hedgeTransform, mdlHedgeTransforms);
                // Hedge model on right
                Matrix hedgeTransform2 = Matrix.CreateScale((GameConstants.FruitScalar) * 15) * Matrix.CreateTranslation(hedgepos);
                DrawModel(mdlHedge, hedgeTransform2, mdlHedgeTransforms);
                // hedge model on top 
                Matrix hedgeTransform3 = Matrix.CreateRotationY(1.567f) * Matrix.CreateScale((GameConstants.FruitScalar) * 15) * Matrix.CreateTranslation(hedgepos2);
                DrawModel(mdlHedge2, hedgeTransform3, mdlHedgeTransforms2);
                // bottom hedge
                Matrix hedgeTransform4 = Matrix.CreateRotationY(1.567f) * Matrix.CreateScale((GameConstants.FruitScalar) * 15) * Matrix.CreateTranslation(hedgepos3);
                DrawModel(mdlHedge2, hedgeTransform4, mdlHedgeTransforms2);




                // TODO: Add your drawing code here
                for (int i = 0; i < GameConstants.NumFruit; i++)
                {
                    // if players have lives draw these models
                    if (lives > 0)
                    {
                        // draw both fruit models
                        if (objFruit.isActive == true)
                        {
                            fruitchange++;

                            if ((fruitchange % 2) == 0)
                            {

                                Matrix dalekTransform = Matrix.CreateScale(GameConstants.FruitScalar * 140) * Matrix.CreateTranslation(objFruit.position);
                                DrawModel(mdlFruit, dalekTransform, mdFruitTransforms);

                            }
                            else
                            {
                                Matrix dalekTransform = Matrix.CreateScale(GameConstants.FruitScalar * 90) * Matrix.CreateTranslation(objFruit.position);
                                DrawModel(mdlBanana, dalekTransform, mdFruitTransforms);

                            }
                        }
                    }
                }
                // Not used
                for (int i = 0; i < GameConstants.NumLasers; i++)
                {
                    if (SnakeList[i].isActive)
                    {
                        Matrix laserTransform = Matrix.CreateScale(GameConstants.LaserScalar) * Matrix.CreateTranslation(SnakeList[i].position);
                        DrawModel(mdlLaser, laserTransform, mdlLaserTransforms);
                    }
                }
                // draws bird models if player still has lives
                if (lives > 0)
                {
                    for (int j = 0; j < GameConstants.NumBirds; j++)
                    {
                        if (BirdList[j].isActive == true)
                        {
                            Matrix BirdsTransform = Matrix.CreateScale(GameConstants.FruitScalar * 10) * Matrix.CreateTranslation(BirdList[j].position);
                            DrawModel(mdlBirds, BirdsTransform, BirdsTransforms);

                        }
                    }
                }
                // Draws Snake model if player still has lives
                if (lives > 0)
                {
                    Matrix modelTransform = Matrix.CreateRotationY(mdlRotation) * Matrix.CreateTranslation(mdlPosition);
                    DrawModel(mdlSnake, modelTransform, mdlSnakeTransforms);
                }



                // Text that displays on screen during game 
                writeText("Snake vs Birds 3D", new Vector2(20, 10), Color.White);
                if (Itimer >= 0)
                {
                    writeText("Instructions\nUse Left and right buttons to rotate and up to move forward\nR to Reset", new Vector2(50, 50), Color.White);
                }
                if (timer > 800 && timer < 1000)
                {
                    //Warns player that the fog is about to drawn in the game
                    writeText(" The Fog is Coming", new Vector2(300,100),Color.White);
                }
                // writes the elements of the game that the player requires to play like life bar, lives etc.
                writeText("Score : " + timer, new Vector2(600, 10), Color.White);
                writeText("Fruit ate by snake: " + score, new Vector2(600, 40), Color.White);
                writeText("Lives : " + lives, new Vector2(500, 10), Color.White);
                writeText("Life Bar : " + LifeBar, new Vector2(300, 10), Color.White);
            }
           
           
            base.Draw(gameTime);
            // Text thats drawn at end of game
            if (activescreen == endScreen)
            {
                
                // Writes onto the screen the players final score for the game along with the amount of fruit they need.
                writeText("Game over" + "\n\nFruit ate: " + score + "\n\nScore : " + timer, new Vector2(300, 100), Color.Black);
                
            }
        }
    }
}
