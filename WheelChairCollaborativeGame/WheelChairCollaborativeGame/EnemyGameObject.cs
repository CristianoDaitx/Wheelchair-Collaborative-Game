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

using KinectForWheelchair;
using KinectForWheelchair.Listeners;

using Microsoft.Kinect;

#endregion

namespace WheelChairCollaborativeGame
{
    class EnemyGameObject : GameObject
    {

        private double time = 0;

        protected int maxhits = 1;
        public int hits = 0;

        public EnemyGameObject(Vector2 position, GameEnhanced game, String tag)
            : base(position, game, tag)
        {
            Collider = new Collider(this, 100, 100);
            Velocity = new Vector2(0, 0.5f);

        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, this.Game.Content.Load<Texture2D>("Space_Invader"),
                      0.5f);




        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            time += gameTime.ElapsedGameTime.TotalMilliseconds;


            //Sprite.position.Y += 0.01f;

            if (this.Position.Y > Config.resolution.Y - 250)
            {
                Velocity = new Vector2(2, 0); 
            }

            //Collider.BoundingBox = new Rectangle((int)this.Position.X, (int)this.Position.Y, Collider.BoundingBox.Width, Collider.BoundingBox.Height);

            //delete if exit screen
            if (Position.X > Config.resolution.X - 150)
                this.ToBeRemoved = true;

            if (Position.Y >= Config.resolution.Y )
            {
                this.ToBeRemoved = true;
            }
        }
        // delete if destroyed
        
        public override void collisionEntered(Collider collider)
        {
            if (collider.GameObject.GetType() == typeof(BallGameObject))
            {
                if (hits == maxhits)
                {
                    ToBeRemoved = true;
                }
                hits++;
            }
        }

    }
}
