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
            
            this.Graphics.PreferredBackBufferWidth = (int)Config.resolution.Y;
            this.Graphics.PreferredBackBufferHeight = (int)Config.resolution.X;



            GameComponent enemy = new EnemyGameObject(this, "asd");
            this.Components.Add(enemy);

            TankGameObject playerTank = new TankGameObject(this, "playerTank");
            this.Components.Add(playerTank);


            

            KinectInput kinectInput = new KinectInput(this, "kinectInput");
            this.Components.Add(kinectInput);

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
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);

            

        }




    }
}