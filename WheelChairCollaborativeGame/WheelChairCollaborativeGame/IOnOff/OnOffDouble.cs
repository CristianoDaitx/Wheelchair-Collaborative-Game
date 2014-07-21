using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelChairCollaborativeGame
{
    class OnOffDouble : IOnOff
    {
        public IOnOff IOnOffOne { private get; set; }
        public IOnOff IOnOffTwo { private get; set; }

        public OnOffDouble(IOnOff IOnOffOne, IOnOff IOnOffTwo)
        {
            this.IOnOffOne = IOnOffOne;
            this.IOnOffTwo = IOnOffTwo;
        }



        public bool isOn()
        {
            return IOnOffOne.isOn() && IOnOffTwo.isOn();
        }
    }
}
