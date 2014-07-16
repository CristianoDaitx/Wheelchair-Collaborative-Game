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


        private bool gameOver = false;
        private TimeSpan timeRan;
        private TimeSpan maxTime = TimeSpan.FromMilliseconds(120000);

        private int updateCounts = 0;

        public MyGame()
        {

            this.Graphics.PreferredBackBufferWidth = (int)Config.resolution.X;
            this.Graphics.PreferredBackBufferHeight = (int)Config.resolution.Y;

            TankGameObject playerTank = new TankGameObject(this, "playerTank");
            this.Components.Add(playerTank);


            KinectInput kinectInput = new KinectInput(this, "kinectInput");
            this.Components.Add(kinectInput);

            Background background = new Background(this, 100);
            this.Components.Add(background);
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
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin();
            GUImessage.MessageDraw(SpriteBatch, Content,
                        "Timer: " + (maxTime - timeRan), new Vector2(30,300));
            SpriteBatch.End();

            base.Draw(gameTime);



        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            timeRan += gameTime.ElapsedGameTime;

            InputState inputState = (InputState)Services.GetService(typeof(InputState));

            PlayerIndex playerIndex;
            if (inputState.IsKeyPressed(Keys.D, null, out playerIndex))
            {
                IsDebugMode = !IsDebugMode;
            }


            //session time

            if (timeRan > maxTime && !gameOver)
            {
                gameOver = true;
                Console.WriteLine("Game Over!");
                return;
                // Change state
            }


            

            //scripted add of enemies
            if (updateCounts == 0)
            {
                this.Components.Add(new WeakEnemy(this, "weakEnemy", WeakEnemy.Type.Right));
            }

            if (updateCounts == 300)
            {
                this.Components.Add(new WeakEnemy(this, "weakEnemy2", WeakEnemy.Type.Left));
                this.Components.Add(new WierdEnemy(this, "wierdEnemy"));
            }

            if (updateCounts == 800)
            {
                this.Components.Add(new AvarageEnemy(this, "avarageEnemy"));
            }

            if (updateCounts == 1500)
            {
                this.Components.Add(new HardEnemy(this, "hardEnemy"));
            }

            updateCounts++;
        }

    }

}






