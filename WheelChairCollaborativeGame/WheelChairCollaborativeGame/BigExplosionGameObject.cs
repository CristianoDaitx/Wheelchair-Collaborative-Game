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
    class BigExplosionGameObject : GameObject
    {

        public BigExplosionGameObject(Vector2 startingPosition, GameEnhanced game)
            : base(startingPosition, game, "Explosion")
        {

        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("explosions"), 3f);

            //todo adjust spritesheet
            Sprite.setActiveSpriteAnimation(new SpriteAnimation(20,
                    new SpriteAnimationData[] {
                        new SpriteAnimationData(0, 0, 12, 28, 0, 0),
                        new SpriteAnimationData(12, 0, 12, 28, 0, 0),
                        new SpriteAnimationData(24, 0, 12, 28, 0, 0),
                        new SpriteAnimationData(36, 0, 12, 28, 0, 0),
                        new SpriteAnimationData(48, 0, 12, 28, 0, 0),
                        new SpriteAnimationData(60, 0, 12, 28, 0, 0),
                        new SpriteAnimationData(72, 0, 12, 28, 0, 0),
                        new SpriteAnimationData(84, 0, 12, 28, 0, 0),
                        new SpriteAnimationData(96, 0, 12, 28, 0, 0),
                        new SpriteAnimationData(108, 0, 12, 28, 0, 0),
                        new SpriteAnimationData(120, 0, 12, 28, 0, 0),
                        new SpriteAnimationData(132, 0, 12, 28, 0, 0),
                        new SpriteAnimationData(144, 0, 12, 28, 0, 0),
                        new SpriteAnimationData(156, 0, 12, 28, 0, 0),
                        new SpriteAnimationData(168, 0, 12, 28, 0, 0),
                        new SpriteAnimationData(180, 0, 12, 28, 0, 0),
                        new SpriteAnimationData(192, 0, 12, 28, 0, 0)}
                ));

        }

        public override void endedAnimation()
        {
            base.endedAnimation();
            ToBeRemoved = true;
        }
    }
}
