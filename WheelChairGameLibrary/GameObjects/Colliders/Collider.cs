#region Using Statements
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WheelChairGameLibrary.Helpers;
#endregion

namespace WheelChairGameLibrary.GameObjects
{
    public class Collider
    {
        private int width;
        public int Width
        {
            get { return width; }
        }

        private int height;
        public int Height
        {
            get { return height; }
        }


        public Rectangle BoundingBox
        {
            get { return new Rectangle((int)gameObject.Position.X, (int)gameObject.Position.Y, width, height); }

        }

        private GameObject gameObject;
        public GameObject GameObject
        {
            get { return gameObject; }
        }

        public Collider(GameObject gameObject, int width, int height)
        {
            this.gameObject = gameObject;
            this.width = width;
            this.height = height;
        }

        public void Draw()
        {
            GameObject.SharedSpriteBatch.Begin();
            PrimitiveDrawing.DrawRectangle(GameObject.Game.WhitePixel, GameObject.SharedSpriteBatch, BoundingBox, Color.Gray);
            GameObject.SharedSpriteBatch.End();
        }
    }
}
