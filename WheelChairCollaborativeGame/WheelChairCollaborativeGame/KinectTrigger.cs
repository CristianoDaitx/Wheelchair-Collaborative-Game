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

        Vector3 baseJoint;
        Vector3 relativePosition;

        public KinectTrigger(Vector3 baseJoint, Vector3 relativePosition)
        {
            this.baseJoint = baseJoint;
            this.relativePosition = relativePosition;
        }

        public Vector3 getPosition()
        {
            /*SkeletonPoint skeletonPoint = new SkeletonPoint();
            skeletonPoint.X = relativePosition.X + baseJoint.Position.X;
            skeletonPoint.Y = relativePosition.Y + baseJoint.Position.Y;
            skeletonPoint.Z = relativePosition.Z + baseJoint.Position.Z;*/
            return baseJoint + relativePosition;
        }
    }
}
