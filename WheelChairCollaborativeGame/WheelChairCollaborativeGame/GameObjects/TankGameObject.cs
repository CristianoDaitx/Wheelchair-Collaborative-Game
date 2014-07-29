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
    class TankGameObject : GameObject2D
    {

        private readonly Vector2 MAX_VELOCITY = new Vector2(1, 0.5f);
        private readonly float ACCELERATION_X = 0.015f;

        private readonly int WIDTH_POSITION_CONSTANT_SPEED = 125;
        private float MaxLeft
        {
            get { return Config.resolution.X / 2 - WIDTH_POSITION_CONSTANT_SPEED; }
        }
        private float MaxRight
        {
            get { return Config.resolution.X / 2 + WIDTH_POSITION_CONSTANT_SPEED; }
        }

        private double time = 0;

        public TankGameObject(GameEnhanced game, String tag)
            : base( game, tag)
        {
            Velocity = MAX_VELOCITY;
            Acceleration = new Vector2(0, -0.005f);
        }

        protected override void LoadContent()
        {
            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, this.Game.Content.Load<Texture2D>("PlayerA"), 0.5f);
            Position = new Vector2(Config.resolution.X / 2 - Size.X / 2, Config.resolution.Y - 120);

            

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            time += gameTime.ElapsedGameTime.TotalMilliseconds;


            //Position.Y += 0.0001f;


            if (Math.Abs(Velocity.Y) >MAX_VELOCITY.Y)
            {
                Acceleration = new Vector2(Acceleration.X, -Acceleration.Y);
            }

            if (Math.Abs(Velocity.X) > MAX_VELOCITY.X)
            {
                Acceleration = new Vector2(0, Acceleration.Y);
            }

            if (PositionCenterX < MaxLeft)
            {
                Acceleration = new Vector2(ACCELERATION_X, Acceleration.Y);
            }
            if (PositionCenterX > MaxRight)
            {
                Acceleration = new Vector2(-ACCELERATION_X, Acceleration.Y);
            }
        }



    }
}
