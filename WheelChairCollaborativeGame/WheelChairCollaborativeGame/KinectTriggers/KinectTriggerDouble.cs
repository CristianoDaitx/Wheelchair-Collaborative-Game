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
    class KinectTriggerDouble : KinnectTrigger
    {

        public Skeleton TrackingSkeletonOne
        {
            get { return trackingSkeletonOne; }
            set { trackingSkeletonOne = value; }
        }
        private Skeleton trackingSkeletonOne;

        public Skeleton TrackingSkeletonTwo
        {
            get { return trackingSkeletonTwo; }
            set { trackingSkeletonTwo = value; }
        }
        private Skeleton trackingSkeletonTwo;

        private JointType baseJointOne;
        private JointType triggerJointOne;        
        private JointType baseJointTwo;
        private JointType triggerJointTwo;


        public KinectTriggerDouble(JointType triggerJointOne, JointType baseJointOne, JointType triggerJointTwo, JointType baseJointTwo, float radius, float radiusThreshold, GraphicsDevice graphicsDevice)
            : base(radius, radiusThreshold, graphicsDevice)
        {
            this.triggerJointOne = triggerJointOne;
            this.baseJointOne = baseJointOne;
            this.triggerJointTwo = triggerJointTwo;
            this.baseJointTwo = baseJointTwo;
        }

        protected override Vector3 getTriggerPosition()
        {
            if (trackingSkeletonOne == null || trackingSkeletonTwo == null)
                return Vector3.Zero;
            return ((skeletonPointToVector3(TrackingSkeletonOne.Joints[baseJointOne]) + skeletonPointToVector3(TrackingSkeletonTwo.Joints[baseJointTwo])) / 2);
        }

        public override sealed bool checkIsTriggered()
        {
            //TODO: should be autamatically used in the xna update method
            if (TrackingSkeletonOne == null || TrackingSkeletonTwo == null)
                return false;

            float testingRadius = Radius;
            if (State == TriggerState.Inside)
                testingRadius += RadiusThreshold;

            BoundingSphere sphereTrigger = new BoundingSphere(getTriggerPosition(), testingRadius);
            BoundingSphere sphereJoint = new BoundingSphere(skeletonPointToVector3(TrackingSkeletonOne.Joints[triggerJointOne]), JOINT_DEFAULT_RADIUS);
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

    }
}
