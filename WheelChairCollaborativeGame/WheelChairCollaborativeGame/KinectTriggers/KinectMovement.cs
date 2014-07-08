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


using KinectForWheelchair;
using KinectForWheelchair.Listeners;

using Microsoft.Kinect;

#endregion

namespace WheelChairCollaborativeGame
{

    /// <summary>
    /// Used with KinectTriggers to keep track of a bigger movement of a kinect skeleton
    /// </summary>
    class KinectMovement : DrawableGameComponent, IOnOff
    {
        public enum MovementState
        {
            Activated,
            Wating
        }
        private MovementState state = MovementState.Wating;
        public MovementState State
        {
            get { return state; }
        }

        private List<KinectTrigger> kinectTriggers = new List<KinectTrigger>();

        /// <summary>
        /// Keep track of the last active index of the triggers in the list
        /// </summary>
        public int lastActiveTriggerIndex = -1;

        private bool invalidatedMovement = false;

        public delegate void MovementCompletedEventHandler(object sender, KinectMovementEventArgs e);
        public event MovementCompletedEventHandler MovementCompleted;

        public delegate void MovementQuitEventHandler(object sender, KinectMovementEventArgs e);
        public event MovementQuitEventHandler MovementQuit;

        /// <summary>
        /// Set a maximum active time for the movement. Zero makes it unlimited
        /// </summary>
        public int MaxActiveTimeMiliseconds {get; set;}
        private double time = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kinectTriggers">A list of triggers to be used in this movement</param>
        public KinectMovement(Game game, params KinectTrigger[] kinectTriggers)
            : base(game)
        {
            foreach (KinectTrigger trigger in kinectTriggers)
                addTrigger(trigger);

            MaxActiveTimeMiliseconds = 0;
        }

        public void addTrigger(KinectTrigger kinectTrigger)
        {
            kinectTriggers.Add(kinectTrigger);
        }

        /// <summary>
        /// Sets the trackingSkeleton for all kinectTriggers in this movement
        /// </summary>
        /// <param name="skeleton"></param>
        public void setTriggersTrackingSkeleton(Skeleton trackingSkeleton)
        {
            foreach (KinectTriggerSingle trigger in kinectTriggers)
                trigger.TrackingSkeleton = trackingSkeleton;
        }

        /// <summary>
        /// Draw all triggers inside this movement
        /// </summary>
        /// 
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            foreach (KinectTrigger trigger in kinectTriggers)
                trigger.draw();
        }

        /// <summary>
        /// Checks the actual situation of the movement. Fires an event if the movement is completed, or when the final trigger is deactivated. 
        /// Should be called in every update
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KinectMovementEventArgs args = new KinectMovementEventArgs();
            args.LastTrigger = kinectTriggers[kinectTriggers.Count() - 1];


            //quit movement if active time reached maximum
            time += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (MaxActiveTimeMiliseconds > 0 && time > MaxActiveTimeMiliseconds)
            {
                if (State == MovementState.Activated)
                {
                    state = MovementState.Wating;
                    MovementQuit(this, args);
                    invalidatedMovement = true;
                    lastActiveTriggerIndex = -1;
                    return;
                }
            }


            // create and populate the status of the triggers
            bool[] triggerStatus = new bool[kinectTriggers.Count()];
            for (int x = 0; x < kinectTriggers.Count(); x++)
                triggerStatus[x] = kinectTriggers[x].checkIsTriggered();




            //check if going forward on move
            for (int x = 0; x < triggerStatus.Count(); x++)
            {
                if (triggerStatus[x])
                {
                    //reset movement if gesture is in the beginning
                    if (x == 0)
                        invalidatedMovement = false;
                    //if last active index is lower than the actual
                    if (lastActiveTriggerIndex + 1 == x)
                    {
                        lastActiveTriggerIndex++;
                    }

                }
            }

            //check if going back on move
            for (int x = triggerStatus.Count() - 1; x >= 0; x--)
            {
                if (!triggerStatus[x])
                {
                    //if last active index is bigger than the actual
                    if (lastActiveTriggerIndex == x)
                    {
                        lastActiveTriggerIndex--;
                        //always invalidates movement if users steps back
                        invalidatedMovement = true;
                    }

                }
            }

            //check if it is a finnished movement
            if (lastActiveTriggerIndex == kinectTriggers.Count() - 1 && !invalidatedMovement && !checkStartEndActive(triggerStatus))
            {
                //fire event once when the movement is finished

                if (state != MovementState.Activated)
                {
                    state = MovementState.Activated;
                    if (MovementCompleted != null)
                    {
                        //resets the time of the movement
                        time = 0;
                        MovementCompleted(this, args);
                    }
                }
                else
                    state = MovementState.Activated;
            }
            else
            {
                //fire event if movement is no more activated
                if (state != MovementState.Wating)
                {
                    state = MovementState.Wating;
                    if (MovementCompleted != null)
                        MovementQuit(this, args);
                }
                else
                    state = MovementState.Wating;
            }
        }

        /// <summary>
        /// Check if first and last triggers is active at the same time. This should be off for an effective movement
        /// </summary>
        /// <param name="triggerStatus">The list of trigger status</param>
        /// <returns></returns>
        private bool checkStartEndActive(bool[] triggerStatus)
        {
            return (triggerStatus[0] && triggerStatus[triggerStatus.Count() - 1]);
        }


        public bool isOn()
        {
            if (this.State == MovementState.Activated)
                return true;
            else
                return false;
        }
    }


    public class KinectMovementEventArgs : EventArgs
    {
        public KinectTrigger LastTrigger { get; set; }
    }
}
