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

        Config.ControlSelect controlSelect;

        public FrontMovementSprite(GameEnhanced game, String tag)
            : base(game, tag)
        {
            DrawOrder--;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            controlSelect = Config.ControlSelected;

            if (Config.ControlSelected == Config.ControlSelect.Front || Config.ControlSelected == Config.ControlSelect.FrontAssyncronous)
            {
                Position = new Vector2(1000, 100);

                Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("frontTutorialSheet"), 1);
                Sprite.ActiveSpriteAnimation = new SpriteAnimation(300, new SpriteAnimationData[] { 
                new SpriteAnimationData(0, 0, 256, 256, 0, 0),
                new SpriteAnimationData(256, 0, 256, 256, 0, 0),
                new SpriteAnimationData(512, 0, 256, 256, 0, 0),
                new SpriteAnimationData(0, 256, 256, 256, 0, 0),
                new SpriteAnimationData(256, 256, 256, 256, 0, 0),
                new SpriteAnimationData(256, 256, 256, 256, 0, 0),
                new SpriteAnimationData(256, 256, 256, 256, 0, 0)});


            }
            if (Config.ControlSelected == Config.ControlSelect.Side)
            {
                Position = new Vector2(10, 100);                
                Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, Game.Content.Load<Texture2D>("sideTutorialSheet"), 1);
                //Sprite.Alpha = 0.7f;
                Sprite.ActiveSpriteAnimation = new SpriteAnimation(300, new SpriteAnimationData[] { 
                    new SpriteAnimationData(0, 0, 512, 256, 0, 0),
                    new SpriteAnimationData(512, 0, 512, 256, 0, 0),
                    new SpriteAnimationData(1024, 0, 512, 256, 0, 0),
                    new SpriteAnimationData(0, 256, 512, 256, 0, 0),
                    new SpriteAnimationData(512, 256, 512, 256, 0, 0),
                    new SpriteAnimationData(512, 256, 512, 256, 0, 0),
                    new SpriteAnimationData(512, 256, 512, 256, 0, 0)});

            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (Config.ControlSelected == Config.ControlSelect.Joystick)
                return;
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (controlSelect != Config.ControlSelected)
                LoadContent();
        }
    }
}
