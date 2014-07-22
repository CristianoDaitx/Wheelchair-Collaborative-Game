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
    class WierdEnemy : EnemyGameObject
    {

        public WierdEnemy(GameEnhanced game, String tag)
            : base(game, tag)
        {
            this.life = 2;
            this.Velocity = new Vector2(2, 0.5f);
            this.Acceleration = new Vector2(0.02f, 0);

        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("mediumEnemy"), 0.5f);
            Position = new Vector2(Config.resolution.X / 2 - Size.X / 2, -Size.Y);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!isLeaving)
                if (Math.Abs(Velocity.X) > 2)
                    this.Acceleration = new Vector2(-this.Acceleration.X, 0);
        }

        protected override void die()
        {
            Game.Components.Add(new BigExplosionGameObject(new Vector2(PositionCenterX, this.PositionCenterY), Game, 3));
        }
    }
}
