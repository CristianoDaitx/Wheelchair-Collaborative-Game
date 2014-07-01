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
    class KinectTriggerDouble : KinectTrigger
    {
        public Skeleton TrackingSkeletonTwo
        {
            get { return trackingSkeletonTwo; }
            set { trackingSkeletonTwo = value; }
        }
        private Skeleton trackingSkeletonTwo;



        private JointType baseJointTwo;
        private JointType triggerJointTwo;


        public KinectTriggerDouble(JointType triggerJoint, JointType baseJoint, JointType triggerJointTwo, JointType baseJointTwo, float radius, float radiusThreshold, GraphicsDevice graphicsDevice)
            : base(triggerJoint, baseJoint, Vector3.Zero, radius, radiusThreshold, graphicsDevice)
        {
            this.triggerJointTwo = triggerJointTwo;
            this.baseJointTwo = baseJointTwo;
        }

        protected override Vector3 getTriggerPosition()
        {
            return ((skeletonPointToVector3(TrackingSkeleton.Joints[baseJoint]) + skeletonPointToVector3(TrackingSkeletonTwo.Joints[baseJointTwo])) / 2);
        }

        public override bool checkIsTriggered()
        {
            //TODO: should be autamatically used in the xna update method
            if (TrackingSkeleton == null || TrackingSkeletonTwo == null)
                return false;

            float testingRadius = Radius;
            if (State == TriggerState.Inside)
                testingRadius += RadiusThreshold;

            BoundingSphere sphereTrigger = new BoundingSphere(getTriggerPosition(), testingRadius);
            BoundingSphere sphereJoint = new BoundingSphere(skeletonPointToVector3(TrackingSkeleton.Joints[triggerJoint]), JOINT_DEFAULT_RADIUS);
            BoundingSphere sphereJointTwo = new BoundingSphere(skeletonPointToVector3(TrackingSkeletonTwo.Joints[triggerJointTwo]), JOINT_DEFAULT_RADIUS);

            if (sphereTrigger.Intersects(sphereJoint) && sphereTrigger.Intersects(sphereJointTwo))
            {
                State = TriggerState.Inside;
                return true;
            }
            else
            {
                State = TriggerState.Outside;
                return false;
            }
        }

        public override void draw()
        {
            if (TrackingSkeleton == null || TrackingSkeletonTwo == null)
                return;

            //creates a world matrix with the proper scale
            Matrix world = new Matrix();
            world = Matrix.CreateTranslation(getTriggerPosition()) * kinectTo3DScale;

            switch (State)
            {
                case TriggerState.Inside:
                    spherePrimitiveThreshold.Draw(world, view, projection, COLOR);
                    break;
                case TriggerState.Outside:
                    spherePrimitive.Draw(world, view, projection, COLOR);
                    break;
            }
        
        }

    }
}
