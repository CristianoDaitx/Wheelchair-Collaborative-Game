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
    class WeakEnemy2 : EnemyGameObject
    {
        public WeakEnemy2(GameEnhanced game, String tag)
            : base(new Vector2(Config.resolution.X - 150, 0), game, tag)
        {
            this.maxhits = 1;
            //120, 138
            
            
            this.Velocity = new Vector2(-0.95f, 0.7f);


        }
        protected override void LoadContent()
        {
            base.LoadContent();

            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("WeakEnemy"), 0.5f);




        }
    }
}
