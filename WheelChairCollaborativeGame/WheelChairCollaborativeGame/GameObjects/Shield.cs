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
    class Shield : GameObject2D
    {
        private readonly float ALPHA_INCREASE = 0.01f;


        public Shield(GameEnhanced game, string tag)
            : base(game, tag)
        {
            Position = new Vector2(-128, 450);
            //Velocity = new Vector2(0, 0.01f);
            DrawOrder -= 2;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Sprite.Alpha < 1)
                Sprite.Alpha += ALPHA_INCREASE;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("shield"), 1);
            Sprite.Alpha = 0.0f;


        }
    }
}
