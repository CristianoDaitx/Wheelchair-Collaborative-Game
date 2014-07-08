using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WheelChairCollaborativeGame
{
    class OnOffController : GameComponent, IOnOff
    {
        private readonly int MAX_ACTIVE_TIME_MILISECONDS = 1000;

        private bool myIsOn;
        public bool IsOn {
            get { return myIsOn; }
            set {
                myIsOn = value;
                if (value == true)
                    time = 0;
            }
        }

        private double time = 0;

        public OnOffController(Game game)
            :base (game)
        {
        }



        public bool isOn()
        {
            return IsOn;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            time += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (time > MAX_ACTIVE_TIME_MILISECONDS)
            {
                IsOn = false;
            }
        }
    }
}
