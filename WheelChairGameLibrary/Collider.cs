#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using WheelChairGameLibrary.Helpers;
#endregion

namespace WheelChairGameLibrary
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

        /// <summary>
        /// The bounding box of the collider, based in it's gameObject position
        /// </summary>
        public Rectangle BoundingBox
        {
            get { return new Rectangle((int)gameObject.Position.X, (int)gameObject.Position.Y, width, height); }

        }

        /// <summary>
        /// The game object this collider is associated to
        /// </summary>
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
