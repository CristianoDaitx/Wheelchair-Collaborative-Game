#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using WheelChairGameLibrary.Helpers;
using WheelChairGameLibrary;
using WheelChairGameLibrary.Sprites;

using KinectForWheelchair;
using KinectForWheelchair.Listeners;

using Microsoft.Kinect;

#endregion

namespace WheelChairCollaborativeGame
{
    class MenuEnemy : EnemyGameObject
    {
        public enum Type
        {
            Left,
            Right
        }

        private int BORDER_STARTING_POSITION_X = 500;
        //private Type type;

        public MenuEnemy(GameEnhanced game, String tag)
            : base(game, tag)
        {
            this.life = 1;


            this.Velocity = new Vector2(0.0f, 0.7f);

            this.Acceleration = new Vector2(0.015f, 0);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("WeakEnemy"), 1f);

            this.Position = new Vector2(Config.resolution.X - BORDER_STARTING_POSITION_X, -Size.Y);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!isLeaving)
                if (Math.Abs(Velocity.X) > 2)
                    this.Acceleration = new Vector2(-this.Acceleration.X, 0);
        }

        public override void die()
        {
            base.die();
            Game.Components.Add(new BigExplosionGameObject(new Vector2(PositionCenterX, this.PositionCenterY), Game, 2));
        }
    }
}
