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
    sealed class KinectTriggerSingle : KinectTrigger
    {
        

        private JointType triggerJoint;
        private JointType baseJoint;
        private Vector3 relativePosition;

        

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
        public KinectTriggerSingle(JointType triggerJoint, JointType baseJoint, Vector3 relativePosition, float radius, float radiusThreshold, GraphicsDevice graphicsDevice)
            :base(radius,radiusThreshold, graphicsDevice)
        {
            this.triggerJoint = triggerJoint;
            this.baseJoint = baseJoint;
            this.relativePosition = relativePosition;

        }

        /// <summary>
        /// Calculates the trigger position, based in the baseJoint
        /// </summary>
        /// <returns>The trigger position</returns>
        protected override Vector3 getTriggerPosition()
        {
            if (trackingSkeleton == null)
                return Vector3.Zero;
            return skeletonPointToVector3(trackingSkeleton.Joints[baseJoint]) + relativePosition;
        }

        /// <summary>
        /// Check if the triggerJoint is inside the trigger zone. 
        /// </summary>
        /// <returns>True if triggerJoint is intersecting trigger zone, false otherwise.</returns>
        public override bool checkIsTriggered(GameTime gameTime)
        {
            //TODO: should be autamatically used in the xna update method
            if (trackingSkeleton == null)
                return false;

            float testingRadius = Radius;
            if (State == TriggerState.Inside)
                testingRadius += RadiusThreshold;

            BoundingSphere sphereTrigger = new BoundingSphere(getTriggerPosition(), testingRadius);
            BoundingSphere sphereJoint = new BoundingSphere( skeletonPointToVector3(trackingSkeleton.Joints[triggerJoint]), JOINT_DEFAULT_RADIUS);

            if (sphereTrigger.Intersects(sphereJoint))
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

