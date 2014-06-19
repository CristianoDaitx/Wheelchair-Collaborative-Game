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

        int secondsToChangeScreen = 5;
        double time = 0;




        
        //KinectSensor kinectSensor;

        //TODO
        //string connectedStatus = "Not connected";
        Texture2D kinectRGBVideo;





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

            kinectRGBVideo = new Texture2D(ScreenManager.GraphicsDevice, 1337, 1337);


            TankGameObject playerTank = new TankGameObject(GameObjectManager, "playerTank");
            GameObjectManager.addGameObject(playerTank);

            KinectInput kinectInput = new KinectInput(GameObjectManager, "kinectInput");
            GameObjectManager.addGameObject(kinectInput);

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

        /*public override void Update(GameTime gameTime, InputState input, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, input, otherScreenHasFocus, coveredByOtherScreen);

            time += gameTime.ElapsedGameTime.TotalMilliseconds;

            PlayerIndex newPlayerIndex;
            /*if (input.IsMenuSelect(null, out newPlayerIndex)){
                ExitScreen();
                //ScreenManager.AddScreen(new FighterChoose(), PlayerIndex.One);

                
            }

            if (time > TimeSpan.FromSeconds(secondsToChangeScreen).TotalMilliseconds)
            {
                ExitScreen();
                //ScreenManager.AddScreen(new FighterChoose(), PlayerIndex.One);
            }* /

        }*/
    }
}
