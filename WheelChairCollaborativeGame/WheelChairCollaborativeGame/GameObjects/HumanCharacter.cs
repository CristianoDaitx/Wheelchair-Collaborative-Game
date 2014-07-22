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
    class HumanCharacter : GameObject2D
    {
        public HumanCharacter(GameEnhanced game, string tag)
            : base(game, tag)
        {
            Position = new Vector2(50, 50);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("characters"), 1);

            Sprite.ActiveSpriteAnimation = new SpriteAnimation(new SpriteAnimationData[] { new SpriteAnimationData(251, 0, 125, 125, 0, 0) });
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SharedSpriteBatch.Begin();
            GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                        "Look!\nBaby aliens!", new Vector2(200, 75));
            SharedSpriteBatch.End();
        }
    }
}
