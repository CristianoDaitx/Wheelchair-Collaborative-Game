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

            

            
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, this.Game.Content.Load<Texture2D>("Space_Invader"),
                    this.Game.WhitePixel, new Vector2(282, 0), 0.5f);

            Sprite.velocity.Y = 0.5f;

            Collider = new Collider(this, new Rectangle(0, 0, (int)Sprite.size.X * 2, (int)Sprite.size.Y * 2));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            time += gameTime.ElapsedGameTime.TotalMilliseconds;


            //Sprite.position.Y += 0.01f;

            if (Sprite.position.Y > 300)
            {
                Sprite.velocity.Y = 0;
                Sprite.velocity.X = 2f;
            }

            Collider.BoundingBox = new Rectangle((int)Sprite.position.X, (int)Sprite.position.Y, Collider.BoundingBox.Width, Collider.BoundingBox.Height);

            //delete if exit screen
            if (Sprite.position.X > Config.resolution.X - 50)
                GameObjectManager.removeGameObject(this);

        }

        public override void collisionEntered(Collider collider)
        {
            if (collider.GameObject.GetType() == typeof (BallGameObject))
                GameObjectManager.removeGameObject(this);
        }

    }
}