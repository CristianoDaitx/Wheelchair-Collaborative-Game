#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using WheelChairGameLibrary;
using WheelChairGameLibrary.Sprites;
using WheelChairGameLibrary.Helpers;

using KinectForWheelchair;
using KinectForWheelchair.Listeners;

using Microsoft.Kinect;

#endregion

namespace WheelChairCollaborativeGame
{
    class AvarageEnemy : EnemyGameObject
    {
        public AvarageEnemy(GameEnhanced game, String tag)
            : base(game, tag)
        {
            this.life = 2;
            Velocity = new Vector2(0, 0.5f);
            
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("AvarageEnemy"), 0.5f);
            Position = new Vector2(Config.resolution.X / 2 - Size.X / 2, -Size.Y);
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Position.Y == 0.10 * Config.resolution.Y)
            {
                Velocity = new Vector2(-1f, 0.5f);
            }

            if (!isLeaving)
            {
                if (Position.X < 500)
                    Velocity = new Vector2(1f, 0.5f);
                if (Position.X > 700 && Position.Y < Config.resolution.Y - 250)
                    Velocity = new Vector2(-1f, 0.5f);
            }

        }

        protected override void die()
        {
            Game.Components.Add(new BigExplosionGameObject(new Vector2(PositionCenterX, this.PositionCenterY), Game, 3));
        }
    }
}
