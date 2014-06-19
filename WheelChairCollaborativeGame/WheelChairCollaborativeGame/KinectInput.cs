#region Using Statements
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using WheelChairGameLibrary.Helpers;
using WheelChairGameLibrary.Screens;
using WheelChairGameLibrary.GameObjects;
using WheelChairGameLibrary.Sprites;

using KinectForWheelchair;
using KinectForWheelchair.Listeners;

using Microsoft.Kinect;

#endregion

namespace WheelChairCollaborativeGame
{
    class KinectInput : GameObject
    {


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


        int binNum;

        float distance;
        float angle;



        public EnhancedSkeletonCollection skeletons;

        //int binNum;




        public KinectInput(GameObjectManager gameObjectManager, String tag)
            : base(gameObjectManager, tag)        
        {
            // Create wheelchair detector
            wheelchairDetector = new WheelchairDetector();
            //wheelchairDetector.SkeletonFrameReady += new EventHandler<KinectForWheelchair.SkeletonFrameReadyEventArgs>(wheelchairDetector_SkeletonFrameReady);
            //wheelchairSkeletonFrame = new KinectInput();
            wheelchairDetector.SkeletonFrameReady += wheelchairDetector_SkeletonFrameReady;

            // Create linear nudge gesture
            linearNudgeGesture = new LinearNudgeListener(wheelchairDetector, skeletonId);
            //!!linearNudgeGesture.Triggered += new DCEventHandler(linearNudgeGesture_Triggered);

            // Create angular nudge gesture
            angularNudgeGesture = new AngularNudgeListener(wheelchairDetector, skeletonId);
            //!!angularNudgeGesture.Triggered += new DCEventHandler(angularNudgeGesture_Triggered);

            skeletons = new EnhancedSkeletonCollection();

            hand = GameObjectManager.GameScreen.ScreenManager.Game.Content.Load<Texture2D>("Space_Invader");
        }


        public override void  Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
 	         base.Draw(spriteBatch, gameTime);

            /*Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            ScreenManager.SpriteBatch.Begin();
            //ScreenManager.SpriteBatch.Draw(backgroundTexture, fullscreen, Color.White);
            ScreenManager.SpriteBatch.End();
            */


            GameObjectManager.GameScreen.ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            //GameObjectManager.GameScreen.ScreenManager.SpriteBatch.Begin();
            //ScreenManager.SpriteBatch.Draw(kinectRGBVideo, new Rectangle(0, 0, 640, 480), Color.White);
            //ScreenManager.SpriteBatch.Draw(hand, headPositionPixels, null, Color.White, 0, new Vector2(hand.Width / 2, hand.Height / 2), 1, SpriteEffects.None, 0);


            //GameObjectManager.GameScreen.ScreenManager.SpriteBatch.Begin();

