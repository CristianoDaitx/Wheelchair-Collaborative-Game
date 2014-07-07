#region Using Statements
using System;
using System.Threading;
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

#endregion

namespace WheelChairCollaborativeGame
{
    class BallGameObject : GameObject
    {
        private Vector2 position;

        public BallGameObject(Vector2 startingPosition, GameEnhanced game, String tag)
            : base(game, tag)
        {
            this.position = startingPosition;
            Collider = new Collider(this, new Rectangle((int)position.X - 4, (int)position.Y - 4, 8, 8));
        }


        public override void  Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
            SharedSpriteBatch.Begin();
            PrimitiveDrawing.DrawCircle(Game.WhitePixel, SharedSpriteBatch, position, 4.0f, Color.Red, 4, 7);
            SharedSpriteBatch.End();

            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (position.Y < 10)
            {
                Game.Components.Remove(this);
                //GameObjectManager.removeGameObject(this);
            }


            position.Y -= 2;

            Collider.BoundingBox = new Rectangle(Collider.BoundingBox.X, Collider.BoundingBox.Y - 2, Collider.BoundingBox.Width, Collider.BoundingBox.Height);
        }

        public override void collisionEntered(Collider collider)
        {
            //Game.Components.Remove(this);
            //GameObjectManager.removeGameObject(this);
            ToBeRemoved = true;
        }
    }
}
