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
    class SmallHuman : GameObject2D
    {
        private readonly Vector2 POSITION = new Vector2(1100, 680);
        private readonly Vector2 POSITION_DIFERENCE = new Vector2(-25, 0);

        public int representations { get; set; }

        public SmallHuman(GameEnhanced game, string tag)
            : base(game, tag)
        {
            Position = new Vector2(1100, 680);
            representations = 15;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("smallHuman"), 1);

            //Sprite.ActiveSpriteAnimation = new SpriteAnimation(new SpriteAnimationData[] { new SpriteAnimationData(376, 251, 125, 125, 0, 0) });
        }

        public override void Draw(GameTime gameTime)
        {
            Position = POSITION;
            base.Draw(gameTime);

            for (int x = 0; x < representations; x++)
            {
                Position += POSITION_DIFERENCE;
                Sprite.Draw(SharedSpriteBatch, gameTime);
            }
            /*base.Draw(gameTime);
            SharedSpriteBatch.Begin();
            GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                        "Shields will be\nup at 1:30!", new Vector2(900, 75));
            SharedSpriteBatch.End();*/
        }
    }
}
