using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelChairGameLibrary;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WheelChairGameLibrary.Sprites;

namespace WheelChairCollaborativeGame.GameObjects
{
    class FrontMovementSprite : GameObject2D
    {
        public FrontMovementSprite(GameEnhanced game, string tag)
            : base(game, tag)
        {
            Position = new Vector2(20, 100);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("frontMovement"), 1);

            Sprite.ActiveSpriteAnimation = new SpriteAnimation(400, new SpriteAnimationData[] { 
                new SpriteAnimationData(0, 0, 256, 256, 0, 0),
                new SpriteAnimationData(256, 0, 256, 256, 0, 0)});
        }

    }
}
