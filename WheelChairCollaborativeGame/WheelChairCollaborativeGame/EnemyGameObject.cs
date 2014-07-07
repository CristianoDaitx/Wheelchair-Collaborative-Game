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

using KinectForWheelchair;
using KinectForWheelchair.Listeners;

using Microsoft.Kinect;

#endregion

namespace WheelChairCollaborativeGame
{
    class EnemyGameObject : GameObject
    {

        private double time = 0;

        public EnemyGameObject(GameEnhanced game, String tag)
            : base(game, tag)
        {


            Collider = new Collider(this, new Rectangle(0, 0, 50, 50));
            
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, this.Game.Content.Load<Texture2D>("Space_Invader"),
                     new Vector2(282, 0), 0.5f);


            this.velocity.Y = 0.5f;

            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            time += gameTime.ElapsedGameTime.TotalMilliseconds;


            //Sprite.position.Y += 0.01f;

            if (this.Position.Y > 300)
            {
                this.velocity.Y = 0;
                this.velocity.X = 2f;
            }

            Collider.BoundingBox = new Rectangle((int)this.Position.X, (int)this.Position.Y, Collider.BoundingBox.Width, Collider.BoundingBox.Height);

            //delete if exit screen
            if (this.Position.X > Config.resolution.X - 50)
                Game.Components.Remove(this);
                //GameObjectManager.removeGameObject(this);

        }

        public override void collisionEntered(Collider collider)
        {
            if (collider.GameObject.GetType() == typeof (BallGameObject))
                ToBeRemoved = true;
                //Game.Components.Remove(this);
                //GameObjectManager.removeGameObject(this);
        }

    }
}