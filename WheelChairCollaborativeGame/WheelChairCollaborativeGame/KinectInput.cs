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
        private double time = 0;
        private double time2 = 0;
        private double timeSinc = 0;


        bool isWireframe = false;
        RasterizerState wireFrameState;

        GeometricPrimitive currentPrimitive;

        KinectMovement movementSideTank;
        KinectMovement movementFrontTank;
        KinectMovement movementSideSoldier;
        KinectMovement movementFrontSoldier;

        KinectMovement movementDouble;
        KinectTriggerDouble triggerDouble;

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


        private int actionCount = 0;
        private int actionCount2 = 0;
        private int actionCountSinc = 0;

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


            // Movement tank

            // Movement front
            Vector3 differenceFront1 = new Vector3(0.25f, -0.15f, -0.10f);
            Vector3 differenceFront2 = new Vector3(0.30f, -0.10f, -0.20f);
            Vector3 differenceFront3 = new Vector3(0.30f, -0.05f, -0.45f);

            movementFrontTank = new KinectMovement(
                new KinectTrigger(JointType.HandRight, JointType.Head, differenceFront1, 0.15f, 0.02f, GameObjectManager.GameScreen.ScreenManager.GraphicsDevice),
                new KinectTrigger(JointType.HandRight, JointType.Head, differenceFront2, 0.15f, 0.02f, GameObjectManager.GameScreen.ScreenManager.GraphicsDevice),
                new KinectTrigger(JointType.HandRight, JointType.Head, differenceFront3, 0.25f, 0.02f, GameObjectManager.GameScreen.ScreenManager.GraphicsDevice)
                );
            movementFrontTank.MovementCompleted += new KinectMovement.MovementCompletedEventHandler(movementOne_MovementCompleted);
            movementFrontTank.MovementQuit += new KinectMovement.MovementQuitEventHandler(movementOne_MovementQuit);

            // Movement side
            Vector3 differenceSide1 = new Vector3(0.35f, -0.25f, -0.10f);
            Vector3 differenceSide2 = new Vector3(0.55f, -0.15f, -0.10f);
            Vector3 differenceSide3 = new Vector3(0.75f, -0.15f, -0.10f);

            movementSideTank = new KinectMovement(
                new KinectTrigger(JointType.HandRight, JointType.Head, differenceSide1, 0.15f, 0.02f, GameObjectManager.GameScreen.ScreenManager.GraphicsDevice),
                new KinectTrigger(JointType.HandRight, JointType.Head, differenceSide2, 0.15f, 0.02f, GameObjectManager.GameScreen.ScreenManager.GraphicsDevice),
                new KinectTrigger(JointType.HandRight, JointType.Head, differenceSide3, 0.25f, 0.02f, GameObjectManager.GameScreen.ScreenManager.GraphicsDevice)
                );
            movementSideTank.MovementCompleted += new KinectMovement.MovementCompletedEventHandler(movementOne_MovementCompleted);
            movementSideTank.MovementQuit += new KinectMovement.MovementQuitEventHandler(movementOne_MovementQuit);


            // Movement soldier
            movementFrontSoldier = new KinectMovement(
                new KinectTrigger(JointType.HandRight, JointType.Head, differenceFront1, 0.15f, 0.02f, GameObjectManager.GameScreen.ScreenManager.GraphicsDevice),
                new KinectTrigger(JointType.HandRight, JointType.Head, differenceFront2, 0.15f, 0.02f, GameObjectManager.GameScreen.ScreenManager.GraphicsDevice),
                new KinectTrigger(JointType.HandRight, JointType.Head, differenceFront3, 0.25f, 0.02f, GameObjectManager.GameScreen.ScreenManager.GraphicsDevice)
                );
            movementFrontSoldier.MovementCompleted += new KinectMovement.MovementCompletedEventHandler(movementOne_MovementCompleted);
            movementFrontSoldier.MovementQuit += new KinectMovement.MovementQuitEventHandler(movementOne_MovementQuit);

            movementSideSoldier = new KinectMovement(
                new KinectTrigger(JointType.HandLeft, JointType.Head, differenceSide1 * new Vector3(-1, 1, 1), 0.15f, 0.02f, GameObjectManager.GameScreen.ScreenManager.GraphicsDevice),
                new KinectTrigger(JointType.HandLeft, JointType.Head, differenceSide2 * new Vector3(-1, 1, 1), 0.15f, 0.02f, GameObjectManager.GameScreen.ScreenManager.GraphicsDevice),
                new KinectTrigger(JointType.HandLeft, JointType.Head, differenceSide3 * new Vector3(-1, 1, 1), 0.25f, 0.02f, GameObjectManager.GameScreen.ScreenManager.GraphicsDevice)
                );
            movementSideSoldier.MovementCompleted += new KinectMovement.MovementCompletedEventHandler(movementOne_MovementCompleted);
            movementSideSoldier.MovementQuit += new KinectMovement.MovementQuitEventHandler(movementOne_MovementQuit);


            //movement to check for side high five

            triggerDouble = new KinectTriggerDouble(JointType.HandRight, JointType.Head, JointType.HandLeft, JointType.Head, 0.2f, 0.02f, GameObjectManager.GameScreen.ScreenManager.GraphicsDevice);
            movementDouble = new KinectMovement(triggerDouble);
            movementDouble.MovementQuit += new KinectMovement.MovementQuitEventHandler(movementDouble_MovementQuit);
            movementDouble.MovementCompleted += new KinectMovement.MovementCompletedEventHandler(movementDouble_MovementCompleted);




            currentPrimitive = new SpherePrimitive(GameObjectManager.GameScreen.ScreenManager.GraphicsDevice, KinectTrigger.JOINT_DEFAULT_RADIUS, 8);
            wireFrameState = new RasterizerState()
            {
                FillMode = FillMode.WireFrame,
                CullMode = CullMode.None,
            };
        }

        void movementDouble_MovementCompleted(object sender, KinectMovementEventArgs e)
        {
            GraphGameObject graph = (GraphGameObject)GameObjectManager.getGameObject("graph");
            GraphGameObject graph2 = (GraphGameObject)GameObjectManager.getGameObject("graphPlayer2");
            GraphGameObject graphSinc = (GraphGameObject)GameObjectManager.getGameObject("graphSinc");
            if (graph.IsPressed == true && graph2.IsPressed == true)
                graphSinc.IsPressed = true;
        }

        void movementDouble_MovementQuit(object sender, KinectMovementEventArgs e)
        {
            GraphGameObject graphSinc = (GraphGameObject)GameObjectManager.getGameObject("graphSinc");
            graphSinc.IsPressed = false;
        }

        void movementOne_MovementQuit(object sender, KinectMovementEventArgs e)
        {
            if (skeletonPlayerTank != null)
                if (e.TrackingID == skeletonPlayerTank.Skeleton.TrackingId)
                {
                    GraphGameObject graph = (GraphGameObject)GameObjectManager.getGameObject("graph");
                    GraphGameObject graphSinc = (GraphGameObject)GameObjectManager.getGameObject("graphSinc");
                    graph.IsPressed = false;
                    graphSinc.IsPressed = false;
                    time = 0;
                }
            if (skeletonPlayerSoldier != null)
                if (e.TrackingID == skeletonPlayerSoldier.Skeleton.TrackingId)
                {
                    GraphGameObject graph2 = (GraphGameObject)GameObjectManager.getGameObject("graphPlayer2");
                    GraphGameObject graphSinc = (GraphGameObject)GameObjectManager.getGameObject("graphSinc");
                    graph2.IsPressed = false;
                    graphSinc.IsPressed = false;
                    time2 = 0;
                }



        }

        void movementOne_MovementCompleted(object sender, KinectMovementEventArgs e)
        {
            if (skeletonPlayerTank != null)
                if (e.TrackingID == skeletonPlayerTank.Skeleton.TrackingId)
                {
                    GraphGameObject graph = (GraphGameObject)GameObjectManager.getGameObject("graph");
                    GraphGameObject graph2 = (GraphGameObject)GameObjectManager.getGameObject("graphPlayer2");
                    GraphGameObject graphSinc = (GraphGameObject)GameObjectManager.getGameObject("graphSinc");
                    graph.IsPressed = true;
                    actionCount++;
                    if (graph2.IsPressed == true && movementDouble.State == KinectMovement.MovementState.Activated)
                    {
                        graphSinc.IsPressed = true;
                        actionCountSinc++;
                    }
                    else
                    {
                        graphSinc.IsPressed = false;
                        
                    }
                }

            if (skeletonPlayerSoldier != null)
                if (e.TrackingID == skeletonPlayerSoldier.Skeleton.TrackingId)
                {
                    GraphGameObject graph = (GraphGameObject)GameObjectManager.getGameObject("graph");
                    GraphGameObject graph2 = (GraphGameObject)GameObjectManager.getGameObject("graphPlayer2");
                    GraphGameObject graphSinc = (GraphGameObject)GameObjectManager.getGameObject("graphSinc");
                    graph2.IsPressed = true;
                    actionCount2++;

                    if (graph.IsPressed == true && movementDouble.State == KinectMovement.MovementState.Activated)
                    {
                        graphSinc.IsPressed = true;
                        actionCountSinc++;

                    }
                    else
                    {
                        graphSinc.IsPressed = false;
                        
                    }
                }


        }






        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);
            GraphGameObject graph = (GraphGameObject)GameObjectManager.getGameObject("graph");
            GraphGameObject graph2 = (GraphGameObject)GameObjectManager.getGameObject("graphPlayer2");
            GraphGameObject graphSinc = (GraphGameObject)GameObjectManager.getGameObject("graphSinc");
            PlayerIndex playerIndex = PlayerIndex.One;
            PlayerIndex player2 = PlayerIndex.Two;
            bool isAction = graph.IsPressed;
            bool isAction2 = graph2.IsPressed;
            bool isActionSinc = graphSinc.IsPressed;









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



            switch (controlSelect)
            {
                case 0:
                    if (controlSelect == 0)
                    {
                        if (inputState.IsButtonPressed(Buttons.A, playerIndex, out playerIndex))
                        {
                            actionCount++;
                            isAction = true;
                            if (isAction2 == true)
                            {
                                isActionSinc = true;
                                actionCountSinc++;
                            }
                        }
                        if (isAction == true)
                        {
                            time += gameTime.ElapsedGameTime.TotalMilliseconds;
                        }

                        if (inputState.IsButtonReleased(Buttons.A, playerIndex, out playerIndex))
                        {
                            time = 0;
                            isAction = false;
                            isActionSinc = false;
                        }
                        graph.IsPressed = isAction;
                        graphSinc.IsPressed = isActionSinc;

                        if (inputState.IsButtonPressed(Buttons.A, player2, out player2))
                        {
                            actionCount2++;
                            isAction2 = true;
                            if (isAction == true)
                            {
                                isActionSinc = true;
                                actionCountSinc++;
                            }
                        }
                        if (isAction2 == true)
                        {
                            time2 += gameTime.ElapsedGameTime.TotalMilliseconds;
                        }

                        if (inputState.IsButtonReleased(Buttons.A, player2, out player2))
                        {
                            time = 0;
                            isAction2 = false;
                            isActionSinc = false;
                        }
                        graph2.IsPressed = isAction2;
                        graphSinc.IsPressed = isActionSinc;

                        if (isAction == true && isAction2 == true)
                        {
                            isActionSinc = true;
                            timeSinc += gameTime.ElapsedGameTime.TotalMilliseconds;
                        }
                        else
                        {
                            timeSinc = 0;
                            isActionSinc = false;
                        }

                    }
                    break;
                case 1:
                    if (controlSelect == 1)
                    {
                        if (kinectFrameChange == true)
                        {
                            //kinect trigger part
                            if (skeletonPlayerTank != null)
                            {
                                movementFrontTank.setTriggersTrackingSkeleton(skeletonPlayerTank.Skeleton);
                                movementFrontTank.update();

                            }
                        
                            if (skeletonPlayerSoldier != null)
                            {
                                movementFrontSoldier.setTriggersTrackingSkeleton(skeletonPlayerSoldier.Skeleton);
                                movementFrontSoldier.update();

                            }
                            if (movementFrontTank.State == KinectMovement.MovementState.Activated && movementFrontSoldier.State == KinectMovement.MovementState.Activated)
                            {

                                timeSinc += gameTime.ElapsedGameTime.TotalMilliseconds;
                                
                            }
                            else
                            {
                                timeSinc = 0;
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
                            //kinect trigger part
                            if (skeletonPlayerTank != null)
                            {
                                movementSideTank.setTriggersTrackingSkeleton(skeletonPlayerTank.Skeleton);
                                movementSideTank.update();

                            }
                            if (movementSideTank.State == KinectMovement.MovementState.Activated)
                            {

                                time += gameTime.ElapsedGameTime.TotalMilliseconds;
                            }


                            if (skeletonPlayerSoldier != null)
                            {
                                movementSideSoldier.setTriggersTrackingSkeleton(skeletonPlayerSoldier.Skeleton);
                                movementSideSoldier.update();

                            }


                            if (skeletonPlayerSoldier != null && skeletonPlayerTank != null)
                            {
                                triggerDouble.TrackingSkeleton = skeletonPlayerTank.Skeleton;
                                triggerDouble.TrackingSkeletonTwo = skeletonPlayerSoldier.Skeleton;
                                movementDouble.update();
                            }
                            if (movementSideTank.State == KinectMovement.MovementState.Activated && movementSideSoldier.State == KinectMovement.MovementState.Activated
                                && movementDouble.State == KinectMovement.MovementState.Activated)
                            {

                                timeSinc += gameTime.ElapsedGameTime.TotalMilliseconds;

                            }
                            else
                            {
                                timeSinc = 0;
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
            GameObjectManager.GameScreen.ScreenManager.SpriteBatch.End();
            GameObjectManager.GameScreen.ScreenManager.SpriteBatch.Begin();
            GameObjectManager.GameScreen.ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            //draw video
            spriteBatch.Draw(kinectRGBVideo, new Rectangle(0, 0, 640, 480), Color.White);

            /*if (skeletonPlayerTank != null)
            {
                DrawSkeleton(skeletonPlayerTank.Skeleton, Color.Yellow);
            }

            if (skeletonPlayerSoldier != null)
            {
                DrawSkeleton(skeletonPlayerSoldier.Skeleton, Color.Red);
            }*/

            GUImessage.MessageDraw(GameObjectManager.GameScreen.ScreenManager.SpriteBatch, GameObjectManager.GameScreen.ScreenManager.Game.Content,
                         actionCountSinc.ToString(), new Vector2(60, 40));
            GUImessage.MessageDraw(GameObjectManager.GameScreen.ScreenManager.SpriteBatch, GameObjectManager.GameScreen.ScreenManager.Game.Content,
                        TimeSpan.FromMilliseconds(timeSinc).Seconds.ToString(), new Vector2(60, 60));

            //GUImessage.MessageDraw(GameObjectManager.GameScreen.ScreenManager.SpriteBatch, GameObjectManager.GameScreen.ScreenManager.Game.Content,
            //            movementDouble.State.ToString(), new Vector2(60, 80));


            string message = ("Actions made");
            Vector2 textPosition = new Vector2(100.0f, 35.0f);
            GUImessage.MessageDraw(GameObjectManager.GameScreen.ScreenManager.SpriteBatch, GameObjectManager.GameScreen.ScreenManager.Game.Content, message, textPosition);



            GameObjectManager.GameScreen.ScreenManager.SpriteBatch.End();
            GameObjectManager.GameScreen.ScreenManager.SpriteBatch.Begin();

            if (isWireframe)
            {
                GameObjectManager.GameScreen.ScreenManager.GraphicsDevice.RasterizerState = wireFrameState;
            }
            else
            {
                GameObjectManager.GameScreen.ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            }


            if (skeletonPlayerTank != null)
                DrawPrimitiveSkeleton(skeletonPlayerTank.Skeleton, currentPrimitive, Color.YellowGreen);

            if (skeletonPlayerSoldier != null)
                DrawPrimitiveSkeleton(skeletonPlayerSoldier.Skeleton, currentPrimitive, Color.Honeydew);


            if (controlSelect == 1)
            {
                movementFrontTank.drawTriggers();
                movementFrontSoldier.drawTriggers();
            }
            else if (controlSelect == 2)
            {
                //movementSideTank.drawTriggers();
                //movementSideSoldier.drawTriggers();

                triggerDouble.draw();
            }







            // Reset the fill mode renderstate.
            GameObjectManager.GameScreen.ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;


        }

        private void DrawPrimitiveSkeleton(Skeleton skeleton, GeometricPrimitive primitive, Color color)
        {

            if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
            {
                foreach (Joint joint in skeleton.Joints)
                {
                    Vector3 position = new Vector3(joint.Position.X, joint.Position.Y, joint.Position.Z);
                    Matrix world = new Matrix();
                    world = Matrix.CreateTranslation(position) * Matrix.CreateScale(new Vector3(-10f, 10f, 10f));
                    primitive.Draw(world, KinectTrigger.view, KinectTrigger.projection, color);
                }
            }

        }


        /*public static Vector3 ConvertRealWorldPoint(Vector3 position)
        {
            var returnVector = new Vector3();

            returnVector.X = position.X * -10f;
            returnVector.Y = position.Y * 10f;
            returnVector.Z = position.Z * 10f;
            return returnVector;
        }*/

        /*public static Vector3 ConvertRealWorldPoint(SkeletonPoint position)
        {
            var returnVector = new Vector3();

            returnVector.X = position.X * -10f;
            returnVector.Y = position.Y * 10f;
            returnVector.Z = position.Z * 10f;
            return returnVector;
        }*/

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

                                    float MIN_DEPTH = -0.3f;
                                    float MAX_DEPTH = 0.1f;

                                    //bool isAction = graph.IsPressed;

                                    Joint Hand = skeletonPlayerTank.Skeleton.Joints[JointType.HandRight];
                                    Joint Head = skeletonPlayerTank.Skeleton.Joints[JointType.Head];
                                    Joint Sholder = skeletonPlayerTank.Skeleton.Joints[JointType.ShoulderCenter];
                                    float HpositionZ = Hand.Position.Z - Head.Position.Z;
                                    float HpositionX = Hand.Position.X - Head.Position.X;
                                    float HpositionY = Hand.Position.Y - Head.Position.Y;


                                    switch (moveState)
                                    {
                                        case MoveState.startPosition:
                                            if (HpositionZ < START_DEPTH + TRESHOLD_DEPTH)
                                            {
                                                if (MIN_WIDTH < HpositionX && HpositionX < MAX_WIDTH)
                                                {

                                                    if (HpositionY > START_HEIGHT + TRESHOLD_HEIGHT)
                                                    {
                                                        moveState = MoveState.active;
                                                    }

                                                    isAction = true;
                                                    action_count++;

                                                }
                                            }

                                            if (HpositionZ > START_DEPTH)
                                            {
                                                moveState = MoveState.outside;
                                            }
                                            if (HpositionX < MIN_WIDTH - TRESHOLD_WIDTH || MAX_WIDTH + TRESHOLD_WIDTH < HpositionX)
                                            {
                                                moveState = MoveState.outside;
                                            }
                                            if (HpositionY < START_HEIGHT)
                                            {
                                                moveState = MoveState.outside;
                                            }
                                            break;

                                        case MoveState.active:
                                            if (HpositionZ > START_DEPTH)
                                            {
                                                moveState = MoveState.outside;
                                            }
                                            if (HpositionX < MIN_WIDTH - TRESHOLD_WIDTH || MAX_WIDTH + TRESHOLD_WIDTH < HpositionX)
                                            {
                                                moveState = MoveState.outside;
                                            }
                                            if (HpositionY < START_HEIGHT)
                                            {
                                                moveState = MoveState.outside;
                                            }
                                            break;

                                        case MoveState.outside:
                                            if (MIN_DEPTH < HpositionZ && HpositionZ < MAX_DEPTH + TRESHOLD_DEPTH) // (HpositionZ < START_DEPTH + TRESHOLD_DEPTH )
                                            {
                                                if (MIN_WIDTH < HpositionX && HpositionX < MAX_WIDTH)
                                                {
                                                    if (HpositionY > START_HEIGHT/* + TRESHOLD_HEIGHT/* /)
                                                    {
                                                        moveState = MoveState.startPosition;
                                                    }
                                                }
                                            }
                                            break;
                                    }

                                    Console.WriteLine("Distance " + HpositionZ);
                                    Console.WriteLine("State " + moveState);

                                    /*if (moveState == MoveState.active)
                                        graph.IsPressed = true;
                                    else
                                        graph.IsPressed = false;*/



                /*
                if (HpositionZ > -0.40 && HpositionZ < 0)
                {

                    Console.WriteLine("HpositionZ" + HpositionZ);
                    graph.IsPressed = false;

                }
                else
                {
                    if (HpositionY > 0)
                    {
                        Console.WriteLine("HpositionY" + HpositionY);
                        graph.IsPressed = true;
                    }

                }*/

                //}



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
