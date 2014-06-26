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
        }

        private readonly int sphereTesselation = 8;
        private readonly Color color = Color.Yellow;

        SpherePrimitive spherePrimitive;
        SpherePrimitive spherePrimitiveThreshold;

        JointType baseJoint;
        Vector3 relativePosition;

        private float radius;
        public float Radius
        {
            get { return radius; }
        }

        private float radiusThreshold;
        public float RadiusThreshold
        {
            get { return radiusThreshold; }
        }

        private Skeleton trackingSkeleton;
        public Skeleton TrackingSkeleton
        {
            get { return trackingSkeleton; }
            set { trackingSkeleton = value; }
        }

        public KinectTrigger(JointType baseJoint, Vector3 relativePosition, float radius, float radiusThreshold, GraphicsDevice graphicsDevice)
        {
            this.baseJoint = baseJoint;
            this.relativePosition = relativePosition;
            this.radius = radius;
            this.radiusThreshold = radiusThreshold;
            this.spherePrimitive = new SpherePrimitive(graphicsDevice, radius * 2, sphereTesselation);
            this.spherePrimitiveThreshold = new SpherePrimitive(graphicsDevice, (radius + radiusThreshold) * 2, sphereTesselation);
        }

        /*public KinectTrigger(Joint baseJoint, Vector3 relativePosition)
        {
            this.baseJoint = skeletonPointToVector3(baseJoint);
            this.relativePosition = relativePosition;
        }*/

        private Vector3 getPosition()
        {
            /*SkeletonPoint skeletonPoint = new SkeletonPoint();
            skeletonPoint.X = relativePosition.X + baseJoint.Position.X;
            skeletonPoint.Y = relativePosition.Y + baseJoint.Position.Y;
            skeletonPoint.Z = relativePosition.Z + baseJoint.Position.Z;*/
            return skeletonPointToVector3(trackingSkeleton.Joints[baseJoint]) + relativePosition;
        }

        public bool checkIsTriggered(Joint joint)
        {
            if (trackingSkeleton == null)
                return false;

            float triggerRadius = radius;
            if (state == TriggerState.Inside)
                triggerRadius += radiusThreshold;

            BoundingSphere sphereTrigger = new BoundingSphere(getPosition(), triggerRadius);
            BoundingSphere sphereJoint = new BoundingSphere( skeletonPointToVector3(joint), 0.05f);

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

        public void draw()
        {
            if (trackingSkeleton == null)
                return;

            //GeometricPrimitive spherePrimitive = new SpherePrimitive(GameObjectManager.GameScreen.ScreenManager.GraphicsDevice, 0.2f, 8); //diameter is double from trigger radius, same scale
            Matrix world = new Matrix();
            world = Matrix.CreateTranslation(getPosition()) * kinectTo3DScale;
            switch (state)
            {
                case TriggerState.Inside:
                    spherePrimitiveThreshold.Draw(world, view, projection, color);
                    break;
                case TriggerState.Outside:
                    spherePrimitive.Draw(world, view, projection, color);
                    break;
            }
        }

        #region static methods and variables

        public static Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 0), new Vector3(0, 0, 100), Vector3.Up);

        public static Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                    //GameObjectManager.GameScreen.ScreenManager.GraphicsDevice.Viewport.AspectRatio,
                                                    new Viewport(0,0, (int) Config.cameraResolution.Y, (int)Config.cameraResolution.X).AspectRatio,
                                                    1.0f,
                                                    100);

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

