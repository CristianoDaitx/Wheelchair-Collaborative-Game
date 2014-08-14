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
        private readonly Vector2 MOVING_VELOCITY = new Vector2(0, 0.01f);
        public Planet(GameEnhanced game, string tag)
            : base(game, tag)
        {
            Position = new Vector2(0, 600);
            DrawOrder--;
        }

        public void startMoving(){
            Velocity = MOVING_VELOCITY;
        }

        public void stopMoving(){
            Velocity = Vector2.Zero;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("planet"), 1);


        }
    }
}
