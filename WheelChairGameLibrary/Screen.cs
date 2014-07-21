using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelChairGameLibrary
{
    public abstract class Screen : GameObject
    {

        public Screen(GameEnhanced game, string tag)
            : base(game, tag)
        {
        }

        public abstract void ExitScreen();

    }
}
