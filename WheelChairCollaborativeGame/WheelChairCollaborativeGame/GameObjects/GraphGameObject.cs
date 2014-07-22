#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using WheelChairGameLibrary;
using WheelChairGameLibrary.Helpers;

using WheelChairGameLibrary.Sprites;

using KinectForWheelchair;
using KinectForWheelchair.Listeners;

using Microsoft.Kinect;

#endregion


namespace WheelChairCollaborativeGame
{
    class GraphGameObject : GameObject2D
    {

        private readonly int STARTING_X = (int)Config.resolution.X;
        private int pressedY = 450;
        private int notPressedY = 470;
        private readonly int MAX_RECORDS = 10;
        private readonly int LINE_WIDTH = 2;

        public int PressedY
        {
            get { return pressedY; }
            set
            {
                if (value < Config.resolution.Y && value > 0)
                    pressedY = value;
            }

        }
        
        public int NotPressedY
        {
            get { return notPressedY; }
            set
            {
                if (value > pressedY && value < Config.resolution.Y)
                    notPressedY = value;
            }

        }



        //private bool isPressed = false;
        /*public bool IsPressed
        {
            get { return isPressed}
            set {
                if (value != IsPressed)
                    isChanged = true;
                if (times.Count() > MAX_RECORDS)
                    times.Dequeue();
                //isPressed = value;
            }
        }*/

        public IOnOff IOnOff { private get; set; }
        private bool lastStatus = false;

        //private bool isChanged = false;

        private double time = 0;

        //private double timeToChange = 0;

        Queue<double> times = new Queue<double>();

        public GraphGameObject(IOnOff IOnOff, GameEnhanced game, String tag)
            : base(game, tag)
        {
            this.IOnOff = IOnOff;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (!Game.IsDebugMode)
                return;

            bool isUp = IOnOff.isOn();
            /*// fix for one time delay
            if (isChanged)
                isUp = !IsPressed;*/

            SharedSpriteBatch.Begin();

            Vector2 startPosition = new Vector2(STARTING_X, (isUp ? pressedY : notPressedY));
            Vector2 endPosition = new Vector2(STARTING_X - (float)scale(time), (isUp ? pressedY : notPressedY));
            PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch, startPosition, endPosition, Color.Gray, LINE_WIDTH);

            double timeSum = time;            
            for (int x = times.Count() - 1; x >= 0; x--)
            {                
                isUp = !isUp;
                startPosition = new Vector2(STARTING_X - (float)scale(timeSum), (isUp ? pressedY : notPressedY));
                endPosition = new Vector2(STARTING_X - (float)scale(times.ElementAt(x) + timeSum), (isUp ? pressedY : notPressedY));
                PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch, startPosition, endPosition, Color.Gray, LINE_WIDTH);

                startPosition = new Vector2(STARTING_X - (float)scale(timeSum), notPressedY);
                endPosition = new Vector2(STARTING_X - (float)scale(timeSum), pressedY - LINE_WIDTH);
                PrimitiveDrawing.DrawLineSegment(Game.WhitePixel, SharedSpriteBatch, startPosition, endPosition, Color.Gray, LINE_WIDTH);
                
                timeSum += times.ElementAt(x);
            }

            SharedSpriteBatch.End();
        }

        private double scale(double valueIn) 
        {
            return STARTING_X * valueIn / 10000;
        }

        /*public static double scale(final double valueIn, final double baseMin, final double baseMax, final double limitMin, final double limitMax) {
            return ((limitMax - limitMin) * (valueIn - baseMin) / (baseMax - baseMin)) + limitMin;
        }*/



        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            time += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (lastStatus != IOnOff.isOn())
            {
                times.Enqueue(time);
                time = 0;
                lastStatus = IOnOff.isOn();

                if (times.Count() > MAX_RECORDS)
                    times.Dequeue();
            }








            /*timeToChange += gameTime.ElapsedGameTime.TotalMilliseconds;



            if (TimeSpan.FromMilliseconds(timeToChange).TotalSeconds > 1) //more than two seconds
            {
                //if (Sprite.position.Y > 160)
                IsPressed = !IsPressed;
                timeToChange = 0;
            }*/

            
        }

    }
}
