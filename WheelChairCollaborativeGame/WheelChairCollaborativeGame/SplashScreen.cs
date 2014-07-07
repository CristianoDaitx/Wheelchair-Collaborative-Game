#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using WheelChairGameLibrary.Helpers;
using WheelChairGameLibrary.Screens;
using WheelChairGameLibrary.GameObjects;

using KinectForWheelchair;
using KinectForWheelchair.Listeners;

using Microsoft.Kinect;

#endregion

namespace WheelChairCollaborativeGame
{
    public class SplashScreen : GameScreen
    {
        Texture2D backgroundTexture;

        int secondsToChangeScreen = 4;
        double time = 0;




        
        //KinectSensor kinectSensor;

        //TODO
        //string connectedStatus = "Not connected";
        





        KinectInput wheelchairSkeletonFrame;

        



        public SplashScreen()
            :base()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1);
            TransitionOffTime = TimeSpan.FromSeconds(0);             
        }


        public override void LoadContent()
        {
            base.LoadContent();

            time = 0;

            TankGameObject playerTank = new TankGameObject(GameObjectManager, "playerTank");
            GameObjectManager.addGameObject(playerTank);


            KinectInput kinectInput = new KinectInput(GameObjectManager, "kinectInput");
            GameObjectManager.addGameObject(kinectInput);
           

            EnemyGameObject weakEnemy = new WeakEnemy(GameObjectManager, "weakEnemy");
            //GameObjectManager.addGameObject(weakEnemy);
            EnemyGameObject weakEnemy2 = new WeakEnemy2(GameObjectManager, "weakEnemy2");
            //GameObjectManager.addGameObject(weakEnemy2);
            EnemyGameObject hardEnemy = new HardEnemy(GameObjectManager, "hardEnemy");
            //GameObjectManager.addGameObject(hardEnemy);
            EnemyGameObject avarageEnemy = new AvarageEnemy(GameObjectManager, "avarageEnemy");
            //GameObjectManager.addGameObject(avarageEnemy);

            GraphGameObject graph = new GraphGameObject(GameObjectManager, "graph");
            GameObjectManager.addGameObject(graph);
            //graph.IsPressed = true;

            GraphGameObject graph2 = new GraphGameObject(GameObjectManager, "graphPlayer2");
            GameObjectManager.addGameObject(graph2);
            graph2.PressedY = 300;
            graph2.NotPressedY = 320;

            GraphGameObject graphSinc = new GraphGameObject(GameObjectManager, "graphSinc");
            GameObjectManager.addGameObject(graphSinc);
            graphSinc.PressedY = 200;
            graphSinc.NotPressedY = 220;


            

            //backgroundTexture = ScreenManager.Game.Content.Load<Texture2D>("tiles/splash");
        }

        public override void Draw(GameTime gameTime)
        {
            
            /*Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            ScreenManager.SpriteBatch.Begin();
            //ScreenManager.SpriteBatch.Draw(backgroundTexture, fullscreen, Color.White);
            ScreenManager.SpriteBatch.End();
            */


            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            ScreenManager.SpriteBatch.Begin();
           

            ScreenManager.SpriteBatch.End();


            base.Draw(gameTime);


        }
        public int X = 0;
        public override void Update(GameTime gameTime, InputState input, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, input, otherScreenHasFocus, coveredByOtherScreen);
            
            time += gameTime.ElapsedGameTime.TotalMilliseconds;

            //PlayerIndex newPlayerIndex;
            /*if (input.IsMenuSelect(null, out newPlayerIndex)){
                ExitScreen();
                //ScreenManager.AddScreen(new FighterChoose(), PlayerIndex.One);

                
            }
            */
            if (X == 0)
            {
                //ExitScreen();
                //ScreenManager.AddScreen(new FighterChoose(), PlayerIndex.One);
               // GameObjectManager.addGameObject(new EnemyGameObject(GameObjectManager,"enemy"));
                
                GameObjectManager.addGameObject(new WeakEnemy(GameObjectManager, "weakEnemy"));
        
                X++;
                

            }

            if (X < 300)
            {

                X++;
            }

            if (X == 300)
            {
                GameObjectManager.addGameObject(new WeakEnemy2(GameObjectManager, "weakEnemy2"));
                X = 350;
            }

            if (X > 340 && X < 800)
            {
                
                X ++;
            }
            if (X == 800)
            {
                GameObjectManager.addGameObject(new AvarageEnemy(GameObjectManager, "avarageEnemy"));
                X = 850;
            }
            if (X > 840 && X < 1500)
            {

                X++;
            }

            if (X == 1500)
            {
                GameObjectManager.addGameObject(new HardEnemy(GameObjectManager, "hardEnemy"));
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
