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


        public Logger(GameEnhanced game)
            : base(game)
        {
        }



        /// <summary>
        /// Writes file to an .txt file
        /// </summary>
        public void saveLog(){
            //TODO open file and append log
        }

        public void resetLog()
        {
            //TODO reset all values
            countTank = 0;
            countSoldier = 0;
        }
    }
}
