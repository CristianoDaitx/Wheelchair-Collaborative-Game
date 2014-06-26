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
        private readonly int PRESSED_Y = 450;
        private readonly int NOT_PRESSED_Y = 470;
        private readonly int MAX_RECORDS = 10;
        private readonly int LINE_WIDTH = 2;


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

        public GraphGameObject(GameObjectManager gameObjectManager, String tag)
            : base(gameObjectManager, tag)
        {

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            bool isUp = IsPressed;
            // fix for one time delay
            if (isChanged)
                isUp = !IsPressed;

            Vector2 startPosition = new Vector2(STARTING_X, (isUp ? PRESSED_Y : NOT_PRESSED_Y));
            Vector2 endPosition = new Vector2(STARTING_X - (float)scale(time), (isUp ? PRESSED_Y : NOT_PRESSED_Y));
            PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch, startPosition, endPosition, Color.White, LINE_WIDTH);

            double timeSum = time;            
            for (int x = times.Count() - 1; x >= 0; x--)
            {                
                isUp = !isUp;
                startPosition = new Vector2(STARTING_X - (float)scale(timeSum), (isUp ? PRESSED_Y : NOT_PRESSED_Y));
                endPosition = new Vector2(STARTING_X - (float)scale(times.ElementAt(x) + timeSum), (isUp ? PRESSED_Y : NOT_PRESSED_Y));
                PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch, startPosition, endPosition, Color.White, LINE_WIDTH);

                startPosition = new Vector2(STARTING_X - (float)scale(timeSum), NOT_PRESSED_Y);
                endPosition = new Vector2(STARTING_X - (float)scale(timeSum), PRESSED_Y - LINE_WIDTH);
                PrimitiveDrawing.DrawLineSegment(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch, startPosition, endPosition, Color.White, LINE_WIDTH);
                
                timeSum += times.ElementAt(x);
            }

        }

        private double scale(double valueIn) 
        {
            return STARTING_X * valueIn / 10000;
        }

        /*public static double scale(final double valueIn, final double baseMin, final double baseMax, final double limitMin, final double limitMax) {
            return ((limitMax - limitMin) * (valueIn - baseMin) / (baseMax - baseMin)) + limitMin;
        }*/



        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);
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
