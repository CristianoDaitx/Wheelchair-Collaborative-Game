#region Using Statements
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;
#endregion

namespace WheelChairCollaborativeGame
{
    public abstract class KinectTrigger
    {

        public enum TriggerState
        {
            Inside,
            Outside
        }

        public TriggerState State
        {
            get { return state; }
            protected set { state = value; }
        }
        private TriggerState state = TriggerState.Outside;
       

        private readonly int SPHERE_TESSELATION = 8;
        private readonly Color COLOR = Color.Yellow;


        private SpherePrimitive spherePrimitive;
        private SpherePrimitive spherePrimitiveThreshold;

        /// <summary>
        /// radius can only be set at once in creation, to create the SpherePrimitive
        /// </summary>       
        public float Radius
        {
            get { return radius; }
            private set { radius = value; }
        }
        private float radius;

        /// <summary>
        /// same as radius
        /// </summary>        
        public float RadiusThreshold
        {
            get { return radiusThreshold; }
            private set { radiusThreshold = value; }
        }
        private float radiusThreshold;


        public KinectTrigger(float radius, float radiusThreshold, GraphicsDevice graphicsDevice)
        {
            this.radius = radius;
            this.radiusThreshold = radiusThreshold;
            this.spherePrimitive = new SpherePrimitive(graphicsDevice, radius * 2, SPHERE_TESSELATION);
            this.spherePrimitiveThreshold = new SpherePrimitive(graphicsDevice, (radius + radiusThreshold) * 2, SPHERE_TESSELATION);
        }

        /// <summary>
        /// Should be used to calculate the trigger position
        /// </summary>
        /// <returns></returns>
        protected abstract Vector3 getTriggerPosition();

        /// <summary>
        /// Should return true if the trigger is triggered
        /// </summary>
        /// <returns>the trigger position or a zero trigger if no position is currently available</returns>
        public abstract bool checkIsTriggered();


        /// <summary>
        /// A valid method to draw the trigger
        /// </summary>
        public void draw()
        {
            Vector3 triggerPosition = getTriggerPosition();
            if (triggerPosition == Vector3.Zero)
                return;

            //creates a world matrix with the proper scale
            Matrix world = new Matrix();
            world = Matrix.CreateTranslation(getTriggerPosition()) * kinectTo3DScale;

            switch (state)
            {
                case TriggerState.Inside:
                    spherePrimitiveThreshold.Draw(world, view, projection, COLOR);
                    break;
                case TriggerState.Outside:
                    spherePrimitive.Draw(world, view, projection, COLOR);
                    break;
            }
        }



        #region static methods and variables

        public static readonly float JOINT_DEFAULT_RADIUS = 0.05f;

        /// <summary>
        /// View that mimics the on from the kinect color frame
        /// </summary>
        public static Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 0), new Vector3(0, 0, 50), Vector3.Up);

        /// <summary>
        /// Perspective of view that mimics the on from the kinect color frame
        /// </summary>
        public static Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
            //GameObjectManager.GameScreen.ScreenManager.GraphicsDevice.Viewport.AspectRatio,
                                                    new Viewport(0, 0, (int)Config.cameraResolution.Y, (int)Config.cameraResolution.X).AspectRatio,
                                                    1.0f,
                                                    100);

        /// <summary>
        /// Transform kinect values (in meters) to a decent value in 3D coordinates. This scale transform the points to match the kinect color frame.
        /// </summary>
        public static Matrix kinectTo3DScale = Matrix.CreateScale(new Vector3(-10f, 10f, 10f));

        public static Vector3 skeletonPointToVector3(SkeletonPoint point)
        {
            return new Vector3(point.X, point.Y, point.Z);
        }

        public static Vector3 skeletonPointToVector3(Joint joint)
        {
            return skeletonPointToVector3(joint.Position);
        }

        #endregion
    }
}
