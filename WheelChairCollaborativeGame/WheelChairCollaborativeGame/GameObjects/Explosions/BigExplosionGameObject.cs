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

        public BigExplosionGameObject(Vector2 startingPosition, GameEnhanced game)
            : base(startingPosition, game, "Explosion")
        {

        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("explosions"), 3f);

            //TODO adjust spritesheet
            Sprite.ActiveSpriteAnimation = (new SpriteAnimation(20,
                    new SpriteAnimationData[] {
                        new SpriteAnimationData(0, 0, 24, 28, 0, 0),
                        new SpriteAnimationData(0, 28, 24, 28, 0, 0),
                        new SpriteAnimationData(24, 0, 24, 28, 0, 0),
                        new SpriteAnimationData(24, 28, 24, 28, 0, 0),
                        new SpriteAnimationData(48, 0, 24, 28, 0, 0),
                        new SpriteAnimationData(48, 28, 24, 28, 0, 0),
                        new SpriteAnimationData(72, 0, 24, 28, 0, 0),
                        new SpriteAnimationData(72, 28, 24, 28, 0, 0),
                        new SpriteAnimationData(96, 0, 24, 28, 0, 0),
                        new SpriteAnimationData(96, 28, 24, 28, 0, 0),
                        new SpriteAnimationData(120, 0, 24, 28, 0, 0),
                        new SpriteAnimationData(120, 28, 24, 28, 0, 0),
                        new SpriteAnimationData(144, 0, 24, 28, 0, 0),
                        new SpriteAnimationData(144, 28, 24, 28, 0, 0),
                        new SpriteAnimationData(168, 0, 24, 28, 0, 0),
                        new SpriteAnimationData(168, 28, 24, 28, 0, 0),
                        new SpriteAnimationData(192, 0, 24, 28, 0, 0)}
                ));


            //TODO it should not be hardcoded
            this.Position = new Vector2(this.Position.X - 12 * 3, this.Position.Y - 14 * 3);

        }

        public override void endedAnimation()
        {
            base.endedAnimation();
            ToBeRemoved = true;
        }
    }
}
