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

#endregion

namespace WheelChairCollaborativeGame
{
    class BigExplosionGameObject : GameObject2D
    {
        private float scale;

        public BigExplosionGameObject(Vector2 startingPosition, GameEnhanced game, float scale)
            : base(startingPosition, game, "Explosion")
        {
            this.scale = scale;
        }


        protected override void LoadContent()
        {
            base.LoadContent();
            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("explosions"), scale);

            //TODO adjust spritesheet
            Sprite.ActiveSpriteAnimation = (new SpriteAnimation(20,
                    new SpriteAnimationData[] {
                        new SpriteAnimationData(0, 0, 23, 27, 0, 0),
                        new SpriteAnimationData(0, 28, 23, 27, 0, 0),
                        new SpriteAnimationData(24, 0, 23, 27, 0, 0),
                        new SpriteAnimationData(24, 28, 23, 27, 0, 0),
                        new SpriteAnimationData(48, 0, 23, 27, 0, 0),
                        new SpriteAnimationData(48, 28, 23, 27, 0, 0),
                        new SpriteAnimationData(72, 0, 23, 27, 0, 0),
                        new SpriteAnimationData(72, 28, 23, 27, 0, 0),
                        new SpriteAnimationData(96, 0, 23, 27, 0, 0),
                        new SpriteAnimationData(96, 28, 23, 27, 0, 0),
                        new SpriteAnimationData(120, 0, 23, 27, 0, 0),
                        new SpriteAnimationData(120, 28, 23, 27, 0, 0),
                        new SpriteAnimationData(144, 0, 23, 27, 0, 0),
                        new SpriteAnimationData(144, 28, 23, 27, 0, 0),
                        new SpriteAnimationData(168, 0, 23, 27, 0, 0),
                        new SpriteAnimationData(168, 28, 23, 27, 0, 0),
                        new SpriteAnimationData(192, 0, 23, 27, 0, 0)}
                ));


            this.Position = new Vector2(this.Position.X - base.Size.X/2, this.Position.Y - base.Size.Y / 2);

        }

        public override void endedAnimation()
        {
            base.endedAnimation();
            ToBeRemoved = true;
        }
    }
}
