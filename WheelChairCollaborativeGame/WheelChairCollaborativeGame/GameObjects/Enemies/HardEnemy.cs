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
        private readonly int TRESHOLD_LIFE = 5;


        public HardEnemy(GameEnhanced game, String tag)
            : base( game, tag)
        {
            this.life = 10;
            
            Velocity = new Vector2(0, 0.7f);            
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("hardEnemy"),2f);
            SpriteAnimation spriteAnimation  = new SpriteAnimation(160,
                    new SpriteAnimationData[] {
                        new SpriteAnimationData(0, 0, 118, 138, 0, 0),
                        new SpriteAnimationData(120, 0, 118, 138, 0, 0)});
            spriteAnimation.AutoChangeState = false;
            Sprite.ActiveSpriteAnimation = spriteAnimation;

            Position = new Vector2(Config.resolution.X / 2 - Size.X / 2, - Size.Y);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Sprite.ActiveSpriteAnimation.ActualState == 0 && life < TRESHOLD_LIFE)
                Sprite.ActiveSpriteAnimation.nextState();
        }

        protected override void die()
        {
            Game.Components.Add(new BigExplosionGameObject(new Vector2(PositionCenterX, this.PositionCenterY), Game, 10));
        }
    }
}
