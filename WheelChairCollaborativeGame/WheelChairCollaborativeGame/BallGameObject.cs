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

        public BallGameObject(Vector2 startingPosition, GameEnhanced game, String tag)
            : base(startingPosition, game, tag)
        {
            //this.Position = startingPosition;
            Collider = new Collider(this, 8, 8);

            Velocity = new Vector2(0, -2);
        }


        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
            SharedSpriteBatch.Begin();
            PrimitiveDrawing.DrawCircle(Game.WhitePixel, SharedSpriteBatch, new Vector2(Position.X + 4, Position.Y + 4), 4.0f, Color.Red, 4, 7);
            SharedSpriteBatch.End();


        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Position.Y < 10)
            {
                //Game.Components.Remove(this);
                ToBeRemoved = true;
                //GameObjectManager.removeGameObject(this);
            }


            //position.Y -= 2;

            //Collider.BoundingBox = new Rectangle(Collider.BoundingBox.X, Collider.BoundingBox.Y - 2, Collider.BoundingBox.Width, Collider.BoundingBox.Height);
        }

        public override void collisionEntered(Collider collider)
        {
            //Game.Components.Remove(this);
            //GameObjectManager.removeGameObject(this);
            ToBeRemoved = true;
        }
    }
}
