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
    class HardEnemy : EnemyGameObject
    {
        public HardEnemy(GameEnhanced game, String tag)
            : base(new Vector2(Config.resolution.X / 2 - 50, 0), game, tag)
        {
            this.maxhits = 3;
            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("Space_InvaderHard"),
                     0.5f);

            Velocity = new Vector2(0, 0.7f);



        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Position.Y > Config.resolution.Y - 150)
            {
                Velocity = new Vector2(0.7f, 0);
            }
        }
    }
}
