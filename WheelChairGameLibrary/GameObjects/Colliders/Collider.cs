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
        private Rectangle boundingBox;
        public Rectangle BoundingBox
        {
            get { return boundingBox; }
            set { boundingBox = value; }
        }

        private GameObject gameObject;
        public GameObject GameObject
        {
            get { return gameObject; }
        }

        public Collider(GameObject gameObject, Rectangle boundingBox)
        {
            this.gameObject = gameObject;
            this.boundingBox = boundingBox;
        }

        public void Draw()
        {
            PrimitiveDrawing.DrawRectangle(gameObject.GameObjectManager.GameScreen.ScreenManager.WhitePixel, gameObject.GameObjectManager.GameScreen.ScreenManager.SpriteBatch, boundingBox, Color.White);
        }
    }
}
