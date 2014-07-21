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


using WheelChairGameLibrary;
using WheelChairGameLibrary.Helpers;

using Microsoft.Kinect;


namespace WheelChairCollaborativeGame
{

    #region Entry Point
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MyGame game = new MyGame())
            {
                game.Run();
            }
        }
    }
    #endregion

    public class MyGame : GameEnhanced
    {

        


        

        
        public MyGame()
        {

            this.Graphics.PreferredBackBufferWidth = (int)Config.resolution.X;
            this.Graphics.PreferredBackBufferHeight = (int)Config.resolution.Y;

            
            
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            
        }


        /// <summary>
        /// Add items to Components
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            

            ActiveScreen = new SplashScreen(this, "SplashScreen");
        }

        /// <summary>
        /// This method renders the current state.
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        protected override void Draw(GameTime gameTime)
        {

            // Clear the screen
            GraphicsDevice.Clear(Color.Black);


            /*switch (activeScreen)
            {

                    
                case Screen.Play:
                    
                case Screen.GameOver:
                    
                    break;
                case Screen.Tutorial:
                    SpriteBatch.Begin();
                    GUImessage.MessageDraw(SpriteBatch, Content,

                                "Tutorial", new Vector2(30, 300));
                    SpriteBatch.End();
                    break;
                case Screen.Settings:
                    SpriteBatch.Begin();
                    GUImessage.MessageDraw(SpriteBatch, Content,

                                "Settings", new Vector2(30, 300));
                    SpriteBatch.End();
                    break;
            }*/


            base.Draw(gameTime);

        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            InputState inputState = (InputState)Services.GetService(typeof(InputState));

            PlayerIndex playerIndex;
            if (inputState.IsKeyPressed(Keys.D, null, out playerIndex))
            {
                IsDebugMode = !IsDebugMode;
            }


            /*switch (activeScreen)
            {
                case Screen.WelcomeSplash:
                    if (inputState.IsKeyPressed(Keys.Enter, null, out playerIndex))
                    {
                        RemoveSpecificComponents();
                        activeScreen = Screen.Intro;
                    }
                    break;
                case Screen.Intro:
                    if (inputState.IsKeyPressed(Keys.Enter, null, out playerIndex))
                    {
                        RemoveSpecificComponents();
                        activeScreen = Screen.MainMenu;
                    }
                    break;          
                case Screen.Play:

                    

                    break;

                case Screen.MainMenu:
                    if (inputState.IsKeyPressed(Keys.Enter, null, out playerIndex))
                    {
                        TankGameObject playerTank = new TankGameObject(this, "playerTank");
                        this.Components.Add(playerTank);

                        KinectInput kinectInput = new KinectInput(this, "kinectInput");
                        this.Components.Add(kinectInput);

                        

                        Background background = new Background(this, 100);
                        this.Components.Add(background);

                        activeScreen = Screen.Play;
                    }
                    break;
                case Screen.GameOver:
                    if (inputState.IsKeyPressed(Keys.Enter, null, out playerIndex))
                    {
                        RemoveSpecificComponents();
                        activeScreen = Screen.MainMenu;
                    }
                    break;
            }*/

        }

        

    }

}






