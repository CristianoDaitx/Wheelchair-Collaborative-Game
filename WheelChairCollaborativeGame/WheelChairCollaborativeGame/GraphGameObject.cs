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
using WheelChairGameLibrary.Screens;
using WheelChairGameLibrary.GameObjects;
using WheelChairGameLibrary.Sprites;

using KinectForWheelchair;
using KinectForWheelchair.Listeners;

using Microsoft.Kinect;

#endregion


namespace WheelChairCollaborativeGame
{
    class GraphGameObject : GameObject
    {

        private readonly int STARTING_X = 640;
        private int pressedY = 450;
        private int notPressedY = 470;
        private readonly int MAX_RECORDS = 10;
        private readonly int LINE_WIDTH = 2;


        public int PressedY
        {
            get { return pressedY; }
            set
            {
                if (value != pressedY)
                    isChanged = true;
                if (value < Config.resolution.Y && value > 0)
                    pressedY = value;
            }

        }
        
        public int NotPressedY
        {
            get { return notPressedY; }
            set
            {
                if (value != notPressedY)
                    isChanged = true;
                if (value > pressedY && value < Config.resolution.Y)
                    notPressedY = value;
            }

        }



        private bool isPressed = false;
        public bool IsPressed
        {
            get { return isPressed; }
            set {
                if (value != isPressed)
                    isChanged = true;
                if (times.Count() > MAX_RECORDS)
                    times.Dequeue();
                isPressed = value; }
        }

        private bool isChanged = false;

        private double time = 0;

        //private double timeToChange = 0;

        Queue<double> times = new Queue<double>();

        public GraphGameObject(GameEnhanced game, String tag)
            : base(game, tag)
        {

        }

        /*public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            bool isUp = IsPressed;
            // fix for one time delay
            if (isChanged)
                isUp = !IsPressed;

            Vector2 startPosition = new Vector2(STARTING_X, (isUp ? pressedY : notPressedY));
            Vector2 endPosition = new Vector2(STARTING_X - (float)scale(time), (isUp ? pressedY : notPressedY));
            PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch, startPosition, endPosition, Color.White, LINE_WIDTH);

            double timeSum = time;            
            for (int x = times.Count() - 1; x >= 0; x--)
            {                
                isUp = !isUp;
                startPosition = new Vector2(STARTING_X - (float)scale(timeSum), (isUp ? pressedY : notPressedY));
                endPosition = new Vector2(STARTING_X - (float)scale(times.ElementAt(x) + timeSum), (isUp ? pressedY : notPressedY));
                PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch, startPosition, endPosition, Color.White, LINE_WIDTH);

                startPosition = new Vector2(STARTING_X - (float)scale(timeSum), notPressedY);
                endPosition = new Vector2(STARTING_X - (float)scale(timeSum), pressedY - LINE_WIDTH);
                PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch, startPosition, endPosition, Color.White, LINE_WIDTH);
                
                timeSum += times.ElementAt(x);
            }

        }*/

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

            if (isChanged)
            {
                times.Enqueue(time);
                time = 0;
                isChanged = false;
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
