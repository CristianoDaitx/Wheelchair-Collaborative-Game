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
        private Vector2 TurnAcceleration = new Vector2(0.02f, 0);
        private bool startedRun = false;

        public enum Type
        {
            Left,
            Right
        }
        private Type type;

        public AvarageEnemy(GameEnhanced game, String tag, Type type)
            : base(game, tag)
        {
            this.life = 2;
            this.HUMANS = 10;
            Velocity = new Vector2(0, 0.7f);
            this.type = type;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("AvarageEnemy"), 1);
            Position = new Vector2(Config.resolution.X / 2 - Size.X / 2, -Size.Y);
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            

            if (!startedRun && Position.Y >= 0.02 * Config.resolution.Y)
            {
                //Velocity = new Vector2(-1f, 0.5f);
                if (type == Type.Right)
                    Acceleration = TurnAcceleration;
                else
                    Acceleration = -TurnAcceleration;
                startedRun = true;
            }

            if (!isLeaving)
            {
                if (Position.X < 500)
                {
                    Acceleration = TurnAcceleration;
                    //Velocity = new Vector2(1f, 0.5f);
                }
                else if (Position.X > 700)
                {
                    //Velocity = new Vector2(-1f, 0.5f);
                    Acceleration = -TurnAcceleration;
                }
                else if (Math.Abs(Velocity.X) > 1)
                    Acceleration = new Vector2(0, 0);
            }

            

        }

        public override void die()
        {
            base.die();
            Game.Components.Add(new BigExplosionGameObject(new Vector2(PositionCenterX, this.PositionCenterY), Game, 3));
        }
    }
}
