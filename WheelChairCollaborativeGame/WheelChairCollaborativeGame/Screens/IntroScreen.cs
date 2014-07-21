using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WheelChairGameLibrary;
using WheelChairGameLibrary.Helpers;
using Microsoft.Xna.Framework.Input;

namespace WheelChairCollaborativeGame
{
    class IntroScreen : Screen
    {

        public IntroScreen(GameEnhanced game, string tag)
            : base(game, tag)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SharedSpriteBatch.Begin();
            GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,

                        "Fellow aliens!\n Humans have wrecked their planet and are now coming for us!\nWe mus keep them from landing on our planet, \n but security shields are dwon\n" +
                        "\n You have armed space-ships, use them wisely to protect our loved families!", new Vector2(30, 30));
            SharedSpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            InputState inputState = (InputState)Game.Services.GetService(typeof(InputState));

            PlayerIndex playerIndex;

            if (inputState.IsKeyPressed(Keys.Enter, null, out playerIndex))
            {
                Game.ActiveScreen = new MainMenuScreen(Game, "MainMenuScreen");
            }
        }

        public override void ExitScreen()
        {
            
        }
    }
}
