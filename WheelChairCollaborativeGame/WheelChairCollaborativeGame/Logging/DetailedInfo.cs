using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelChairCollaborativeGame.Logging
{
    class DetailedInfo
    {
        public enum Type
        {
            PLAYER_A_ACTION_START,
            PLAYER_A_ACTION_COMPLETION,
            PLAYER_A_ACTION_FAILED,
            PLAYER_B_ACTION_START,
            PLAYER_B_ACTION_COMPLETION,
            PLAYER_B_ACTION_FAILED,
            SHOT_FIRED,
            SHOT_WITHOUT_ENERGY,
            SPACESHIP_HIT
        }

        public Type type { get; set; }

        public int groupId { get; set; }



        public int inputId { get; set; }


        public DetailedInfo(Type type)
        {

            this.type = type;
            groupId = Config.GroupId;
            inputId = (int)Config.ControlSelected + 1;
        }
    }
}
