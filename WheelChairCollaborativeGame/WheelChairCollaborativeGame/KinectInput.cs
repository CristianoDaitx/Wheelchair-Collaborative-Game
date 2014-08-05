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
using WheelChairGameLibrary.Sprites;

using KinectForWheelchair;
using KinectForWheelchair.Listeners;

using Microsoft.Kinect;

#endregion

namespace WheelChairCollaborativeGame
{
    class KinectInput : GameObject
    {

        

        // Position constants
        private readonly int GRAPH1_PRESSED_Y = 700;
        private readonly int GRAPH1_NOT_PRESSED_Y = 710;
        private readonly int GRAPH2_PRESSED_Y = 685;
        private readonly int GRAPH2_NOT_PRESSED_Y = 695;
        private readonly int GRAPH3_PRESSED_Y = 670;
        private readonly int GRAPH3_NOT_PRESSED_Y = 680;
        private readonly Vector2 ACTION_COUNT_POSITION = new Vector2(30, 170);
        private readonly Vector2 ACTION_TIME_POSITION = new Vector2(30, 200);
        private readonly Vector2 INPUT_METHOD_POSITION = new Vector2(30, 230);
        private readonly Vector2 PLAYER1_TRACKING_POSITION = new Vector2(180, 470);
        private readonly Vector2 PLAYER2_TRACKING_POSITION = new Vector2(870, 470);
        private readonly Vector2 JOINT_ONE_VELOCITY_POSITION = new Vector2(30, 350);
        private readonly Vector2 JOINT_TWO_VELOCITY_POSITION = new Vector2(30, 380);


        private Skeleton skeletonPlayerTank;
        private Skeleton skeletonPlayerSoldier;
        private TankGameObject tankGameObject;

        
        private double timePressed1 = 0;
        private double timePressed2 = 0;
        private double timePressedSync = 0;
        private int actionCount1 = 0;
        private int actionCount2 = 0;
        private int actionCountSync = 0;

        //drawing things
        private bool isWireframe = true;
        private RasterizerState wireFrameState;
        private GeometricPrimitive currentPrimitive;

        //movements
        private KinectMovement movementSideTank;
        private KinectMovement movementFrontTank;
        private KinectMovement movementSideSoldier;
        private KinectMovement movementFrontSoldier;
        private KinectMovement movementDouble;
        private KinectTriggerDouble triggerDouble;

        GraphGameObject graph1;
        GraphGameObject graph2;
        GraphGameObject graphSync;

        /// <summary>
        /// The last frame skeleton data.
        /// </summary>
        private static Skeleton[] skeletons;

        //controller input handling
        OnOffController controllerOneOnOff;
        OnOffController controllerTwoOnOff;
        OnOffDouble controllerBothOnOff;

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

            // finds the tank game object
            tankGameObject = (TankGameObject)Game.Components.FirstOrDefault(x => x.GetType() == typeof(TankGameObject) && ((TankGameObject)x).Tag == "playerTank");

        }

        void movementDouble_MovementCompleted(object sender, KinectMovementEventArgs e)
        {
            actionCountSync++;
            tankGameObject.fire();


        }

        void movementDouble_MovementQuit(object sender, KinectMovementEventArgs e)
        {
            //graphSinc.IsPressed = false;
        }


        void movementSingle_MovementCompleted(object sender, KinectMovementEventArgs e)
        {
            //count action for both movements of tank
            if (sender.Equals(movementFrontTank))
            {
                actionCount1++;
                //count and add ball if other action is also active

                if (movementFrontSoldier.isOn() || Config.ControlSelected == Config.ControlSelect.FrontAssyncronous)
                {
                    actionCountSync++;
                    tankGameObject.fire();
                }
            }

            //count action for both movements of soldier
            if (sender.Equals(movementFrontSoldier))
            {
                actionCount2++;
                //count and add ball if other action is also active
                if (movementFrontTank.isOn() || Config.ControlSelected == Config.ControlSelect.FrontAssyncronous)
                {
                    actionCountSync++;
                    tankGameObject.fire();
                }
            }
        }

        void movementSingle_MovementQuit(object sender, KinectMovementEventArgs e)
        {
            if (skeletonPlayerTank != null)
                //reset time for both movements of tank
                if (sender.Equals(movementFrontTank) || sender.Equals(movementSideTank))
                {
                    timePressed1 = 0;
                }
            if (skeletonPlayerSoldier != null)
                //reset time for both movements of soldier
                if (sender.Equals(movementFrontSoldier) || sender.Equals(movementSideSoldier))
                {
                    timePressed2 = 0;
                }

        }




        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            PlayerIndex playerIndex1 = PlayerIndex.One;
            PlayerIndex playerIndex2 = PlayerIndex.Two;

