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
    class AlienCharacter : GameObject2D
    {
        public AlienCharacter(GameEnhanced game, string tag)
            : base(game, tag)
        {
            Position = new Vector2(Config.resolution.X - 50 - 125, 50);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("characters"), 1);

            Sprite.ActiveSpriteAnimation = new SpriteAnimation(new SpriteAnimationData[] { new SpriteAnimationData(376, 251, 125, 125, 0, 0) });
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            base.Draw(gameTime);
            SharedSpriteBatch.Begin();
            GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                        "Shields will be\nup at 1:30!", new Vector2(900, 50));
            SharedSpriteBatch.End();
        }
    }
}
