using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using WheelChairGameLibrary;
using WheelChairGameLibrary.Helpers;


namespace WheelChairCollaborativeGame
{
    public class Logger
    {

        public int GroupId
        {
            get { return Config.GroupId; }
        }
        public int InputId
        {
            get { return (int)Config.ControlSelected + 1; }
        }
        public int PlayerAActionsStarted { get; set; }
        public int PlayerAActionsCompleted { get; set; }
        public int PlayerAActionsFailed { get; set; }
        public int PlayerBActionsStarted { get; set; }
        public int PlayerBActionsCompleted { get; set; }
        public int PlayerBActionsFailed { get; set; }
        public int ShotsFired { get; set; }
        public int ShotsHit { get; set; }
        public int ShotsMissed { get; set; }
        public int ShotsWithoutEnergy { get; set; }        
        public int Score { get; set; }
        public int Invaders { get; set; }

        public void resetLog()
        {
            PlayerAActionsStarted = 0;
            PlayerAActionsCompleted = 0;
            PlayerAActionsFailed = 0;
            PlayerBActionsStarted = 0;
            PlayerBActionsCompleted = 0;
            PlayerBActionsFailed = 0;
            ShotsFired = 0;
            ShotsMissed = 0;
            ShotsWithoutEnergy = 0;
            ShotsHit = 0;
            Score = 0;
            Invaders = 0;
        }
    }
}
