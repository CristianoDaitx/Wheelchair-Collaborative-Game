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
            
            this.Graphics.PreferredBackBufferWidth = (int)Config.resolution.Y;
            this.Graphics.PreferredBackBufferHeight = (int)Config.resolution.X;
            
            //TODO: add: this.graphics.PreparingDeviceSettings += this.GraphicsDevicePreparingDeviceSettings;


            GameComponent enemy = new EnemyGameObject(this, "asd");
            this.Components.Add(enemy);

            TankGameObject playerTank = new TankGameObject(this, "playerTank");
            this.Components.Add(playerTank);


            GraphGameObject graph = new GraphGameObject(this, "graph");
            this.Components.Add(graph);
            graph.PressedY = 400;
            graph.NotPressedY = 420;

            GraphGameObject graph2 = new GraphGameObject(this, "graphPlayer2");
            this.Components.Add(graph2);
            graph2.PressedY = 300;
            graph2.NotPressedY = 320;

            GraphGameObject graphSinc = new GraphGameObject(this, "graphSinc");
            this.Components.Add(graphSinc);
            graphSinc.PressedY = 200;
            graph2.NotPressedY = 220;

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

        //TODO: add GraphicsDevicePreparingDeviceSettings from XnaBasicsGame




    }
}