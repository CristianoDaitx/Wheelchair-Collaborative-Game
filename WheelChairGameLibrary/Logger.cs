using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WheelChairGameLibrary
{
    public class Logger : GameComponent
    {

        public int GroupId { get; set; }

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







        public Logger(GameEnhanced game)
            : base(game)
        {
        }



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