            InputState inputState = this.InputState;

            


            //allow to shot with spacebar if in debug mode
            //if (Game.IsDebugMode)
            if (inputState.IsKeyPressed(Keys.Space, playerIndex1, out playerIndex1))
            {                
                tankGameObject.fire();
            }



            // If the sensor is not found, not running, or not connected, stop now
            if (!this.Chooser.IsAvailable)
            {
                return;
            }


            //set correct input for graph and activate/deactivate movements
            switch (Config.ControlSelected)
            {
                case Config.ControlSelect.Joystick:
                    graph1.IOnOff = controllerOneOnOff;
                    graph2.IOnOff = controllerTwoOnOff;
                    graphSync.IOnOff = controllerBothOnOff;

                    //set to draw or not
                    movementFrontTank.Enabled = false;
                    movementFrontTank.Visible = false;
                    movementFrontSoldier.Enabled = false;
                    movementFrontSoldier.Visible = false;
                    movementDouble.Enabled = false;
                    movementDouble.Visible = false;

                    break;
                case Config.ControlSelect.Front:
                    graph1.IOnOff = movementFrontTank;
                    graph2.IOnOff = movementFrontSoldier;
                    graphSync.IOnOff = new OnOffDouble(movementFrontSoldier, movementFrontTank);

                    //set to draw or not
                    movementFrontTank.Enabled = true;
                    movementFrontTank.Visible = true;
                    movementFrontSoldier.Enabled = true;
                    movementFrontSoldier.Visible = true;
                    movementDouble.Enabled = false;
                    movementDouble.Visible = false;

                    movementFrontTank.setTriggersTrackingSkeleton(skeletonPlayerTank);
                    movementFrontSoldier.setTriggersTrackingSkeleton(skeletonPlayerSoldier);

                    break;
                case Config.ControlSelect.Side:
                    graph1.IOnOff = movementSideTank;
                    graph2.IOnOff = movementSideSoldier;
                    graphSync.IOnOff = movementDouble;

                    //set to draw or not
                    movementFrontTank.Enabled = false;
                    movementFrontTank.Visible = false;
                    movementFrontSoldier.Enabled = false;
                    movementFrontSoldier.Visible = false;
                    movementDouble.Enabled = true;
                    movementDouble.Visible = true;

                    triggerDouble.TrackingSkeletonOne = skeletonPlayerTank;
                    triggerDouble.TrackingSkeletonTwo = skeletonPlayerSoldier;

                    //test for sides of players:
                    if (skeletonPlayerTank != null && skeletonPlayerSoldier != null)
                        if ((KinectTrigger.skeletonPointToVector3(skeletonPlayerTank.Joints[JointType.Head].Position) - KinectTrigger.skeletonPointToVector3(skeletonPlayerSoldier.Joints[JointType.Head].Position)).X > 0)
                        {
                            //switched positions
                            Skeleton switchSkeleton = triggerDouble.TrackingSkeletonOne;
                            triggerDouble.TrackingSkeletonOne = triggerDouble.TrackingSkeletonTwo;
                            triggerDouble.TrackingSkeletonTwo = switchSkeleton;
                        }

                    break;
                case Config.ControlSelect.FrontAssyncronous:
                    graph1.IOnOff = movementFrontTank;
                    graph2.IOnOff = movementFrontSoldier;
                    graphSync.IOnOff = new OnOffDouble(movementFrontSoldier, movementFrontTank);

                    //set to draw or not
                    movementFrontTank.Enabled = true;
                    movementFrontTank.Visible = true;
                    movementFrontSoldier.Enabled = true;
                    movementFrontSoldier.Visible = true;
                    movementDouble.Enabled = false;
                    movementDouble.Visible = false;

                    movementFrontTank.setTriggersTrackingSkeleton(skeletonPlayerTank);
                    movementFrontSoldier.setTriggersTrackingSkeleton(skeletonPlayerSoldier);

                    break;
            }


