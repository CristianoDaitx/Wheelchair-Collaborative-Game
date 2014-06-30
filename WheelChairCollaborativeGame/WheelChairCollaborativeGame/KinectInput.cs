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


        EnhancedSkeleton skeletonPlayerTank;
        EnhancedSkeleton skeletonPlayerSoldier;
        bool kinectFrameChange;
        int controlSelect;
        


        Texture2D kinectRGBVideo;

        float wirstRotation;

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

        private float action_count = 0;
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
            wheelchairDetector.KinectSensor.SkeletonStream.Enable(new TransformSmoothParameters()
            {
                Smoothing = 0.5f,
                Correction = 0.5f,
                Prediction = 0.5f,
                JitterRadius = 0.05f,
                MaxDeviationRadius = 0.04f
            });
            //wheelchairDetector.SkeletonFrameReady += new EventHandler<KinectForWheelchair.SkeletonFrameReadyEventArgs>(wheelchairDetector_SkeletonFrameReady);
            //wheelchairSkeletonFrame = new KinectInput();
            wheelchairDetector.SkeletonFrameReady += wheelchairDetector_SkeletonFrameReady;

            kinectRGBVideo = new Texture2D(GameObjectManager.GameScreen.ScreenManager.GraphicsDevice, 480, 640);
            wheelchairDetector.KinectSensor.ColorStream.Enable();
            wheelchairDetector.KinectSensor.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(kinectSensor_ColorFrameReady);
            //wheelchairDetector.AllFramesReady += new EventHandler<KinectForWheelchair.AllFramesReadyEventArgs>(wheelchairDetector_AllFramesReady);

            // Create linear nudge gesture
            linearNudgeGesture = new LinearNudgeListener(wheelchairDetector, skeletonId);
            //!!linearNudgeGesture.Triggered += new DCEventHandler(linearNudgeGesture_Triggered);

            // Create angular nudge gesture
            angularNudgeGesture = new AngularNudgeListener(wheelchairDetector, skeletonId);
            //!!angularNudgeGesture.Triggered += new DCEventHandler(angularNudgeGesture_Triggered);

            skeletons = new EnhancedSkeletonCollection();

            hand = GameObjectManager.GameScreen.ScreenManager.Game.Content.Load<Texture2D>("Space_Invader");
        }






        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);
            GraphGameObject graph = (GraphGameObject)GameObjectManager.getGameObject("graph");
            PlayerIndex playerIndex = PlayerIndex.One;
            bool isAction = graph.IsPressed;
            
            



           
            //Action by pressing A on gamepad.
            
           
        
                if (inputState.IsKeyPressed(Keys.X, playerIndex, out playerIndex))
                {
                    controlSelect--;
                }
                if (inputState.IsKeyPressed(Keys.Space, playerIndex, out playerIndex))
                {
                    controlSelect++;
                }
            

            if (controlSelect == 3)
            {
                controlSelect = 2; 
            }
            if (controlSelect < 0)
            {
                controlSelect = 0;
            }
           
          
            
            switch (controlSelect){
                case 0:
                    if (controlSelect == 0){

                       
 
                        if (inputState.IsButtonPressed(Buttons.A, playerIndex, out playerIndex))
                            {
                                
                            
                            action_count++;
                            isAction = true;

                            
                            }
                        if (isAction == true)
                        {
                            action_count = action_count + 0.0166667f;
                        }
                            
                        if (inputState.IsButtonReleased(Buttons.A, playerIndex, out playerIndex))
                            {
                                
                            isAction = false;
                            }
                        graph.IsPressed = isAction;
                    }
                    break;
                    case 1:
                    if (controlSelect == 1){
                       
                        if (kinectFrameChange == true)
                            {
                            if (skeletonPlayerTank != null)
                                {
                                kinectFrameChange = false;
                                //Get arms position
                                float TRESHOLD_DEPTH = 0.2f;
                                float TRESHOLD_WIDTH = 0.1f;
                                float TRESHOLD_HEIGHT = 0.1f;
                                float MIN_WIDTH = -0.0f;
                                float MAX_WIDTH = 0.3f;
                                float START_DEPTH = -0.2f;
                                float START_HEIGHT = -0.1f;
                                
                                Joint Hand = skeletonPlayerTank.Skeleton.Joints[JointType.HandRight];
                                Joint Head = skeletonPlayerTank.Skeleton.Joints[JointType.Head];
                                Joint Sholder = skeletonPlayerTank.Skeleton.Joints[JointType.ShoulderCenter];
                                float HpositionZ = Hand.Position.Z - Head.Position.Z;
                                float HpositionX = Hand.Position.X - Head.Position.X;
                                float HpositionY = Hand.Position.Y - Head.Position.Y;
                                if (!isAction)
                                    {
                                    if (HpositionZ < START_DEPTH + TRESHOLD_DEPTH)
                                        {
                                        if (MIN_WIDTH < HpositionX && HpositionX < MAX_WIDTH)
                                            {
                                            if (HpositionY > START_HEIGHT + TRESHOLD_HEIGHT)
                                                {
                                                isAction = true;
                                                action_count++;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                    if (HpositionZ > START_DEPTH)
                                        {
                                        isAction = false;
                                        }
                                    if (HpositionX < MIN_WIDTH - TRESHOLD_WIDTH || MAX_WIDTH + TRESHOLD_WIDTH < HpositionX)
                                        {
                                        isAction = false;
                                        }
                                    if (HpositionY < START_HEIGHT)
                                        {
                                        isAction = false;
                                        }
                                    }
                                Console.WriteLine("Distance " + HpositionY);
                                graph.IsPressed = isAction;
                                }
                        }
                    }
                    break;
                case 2:
                    if (controlSelect == 2)
                    {

                        // high five
                        if (kinectFrameChange == true)
                        {
                            if (skeletonPlayerTank != null)
                            {
                                kinectFrameChange = false;
                                float TRESHOLD_DEPTH = 0.05f;
                                float TRESHOLD_WIDTH = 0.1f;
                                float TRESHOLD_HEIGHT = 0.1f;
                                float MIN_DEPTH = -0.2f;
                                float MAX_DEPTH = 0.1f;
                                float START_WIDTH = 0.4f;
                                float START_HEIGHT = 0.1f;

                                Joint Shoulder = skeletonPlayerTank.Skeleton.Joints[JointType.ShoulderRight];
                                Joint Hand = skeletonPlayerTank.Skeleton.Joints[JointType.HandRight];
                                Joint Head = skeletonPlayerTank.Skeleton.Joints[JointType.Head];




                                float xPosition = Hand.Position.X - Head.Position.X;
                                float zPosition = Hand.Position.Z - Head.Position.Z;
                                float yPosition = Hand.Position.Y - Shoulder.Position.Y;

                                //coming from not action
                                if (!graph.IsPressed)
                                {
                                    if (xPosition > START_WIDTH + TRESHOLD_WIDTH)
                                    {
                                        if (MIN_DEPTH < zPosition && zPosition < MAX_DEPTH)
                                        {
                                            if (yPosition > START_HEIGHT + TRESHOLD_HEIGHT)
                                            {
                                                isAction = true;
                                                action_count++;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (xPosition < START_WIDTH)
                                    {
                                        isAction = false;
                                    }
                                    if (zPosition < MIN_DEPTH - TRESHOLD_DEPTH || MAX_DEPTH + TRESHOLD_DEPTH < zPosition)
                                    {
                                        isAction = false;
                                    }
                                    if (yPosition < START_HEIGHT)
                                    {
                                        isAction = false;
                                    }



                                }

                                Console.WriteLine("Distance " + zPosition);
                                

                                graph.IsPressed = isAction;

                                wirstRotation = MathHelper.ToDegrees(skeletonPlayerTank.Skeleton.BoneOrientations[JointType.ShoulderRight].AbsoluteRotation.Quaternion.Z);

                            }
                        }
                    }
                    break;
                }
            Console.WriteLine("ControlSelect " + controlSelect);
            }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
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

            /*foreach (EnhancedSkeleton enhancedSkeleton in skeletons)
            {
                if (enhancedSkeleton.Skeleton != null)
                {
                    foreach (Joint joint in enhancedSkeleton.Skeleton.Joints)
                    {
                        Vector2 position = new Vector2((((0.5f * joint.Position.X) + 0.5f) * (640)), (((-0.5f * joint.Position.Y) + 0.5f) * (480)));
                        GameObjectManager.GameScreen.ScreenManager.SpriteBatch.Draw(hand, new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), 10, 10), Color.Red);
                    }
                }
            }*/

            spriteBatch.Draw(kinectRGBVideo, new Rectangle(0, 0, 640, 480), Color.White);

            if (skeletonPlayerTank != null)
            {
                DrawSkeleton(skeletonPlayerTank.Skeleton, Color.Yellow);
            }

            if (skeletonPlayerSoldier != null)
            {
                DrawSkeleton(skeletonPlayerSoldier.Skeleton, Color.Red);
            }
            
            GUImessage.MessageDraw(GameObjectManager.GameScreen.ScreenManager.SpriteBatch, GameObjectManager.GameScreen.ScreenManager.Game.Content,
                         action_count.ToString(), new Vector2(60, 40));



            string message = ("Actions made");
            Vector2 textPosition = new Vector2(100.0f, 35.0f);
            GUImessage.MessageDraw(GameObjectManager.GameScreen.ScreenManager.SpriteBatch, GameObjectManager.GameScreen.ScreenManager.Game.Content, message, textPosition);


            if (wirstRotation != null)
                GUImessage.MessageDraw(GameObjectManager.GameScreen.ScreenManager.SpriteBatch, GameObjectManager.GameScreen.ScreenManager.Game.Content,
                        wirstRotation.ToString(), new Vector2(600, 400));


            //ScreenManager.SpriteBatch.End();
            //GameObjectManager.GameScreen.ScreenManager.SpriteBatch.End();


        }
        

        private void DrawSkeleton(Skeleton skeleton, Color color)
        {
            foreach (Joint joint in skeleton.Joints)
            {
                Vector2 position = new Vector2((((0.5f * joint.Position.X) + 0.5f) * (640)), (((-0.5f * joint.Position.Y) + 0.5f) * (480)));

                PrimitiveDrawing.DrawCircle(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch, position, 4.0f, color, 4, 8);

            }

            //skeleton.JointsJoints[JointType.HandRight].position
            //Vector2 position1 = new Vector2((((0.5f * skeleton.Joints[JointType.HandRight].Position.X) + 0.5f) * (640)), (((-0.5f * skeleton.Joints[JointType.HandRight].Position.Y) + 0.5f) * (480)));
            //Vector2 position2 = new Vector2((((0.5f * skeleton.Joints[JointType.ElbowRight].Position.X) + 0.5f) * (640)), (((-0.5f * skeleton.Joints[JointType.ElbowRight].Position.Y) + 0.5f) * (480)));


            //head
            PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                screenPosition(skeleton.Joints[JointType.Head].Position), screenPosition(skeleton.Joints[JointType.ShoulderCenter].Position), color, 1);

            //left arm
            PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                screenPosition(skeleton.Joints[JointType.ShoulderCenter].Position), screenPosition(skeleton.Joints[JointType.ShoulderLeft].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                screenPosition(skeleton.Joints[JointType.ShoulderLeft].Position), screenPosition(skeleton.Joints[JointType.ElbowLeft].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                screenPosition(skeleton.Joints[JointType.ElbowLeft].Position), screenPosition(skeleton.Joints[JointType.WristLeft].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                screenPosition(skeleton.Joints[JointType.WristLeft].Position), screenPosition(skeleton.Joints[JointType.HandLeft].Position), color, 1);

            //right arm
            PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                screenPosition(skeleton.Joints[JointType.ShoulderCenter].Position), screenPosition(skeleton.Joints[JointType.ShoulderRight].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                screenPosition(skeleton.Joints[JointType.ShoulderRight].Position), screenPosition(skeleton.Joints[JointType.ElbowRight].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                screenPosition(skeleton.Joints[JointType.ElbowRight].Position), screenPosition(skeleton.Joints[JointType.WristRight].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                screenPosition(skeleton.Joints[JointType.WristRight].Position), screenPosition(skeleton.Joints[JointType.HandRight].Position), color, 1);

            //trunk
            PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                screenPosition(skeleton.Joints[JointType.ShoulderLeft].Position), screenPosition(skeleton.Joints[JointType.Spine].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                screenPosition(skeleton.Joints[JointType.ShoulderRight].Position), screenPosition(skeleton.Joints[JointType.Spine].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                screenPosition(skeleton.Joints[JointType.Spine].Position), screenPosition(skeleton.Joints[JointType.HipCenter].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                screenPosition(skeleton.Joints[JointType.HipCenter].Position), screenPosition(skeleton.Joints[JointType.HipLeft].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                screenPosition(skeleton.Joints[JointType.HipCenter].Position), screenPosition(skeleton.Joints[JointType.HipRight].Position), color, 1);

            //left leg
            PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                screenPosition(skeleton.Joints[JointType.HipLeft].Position), screenPosition(skeleton.Joints[JointType.KneeLeft].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                screenPosition(skeleton.Joints[JointType.KneeLeft].Position), screenPosition(skeleton.Joints[JointType.AnkleLeft].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                screenPosition(skeleton.Joints[JointType.AnkleLeft].Position), screenPosition(skeleton.Joints[JointType.FootLeft].Position), color, 1);

            //right leg
            PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                screenPosition(skeleton.Joints[JointType.HipRight].Position), screenPosition(skeleton.Joints[JointType.KneeRight].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                screenPosition(skeleton.Joints[JointType.KneeRight].Position), screenPosition(skeleton.Joints[JointType.AnkleRight].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                screenPosition(skeleton.Joints[JointType.AnkleRight].Position), screenPosition(skeleton.Joints[JointType.FootRight].Position), color, 1);

        }

        private Vector2 screenPosition(SkeletonPoint position)
        {
            return new Vector2(
                (0.5f * position.X + 0.5f) * Config.cameraResolution.Y,
                (-0.5f * position.Y + 0.5f) * Config.cameraResolution.X);
        }




        public void wheelchairDetector_SkeletonFrameReady(object sender, KinectForWheelchair.SkeletonFrameReadyEventArgs e)
        {
            using (EnhancedSkeletonFrame frame = e.OpenSkeletonFrame())
            {

                // Sometimes the frame can be null
                if (frame == null)
                    return;
                kinectFrameChange = true;
                // Get skeletons
                frame.CopySkeletonDataTo(skeletons);

                // Track closest skeleton to Kinect
                IEnumerable<EnhancedSkeleton> trackedSkeletons = skeletons.Where(x => x.Skeleton.TrackingState == SkeletonTrackingState.Tracked);
                if (trackedSkeletons.Count() == 0)
                {
                    skeletonPlayerSoldier = null;
                    skeletonPlayerTank = null;
                    return;
                }

                IEnumerable<EnhancedSkeleton> trackedSkeletonTankPlayers = new EnhancedSkeletonCollection();
                if (skeletonPlayerTank != null)
                {
                    trackedSkeletonTankPlayers = trackedSkeletons.Where(x => x.Skeleton.TrackingId == skeletonPlayerTank.Skeleton.TrackingId);
                    
                }

                IEnumerable<EnhancedSkeleton> trackedSkeletonSoldierPlayers = new EnhancedSkeletonCollection();
                if (skeletonPlayerSoldier != null)
                {
                    trackedSkeletonSoldierPlayers = trackedSkeletons.Where(x => x.Skeleton.TrackingId == skeletonPlayerSoldier.Skeleton.TrackingId);
                    
                }

                if (trackedSkeletonTankPlayers.Count() == 1 && trackedSkeletonSoldierPlayers.Count() == 1)
                {
                    //matching skeletons
                }
                else if (trackedSkeletonTankPlayers.Count() == 0 && trackedSkeletonSoldierPlayers.Count() == 0)
                {
                    // no matching skeletons
                    skeletonPlayerSoldier = null;
                    skeletonPlayerTank = null;


                    if (trackedSkeletons.Count() > 0)
                    {
                        skeletonPlayerTank = trackedSkeletons.FirstOrDefault(x => x.Skeleton.TrackingState == SkeletonTrackingState.Tracked && x.Mode == Mode.Seated);
                        if (skeletonPlayerTank != null)
                        {
                            // found tank skeleton
                        }

                        skeletonPlayerSoldier = trackedSkeletons.FirstOrDefault(x => x.Skeleton.TrackingState == SkeletonTrackingState.Tracked && x.Mode == Mode.Standing);
                        if (skeletonPlayerSoldier != null)
                        {
                            // found soldier skeleton
                        }

                    }

                }
                else if (trackedSkeletonTankPlayers.Count() == 1)
                {
                    // only tank skeleton match
                    skeletonPlayerSoldier = null;
                    if (trackedSkeletons.Count() == 2)
                    {

                        skeletonPlayerSoldier = trackedSkeletons.FirstOrDefault(x => x.Skeleton.TrackingId != skeletonPlayerTank.Skeleton.TrackingId && x.Mode == Mode.Standing);
                        if (skeletonPlayerSoldier != null)
                        {
                            // found soldier skeleton
                        }
                    }
                }
                else if (trackedSkeletonSoldierPlayers.Count() == 1)
                {
                    // only soldier skeleton match
                    skeletonPlayerTank = null;
                    if (trackedSkeletons.Count() == 2)
                    {
                        // found tank skeleton
                        skeletonPlayerTank = trackedSkeletons.FirstOrDefault(x => x.Skeleton.TrackingId != skeletonPlayerSoldier.Skeleton.TrackingId && x.Mode == Mode.Seated);
                        if (skeletonPlayerTank != null)
                        {
                            // found tank skeleton
                        }
                    }
                }



                /*EnhancedSkeleton skeleton = trackedSkeletons.Aggregate((x, y) => (x.Skeleton.Position.Z < y.Skeleton.Position.Z) ? x : y);


                // Make sure skeleton has valid info for listening
                if (skeleton == null || skeleton.Mode != Mode.Seated)
                    return;*/

                if (skeletonPlayerTank == null)
                    return;
/*
                GraphGameObject graph = (GraphGameObject)GameObjectManager.getGameObject("graph");
                 
                //Get arms position
                {

                    
                    float TRESHOLD_DEPTH = 0.05f;
                    float TRESHOLD_WIDTH = 0.1f;
                    float TRESHOLD_HEIGHT = 0.1f;
                    float MIN_WIDTH = -0.0f;
                    float MAX_WIDTH = 0.3f;
                    float START_DEPTH = -0.2f;
                    float START_HEIGHT = -0.1f;
                    
                    bool isAction = graph.IsPressed;

                    Joint Hand = skeletonPlayerTank.Skeleton.Joints[JointType.HandRight];
                    Joint Head = skeletonPlayerTank.Skeleton.Joints[JointType.Head];
                    Joint Sholder = skeletonPlayerTank.Skeleton.Joints[JointType.ShoulderCenter];
                    float HpositionZ = Hand.Position.Z - Head.Position.Z;
                    float HpositionX = Hand.Position.X - Head.Position.X;
                    float HpositionY = Hand.Position.Y - Head.Position.Y;

                    if (!isAction)
                    {
                        if (HpositionZ < START_DEPTH + TRESHOLD_DEPTH)
                        {
                            if (MIN_WIDTH < HpositionX && HpositionX < MAX_WIDTH)
                            {
                                if (HpositionY > START_HEIGHT + TRESHOLD_HEIGHT)
                                {
                                    isAction = true;
                                    action_count++;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (HpositionZ > START_DEPTH)
                        {
                            isAction = false;
                        }
                        if (HpositionX < MIN_WIDTH - TRESHOLD_WIDTH || MAX_WIDTH + TRESHOLD_WIDTH < HpositionX)
                        {
                            isAction = false;
                        }
                        if (HpositionY < START_HEIGHT)
                        {
                            isAction = false;
                        }




                    }

                    Console.WriteLine("Distance " + HpositionY);

                    graph.IsPressed = isAction;

                */

                    
                }



                /*
                // high five
                {

                    float TRESHOLD_DEPTH = 0.05f;
                    float TRESHOLD_WIDTH = 0.1f;
                    float TRESHOLD_HEIGHT = 0.1f;
                    float MIN_DEPTH = -0.2f;
                    float MAX_DEPTH = 0.1f;
                    float START_WIDTH = 0.4f;
                    float START_HEIGHT = 0.1f;

                    Joint Shoulder = skeletonPlayerTank.Skeleton.Joints[JointType.ShoulderRight];
                    Joint Hand = skeletonPlayerTank.Skeleton.Joints[JointType.HandRight];
                    Joint Head = skeletonPlayerTank.Skeleton.Joints[JointType.Head];

                    bool isAction = graph.IsPressed;


                    float xPosition = Hand.Position.X - Head.Position.X;
                    float zPosition = Hand.Position.Z - Head.Position.Z;
                    float yPosition = Hand.Position.Y - Shoulder.Position.Y;

                    //coming from not action
                    if (!graph.IsPressed)
                    {
                        if (xPosition > START_WIDTH + TRESHOLD_WIDTH)
                        {
                            if (MIN_DEPTH < zPosition && zPosition < MAX_DEPTH)
                            {
                                if (yPosition > START_HEIGHT + TRESHOLD_HEIGHT)
                                {
                                    isAction = true;
                                    action_count ++;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (xPosition < START_WIDTH)
                        {
                            isAction = false;
                        }
                        if (zPosition < MIN_DEPTH - TRESHOLD_DEPTH || MAX_DEPTH + TRESHOLD_DEPTH < zPosition)
                        {
                            isAction = false;
                        }
                        if (yPosition < START_HEIGHT)
                        {
                            isAction = false;
                        }



                    }

                    Console.WriteLine("Distance " + zPosition);


                    graph.IsPressed = isAction;

                    wirstRotation = MathHelper.ToDegrees(skeletonPlayerTank.Skeleton.BoneOrientations[JointType.ShoulderRight].AbsoluteRotation.Quaternion.Z);


                }
                */







                // Position
                {
                    // Get distance from Kinect
                    distance = skeletonPlayerTank.SeatedInfo.Features.Position.Length();

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
                    angle = skeletonPlayerTank.SeatedInfo.Features.Angle;

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
                



       void kinectSensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorImageFrame = e.OpenColorImageFrame())
            {
                if (colorImageFrame != null)
                {

                    byte[] pixelsFromFrame = new byte[colorImageFrame.PixelDataLength];

                    colorImageFrame.CopyPixelDataTo(pixelsFromFrame);

                    Color[] color = new Color[colorImageFrame.Height * colorImageFrame.Width];
                    kinectRGBVideo = new Texture2D(GameObjectManager.GameScreen.ScreenManager.GraphicsDevice, colorImageFrame.Width, colorImageFrame.Height);

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
        }
    
    }
}