            foreach (EnhancedSkeleton enhancedSkeleton in skeletons)
            {
                if (enhancedSkeleton.Skeleton != null)
                {
                    foreach (Joint joint in enhancedSkeleton.Skeleton.Joints)
                    {
                        Vector2 position = new Vector2((((0.5f * joint.Position.X) + 0.5f) * (640)), (((-0.5f * joint.Position.Y) + 0.5f) * (480)));
                        GameObjectManager.GameScreen.ScreenManager.SpriteBatch.Draw(hand, new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), 10, 10), Color.Red);
                    }
                }
            }

            string message = ("This is it");
            Vector2 textPosition = new Vector2(100.0f, 35.0f);
            GUImessage.MessageDraw(GameObjectManager.GameScreen.ScreenManager.SpriteBatch, GameObjectManager.GameScreen.ScreenManager.Game.Content, message, textPosition);

            //ScreenManager.SpriteBatch.End();
            //GameObjectManager.GameScreen.ScreenManager.SpriteBatch.End();


        }




        public void wheelchairDetector_SkeletonFrameReady(object sender, KinectForWheelchair.SkeletonFrameReadyEventArgs e)
        {
            using (EnhancedSkeletonFrame frame = e.OpenSkeletonFrame())
            {

                // Sometimes the frame can be null
                if (frame == null)
                    return;

                // Get skeletons
                frame.CopySkeletonDataTo(skeletons);

                // Track closest skeleton to Kinect
                IEnumerable<EnhancedSkeleton> trackedSkeletons = skeletons.Where(x => x.Skeleton.TrackingState == SkeletonTrackingState.Tracked);
                if (trackedSkeletons.Count() == 0)
                    return;

                EnhancedSkeleton skeleton = trackedSkeletons.Aggregate((x, y) => (x.Skeleton.Position.Z < y.Skeleton.Position.Z) ? x : y);


                // Make sure skeleton has valid info for listening
                if (skeleton == null || skeleton.Mode != Mode.Seated)
                    return;


                // Position
                {
                    // Get distance from Kinect
                    distance = skeleton.SeatedInfo.Features.Position.Length();

                    // Normalize distance between 0 and 1
                    const float minDistance = 1;
                    const float maxDistance = 2.5f;
                    distance = (distance - minDistance) / (maxDistance - minDistance);

                    // Invert distance
                    //distance = 1 - distance;

                    // Clamp values
                    distance = MathHelper.Clamp(distance, 0, 1);

                    // Calculate duty cycle
                    //int newDuty = (int)(Math.Abs(distance) * forward.Period);
                    //if (newDuty < 0)
                    //    newDuty = 0;
                    //if (newDuty > forward.Period)
                    //    newDuty = forward.Period;

                    // Set duty cycle
                    TankGameObject playerTank = (TankGameObject)GameObjectManager.getGameObject("playerTank");
                    //forward.Duty = newDuty;
                    Console.WriteLine("Distance: " + distance);
                    if (distance < 0.5f)
                    {
                        playerTank.setAttackStance();
                    }
                    else
                    {
                        playerTank.setDefenceStance();
                    }

                }


                // Angle
                {

                    // Get angle
                    angle = skeleton.SeatedInfo.Features.Angle;

                    // Normalize angle between -1 and 1
                    const float maxValue = 0.008f;
                    angle /= maxValue;

                    // Clamp values
                    angle = MathHelper.Clamp(angle, -1, 1);

                    // Use a quadratic relationship
                    angle = Math.Sign(angle) * angle * angle;

                    // Set dead zone
                    if (Math.Abs(angle) < 0.05)
                        angle = 0;

                    // Calculate duty cycle
                    int newDuty = (int)angle * pwmTurningPeriod;
                    if (newDuty < 0)
                        newDuty = -pwmTurningPeriod;
                    if (newDuty > pwmTurningPeriod)
                        newDuty = pwmTurningPeriod;

                    // Set duty cycle
                    //turning.Duty = newDuty;

                    //labelKey.Text = newDuty.ToString();
                    Console.WriteLine("Turn: " + newDuty.ToString());


                    TankGameObject playerTank = (TankGameObject)GameObjectManager.getGameObject("playerTank");

                    if (newDuty < 0)
                    {
                        playerTank.slideToRight();
                    }

                    if (newDuty > 0)
                    {
                        playerTank.slideToLeft();

                    }


                }



                //testing to draw
                //if (skeletonData != null)
                //{
                //    foreach (Skeleton skel in skeletonData)
                //    {
                //        if (skel.TrackingState == SkeletonTrackingState.Tracked)
                //        {
                            //aSkeleton = skeleton.Skeleton;
                //        }
                //    }
                //}


                /*
                // Normalize
                const float maxValue = 0.008f;
                angle = (angle + maxValue) / (2 * maxValue);


                // Clamp to between zero and one
                if (angle < 0)
                    angle = 0;
                if (angle > 1)
                    angle = 1;

                // Place in bin
                const int numBins = 3;
                int newBinNum = (int)Math.Floor(angle * numBins);
                if (newBinNum == 3)
                    newBinNum = 2;


                // Set timer
                if (newBinNum != binNum)
                {

                    // Unpress key
                    switch (binNum)
                    {
                        case 0: KeyMessaging.KeySender.SendKey(Keys.Right, false); break;
                        case 1: KeyMessaging.KeySender.SendKey(Keys.Up, false); break;
                        case 2: KeyMessaging.KeySender.SendKey(Keys.Left, false); break;
                    }

                    // Press key
                    switch (newBinNum)
                    {
                        case 0:
                            KeyMessaging.KeySender.SendKey(Keys.Right, true);
                            labelKey.Text = "Right";
                            break;

                        case 1:
                            KeyMessaging.KeySender.SendKey(Keys.Up, true);
                            labelKey.Text = "Straight"; break;

                        case 2:
                            KeyMessaging.KeySender.SendKey(Keys.Left, true);
                            labelKey.Text = "Left";
                            break;
                    }

                    binNum = newBinNum;
                }
                */

            }
        }
    }
}
