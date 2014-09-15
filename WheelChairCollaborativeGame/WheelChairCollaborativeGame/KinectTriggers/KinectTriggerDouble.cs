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
        private double activationVelocity = 1.0f;
        public double ActivationVelocity
        {
            get { return activationVelocity; }
            set { activationVelocity = value; }
        }
        private static readonly int MAX_DATAS = 4;

        private Queue<PositionTime> jointOneDatas = new Queue<PositionTime>(MAX_DATAS);
        private Queue<PositionTime> jointTwoDatas = new Queue<PositionTime>(MAX_DATAS);
        // Sum time between updates that have been called withoud a skeleton change
        private double helperTime = 0;

        private bool flagInInvalidTrigger = false;

        public delegate void NoVelocityOneEventHandler(object sender, EventArgs e);
        public event NoVelocityOneEventHandler NoVelocityOne;

        public delegate void NoVelocityTwoEventHandler(object sender, EventArgs e);
        public event NoVelocityTwoEventHandler NoVelocityTwo;

        /// <summary>
        /// TODO: I think its measuerd as meters per second
        /// </summary>
        public double JointOneVelocity
        {
            get
            {
                double sumation = 0;
                int x = 1;
                while (x < jointOneDatas.Count)
                {
                    Vector3 distance = (jointOneDatas.ElementAt(x).Position - jointOneDatas.ElementAt(x - 1).Position);
                    sumation += distance.Length() / jointOneDatas.ElementAt(x).Time;
                    x++;
                }

                if (sumation == 0)
                    return 0;
                else
                    return (sumation / x) * 1000.0f;
            }
        }

        public double JointTwoVelocity
        {
            get
            {
                double sumation = 0;
                int x = 1;
                while (x < jointTwoDatas.Count)
                {
                    Vector3 distance = (jointTwoDatas.ElementAt(x).Position - jointTwoDatas.ElementAt(x - 1).Position);
                    sumation += distance.Length() / jointTwoDatas.ElementAt(x).Time;
                    x++;
                }

                if (sumation == 0)
                    return 0;
                else
                    return (sumation / x) * 1000.0f;
            }
        }


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

        public override sealed bool checkIsTriggered(GameTime gameTime)
        {


            //TODO: should be autamatically used in the xna update method
            if (TrackingSkeletonOne == null || TrackingSkeletonTwo == null)
                return false;

            // adding data to velocity test
            {
                //testing only at one skeleton, because at this point, both skeletons have the same sizes and are not null
                if (jointOneDatas.Count < 1)
                {
                    jointOneDatas.Enqueue(new PositionTime(KinectTrigger.skeletonPointToVector3(TrackingSkeletonOne.Joints[triggerJointOne].Position), gameTime.ElapsedGameTime.TotalMilliseconds));
                    jointTwoDatas.Enqueue(new PositionTime(KinectTrigger.skeletonPointToVector3(TrackingSkeletonOne.Joints[triggerJointTwo].Position), gameTime.ElapsedGameTime.TotalMilliseconds));
                }
                //do not add if position is the same as before
                if (jointOneDatas.Last().Position != KinectTrigger.skeletonPointToVector3(TrackingSkeletonOne.Joints[triggerJointOne].Position) &&
                    jointTwoDatas.Last().Position != KinectTrigger.skeletonPointToVector3(TrackingSkeletonTwo.Joints[triggerJointTwo].Position))
                {
                    helperTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                    //add data to calculate velocity
                    jointOneDatas.Enqueue(new PositionTime(KinectTrigger.skeletonPointToVector3(TrackingSkeletonOne.Joints[triggerJointOne].Position), helperTime));
                    jointTwoDatas.Enqueue(new PositionTime(KinectTrigger.skeletonPointToVector3(TrackingSkeletonTwo.Joints[triggerJointTwo].Position), helperTime));

                    //limit size of data sample
                    if (jointOneDatas.Count >= MAX_DATAS)
                    {
                        jointOneDatas.Dequeue();
                        jointTwoDatas.Dequeue();
                    }

                    helperTime = 0;
                }
                else
                {
                    helperTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }


            float testingRadius = Radius;
            if (State == TriggerState.Inside)
                testingRadius += RadiusThreshold;

            BoundingSphere sphereTrigger = new BoundingSphere(getTriggerPosition(), testingRadius);
            BoundingSphere sphereJoint = new BoundingSphere(skeletonPointToVector3(TrackingSkeletonOne.Joints[triggerJointOne]), JOINT_DEFAULT_RADIUS);
            BoundingSphere sphereJointTwo = new BoundingSphere(skeletonPointToVector3(TrackingSkeletonTwo.Joints[triggerJointTwo]), JOINT_DEFAULT_RADIUS);

            //tests for velocity. If already inside, velocity isn't taken into account (as result of the OR)
            if (sphereTrigger.Intersects(sphereJoint) && sphereTrigger.Intersects(sphereJointTwo)
              && ((JointOneVelocity > 1f && JointTwoVelocity > 1f) || State == TriggerState.Inside))
            {
                //this IF can be turned off to not allow that reaching desired speed when already inside sphere, result
                //in an valid movement
                //if (!flagInInvalidTrigger)
                //{
                    State = TriggerState.Inside;
                    return true;
                //}
            }
            //tests to call events if some skelton didn't have enough speed. flagInInvalidatedTrigger is used
            //to not call event's in every update
            else if (sphereTrigger.Intersects(sphereJoint) && sphereTrigger.Intersects(sphereJointTwo)
              && ((JointOneVelocity <= 1f && JointTwoVelocity <= 1f)) && State == TriggerState.Outside)
            {
                if (!flagInInvalidTrigger)
                {
                    NoVelocityOne(this, EventArgs.Empty);
                    NoVelocityTwo(this, EventArgs.Empty);
                }
                flagInInvalidTrigger = true;
            }
            else if (sphereTrigger.Intersects(sphereJoint) && sphereTrigger.Intersects(sphereJointTwo)
              && ((JointOneVelocity <= 1f && JointTwoVelocity > 1f)) && State == TriggerState.Outside)
            {
                if (!flagInInvalidTrigger)
                    NoVelocityOne(this, EventArgs.Empty);
                flagInInvalidTrigger = true;
            }
            else if (sphereTrigger.Intersects(sphereJoint) && sphereTrigger.Intersects(sphereJointTwo)
              && ((JointOneVelocity > 1f && JointTwoVelocity <= 1f)) && State == TriggerState.Outside)
            {
                if (!flagInInvalidTrigger)
                    NoVelocityTwo(this, EventArgs.Empty);
                flagInInvalidTrigger = true;
            }
            


            else
            {
                State = TriggerState.Outside;
                flagInInvalidTrigger = false;
            }
            return false;
        }

    }

    class PositionTime
    {
        public Vector3 Position { get; set; }
        public double Time { get; set; }

        public PositionTime(Vector3 position, double time)
        {
            this.Position = position;
            this.Time = time;
        }
    }
}
