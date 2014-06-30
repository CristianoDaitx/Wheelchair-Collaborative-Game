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
    /// <summary>
    /// An helper that can be used to test if a specific joint is inside a trigger zone (relative to another joint), given a tracking skeleton.
    /// Can be used alone or with KinectMovemnt
    /// </summary>
    class KinectTrigger
    {
        public enum TriggerState
        {
            Inside,
            Outside
        }
        private TriggerState state = TriggerState.Outside;
        public TriggerState State
        {
            get { return state; }
            protected set { state = value; }
        }

        protected readonly int SPHERE_TESSELATION = 8;
        protected readonly Color COLOR = Color.Yellow;

        protected SpherePrimitive spherePrimitive;
        protected SpherePrimitive spherePrimitiveThreshold;

        protected JointType triggerJoint;
        protected JointType baseJoint;
        private Vector3 relativePosition;

        /// <summary>
        /// radius can only be set at once in creation, to create the SpherePrimitive
        /// </summary>       
        public float Radius
        {
            get { return radius; }
        }
        private float radius;

        /// <summary>
        /// same as radius
        /// </summary>        
        public float RadiusThreshold
        {
            get { return radiusThreshold; }
        }
        private float radiusThreshold;

        /// <summary>
        /// Sets a skeleton to the trigger work on.
        /// </summary>
        public Skeleton TrackingSkeleton
        {
            get { return trackingSkeleton; }
            set { trackingSkeleton = value; }
        }
        private Skeleton trackingSkeleton;

        /// <summary>
        /// Set base parameters and set drawing parameters
        /// </summary>
        /// <param name="triggerJoint">Trigger joint</param>
        /// <param name="baseJoint">The base joint of the trigger zone</param>
        /// <param name="relativePosition">The relative position of the trigger zone to the base joint</param>
        /// <param name="radius">Radius of the trigger zone sphere</param>
        /// <param name="radiusThreshold">Radius of the trigger that is used when the trigger is triggered, so the triggerOut position is a bit bigger</param>
        /// <param name="graphicsDevice">Used to draw</param>
        public KinectTrigger(JointType triggerJoint, JointType baseJoint, Vector3 relativePosition, float radius, float radiusThreshold, GraphicsDevice graphicsDevice)
        {
            this.triggerJoint = triggerJoint;
            this.baseJoint = baseJoint;
            this.relativePosition = relativePosition;
            this.radius = radius;
            this.radiusThreshold = radiusThreshold;
            this.spherePrimitive = new SpherePrimitive(graphicsDevice, radius * 2, SPHERE_TESSELATION);
            this.spherePrimitiveThreshold = new SpherePrimitive(graphicsDevice, (radius + radiusThreshold) * 2, SPHERE_TESSELATION);
        }

        /// <summary>
        /// Calculates the trigger position, based in the baseJoint
        /// </summary>
        /// <returns>The trigger position</returns>
        protected virtual Vector3 getTriggerPosition()
        {
            return skeletonPointToVector3(trackingSkeleton.Joints[baseJoint]) + relativePosition;
        }

        /// <summary>
        /// Check if the triggerJoint is inside the trigger zone. 
        /// </summary>
        /// <returns>True if triggerJoint is intersecting trigger zone, false otherwise.</returns>
        public virtual bool checkIsTriggered()
        {
            //TODO: should be autamatically used in the xna update method
            if (trackingSkeleton == null)
                return false;

            float testingRadius = radius;
            if (state == TriggerState.Inside)
                testingRadius += radiusThreshold;

            BoundingSphere sphereTrigger = new BoundingSphere(getTriggerPosition(), testingRadius);
            BoundingSphere sphereJoint = new BoundingSphere( skeletonPointToVector3(trackingSkeleton.Joints[triggerJoint]), JOINT_DEFAULT_RADIUS);

            if (sphereTrigger.Intersects(sphereJoint))
            {
                state = TriggerState.Inside;
                return true;
            }
            else
            {
                state = TriggerState.Outside;
                return false;
            }

        }

        public virtual void draw()
        {
            if (trackingSkeleton == null)
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
                                                    new Viewport(0,0, (int) Config.cameraResolution.Y, (int)Config.cameraResolution.X).AspectRatio,
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

