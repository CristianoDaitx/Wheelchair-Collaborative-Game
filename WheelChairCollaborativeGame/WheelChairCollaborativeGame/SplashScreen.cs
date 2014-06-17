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
        Texture2D hand;
        Vector2 headPositionPixels = new Vector2();

        //Skeleton aSkeleton;


        // Constants //

        const int skeletonId = -1;

        const int pwmForwardPeriod = 1000;
        const int pwmTurningPeriod = 200;

        const float handThreshold = 0.5f;
        const float handThresholdError = 0.1f;

        const float handDistanceThreshold = 0.7f;
        const float handDistanceThresholdError = 0.1f;

        // Readonly //

        readonly WheelchairDetector wheelchairDetector;

        // Listeners
        readonly LinearNudgeListener linearNudgeGesture;
        readonly AngularNudgeListener angularNudgeGesture;

         readonly ThresholdListener menuLeftListener;
         readonly ThresholdListener menuRightListener;
         readonly ThresholdListener menuEnterListener;


         // Mutable //

         EnhancedSkeletonCollection skeletons;

         int binNum;

         float distance;
         float angle;




        WheelchairSkeletonFrame wheelchairSkeletonFrame;

        



        public SplashScreen()
            :base()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1);
            TransitionOffTime = TimeSpan.FromSeconds(0);


            
            // Create wheelchair detector
            wheelchairDetector = new WheelchairDetector();
            //wheelchairDetector.SkeletonFrameReady += new EventHandler<KinectForWheelchair.SkeletonFrameReadyEventArgs>(wheelchairDetector_SkeletonFrameReady);
            wheelchairSkeletonFrame = new WheelchairSkeletonFrame();
            wheelchairDetector.SkeletonFrameReady += wheelchairSkeletonFrame.wheelchairDetector_SkeletonFrameReady;

            // Create linear nudge gesture
            linearNudgeGesture = new LinearNudgeListener(wheelchairDetector, skeletonId);
            //!!linearNudgeGesture.Triggered += new DCEventHandler(linearNudgeGesture_Triggered);

            // Create angular nudge gesture
            angularNudgeGesture = new AngularNudgeListener(wheelchairDetector, skeletonId);
            //!!angularNudgeGesture.Triggered += new DCEventHandler(angularNudgeGesture_Triggered);

            wheelchairSkeletonFrame.skeletons = new EnhancedSkeletonCollection();
             
        }


        public override void LoadContent()
        {
            base.LoadContent();

            time = 0;

            kinectRGBVideo = new Texture2D(ScreenManager.GraphicsDevice, 1337, 1337);

            hand = ScreenManager.Game.Content.Load<Texture2D>("Space_Invader");
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
            //ScreenManager.SpriteBatch.Draw(kinectRGBVideo, new Rectangle(0, 0, 640, 480), Color.White);
            //ScreenManager.SpriteBatch.Draw(hand, headPositionPixels, null, Color.White, 0, new Vector2(hand.Width / 2, hand.Height / 2), 1, SpriteEffects.None, 0);




            foreach (EnhancedSkeleton enhancedSkeleton in wheelchairSkeletonFrame.skeletons)
            {
                if (enhancedSkeleton.Skeleton != null)
                {
                    foreach (Joint joint in enhancedSkeleton.Skeleton.Joints)
                    {
                        Vector2 position = new Vector2((((0.5f * joint.Position.X) + 0.5f) * (640)), (((-0.5f * joint.Position.Y) + 0.5f) * (480)));
                        ScreenManager.SpriteBatch.Draw(hand, new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), 10, 10), Color.Red);
                    }
                }
            }

            GUImessage.MessageDraw(ScreenManager.SpriteBatch, ScreenManager.Game.Content);

            ScreenManager.SpriteBatch.End();


            base.Draw(gameTime);


        }

        public override void Update(GameTime gameTime, InputState input, bool otherScreenHasFocus, bool coveredByOtherScreen)
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
            }*/

        }
    }
}
