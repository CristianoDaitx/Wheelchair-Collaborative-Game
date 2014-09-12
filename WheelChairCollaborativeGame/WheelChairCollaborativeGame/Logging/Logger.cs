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

        public int GroupId {
            get { return Config.GroupId; }
        }

        public int InputId
        {
            get { return (int)Config.ControlSelected + 1; }
        }

        private int completedActionsA;
        public int CompletedActionsA
        {
            get { return completedActionsA; }
            set { completedActionsA = value; }
        }

        public int ActionsFailureA { get; set; }

        private int completedActionsB;
        public int CompletedActionsB
        {
            get { return completedActionsB; }
            set { completedActionsB = value; }
        }

        public int ActionsFailureB { get; set; }

        public int TotalShotsFired
        {
            get { return CompletedActionsA + CompletedActionsB; }
        }

        public int ShotsWithoutEnergy { get; set; }

        public int shotsHit;
        public int ShotsHit
        {
            get { return shotsHit; }
            set { shotsHit = value; }
        }

        public int shotsMissed;
        public int ShotsMissed
        {
            get { return shotsMissed; }
            set { shotsMissed = value; }
        }

        public int Score { get; set; }

        public int Invaders { get; set; }









        /// <summary>
        /// Writes file to an .csv file
        /// </summary>
        public void saveLog()
        {
            //TODO open file and append log

        }

        public void resetLog()
        {
            //TODO reset all values
            completedActionsA = 0;
            completedActionsB = 0;
            //shotsFired = 0;
            shotsMissed = 0;
            shotsHit = 0;
            //lowEnergyFail = 0;
            //score = 0;
            //invaders = 0;
        }
    }
}
