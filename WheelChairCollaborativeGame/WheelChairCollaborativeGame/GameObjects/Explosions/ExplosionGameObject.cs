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
    class ExplosionGameObject : GameObject2D
    {
        public ExplosionGameObject(Vector2 startingPosition, GameEnhanced game)
            : base(startingPosition, game, "Explosion")
        {

        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("explosions"), 1.5f);
            Sprite.ActiveSpriteAnimation = (new SpriteAnimation( 20,
                    new SpriteAnimationData[] {
                        new SpriteAnimationData(72, 84, 12, 14, 0, 0),
                        new SpriteAnimationData(84, 84, 12, 14, 0, 0),
                        new SpriteAnimationData(96, 84, 12, 14, 0, 0),
                        new SpriteAnimationData(108, 84, 12, 14, 0, 0),
                        new SpriteAnimationData(120, 84, 12, 14, 0, 0),
                        new SpriteAnimationData(132, 84, 12, 14, 0, 0),
                        new SpriteAnimationData(144, 84, 12, 14, 0, 0),
                        new SpriteAnimationData(156, 84, 12, 14, 0, 0),
                        new SpriteAnimationData(168, 84, 12, 14, 0, 0),
                        new SpriteAnimationData(180, 84, 12, 14, 0, 0),
                        new SpriteAnimationData(192, 84, 12, 14, 0, 0),
                        new SpriteAnimationData(204, 84, 12, 14, 0, 0),
                        new SpriteAnimationData(216, 84, 12, 14, 0, 0)}
                ));
            this.Position = new Vector2(this.Position.X - 3, this.Position.Y);
        }

        public override void endedAnimation()
        {
            base.endedAnimation();
            ToBeRemoved = true;
        }
    }
}
