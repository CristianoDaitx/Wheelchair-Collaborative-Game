using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WheelChairGameLibrary;
using WheelChairGameLibrary.Helpers;
using Microsoft.Xna.Framework.Input;
using WheelChairCollaborativeGame.GameObjects;

namespace WheelChairCollaborativeGame
{
    class SplashScreen : Screen
    {

        public SplashScreen(GameEnhanced game, string tag)
            : base(game, tag)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Game.Components.Add(new MainMenuAlien(Game, "MainMenuAlien"));
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SharedSpriteBatch.Begin();
            GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                        "A game produced by somebody!", new Vector2(450, 300));
            SharedSpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            InputState inputState = (InputState)Game.Services.GetService(typeof(InputState));

            PlayerIndex playerIndex;

            if (inputState.IsKeyPressed(Keys.Enter, null, out playerIndex))
            {
                Game.ActiveScreen = new IntroScreen(Game, "IntroScreen");
            }
        }

        public override void ExitScreen()
        {
            //nothing to remove
        }
    }
}
