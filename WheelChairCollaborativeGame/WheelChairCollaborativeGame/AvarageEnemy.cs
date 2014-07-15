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
            : base(new Vector2(600, 0), game, tag)
        {
            this.maxhits = 2;
          
            Velocity = new Vector2(0, 0.5f);

         
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("AvarageEnemy"), 0.5f);

        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Position.Y == 0.10*Config.resolution.Y)
            {
                Velocity = new Vector2(-1f, 0.5f);
            }
            if (Position.X < 500)
                Velocity = new Vector2(1f, 0.5f);
            if (Position.X > 700 && Position.Y < Config.resolution.Y -250)
                Velocity = new Vector2(-1f, 0.5f);
            //if (Position.X >= 700)
            //{
              //  Velocity = new Vector2(-0.7f, 0.5f);
            //}
                            //if (Position.X > 800)
                //{
                //    Velocity = new Vector2(Velocity.X, 0.3f);
                //}
            




            /* if (Position.Y == 0.20*Config.resolution.Y)
             {
                 Velocity = new Vector2(Velocity.X, 0.3f);
                 if (Position.X > 50)
                 {
                     Velocity = new Vector2(-0.7f, Velocity.Y);
                 }
                 else 
                 {
                     Velocity = new Vector2(0, 1);
                 }
             }
             if (Position.Y == 0.40 * Config.resolution.Y)
             {
                 Velocity = new Vector2(Velocity.X, 0.3f);
                 if (Position.X < Config.resolution.X - 150)
                 {
                     Velocity = new Vector2(0.7f, Velocity.Y);
                 }
                 else
                 {
                     Velocity = new Vector2(0, 0.3f);
                 }
             }
             if (Position.Y == 0.60 * Config.resolution.Y)
             {
                 Velocity = new Vector2(Velocity.X, 0.3f);
                 if (Position.X > 50)
                 {
                     Velocity = new Vector2(-0.7f, Velocity.Y);
                 }
                 else
                 {
                     Velocity = new Vector2(0, 1);
                 }
             }
             if (Position.Y == 0.8 *Config.resolution.Y)
             {
                 Velocity = new Vector2(Velocity.X, 0.3f);
                 if (Position.X < Config.resolution.X - 150)
                 {
                     Velocity = new Vector2(0.7f, Velocity.Y);
                 }
                 else
                 {
                     Velocity = new Vector2(0, 1);
                 }
             
             } 
             */
        }
    }
}