            //timing counting and joystick input
            switch (Config.ControlSelected)
            {
                case Config.ControlSelect.Joystick:
                    {//check button pressing
                        if (inputState.IsButtonPressed(Buttons.A, playerIndex1, out playerIndex1))
                        {
                            actionCount1++;
                            controllerOneOnOff.IsOn = true;
                            if (controllerTwoOnOff.IsOn == true)
                            {
                                actionCountSync++;
                                //add ball
                                tankGameObject.fire();
                            }
                        }

                        if (inputState.IsButtonReleased(Buttons.A, playerIndex1, out playerIndex1))
                            controllerOneOnOff.IsOn = false;

                        if (inputState.IsButtonPressed(Buttons.A, playerIndex2, out playerIndex2))
                        {
                            actionCount2++;
                            controllerTwoOnOff.IsOn = true;
                            if (controllerOneOnOff.IsOn == true)
                            {
                                actionCountSync++;
                                //add ball
                                tankGameObject.fire();
                            }
                        }
                        if (inputState.IsButtonReleased(Buttons.A, playerIndex2, out playerIndex2))
                            controllerTwoOnOff.IsOn = false;
                    }

                    if (controllerBothOnOff.isOn())
                        timePressedSync += gameTime.ElapsedGameTime.TotalMilliseconds;
                    else
                        timePressedSync = 0;
                    break;

                case Config.ControlSelect.Front:
                    if (movementFrontTank.State == KinectMovement.MovementState.Activated)
                        timePressed1 += gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (movementFrontSoldier.State == KinectMovement.MovementState.Activated)
                        timePressed2 += gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (movementFrontTank.State == KinectMovement.MovementState.Activated && movementFrontSoldier.State == KinectMovement.MovementState.Activated)
                        timePressedSync += gameTime.ElapsedGameTime.TotalMilliseconds;
                    else
                        timePressedSync = 0;

                    break;

                case Config.ControlSelect.Side:
                    if (movementDouble.State == KinectMovement.MovementState.Activated)
                        timePressedSync += gameTime.ElapsedGameTime.TotalMilliseconds;
                    else
                        timePressedSync = 0;

                    break;
            }






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

