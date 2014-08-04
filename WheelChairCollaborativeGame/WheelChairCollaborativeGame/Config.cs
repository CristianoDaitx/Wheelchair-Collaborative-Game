#region Using Statements
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using WheelChairGameLibrary.Sprites;
using WheelChairGameLibrary.Helpers;
#endregion

namespace WheelChairCollaborativeGame
{
    public class Config{
        public static Vector2 resolution = new Vector2(1280, 720); //480, 640//576, 1024

        public static Vector2 cameraResolution = new Vector2(480, 640); //480, 640


        public enum ControlSelect
        {
            Joystick = 0,
            Front,
            Side,
            FrontAssyncronous
        }

        private static ControlSelect controlSelect = ControlSelect.Joystick;
        public static ControlSelect ControlSelected
        {
            get { return controlSelect; }
            set { controlSelect = value; }
        }



    }


}
