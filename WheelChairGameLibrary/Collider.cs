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

        /// <summary>
        /// The bounding box of the collider, based in it's gameObject position
        /// </summary>
        public Rectangle BoundingBox
        {
            get { return new Rectangle((int)gameObject.Position.X, (int)gameObject.Position.Y, (int)gameObject.Size.X, (int)gameObject.Size.Y); }

        }

        /// <summary>
        /// The game object this collider is associated to
        /// </summary>
        private GameObject2D gameObject;
        public GameObject2D GameObject
        {
            get { return gameObject; }
        }

        public Collider(GameObject2D gameObject)
        {
            this.gameObject = gameObject;
        }

        public void Draw()
        {
            if (!GameObject.Game.IsDebugMode)
                return;
            GameObject.SharedSpriteBatch.Begin();
            PrimitiveDrawing.DrawRectangle(GameObject.Game.WhitePixel, GameObject.SharedSpriteBatch, BoundingBox, Color.Gray);
            GameObject.SharedSpriteBatch.End();
        }
    }
}
