#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WheelChairGameLibrary.Helpers;

using Microsoft.Kinect;
#endregion



namespace WheelChairGameLibrary
{

    public class GameEnhanced : Microsoft.Xna.Framework.Game
    {

        /// <summary>
        /// Correctively manage the active single screen. It automatically removes the previous screen and call it's ExitScreen()
        /// </summary>
        private Screen activeScreen;
        public Screen ActiveScreen
        {
            set
            {
                if (activeScreen != null)
                {
                    activeScreen.ExitScreen();
                    Components.Remove(activeScreen);
                }
                activeScreen = value;
                Components.Add(activeScreen);
            }
        }

        /// <summary>
        /// The graphics device manager provided by Xna.
        /// </summary>
        private readonly GraphicsDeviceManager graphics;
        protected GraphicsDeviceManager Graphics
        {
            get { return graphics; }
        }

        /// <summary>
        /// This control selects a sensor, and displays a notice if one is
        /// not connected.
        /// </summary>
        private readonly KinectChooser chooser;
        protected KinectChooser Chooser
        {
            get { return chooser; }
        }

        /// <summary>
        /// Takes care of all collisions in game
        /// </summary>
        private readonly CollisionManager collisionManager;
        protected CollisionManager CollisionManager
        {
            get { return collisionManager; }
        }

        /// <summary>
        /// This is the SpriteBatch used for SharedSpriteBatch in GameObject.
        /// </summary>
        private SpriteBatch spriteBatch;
        protected SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        private Texture2D whitePixel;
        /// <summary>
        /// A white pixel texture to draw primitive forms
        /// </summary>
        public Texture2D WhitePixel
        {
            get { return whitePixel; }
        }

        private SpriteFont defaultFont;
        /// <summary>
        /// Main font in the game
        /// </summary>
        public SpriteFont DefaultFont
        {
            get { return defaultFont; }
        }

        /// <summary>
        /// A content to load stuff from content inside library
        /// </summary>
        private readonly ContentManager LibContent;


        private bool isDebugMode = true;
        public bool IsDebugMode
        {
            get { return isDebugMode; }
            set { isDebugMode = value; }
        }


        public GameEnhanced()
        {
            Content.RootDirectory = "Content";
            //Starts content for use inside library
            LibContent = new ResourceContentManager(this.Services, ResourceFile.ResourceManager);

            //TODO: check this settings
            this.IsFixedTimeStep = false;
            this.Window.Title = "Collaborative Game";

            //TODO: make resolution and graphics settings
            this.graphics = new GraphicsDeviceManager(this);
            
            //TODO: add: this.graphics.PreparingDeviceSettings += this.GraphicsDevicePreparingDeviceSettings;
            // The Kinect sensor will use 640x480 for both streams
            // To make your app handle multiple Kinects and other scenarios,
            // it is recommended to use KinectSensorChooser provided in Microsoft.Kinect.Toolkit (See components in Toolkit Browser).
            this.chooser = new KinectChooser(this, ColorImageFormat.RgbResolution640x480Fps30, DepthImageFormat.Resolution640x480Fps30);
            this.Services.RemoveService(typeof(KinectChooser));
            this.Services.AddService(typeof(KinectChooser), this.chooser);
            this.Components.Add(this.chooser);


            // start input service
            InputState inputState = new InputState(this);
            this.Services.RemoveService(typeof(InputState));
            this.Services.AddService(typeof(InputState), inputState);
            this.Components.Add(inputState);

            // start collision manager service
            this.collisionManager = new CollisionManager(this);
            this.Services.RemoveService(typeof(CollisionManager));
            this.Services.AddService(typeof(CollisionManager), collisionManager);
            this.Components.ComponentAdded += new EventHandler<GameComponentCollectionEventArgs>(Components_ComponentAdded);
            this.Components.ComponentRemoved += new EventHandler<GameComponentCollectionEventArgs>(Components_ComponentRemoved);
            this.Components.Add(collisionManager);   

                     
        }

        

        void Components_ComponentAdded(object sender, GameComponentCollectionEventArgs e)
        {
            //add collider of the object to the collider list
            if (typeof(GameObject).IsAssignableFrom(e.GameComponent.GetType()))
            {
                GameObject gameObject = (GameObject)e.GameComponent;
                if (gameObject.Collider != null)
                    this.CollisionManager.addCollider(gameObject.Collider);

            }
        }

        void Components_ComponentRemoved(object sender, GameComponentCollectionEventArgs e)
        {
            //removes the collider from the collider list
            if (typeof(GameObject).IsAssignableFrom(e.GameComponent.GetType()))
            {
                GameObject gameObject = (GameObject)e.GameComponent;
                if (gameObject.Collider != null)
                    this.CollisionManager.removeCollider(gameObject.Collider);
            }

        }

        /// <summary>
        /// Loads the Xna related content.
        /// </summary>
        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.Services.RemoveService(typeof(SpriteBatch));
            this.Services.AddService(typeof(SpriteBatch), this.spriteBatch);

            this.whitePixel = LibContent.Load<Texture2D>("whitePixel");
            this.defaultFont = LibContent.Load<SpriteFont>("mainFont");


            


            base.LoadContent();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);   
        }

        //TODO: add GraphicsDevicePreparingDeviceSettings from XnaBasicsGame



    }
}