using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelChairCollaborativeGame
{
    class OnOffDoubleAND : IOnOff
    {
        public IOnOff IOnOffOne { private get; set; }
        public IOnOff IOnOffTwo { private get; set; }

        public OnOffDoubleAND(IOnOff IOnOffOne, IOnOff IOnOffTwo)
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