                determineSkeletons();
            }



        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SharedSpriteBatch.Begin();

            /*if (skeletonPlayerTank != null)
            {
                DrawSkeleton(skeletonPlayerTank.Skeleton, Color.Yellow);
            }

            if (skeletonPlayerSoldier != null)
            {
                DrawSkeleton(skeletonPlayerSoldier.Skeleton, Color.Red);
            }*/



            //draw GUI text
            if (Game.IsDebugMode)
            {
                GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                            "Actions made:" + actionCountSync.ToString(), ACTION_COUNT_POSITION);
                GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                            "Bullet Size:" + TimeSpan.FromMilliseconds(timePressedSync).Seconds.ToString(), ACTION_TIME_POSITION);
                GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                            "Input method: " + Config.ControlSelected.ToString(), INPUT_METHOD_POSITION);
            }
            if (skeletonPlayerTank == null && Config.ControlSelected != Config.ControlSelect.Joystick)
                GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                    "Player One not tracked!", 1,  PLAYER1_TRACKING_POSITION);
            if (skeletonPlayerSoldier == null && Config.ControlSelected != Config.ControlSelect.Joystick)
                GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                    "Player Two not tracked!",1 , PLAYER2_TRACKING_POSITION);

            if (Config.ControlSelected == Config.ControlSelect.Side && Game.IsDebugMode)
            {
                GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                    "Joint One Velocity (m/s): " + triggerDouble.JointOneVelocity, JOINT_ONE_VELOCITY_POSITION);
                GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                    "Joint Two Velocity (m/s): " + triggerDouble.JointTwoVelocity, JOINT_TWO_VELOCITY_POSITION);
            }

            if (Game.IsDebugMode)
            {
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
            }


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
            currentPrimitive = new SpherePrimitive(Game.GraphicsDevice, KinectTriggerSingle.JOINT_DEFAULT_RADIUS, 8);

            // Movement tank

            // Movement front
            Vector3 differenceFront1 = new Vector3(0.25f, -0.15f, -0.25f);
            //Vector3 differenceFront2 = new Vector3(0.30f, -0.10f, -0.20f);
            Vector3 differenceFront3 = new Vector3(0.30f, 0.10f, -0.57f);

            movementFrontTank = new KinectMovement(Game,
                new KinectTriggerSingle(JointType.HandRight, JointType.ShoulderCenter, differenceFront1, 0.20f, 0.02f, Game.GraphicsDevice),
                //new KinectTriggerSingle(JointType.HandRight, JointType.ShoulderCenter, differenceFront2, 0.15f, 0.02f, Game.GraphicsDevice),
                new KinectTriggerSingle(JointType.HandRight, JointType.ShoulderCenter, differenceFront3, 0.27f, 0.02f, Game.GraphicsDevice)
                );
            movementFrontTank.MovementCompleted += new KinectMovement.MovementCompletedEventHandler(movementSingle_MovementCompleted);
            movementFrontTank.MovementQuit += new KinectMovement.MovementQuitEventHandler(movementSingle_MovementQuit);
            movementFrontTank.MaxActiveTimeMiliseconds = 150;
            Game.Components.Add(movementFrontTank);

            // Movement side
            Vector3 differenceSide1 = new Vector3(0.35f, -0.25f, -0.10f);
            Vector3 differenceSide2 = new Vector3(0.55f, -0.15f, -0.10f);
            Vector3 differenceSide3 = new Vector3(0.75f, -0.12f, -0.10f);

            movementSideTank = new KinectMovement(Game,
                new KinectTriggerSingle(JointType.HandRight, JointType.Head, differenceSide1, 0.15f, 0.02f, Game.GraphicsDevice),
                new KinectTriggerSingle(JointType.HandRight, JointType.Head, differenceSide2, 0.15f, 0.02f, Game.GraphicsDevice),
                new KinectTriggerSingle(JointType.HandRight, JointType.Head, differenceSide3, 0.25f, 0.02f, Game.GraphicsDevice)
                );
            movementSideTank.MovementCompleted += new KinectMovement.MovementCompletedEventHandler(movementSingle_MovementCompleted);
            movementSideTank.MovementQuit += new KinectMovement.MovementQuitEventHandler(movementSingle_MovementQuit);
            movementSideTank.MaxActiveTimeMiliseconds = 1000;
            Game.Components.Add(movementSideTank);

            // Movement soldier
            movementFrontSoldier = new KinectMovement(Game,
                new KinectTriggerSingle(JointType.HandRight, JointType.ShoulderCenter, differenceFront1, 0.15f, 0.02f, Game.GraphicsDevice),
                //new KinectTriggerSingle(JointType.HandRight, JointType.ShoulderCenter, differenceFront2, 0.15f, 0.02f, Game.GraphicsDevice),
                new KinectTriggerSingle(JointType.HandRight, JointType.ShoulderCenter, differenceFront3, 0.25f, 0.02f, Game.GraphicsDevice)
                );
            movementFrontSoldier.MovementCompleted += new KinectMovement.MovementCompletedEventHandler(movementSingle_MovementCompleted);
            movementFrontSoldier.MovementQuit += new KinectMovement.MovementQuitEventHandler(movementSingle_MovementQuit);
            movementFrontSoldier.MaxActiveTimeMiliseconds = 150;
            Game.Components.Add(movementFrontSoldier);

            movementSideSoldier = new KinectMovement(Game,
                new KinectTriggerSingle(JointType.HandLeft, JointType.Head, differenceSide1 * new Vector3(-1, 1, 1), 0.15f, 0.02f, Game.GraphicsDevice),
                new KinectTriggerSingle(JointType.HandLeft, JointType.Head, differenceSide2 * new Vector3(-1, 1, 1), 0.15f, 0.02f, Game.GraphicsDevice),
                new KinectTriggerSingle(JointType.HandLeft, JointType.Head, differenceSide3 * new Vector3(-1, 1, 1), 0.25f, 0.02f, Game.GraphicsDevice)
                );
            movementSideSoldier.MovementCompleted += new KinectMovement.MovementCompletedEventHandler(movementSingle_MovementCompleted);
            movementSideSoldier.MovementQuit += new KinectMovement.MovementQuitEventHandler(movementSingle_MovementQuit);
            movementSideSoldier.MaxActiveTimeMiliseconds = 1000;
            Game.Components.Add(movementSideSoldier);

            //movement to check for side high five

            triggerDouble = new KinectTriggerDouble(JointType.HandRight, JointType.HandRight, JointType.HandLeft, JointType.HandLeft, 0.1f, 0.02f, Game.GraphicsDevice);
            movementDouble = new KinectMovement(Game, triggerDouble);
            movementDouble.MovementQuit += new KinectMovement.MovementQuitEventHandler(movementDouble_MovementQuit);
            movementDouble.MovementCompleted += new KinectMovement.MovementCompletedEventHandler(movementDouble_MovementCompleted);
            Game.Components.Add(movementDouble);

            graph1 = new GraphGameObject(controllerOneOnOff, Game, "graph1");
            graph1.PressedY = GRAPH1_PRESSED_Y;
            graph1.NotPressedY = GRAPH1_NOT_PRESSED_Y;
            Game.Components.Add(graph1);

            graph2 = new GraphGameObject(controllerTwoOnOff, Game, "graph2");
            graph2.PressedY = GRAPH2_PRESSED_Y;
            graph2.NotPressedY = GRAPH2_NOT_PRESSED_Y;
            Game.Components.Add(graph2);

            graphSync = new GraphGameObject(controllerBothOnOff, Game, "graphSync");
            graphSync.PressedY = GRAPH3_PRESSED_Y;
            graphSync.NotPressedY = GRAPH3_NOT_PRESSED_Y;
            Game.Components.Add(graphSync);

            base.LoadContent();
        }

        public void determineSkeletons()
        {
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


        }
    }
}
