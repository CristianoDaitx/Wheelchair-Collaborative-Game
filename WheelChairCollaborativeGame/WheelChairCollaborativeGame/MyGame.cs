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

        
        public bool gameOver = false;
        double time;
        public TimeSpan timeSpan = TimeSpan.FromMilliseconds(120000);
      

        public MyGame()
        {
            
            this.Graphics.PreferredBackBufferWidth = (int)Config.resolution.X;
            this.Graphics.PreferredBackBufferHeight = (int)Config.resolution.Y;



            GameComponent enemy = new EnemyGameObject(new Vector2(50,50), this, "asd");
            this.Components.Add(enemy);

            

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
            base.Draw(gameTime);

            

        }
        public int X = 0;
        protected override void Update(GameTime gameTime)
        {
            
            
            
            timeSpan -= gameTime.ElapsedGameTime;
            Console.WriteLine("timer: " + timeSpan);
           
            if (timeSpan < TimeSpan.Zero && !gameOver)
            {
                
                gameOver = true;
             

                // Change state

            }
            

            base.Update(gameTime);
            InputState inputState = (InputState)Services.GetService(typeof(InputState));

            PlayerIndex playerIndex;
              if (inputState.IsKeyPressed(Keys.D, null, out playerIndex))
            {
                IsDebugMode = !IsDebugMode;
            }

            
            time += gameTime.ElapsedGameTime.TotalMilliseconds;


            //PlayerIndex newPlayerIndex;
            /*if (input.IsMenuSelect(null, out newPlayerIndex)){
                ExitScreen();
                //ScreenManager.AddScreen(new FighterChoose(), PlayerIndex.One);

                
            }
            */


           if (time > 120000)
            {
                X = -1;
                Console.WriteLine("Game Over!");
            }

            if (X == 0)
            {
                //ExitScreen();
                //ScreenManager.AddScreen(new FighterChoose(), PlayerIndex.One);
                // GameObjectManager.addGameObject(new EnemyGameObject(GameObjectManager,"enemy"));

                this.Components.Add(new WeakEnemy(this, "weakEnemy"));
                this.Components.Add(new WierdEnemy(this, "wierdEnemy"));
                X++;
            }

            if (X < 300)
            {
                X++;
            }

            if (X == 300)
            {
                this.Components.Add(new WeakEnemy2(this, "weakEnemy2"));
                X = 350;
            }

            if (X > 340 && X < 800)
            {
                     X++;
            }

           
            if (X == 800)
            {   
                this.Components.Add(new AvarageEnemy(this, "avarageEnemy"));
                X = 850;
            }
            if (X > 840 && X < 1500)
            {
                X++;
            }

            if (X == 1500)
            {   
                this.Components.Add(new HardEnemy(this, "hardEnemy"));
                X = 3500;
            }
            if (X > 1540)
            {
                X--;
            }
            if (X == 1540)
            {
                X = 0;
            }
        
        }
    
    }

}






