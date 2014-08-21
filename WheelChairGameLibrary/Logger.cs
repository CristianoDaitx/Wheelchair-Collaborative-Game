using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WheelChairGameLibrary
{
    public class Logger : GameComponent
    {

        private int countTank;
        public int CountTank
        {
            get { return countTank; }
            set { countTank = value; }
        }

        private int countSoldier;
        public int CountSoldier
        {
            get { return countSoldier; }
            set { countSoldier = value; }
        }

        public int CountTotal
        {
            get { return CountTank + CountSoldier; }
        }

        public int shotsFired;
        public int ShotsFired
        {
            get { return shotsFired; }
            set { shotsFired = value; }
        }

        public int shotsMissed;
        public int ShotsMissed
        {
            get { return shotsMissed; }
            set { shotsMissed = value; }
        }

        public int shotsHit;
        public int ShotsHit
        {
            get { return shotsHit; }
            set { shotsHit = value; }
        }

        public int lowEnergyFail;
        public int LowEnergyFail
        {
            get { return lowEnergyFail; }
            set { lowEnergyFail = value; }
        }

        public int score;
        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public int invaders;
        public int Invaders 
        {
            get { return invaders; }
            set { invaders = value; }
        }

        


        public Logger(GameEnhanced game)
            : base(game)
        {
        }



        /// <summary>
        /// Writes file to an .csv file
        /// </summary>
        public void saveLog(){
            //TODO open file and append log
        }

        public void resetLog()
        {
            //TODO reset all values
            countTank = 0;
            countSoldier = 0;
            shotsFired = 0;
            shotsMissed = 0;
            shotsHit = 0;
            lowEnergyFail = 0;
        }
    }
}
