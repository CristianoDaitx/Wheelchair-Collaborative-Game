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
    
    /// <summary>
    /// Used with KinectTriggers to keep track of a bigger movement of a kinect skeleton
    /// </summary>
    class KinectMovement
    {        
        private enum MovementState
        {
            Activated,
            Wating
        }
        private MovementState state = MovementState.Wating;

        private List<KinectTrigger> kinectTriggers = new List<KinectTrigger>();

        /// <summary>
        /// Keep track of the last active index of the triggers in the list
        /// </summary>
        public int lastActiveTriggerIndex = -1;

        private bool invalidatedMovement = false;

        public delegate void MovementCompletedEventHandler(object sender, EventArgs e);
        public event MovementCompletedEventHandler MovementCompleted;

        public delegate void MovementQuitEventHandler(object sender, EventArgs e);
        public event MovementQuitEventHandler MovementQuit;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kinectTriggers">A list of triggers to be used in this movement</param>
        public KinectMovement(params KinectTrigger[] kinectTriggers)
        {
            foreach (KinectTrigger trigger in kinectTriggers)
                addTrigger(trigger);
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
            foreach (KinectTrigger trigger in kinectTriggers)
                trigger.TrackingSkeleton = trackingSkeleton;
        }

        /// <summary>
        /// Draw all triggers inside this movement
        /// </summary>
        public void drawTriggers()
        {
            foreach (KinectTrigger trigger in kinectTriggers)
                trigger.draw();
        }

        /// <summary>
        /// Checks the actual situation of the movement. Fires an event if the movement is completed, or when the final trigger is deactivated. 
        /// Should be called in every update
        /// </summary>
        public void update()
        {
            //TODO: make this method be automatically called insade XNA update

            // create and populate the status of the triggers
            bool[] triggerStatus = new bool[kinectTriggers.Count()];
            for (int x = 0; x < kinectTriggers.Count(); x++)
                triggerStatus[x] = kinectTriggers[x].checkIsTriggered();




            //check if going forward on move
            for (int x = 0; x < kinectTriggers.Count(); x++)
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
            for (int x = kinectTriggers.Count() -1 ; x >= 0; x--)
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
            if (lastActiveTriggerIndex == kinectTriggers.Count() - 1 && !invalidatedMovement)
            {
                //fire event once when the movement is finished
                if (state != MovementState.Activated)
                    MovementCompleted(this, EventArgs.Empty);

                state = MovementState.Activated;
            }
            else
            {
                //fire event if movement is no more activated
                if (state != MovementState.Wating)
                    MovementQuit(this, EventArgs.Empty);

                state = MovementState.Wating;
            }
        }
    }
}
