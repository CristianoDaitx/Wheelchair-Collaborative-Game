#region Using Statements
using System;
using System.Linq;
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
using log4net;
using WheelChairCollaborativeGame.Logging;

#endregion

namespace WheelChairCollaborativeGame
{
    abstract class EnemyGameObject : GameObject2D
    {
        private readonly ILog detailedLog = LogManager.GetLogger("DetailedLogger");

        public delegate void DiedEventHandler(object sender, EnemyGameObjectEventArgs e);
        public event DiedEventHandler DiedCompleted;

        private static readonly int REAMINING_Y_TO_LEAVE = 250;

        public SoundEffect explosionSound;

        protected bool isLeaving = false;

        protected int life = 1;
        protected int HUMANS = 5;

        private static Random random = new Random();

        public EnemyGameObject(Vector2 position, GameEnhanced game, String tag)
            : base(position, game, tag)
        {
            Collider = new Collider(this);
            DrawOrder--;
        }
        public EnemyGameObject(GameEnhanced game, String tag)
            : base(game, tag)
        {
            Collider = new Collider(this);
            DrawOrder--;
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


                if (this.Velocity.X > 0)
                    Acceleration = new Vector2(0.2f, 0);
                else if (this.Velocity.X < 0)
                    Acceleration = new Vector2(-0.2f, 0);
                else
                    Acceleration = (random.Next(0, 1) == 0 ? new Vector2(0.2f, 0) : new Vector2(-0.2f, 0));
            }


            //delete if exit screen
            if (Position.X > Config.resolution.X ||
                PositionRightX < 0 ||
                Position.Y > Config.resolution.Y ||
                PositionBottomY < 0)
            {
                PlayScreen playScreen = (PlayScreen)Game.Components.FirstOrDefault(x => x.GetType() == typeof(PlayScreen));
                if (playScreen != null)
                    playScreen.Invaders += this.HUMANS;

                // only call event once
                if (!this.ToBeRemoved)
                {
                    if (DiedCompleted != null)
                        DiedCompleted(this, new EnemyGameObjectEventArgs(false));
                }
                //if (DiedCompleted != null)
                //    DiedCompleted(this, EventArgs.Empty);
                this.ToBeRemoved = true;
            }

        }


        public override void collisionEntered(Collider collider)
        {
            if (collider.GameObject.GetType() == typeof(BallGameObject))
            {
                detailedLog.Info(new DetailedInfo(DetailedInfo.Type.SPACESHIP_HIT));
                ((MyGame)Game).Logger.shotsHit++;
                
                life--;
                if (0 == life)
                {
                    ToBeRemoved = true;
                    PlayScreen playScreen = (PlayScreen)Game.Components.FirstOrDefault(x => x.GetType() == typeof(PlayScreen));
                    if (playScreen != null)
                        playScreen.Score += 2 * this.HUMANS;

                    die();

                    explosionSound.Play();
                }

            }
        }

        public virtual void die()
        {
            if (DiedCompleted != null)
                DiedCompleted(this, new EnemyGameObjectEventArgs(true));
        }


        public class EnemyGameObjectEventArgs : EventArgs
        {
            public EnemyGameObjectEventArgs(bool wasShot)
            {
                this.wasShot = wasShot;
            }
            public bool wasShot { get; set; }
        }

    }
}
