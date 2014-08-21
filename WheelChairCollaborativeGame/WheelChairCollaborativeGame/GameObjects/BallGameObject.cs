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

using WheelChairGameLibrary.Sprites;

#endregion

namespace WheelChairCollaborativeGame
{
    class BallGameObject : GameObject2D
    {
        private readonly int MAX_Y_TO_SURVIVE = 50;

        SoundEffect hit;

        public BallGameObject(Vector2 startingPosition, GameEnhanced game, String tag)
            : base(startingPosition, game, tag)
        {
            //this.Position = startingPosition;
            //this.Size = new Vector2(8, 8);
            Collider = new Collider(this);

            Velocity = new Vector2(0, -9);

            Position = new Vector2(Position.X - 6, Position.Y - 10);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("fire"), 1.5f);
            hit = Game.Content.Load<SoundEffect>("hit");
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Position.Y < MAX_Y_TO_SURVIVE)
            {
                //Game.Log.ShotsMissed++;
                
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
            if (collider.GameObject.GetType() ==  typeof(BallGameObject))
                return;
            ToBeRemoved = true;
            hit.Play();
            Game.Components.Add(new ExplosionGameObject(this.Position, Game));
        }

      
    }
}
