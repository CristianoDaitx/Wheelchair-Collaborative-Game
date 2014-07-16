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
        private int lastSecond = -1;
        private TimeSpan countdown;
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
                        "Timer: " + string.Format("{0:mm\\:ss}", countdown), new Vector2(30, 300));
            SpriteBatch.End();

            base.Draw(gameTime);

            

        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            timeRan += gameTime.ElapsedGameTime;
            countdown = (maxTime - timeRan);

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
            if (timeRan.Seconds != lastSecond) // the second has changed
            {
                if (timeRan.Seconds == 0)
                {
                    this.Components.Add(new WeakEnemy(this, "weakEnemy", WeakEnemy.Type.Right));
                }

                if (timeRan.Seconds == 2)
                {
                    this.Components.Add(new WeakEnemy(this, "weakEnemy2", WeakEnemy.Type.Left));
                    this.Components.Add(new WierdEnemy(this, "wierdEnemy"));
                }

                if (timeRan.Seconds == 5)
                {
                    this.Components.Add(new AvarageEnemy(this, "avarageEnemy"));
                }

                if (timeRan.Seconds == 20)
                {
                    this.Components.Add(new HardEnemy(this, "hardEnemy"));
                }

                if (timeRan.Seconds == 28)
                {
                    this.Components.Add(new WeakEnemy(this, "weakEnemy2", WeakEnemy.Type.Left));
                    this.Components.Add(new WierdEnemy(this, "wierdEnemy"));
                }

                if (timeRan.Seconds == 30)
                {
                    this.Components.Add(new WeakEnemy(this, "weakEnemy2", WeakEnemy.Type.Right));
                }

                if (timeRan.Seconds == 31)
                {
                    this.Components.Add(new WeakEnemy(this, "weakEnemy2", WeakEnemy.Type.Left));
                }

                if (timeRan.Seconds == 35)
                {
                    this.Components.Add(new AvarageEnemy(this, "avarageEnemy"));
                }

                if (timeRan.Seconds == 36)
                {
                    this.Components.Add(new WeakEnemy(this, "weakEnemy", WeakEnemy.Type.Right));
                }

                if (timeRan.Seconds == 37)
                {
                    this.Components.Add(new WeakEnemy(this, "weakEnemy", WeakEnemy.Type.Right));
                }
                if (timeRan.Seconds == 38)
                {
                    this.Components.Add(new WeakEnemy(this, "weakEnemy", WeakEnemy.Type.Right));
                }

                if (timeRan.Seconds == 39)
                {
                    this.Components.Add(new WeakEnemy(this, "weakEnemy", WeakEnemy.Type.Left));
                }
                if (timeRan.Seconds == 40)
                {
                    this.Components.Add(new WeakEnemy(this, "weakEnemy", WeakEnemy.Type.Left));
                }
                if (timeRan.Seconds == 41)
                {
                    this.Components.Add(new WeakEnemy(this, "weakEnemy", WeakEnemy.Type.Left));
                }

                if (timeRan.Seconds == 50)
                {
                    this.Components.Add(new HardEnemy(this, "hardEnemy"));
                }

                lastSecond = timeRan.Seconds;
            }

        }

    }

}






