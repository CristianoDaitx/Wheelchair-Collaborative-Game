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

using Microsoft.Kinect;

using KinectForWheelchair;
using KinectForWheelchair.Listeners;
using KeyMessaging;

namespace WheelChairCollaborativeGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


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

        /* readonly ThresholdListener menuLeftListener;
         readonly ThresholdListener menuRightListener;
         readonly ThresholdListener menuEnterListener;


         // Mutable //

         EnhancedSkeletonCollection skeletons;

         int binNum;

         float distance;
         float angle;*/




        WheelchairSkeletonFrame wheelchairSkeletonFrame;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";







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

            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;


        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //KinectSensor.KinectSensors.StatusChanged += new EventHandler<StatusChangedEventArgs>(KinectSensors_StatusChanged);
            //DiscoverKinectSensor();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            kinectRGBVideo = new Texture2D(GraphicsDevice, 1337, 1337);

            hand = Content.Load<Texture2D>("Space_Invader");


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(kinectRGBVideo, new Rectangle(0, 0, 640, 480), Color.White);
            spriteBatch.Draw(hand, headPositionPixels, null, Color.White, 0, new Vector2(hand.Width / 2, hand.Height / 2), 1, SpriteEffects.None, 0);




            foreach (EnhancedSkeleton enhancedSkeleton in wheelchairSkeletonFrame.skeletons)
            {
                if (enhancedSkeleton.Skeleton != null)
                {
                    foreach (Joint joint in enhancedSkeleton.Skeleton.Joints)
                    {
                        Vector2 position = new Vector2((((0.5f * joint.Position.X) + 0.5f) * (640)), (((-0.5f * joint.Position.Y) + 0.5f) * (480)));
                        spriteBatch.Draw(hand, new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), 10, 10), Color.Red);
                    }
                }
            }

            GUImessage.MessageDraw(spriteBatch, Content);

            spriteBatch.End();


            base.Draw(gameTime);
        }








        /* void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
         {
             if (this.kinectSensor == e.Sensor)
             {
                 if (e.Status == KinectStatus.Disconnected ||
                     e.Status == KinectStatus.NotPowered)
                 {
                     this.kinectSensor = null;
                     this.DiscoverKinectSensor();
                 }
             }
         }*/

        /*private bool InitializeKinect()
        {
            // Color stream
            kinectSensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            kinectSensor.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(kinectSensor_ColorFrameReady);

            // Skeleton Stream
            kinectSensor.SkeletonStream.Enable(new TransformSmoothParameters()
            {
                Smoothing = 0.5f,
                Correction = 0.5f,
                Prediction = 0.5f,
                JitterRadius = 0.05f,
                MaxDeviationRadius = 0.04f
            });
            //kinectSensor.SkeletonFrameReady += new EventHandler<Microsoft.Kinect.SkeletonFrameReadyEventArgs>(kinectSensor_SkeletonFrameReady);

            try
            {
                kinectSensor.Start();
            }
            catch
            {
                //connectedStatus = "Unable to start the Kinect Sensor";
                return false;
            }
            return true;
        }*/

        /*void kinectSensor_SkeletonFrameReady(object sender, Microsoft.Kinect.SkeletonFrameReadyEventArgs  e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    Skeleton[] skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];

                    skeletonFrame.CopySkeletonDataTo(skeletonData);
                    Skeleton playerSkeleton = (from s in skeletonData where s.TrackingState == SkeletonTrackingState.Tracked select s).FirstOrDefault();
                    if (playerSkeleton != null)
                    {
                        Joint head = playerSkeleton.Joints[JointType.Head];
                        Console.Write("head position: " + head.Position.X + ", " + head.Position.Y + "\n");
                        headPositionPixels = new Vector2((((0.5f * head.Position.X) + 0.5f) * (640)), (((-0.5f * head.Position.Y) + 0.5f) * (480)));
                    }

                    





                }

            }
        }*/

        /*private void DiscoverKinectSensor()
        {
            foreach (KinectSensor sensor in KinectSensor.KinectSensors)
            {
                if (sensor.Status == KinectStatus.Connected)
                {
                    // Found one, set our sensor to this
                    kinectSensor = sensor;
                    break;
                }
            }

            if (this.kinectSensor == null)
            {
                connectedStatus = "Found none Kinect Sensors connected to USB";
                return;
            }

            // You can use the kinectSensor.Status to check for status
            // and give the user some kind of feedback
            switch (kinectSensor.Status)
            {
                case KinectStatus.Connected:
                    {
                        connectedStatus = "Status: Connected";
                        break;
                    }
                case KinectStatus.Disconnected:
                    {
                        connectedStatus = "Status: Disconnected";
                        break;
                    }
                case KinectStatus.NotPowered:
                    {
                        connectedStatus = "Status: Connect the power";
                        break;
                    }
                default:
                    {
                        connectedStatus = "Status: Error";
                        break;
                    }
            }

            // Init the found and connected device
            if (kinectSensor.Status == KinectStatus.Connected)
            {
                InitializeKinect();
            }
        }*/
        public float Scale(float value, int max)
        {
            return MathHelper.Clamp((max >> 1) + (value * (max >> 1)), 0, max);
        }
        /*void kinectSensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorImageFrame = e.OpenColorImageFrame())
            {
                if (colorImageFrame != null)
                {

                    byte[] pixelsFromFrame = new byte[colorImageFrame.PixelDataLength];

                    colorImageFrame.CopyPixelDataTo(pixelsFromFrame);

                    Color[] color = new Color[colorImageFrame.Height * colorImageFrame.Width];
                    kinectRGBVideo = new Texture2D(graphics.GraphicsDevice, colorImageFrame.Width, colorImageFrame.Height);

                    // Go through each pixel and set the bytes correctly
                    // Remember, each pixel got a Rad, Green and Blue
                    int index = 0;
                    for (int y = 0; y < colorImageFrame.Height; y++)
                    {
                        for (int x = 0; x < colorImageFrame.Width; x++, index += 4)
                        {
                            color[y * colorImageFrame.Width + x] = new Color(pixelsFromFrame[index + 2], pixelsFromFrame[index + 1], pixelsFromFrame[index + 0]);
                        }
                    }

                    // Set pixeldata from the ColorImageFrame to a Texture2D
                    kinectRGBVideo.SetData(color);
                }
            }
        }*/






















        /*void wheelchairDetector_SkeletonFrameReady(object sender, KinectForWheelchair.SkeletonFrameReadyEventArgs e)
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
                    const float minDistance = 2;
                    const float maxDistance = 2.5f;
                    distance = (distance - minDistance) / (maxDistance - minDistance);

                    // Invert distance
                    distance = 1 - distance;

                    // Clamp values
                    distance = MathHelper.Clamp(distance, 0, 1);

                    // Calculate duty cycle
                    //int newDuty = (int)(Math.Abs(distance) * forward.Period);
                    //if (newDuty < 0)
                    //    newDuty = 0;
                    //if (newDuty > forward.Period)
                    //    newDuty = forward.Period;

                    // Set duty cycle
                    //forward.Duty = newDuty;

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
                    int newDuty = (int)(Math.Abs(angle) * pwmTurningPeriod);
                    if (newDuty < 0)
                        newDuty = 0;
                    if (newDuty > pwmTurningPeriod)
                        newDuty = pwmTurningPeriod;

                    // Set duty cycle
                    //turning.Duty = newDuty;

                    //labelKey.Text = newDuty.ToString();
                    Console.WriteLine("Turn: " + newDuty.ToString());

                }



                //testing to draw
                //if (skeletonData != null)
                //{
                //    foreach (Skeleton skel in skeletonData)
                //    {
                //        if (skel.TrackingState == SkeletonTrackingState.Tracked)
                //        {
                            aSkeleton = skeleton.Skeleton;
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
                /

            }
        }*/


    }
}