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
    class WeakEnemy : EnemyGameObject
    {
        public enum Type
        {
            Left,
            Right
        }

        private int BORDER_STARTING_POSITION_X = 200;
        private Type type;

        public WeakEnemy(GameEnhanced game, String tag, Type type)
            : base( game, tag)
        {
            this.life = 1;

            this.HUMANS = 5;

            this.type = type;

            if (type == Type.Right)                        
                this.Velocity = new Vector2(0.95f, 0.7f);            
            else            
                this.Velocity = new Vector2(-0.95f, 0.7f);
            

        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("WeakEnemy"), 1);

            if (type == Type.Right)
                this.Position = new Vector2(BORDER_STARTING_POSITION_X, -Size.Y);
            else
                this.Position = new Vector2(Config.resolution.X - Size.X - BORDER_STARTING_POSITION_X, -Size.Y);

            

        }

        public override void die()
        {
            base.die();
            Game.Components.Add(new BigExplosionGameObject(new Vector2(PositionCenterX, this.PositionCenterY), Game, 2));
        }
    }
}
