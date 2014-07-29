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


    /// <summary>
    /// This class can be used for things to be drawn or updated troghout all the game.
    /// </summary>
    public class MyGame : GameEnhanced
    {
    
        public MyGame()
        {
            this.Graphics.PreferredBackBufferWidth = (int)Config.resolution.X;
            this.Graphics.PreferredBackBufferHeight = (int)Config.resolution.Y;

            //ActiveScreen = new SplashScreen(this, "SplashScreen");
            //ActiveScreen = new PlayScreen(this, "PlayScreen");
            ActiveScreen = new MainMenuScreen(this, "MainMenuScreen");
        }

        /// <summary>
        /// This method renders the current state.
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Clear the screen
            GraphicsDevice.Clear(Color.Black);
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
        }       

    }

}






