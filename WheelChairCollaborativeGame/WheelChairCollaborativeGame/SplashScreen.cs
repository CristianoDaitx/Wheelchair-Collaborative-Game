﻿#region Using Statements
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

            //TankGameObject playerTank = new TankGameObject(GameObjectManager, "playerTank");
            //GameObjectManager.addGameObject(playerTank);


            //KinectInput kinectInput = new KinectInput(GameObjectManager, "kinectInput");
            //GameObjectManager.addGameObject(kinectInput);
           

            //EnemyGameObject enemy = new EnemyGameObject(GameObjectManager, "enemy");
            //GameObjectManager.addGameObject(enemy);

            //GraphGameObject graph = new GraphGameObject(GameObjectManager, "graph");
            //GameObjectManager.addGameObject(graph);
            //graph.IsPressed = true;

            /*GraphGameObject graph2 = new GraphGameObject(GameObjectManager, "graphPlayer2");
            GameObjectManager.addGameObject(graph2);
            graph2.PressedY = 300;
            graph2.NotPressedY = 320;

            GraphGameObject graphSinc = new GraphGameObject(GameObjectManager, "graphSinc");
            GameObjectManager.addGameObject(graphSinc);
            graphSinc.PressedY = 200;
            graphSinc.NotPressedY = 220;*/


            

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
            if (time > TimeSpan.FromSeconds(secondsToChangeScreen).TotalMilliseconds)
            {
                //ExitScreen();
                //ScreenManager.AddScreen(new FighterChoose(), PlayerIndex.One);
                //GameObjectManager.addGameObject(new EnemyGameObject(GameObjectManager,"enemy"));
                time = 0;
            }

        }
    }
}
