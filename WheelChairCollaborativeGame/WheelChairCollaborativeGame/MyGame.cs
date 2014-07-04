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

using WheelChairGameLibrary.Screens;
using WheelChairGameLibrary;

using Microsoft.Kinect;

using KinectForWheelchair;
using KinectForWheelchair.Listeners;
using KeyMessaging;

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

        private EnemyGameObject enemy;

      

        public MyGame()
        {
            
            this.Graphics.PreferredBackBufferWidth = 480;
            this.Graphics.PreferredBackBufferHeight = ((480 / 4) * 3) + 110;
            
            //TODO: add: this.graphics.PreparingDeviceSettings += this.GraphicsDevicePreparingDeviceSettings;


            GameComponent enemy = new EnemyGameObject(this, "asd");
            this.Components.Add(enemy);
        }

       

        /// <summary>
        /// Add items to Components
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// This method renders the current state.
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Clear the screen
            GraphicsDevice.Clear(Color.White);

            base.Draw(gameTime);

        }

        //TODO: add GraphicsDevicePreparingDeviceSettings from XnaBasicsGame




    }
}