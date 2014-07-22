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
    abstract class EnemyGameObject : GameObject2D
    {
        protected static readonly int BORDER_STARTING_POSITION_Y = 20;
        private static readonly int REAMINING_Y_TO_LEAVE = 250;

        private SoundEffect explosionSound;

        protected bool isLeaving = false;

        protected int life = 1;

        public EnemyGameObject(Vector2 position, GameEnhanced game, String tag)
            : base(position, game, tag)
        {
            Collider = new Collider(this);
        }
        public EnemyGameObject(GameEnhanced game, String tag)
            : base(game, tag)
        {
            Collider = new Collider(this);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            explosionSound = Game.Content.Load<SoundEffect>("explosion");
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //test to start leaving
            if (this.PositionBottomY > Config.resolution.Y - REAMINING_Y_TO_LEAVE)
            {
                isLeaving = true;
                if (this.PositionCenterX > Config.resolution.X / 2)
                    Acceleration = new Vector2(0.2f, 0);
                else
                    Acceleration = new Vector2(-0.2f, 0);
            }


            //delete if exit screen
            if (Position.X > Config.resolution.X ||
                PositionRightX < 0 ||
                Position.Y > Config.resolution.Y ||
                PositionBottomY < 0)
                this.ToBeRemoved = true;

        }


        public override void collisionEntered(Collider collider)
        {
            if (collider.GameObject.GetType() == typeof(BallGameObject))
            {
                life--;
                if (0 == life)
                {
                    ToBeRemoved = true;
                    
                    Game.Components.Add(new BigExplosionGameObject(new Vector2(PositionCenterX,this.PositionCenterY  ), Game));

                    
                    explosionSound.Play();
                }
                
            }
        }

    }
}
