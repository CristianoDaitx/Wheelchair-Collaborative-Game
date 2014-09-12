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
using log4net.Config;
using log4net;
using System.Reflection;
using WheelChairCollaborativeGame.Logging;

//[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
//[assembly: log4net.Config.XmlConfigurator(Watch = true)]


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
            log4net.Config.XmlConfigurator.Configure();
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

        //private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //private readonly ILog log2 = LogManager.GetLogger("MyAwesomeLogger");

        private readonly ILog detailedLog = LogManager.GetLogger("DetailedLogger");

        public MyGame()
        {


            /*this.log2.Debug("Debug message");
            this.log2.Info("Info message");
            this.log2.Warn("Warning message");
            this.log2.Error("Error message");
            this.log2.Fatal("Fatal message");

            log.Info(new MyEvent
            {
                UserId = "foo",
                EventCode = "xyz",
                Details = "Some event"
            });*/

            detailedLog.Info(new DetailedInfo(DetailedInfo.Type.PLAYER_A_ACTION_START));
            detailedLog.Info(new DetailedInfo(DetailedInfo.Type.PLAYER_B_ACTION_COMPLETION));

            this.Graphics.PreferredBackBufferWidth = (int)Config.resolution.X;
            this.Graphics.PreferredBackBufferHeight = (int)Config.resolution.Y;

            ActiveScreen = new SplashScreen(this, "SplashScreen");
            //ActiveScreen = new PlayScreen(this, "PlayScreen");
            //ActiveScreen = new MainMenuScreen(this, "MainMenuScreen");
            //ActiveScreen = new TutorialScreen(this, "TutorialScreen");
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

            //changes control type
            if (inputState.IsKeyPressed(Keys.Z, null, out playerIndex))
                Config.ControlSelected--;
            if (inputState.IsKeyPressed(Keys.X, null, out playerIndex))
                Config.ControlSelected++;

            //check borders of control tipe
            if ((int)Config.ControlSelected == 4)
            {
                Config.ControlSelected = Config.ControlSelect.FrontAssyncronous;
            }
            if ((int)Config.ControlSelected < 0)
            {
                Config.ControlSelected = Config.ControlSelect.Joystick;
            }
        }

    }

}






