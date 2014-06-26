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
    

    class KinectMovement
    {

        
        private enum MovementState
        {
            Activated,
            Wating
        }
        private MovementState state = MovementState.Wating;

        private List<KinectTrigger> kinectTriggers = new List<KinectTrigger>();

        public int lastActiveTriggerIndex = -1;

        private bool invalidatedMovement = false;


        public delegate void MovementCompletedEventHandler(object sender, EventArgs e);
        public event MovementCompletedEventHandler MovementCompleted;

        public delegate void MovementQuitEventHandler(object sender, EventArgs e);
        public event MovementQuitEventHandler MovementQuit;
        



        public void addTrigger(KinectTrigger kinectTrigger)
        {
            kinectTriggers.Add(kinectTrigger);
        }


        public bool update(Joint joint)
        {

            //check if going forward on move
            for (int x = 0; x < kinectTriggers.Count(); x++)
            {
                if (kinectTriggers[x].checkIsTriggered(joint))
                {
                    if (x == 0)
                        invalidatedMovement = false; //reset movement if gesture is in the beginning
                    if (lastActiveTriggerIndex + 1 == x)
                    {
                        lastActiveTriggerIndex++;
                    }

                }
            }

            //check if going back on move
            for (int x = kinectTriggers.Count() -1 ; x >= 0; x--)
            {
                if (!kinectTriggers[x].checkIsTriggered(joint))
                {
                    if (lastActiveTriggerIndex == x)
                    {
                        lastActiveTriggerIndex--;
                        invalidatedMovement = true; //invalidates movement if users stepped back
                    }

                }
            }

            if (lastActiveTriggerIndex == kinectTriggers.Count() - 1 && !invalidatedMovement)
            {
                if (state != MovementState.Activated)
                    MovementCompleted(this, EventArgs.Empty);

                state = MovementState.Activated;
                return true;
            }
            else
            {
                if (state != MovementState.Wating)
                    MovementQuit(this, EventArgs.Empty);

                state = MovementState.Wating;
                return false;

            }
        }



    }
}
