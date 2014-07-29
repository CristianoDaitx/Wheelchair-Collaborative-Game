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
    class Planet : GameObject2D
    {
        public Planet(GameEnhanced game, string tag)
            : base(game, tag)
        {
            Position = new Vector2(0, 600);
            Velocity = new Vector2(0, 0.01f);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("planet"), 1);

           
        }
    }
}
