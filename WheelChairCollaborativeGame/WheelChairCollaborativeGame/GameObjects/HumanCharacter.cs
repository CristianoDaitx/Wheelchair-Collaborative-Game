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
    class HumanCharacter : MessageCharacter
    {
        public HumanCharacter(String message, GameEnhanced game, string tag)
            : base(message, game, tag)
        {
            Position = new Vector2(Config.resolution.X - 50 - 125, 50);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("CharHuman"), 1);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SharedSpriteBatch.Begin();
            GUImessage.MessageDraw(SharedSpriteBatch, Game.Content, message, new Vector2(900, 75));
            SharedSpriteBatch.End();
        }
    }
}
