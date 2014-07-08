#region Using Statements
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using WheelChairGameLibrary;
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
        Skeleton skeletonPlayerTank;
        Skeleton skeletonPlayerSoldier;
        bool kinectFrameChange;
        int controlSelect;
        private double time = 0;
        private double time2 = 0;
        private double timeSinc = 0;
        private int actionCount = 0;
        private int actionCount2 = 0;
        private int actionCountSinc = 0;


        bool isWireframe = true;
        RasterizerState wireFrameState;

        GeometricPrimitive currentPrimitive;

        KinectMovement movementSideTank;
        KinectMovement movementFrontTank;
        KinectMovement movementSideSoldier;
        KinectMovement movementFrontSoldier;

        KinectMovement movementDouble;
        KinectTriggerDouble triggerDouble;

        Texture2D kinectRGBVideo;

        /// <summary>
        /// This flag ensures only request a frame once per update call
        /// across the entire application.
        /// </summary>
        private static bool skeletonDrawn = true;

        /// <summary>
        /// The last frames skeleton data.
        /// </summary>
        private static Skeleton[] skeletons;

        // Constants //

        const int pwmForwardPeriod = 1000;
        const int pwmTurningPeriod = 200;

        // Readonly //

        readonly WheelchairDetector wheelchairDetector;

        float distance;
        float angle;

        //public EnhancedSkeletonCollection skeletons;


        private TankGameObject tankGameObject;



        //controller input handling
        OnOffController controllerOneOnOff;
        OnOffController controllerTwoOnOff;
        OnOffDouble controllerBothOnOff;

        //IOnOff movementBothFront;
        //IOnOff movementBothSide;

        public KinectInput(GameEnhanced game, String tag)
            : base(game, tag)
        {
            // controller input instantiaton
            controllerOneOnOff = new OnOffController(game);
            controllerTwoOnOff = new OnOffController(game);
            controllerBothOnOff = new OnOffDouble(controllerOneOnOff, controllerTwoOnOff);

            Game.Components.Add(controllerOneOnOff);
            Game.Components.Add(controllerTwoOnOff);








            wireFrameState = new RasterizerState()
            {
                FillMode = FillMode.WireFrame,
                CullMode = CullMode.None,
            };

            tankGameObject = (TankGameObject)Game.Components.FirstOrDefault(x => x.GetType() == typeof(TankGameObject) && ((TankGameObject)x).Tag == "playerTank"); //GameObjectManager.getGameObject("playerTank");

        }

        void movementDouble_MovementCompleted(object sender, KinectMovementEventArgs e)
        {
            GraphGameObject graph = (GraphGameObject)Game.Components.FirstOrDefault(x => x.GetType() == typeof(GraphGameObject) && ((GraphGameObject)x).Tag == "graph");
            GraphGameObject graph2 = (GraphGameObject)Game.Components.FirstOrDefault(x => x.GetType() == typeof(GraphGameObject) && ((GraphGameObject)x).Tag == "graphPlayer2");
            GraphGameObject graphSinc = (GraphGameObject)Game.Components.FirstOrDefault(x => x.GetType() == typeof(GraphGameObject) && ((GraphGameObject)x).Tag == "graphSinc");
            //if (graph.IsPressed == true && graph2.IsPressed == true)
            //graphSinc.IsPressed = true;
            //add ball
            actionCountSinc++;
            Game.Components.Add(new BallGameObject(tankGameObject.Position + new Vector2(tankGameObject.Size.X / 2, 0), Game, "ball"));

        }

        void movementDouble_MovementQuit(object sender, KinectMovementEventArgs e)
        {
            GraphGameObject graphSinc = (GraphGameObject)Game.Components.FirstOrDefault(x => x.GetType() == typeof(GraphGameObject) && ((GraphGameObject)x).Tag == "graphSinc");
            //graphSinc.IsPressed = false;
        }

        void movementOne_MovementQuit(object sender, KinectMovementEventArgs e)
        {
            if (skeletonPlayerTank != null)
                if (sender.Equals(movementFrontTank) || sender.Equals(movementSideTank))
                {
                    GraphGameObject graph = (GraphGameObject)Game.Components.FirstOrDefault(x => x.GetType() == typeof(GraphGameObject) && ((GraphGameObject)x).Tag == "graph");
                    GraphGameObject graphSinc = (GraphGameObject)Game.Components.FirstOrDefault(x => x.GetType() == typeof(GraphGameObject) && ((GraphGameObject)x).Tag == "graphSinc");
                    //graph.IsPressed = false;
                    //graphSinc.IsPressed = false;
                    time = 0;
                }
            if (skeletonPlayerSoldier != null)
                if (sender.Equals(movementFrontSoldier) || sender.Equals(movementSideSoldier))
                {
                    GraphGameObject graph2 = (GraphGameObject)Game.Components.FirstOrDefault(x => x.GetType() == typeof(GraphGameObject) && ((GraphGameObject)x).Tag == "graphPlayer2");
                    GraphGameObject graphSinc = (GraphGameObject)Game.Components.FirstOrDefault(x => x.GetType() == typeof(GraphGameObject) && ((GraphGameObject)x).Tag == "graphSinc");
                    //graph2.IsPressed = false;
                    //graphSinc.IsPressed = false;
                    time2 = 0;
                }



        }

        void movementOne_MovementCompleted(object sender, KinectMovementEventArgs e)
        {
            if (skeletonPlayerTank != null)
                if (sender.Equals(movementFrontTank) || sender.Equals(movementSideTank))
                {
                    GraphGameObject graph = (GraphGameObject)Game.Components.FirstOrDefault(x => x.GetType() == typeof(GraphGameObject) && ((GraphGameObject)x).Tag == "graph");
                    GraphGameObject graph2 = (GraphGameObject)Game.Components.FirstOrDefault(x => x.GetType() == typeof(GraphGameObject) && ((GraphGameObject)x).Tag == "graphPlayer2");
                    GraphGameObject graphSinc = (GraphGameObject)Game.Components.FirstOrDefault(x => x.GetType() == typeof(GraphGameObject) && ((GraphGameObject)x).Tag == "graphSinc");
                    //graph.IsPressed = true;
                    actionCount++;

                    /*if (graph2.IsPressed == true && (movementDouble.State == KinectMovement.MovementState.Activated || sender.Equals(movementFrontTank)))
                    {
                        //graphSinc.IsPressed = true;
                        actionCountSinc++;
                        //add ball
                        Game.Components.Add(new BallGameObject(tankGameObject.Position + new Vector2(tankGameObject.Size.X / 2, 0), Game, "ball"));
                    }
                    else
                    {
                        //graphSinc.IsPressed = false;

                    }*/
                }

            if (skeletonPlayerSoldier != null)
                if (sender.Equals(movementFrontSoldier) || sender.Equals(movementSideSoldier))
                {
                    GraphGameObject graph = (GraphGameObject)Game.Components.FirstOrDefault(x => x.GetType() == typeof(GraphGameObject) && ((GraphGameObject)x).Tag == "graph");
                    GraphGameObject graph2 = (GraphGameObject)Game.Components.FirstOrDefault(x => x.GetType() == typeof(GraphGameObject) && ((GraphGameObject)x).Tag == "graphPlayer2");
                    GraphGameObject graphSinc = (GraphGameObject)Game.Components.FirstOrDefault(x => x.GetType() == typeof(GraphGameObject) && ((GraphGameObject)x).Tag == "graphSinc");
                    //graph2.IsPressed = true;
                    actionCount2++;

                    /*if (graph.IsPressed == true && (movementDouble.State == KinectMovement.MovementState.Activated || sender.Equals(movementFrontSoldier)))
                    {
                        graphSinc.IsPressed = true;
                        actionCountSinc++;
                        //add ball
                        Game.Components.Add(new BallGameObject(tankGameObject.Position + new Vector2(tankGameObject.Size.X / 2, 0), Game, "ball"));

                    }
                    else
                    {
                        graphSinc.IsPressed = false;

                    }*/
                }


        }




        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // If the sensor is not found, not running, or not connected, stop now
            if (null == this.Chooser.Sensor ||
                false == this.Chooser.Sensor.IsRunning ||
                KinectStatus.Connected != this.Chooser.Sensor.Status)
            {
                return;
            }




            GraphGameObject graph = (GraphGameObject)Game.Components.FirstOrDefault(x => x.GetType() == typeof(GraphGameObject) && ((GraphGameObject)x).Tag == "graph");
            GraphGameObject graph2 = (GraphGameObject)Game.Components.FirstOrDefault(x => x.GetType() == typeof(GraphGameObject) && ((GraphGameObject)x).Tag == "graphPlayer2");
            GraphGameObject graphSinc = (GraphGameObject)Game.Components.FirstOrDefault(x => x.GetType() == typeof(GraphGameObject) && ((GraphGameObject)x).Tag == "graphSinc");
            PlayerIndex playerIndex = PlayerIndex.One;
            PlayerIndex player2 = PlayerIndex.Two;
            //bool isAction = graph.IsPressed;
            //bool isAction2 = graph2.IsPressed;
            //bool isActionSinc = graphSinc.IsPressed;

            InputState inputState = (InputState)Game.Services.GetService(typeof(InputState));


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

            //set correct input for graph
            switch (controlSelect)
            {
                case 0:
                    graph.IOnOff = controllerOneOnOff;
                    graph2.IOnOff = controllerTwoOnOff;
                    graphSinc.IOnOff = controllerBothOnOff;

                    //set to draw or not
                    movementFrontTank.Enabled = false;
                    movementFrontTank.Visible = false;
                    movementFrontSoldier.Enabled = false;
                    movementFrontSoldier.Visible = false;  
                    movementDouble.Enabled = false;
                    movementDouble.Visible = false;

                    break;
                case 1:
                    graph.IOnOff = movementFrontTank;
                    graph2.IOnOff = movementFrontSoldier;
                    graphSinc.IOnOff = new OnOffDouble(movementFrontSoldier, movementFrontTank);

                    //set to draw or not
                    movementFrontTank.Enabled = true;
                    movementFrontTank.Visible = true;       
                    movementFrontSoldier.Enabled = true;
                    movementFrontSoldier.Visible = true;    
                    movementDouble.Enabled = false;
                    movementDouble.Visible = false;


                    break;
                case 2:
                    graph.IOnOff = movementSideTank;
                    graph2.IOnOff = movementSideSoldier;
                    graphSinc.IOnOff = movementDouble;

                    //set to draw or not
                    movementFrontTank.Enabled = false;
                    movementFrontTank.Visible = false;
                    movementFrontSoldier.Enabled = false;
                    movementFrontSoldier.Visible = false;   
                    movementDouble.Enabled = true;
                    movementDouble.Visible = true;

                    break;
            }



            switch (controlSelect)
            {
                case 0:
                    if (controlSelect == 0)
                    {
                        if (inputState.IsButtonPressed(Buttons.A, playerIndex, out playerIndex))
                        {
                            actionCount++;
                            //isAction = true;
                            //Game.Components.Add(new BallGameObject(tankGameObject.Position + new Vector2(tankGameObject.Size.X / 2, 0), Game, "ball"));
                            controllerOneOnOff.IsOn = true;
                            if (controllerTwoOnOff.IsOn == true)
                            {
                                //isActionSinc = true;
                                actionCountSinc++;
                                //add ball
                                Game.Components.Add(new BallGameObject(tankGameObject.Position + new Vector2(tankGameObject.Size.X / 2, 0), Game, "ball"));
                            }

                        }


                        if (inputState.IsButtonReleased(Buttons.A, playerIndex, out playerIndex))
                        {
                            controllerOneOnOff.IsOn = false;
                        }


                        if (inputState.IsButtonPressed(Buttons.A, player2, out player2))
                        {
                            actionCount2++;
                            controllerTwoOnOff.IsOn = true;
                            if (controllerOneOnOff.IsOn == true)
                            {
                                actionCountSinc++;
                                //add ball
                                Game.Components.Add(new BallGameObject(tankGameObject.Position + new Vector2(tankGameObject.Size.X / 2, 0), Game, "ball"));
                            }
                        }

                        if (inputState.IsButtonReleased(Buttons.A, player2, out player2))
                        {
                            controllerTwoOnOff.IsOn = false;
                        }


                        if (controllerBothOnOff.isOn())
                        {
                            timeSinc += gameTime.ElapsedGameTime.TotalMilliseconds;
                        }
                        else
                        {
                            timeSinc = 0;
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
                                movementFrontTank.setTriggersTrackingSkeleton(skeletonPlayerTank);


                            }

                            if (skeletonPlayerSoldier != null)
                            {
                                movementFrontSoldier.setTriggersTrackingSkeleton(skeletonPlayerSoldier);


                            }

                            if (movementFrontTank.State == KinectMovement.MovementState.Activated)
                            {
                                time += gameTime.ElapsedGameTime.TotalMilliseconds;
                                /*if (time > 1000)
                                {
                                    if (mo == false)
                                    {
                                        isAction = false;
                                    }
                                }*/
                            }

                            if (movementFrontSoldier.State == KinectMovement.MovementState.Activated)
                            {
                                time2 += gameTime.ElapsedGameTime.TotalMilliseconds;
                                /*if (time2 > 1000)
                                {
                                    if (isAction == false)
                                    {
                                        isAction2 = false;
                                    }
                                }*/

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
                            if (skeletonPlayerSoldier != null && skeletonPlayerTank != null)
                            {
                                triggerDouble.TrackingSkeletonOne = skeletonPlayerTank;
                                triggerDouble.TrackingSkeletonTwo = skeletonPlayerSoldier;

                            }

                            if (movementDouble.State == KinectMovement.MovementState.Activated)
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





            if (skeletonDrawn)
            {
                using (var skeletonFrame = this.Chooser.Sensor.SkeletonStream.OpenNextFrame(0))
                {
                    // Sometimes we get a null frame back if no data is ready
                    if (null == skeletonFrame)
                    {
                        return;
                    }

                    // Reallocate if necessary
                    if (null == skeletons || skeletons.Length != skeletonFrame.SkeletonArrayLength)
                    {
                        skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    }

                    skeletonFrame.CopySkeletonDataTo(skeletons);

                    wheelchairDetector_SkeletonFrameReady();
                    //skeletonDrawn = false;
                    kinectFrameChange = true;
                }
            }


        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);


            /*Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            ScreenManager.SpriteBatch.Begin();
            //ScreenManager.SpriteBatch.Draw(backgroundTexture, fullscreen, Color.White);
            ScreenManager.SpriteBatch.End();
            */
            SharedSpriteBatch.Begin();
            //Game.GraphicsDevice.Clear(Color.CornflowerBlue);

            //draw video
            SharedSpriteBatch.Draw(kinectRGBVideo, new Rectangle(0, 0, 640, 480), Color.White);

            /*if (skeletonPlayerTank != null)
            {
                DrawSkeleton(skeletonPlayerTank.Skeleton, Color.Yellow);
            }

            if (skeletonPlayerSoldier != null)
            {
                DrawSkeleton(skeletonPlayerSoldier.Skeleton, Color.Red);
            }*/

            GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                         actionCountSinc.ToString(), new Vector2(60, 40));
            GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                        TimeSpan.FromMilliseconds(timeSinc).Seconds.ToString() + "    Bullet Size", new Vector2(60, 60));

            GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                        "Input method : " + controlSelect.ToString(), new Vector2(400, 80));

            //GUImessage.MessageDraw(SharedSpriteBatch, Game.Game.Content,
            //            movementDouble.State.ToString(), new Vector2(60, 80));


            string message = ("Actions made");
            Vector2 textPosition = new Vector2(100.0f, 35.0f);
            GUImessage.MessageDraw(SharedSpriteBatch, Game.Content, message, textPosition);



            SharedSpriteBatch.End();
            SharedSpriteBatch.Begin();

            if (isWireframe)
            {
                Game.GraphicsDevice.RasterizerState = wireFrameState;
            }
            else
            {
                Game.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            }


            if (skeletonPlayerTank != null)
                DrawPrimitiveSkeleton(skeletonPlayerTank, currentPrimitive, Color.YellowGreen);

            if (skeletonPlayerSoldier != null)
                DrawPrimitiveSkeleton(skeletonPlayerSoldier, currentPrimitive, Color.Honeydew);



            SharedSpriteBatch.End();

            // Reset the fill mode renderstate.
            Game.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;


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
                    primitive.Draw(world, KinectTriggerSingle.view, KinectTriggerSingle.projection, color);
                }
            }

        }

        private void DrawSkeleton(Skeleton skeleton, Color color)
        {
            foreach (Joint joint in skeleton.Joints)
            {
                Vector2 position = new Vector2((((0.5f * joint.Position.X) + 0.5f) * (640)), (((-0.5f * joint.Position.Y) + 0.5f) * (480)));

                PrimitiveDrawing.DrawCircle(Game.WhitePixel, SharedSpriteBatch, position, 4.0f, color, 4, 8);

            }

            //head
            PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch,
                screenPosition(skeleton.Joints[JointType.Head].Position), screenPosition(skeleton.Joints[JointType.ShoulderCenter].Position), color, 1);

            //left arm
            PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch,
                screenPosition(skeleton.Joints[JointType.ShoulderCenter].Position), screenPosition(skeleton.Joints[JointType.ShoulderLeft].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch,
                screenPosition(skeleton.Joints[JointType.ShoulderLeft].Position), screenPosition(skeleton.Joints[JointType.ElbowLeft].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch,
                screenPosition(skeleton.Joints[JointType.ElbowLeft].Position), screenPosition(skeleton.Joints[JointType.WristLeft].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch,
                screenPosition(skeleton.Joints[JointType.WristLeft].Position), screenPosition(skeleton.Joints[JointType.HandLeft].Position), color, 1);

            //right arm
            PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch,
                screenPosition(skeleton.Joints[JointType.ShoulderCenter].Position), screenPosition(skeleton.Joints[JointType.ShoulderRight].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch,
                screenPosition(skeleton.Joints[JointType.ShoulderRight].Position), screenPosition(skeleton.Joints[JointType.ElbowRight].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch,
                screenPosition(skeleton.Joints[JointType.ElbowRight].Position), screenPosition(skeleton.Joints[JointType.WristRight].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch,
                screenPosition(skeleton.Joints[JointType.WristRight].Position), screenPosition(skeleton.Joints[JointType.HandRight].Position), color, 1);

            //trunk
            PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch,
                screenPosition(skeleton.Joints[JointType.ShoulderLeft].Position), screenPosition(skeleton.Joints[JointType.Spine].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch,
                screenPosition(skeleton.Joints[JointType.ShoulderRight].Position), screenPosition(skeleton.Joints[JointType.Spine].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch,
                screenPosition(skeleton.Joints[JointType.Spine].Position), screenPosition(skeleton.Joints[JointType.HipCenter].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch,
                screenPosition(skeleton.Joints[JointType.HipCenter].Position), screenPosition(skeleton.Joints[JointType.HipLeft].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch,
                screenPosition(skeleton.Joints[JointType.HipCenter].Position), screenPosition(skeleton.Joints[JointType.HipRight].Position), color, 1);

            //left leg
            PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch,
                screenPosition(skeleton.Joints[JointType.HipLeft].Position), screenPosition(skeleton.Joints[JointType.KneeLeft].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch,
                screenPosition(skeleton.Joints[JointType.KneeLeft].Position), screenPosition(skeleton.Joints[JointType.AnkleLeft].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch,
                screenPosition(skeleton.Joints[JointType.AnkleLeft].Position), screenPosition(skeleton.Joints[JointType.FootLeft].Position), color, 1);

            //right leg
            PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch,
                screenPosition(skeleton.Joints[JointType.HipRight].Position), screenPosition(skeleton.Joints[JointType.KneeRight].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch,
                screenPosition(skeleton.Joints[JointType.KneeRight].Position), screenPosition(skeleton.Joints[JointType.AnkleRight].Position), color, 1);
            PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch,
                screenPosition(skeleton.Joints[JointType.AnkleRight].Position), screenPosition(skeleton.Joints[JointType.FootRight].Position), color, 1);

        }

        private Vector2 screenPosition(SkeletonPoint position)
        {
            return new Vector2(
                (0.5f * position.X + 0.5f) * Config.cameraResolution.Y,
                (-0.5f * position.Y + 0.5f) * Config.cameraResolution.X);
        }


        protected override void LoadContent()
        {
            kinectRGBVideo = new Texture2D(Game.GraphicsDevice, 480, 640);

            currentPrimitive = new SpherePrimitive(Game.GraphicsDevice, KinectTriggerSingle.JOINT_DEFAULT_RADIUS, 8);

            // Movement tank

            // Movement front
            Vector3 differenceFront1 = new Vector3(0.25f, -0.15f, -0.10f);
            Vector3 differenceFront2 = new Vector3(0.30f, -0.10f, -0.20f);
            Vector3 differenceFront3 = new Vector3(0.30f, -0.05f, -0.45f);

            movementFrontTank = new KinectMovement(Game,
                new KinectTriggerSingle(JointType.HandRight, JointType.Head, differenceFront1, 0.15f, 0.02f, Game.GraphicsDevice),
                new KinectTriggerSingle(JointType.HandRight, JointType.Head, differenceFront2, 0.15f, 0.02f, Game.GraphicsDevice),
                new KinectTriggerSingle(JointType.HandRight, JointType.Head, differenceFront3, 0.25f, 0.02f, Game.GraphicsDevice)
                );
            movementFrontTank.MovementCompleted += new KinectMovement.MovementCompletedEventHandler(movementOne_MovementCompleted);
            movementFrontTank.MovementQuit += new KinectMovement.MovementQuitEventHandler(movementOne_MovementQuit);
            movementFrontTank.MaxActiveTimeMiliseconds = 1000;
            Game.Components.Add(movementFrontTank);

            // Movement side
            Vector3 differenceSide1 = new Vector3(0.35f, -0.25f, -0.10f);
            Vector3 differenceSide2 = new Vector3(0.55f, -0.15f, -0.10f);
            Vector3 differenceSide3 = new Vector3(0.75f, -0.15f, -0.10f);

            movementSideTank = new KinectMovement(Game,
                new KinectTriggerSingle(JointType.HandRight, JointType.Head, differenceSide1, 0.15f, 0.02f, Game.GraphicsDevice),
                new KinectTriggerSingle(JointType.HandRight, JointType.Head, differenceSide2, 0.15f, 0.02f, Game.GraphicsDevice),
                new KinectTriggerSingle(JointType.HandRight, JointType.Head, differenceSide3, 0.25f, 0.02f, Game.GraphicsDevice)
                );
            movementSideTank.MovementCompleted += new KinectMovement.MovementCompletedEventHandler(movementOne_MovementCompleted);
            movementSideTank.MovementQuit += new KinectMovement.MovementQuitEventHandler(movementOne_MovementQuit);
            movementSideTank.MaxActiveTimeMiliseconds = 1000;
            Game.Components.Add(movementSideTank);

            // Movement soldier
            movementFrontSoldier = new KinectMovement(Game,
                new KinectTriggerSingle(JointType.HandRight, JointType.Head, differenceFront1, 0.15f, 0.02f, Game.GraphicsDevice),
                new KinectTriggerSingle(JointType.HandRight, JointType.Head, differenceFront2, 0.15f, 0.02f, Game.GraphicsDevice),
                new KinectTriggerSingle(JointType.HandRight, JointType.Head, differenceFront3, 0.25f, 0.02f, Game.GraphicsDevice)
                );
            movementFrontSoldier.MovementCompleted += new KinectMovement.MovementCompletedEventHandler(movementOne_MovementCompleted);
            movementFrontSoldier.MovementQuit += new KinectMovement.MovementQuitEventHandler(movementOne_MovementQuit);
            movementFrontSoldier.MaxActiveTimeMiliseconds = 1000;
            Game.Components.Add(movementFrontSoldier);

            movementSideSoldier = new KinectMovement(Game, 
                new KinectTriggerSingle(JointType.HandLeft, JointType.Head, differenceSide1 * new Vector3(-1, 1, 1), 0.15f, 0.02f, Game.GraphicsDevice),
                new KinectTriggerSingle(JointType.HandLeft, JointType.Head, differenceSide2 * new Vector3(-1, 1, 1), 0.15f, 0.02f, Game.GraphicsDevice),
                new KinectTriggerSingle(JointType.HandLeft, JointType.Head, differenceSide3 * new Vector3(-1, 1, 1), 0.25f, 0.02f, Game.GraphicsDevice)
                );
            movementSideSoldier.MovementCompleted += new KinectMovement.MovementCompletedEventHandler(movementOne_MovementCompleted);
            movementSideSoldier.MovementQuit += new KinectMovement.MovementQuitEventHandler(movementOne_MovementQuit);
            movementSideSoldier.MaxActiveTimeMiliseconds = 1000;
            Game.Components.Add(movementSideSoldier);

            //movement to check for side high five

            triggerDouble = new KinectTriggerDouble(JointType.HandRight, JointType.HandRight, JointType.HandLeft, JointType.HandLeft, 0.1f, 0.02f, Game.GraphicsDevice);
            movementDouble = new KinectMovement(Game, triggerDouble);
            movementDouble.MovementQuit += new KinectMovement.MovementQuitEventHandler(movementDouble_MovementQuit);
            movementDouble.MovementCompleted += new KinectMovement.MovementCompletedEventHandler(movementDouble_MovementCompleted);
            Game.Components.Add(movementDouble);

            GraphGameObject graph = new GraphGameObject(controllerOneOnOff, Game, "graph");
            Game.Components.Add(graph);
            graph.PressedY = 400;
            graph.NotPressedY = 420;

            GraphGameObject graph2 = new GraphGameObject(controllerTwoOnOff, Game, "graphPlayer2");
            Game.Components.Add(graph2);
            graph2.PressedY = 300;
            graph2.NotPressedY = 320;

            GraphGameObject graphSinc = new GraphGameObject(controllerBothOnOff, Game, "graphSinc");
            Game.Components.Add(graphSinc);
            graphSinc.PressedY = 200;
            graphSinc.NotPressedY = 220;

            base.LoadContent();
        }

        public void wheelchairDetector_SkeletonFrameReady()
        {


            /*// Sometimes the frame can be null
            if (frame == null)
                return;
            kinectFrameChange = true;
            // Get skeletons
            frame.CopySkeletonDataTo(skeletons);*/

            // Find and match skeletons
            IEnumerable<Skeleton> trackedSkeletons = skeletons.Where(x => x.TrackingState == SkeletonTrackingState.Tracked);
            if (trackedSkeletons.Count() == 0)
            {
                skeletonPlayerSoldier = null;
                skeletonPlayerTank = null;
                return;
            }

            IEnumerable<Skeleton> trackedSkeletonTankPlayers = new Skeleton[0];
            if (skeletonPlayerTank != null)
            {
                trackedSkeletonTankPlayers = trackedSkeletons.Where(x => x.TrackingId == skeletonPlayerTank.TrackingId);

            }

            IEnumerable<Skeleton> trackedSkeletonSoldierPlayers = new Skeleton[0];
            if (skeletonPlayerSoldier != null)
            {
                trackedSkeletonSoldierPlayers = trackedSkeletons.Where(x => x.TrackingId == skeletonPlayerSoldier.TrackingId);

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
                    skeletonPlayerTank = trackedSkeletons.FirstOrDefault(x => x.TrackingState == SkeletonTrackingState.Tracked);
                    if (skeletonPlayerTank != null)
                    {
                        // found tank skeleton
                    }


                    //TODO: track two skeletons
                    /*skeletonPlayerSoldier = trackedSkeletons.FirstOrDefault(x => x.TrackingState == SkeletonTrackingState.Tracked);
                    if (skeletonPlayerSoldier != null)
                    {
                        // found soldier skeleton
                    }*/

                }

            }
            else if (trackedSkeletonTankPlayers.Count() == 1)
            {
                // only tank skeleton match
                skeletonPlayerSoldier = null;
                if (trackedSkeletons.Count() == 2)
                {

                    skeletonPlayerSoldier = trackedSkeletons.FirstOrDefault(x => x.TrackingId != skeletonPlayerTank.TrackingId);
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
                    skeletonPlayerTank = trackedSkeletons.FirstOrDefault(x => x.TrackingId != skeletonPlayerSoldier.TrackingId);
                    if (skeletonPlayerTank != null)
                    {
                        // found tank skeleton
                    }
                }
            }



            /*// Make sure skeleton has valid info for listening
            if (skeleton == null || skeleton.Mode != Mode.Seated)
                return;*/

            if (skeletonPlayerTank == null)
                return;

            /*// Position
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
                TankGameObject playerTank = (TankGameObject)Game.Components.FirstOrDefault(x => ((GameObject)x).Tag == "playerTank");
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

                TankGameObject playerTank = (TankGameObject)Game.Components.FirstOrDefault(x => x.GetType() == typeof(GameObject) && ((GameObject)x).Tag == "playerTank");

                if (newDuty < 0)
                {
                    playerTank.slideToRight();
                }

                if (newDuty > 0)
                {
                    playerTank.slideToLeft();

                }
            }*/

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
                    kinectRGBVideo = new Texture2D(Game.GraphicsDevice, colorImageFrame.Width, colorImageFrame.Height);

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
