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
    class IntroScreen : Screen
    {

        TimeSpan timeRan = new TimeSpan();
        TimeSpan maxTime = TimeSpan.FromSeconds(14);

        public IntroScreen(GameEnhanced game, string tag)
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

                        "Fellow aliens!"+
                        "\nHumans have wrecked their planet and are" +
                        "\nnow coming for us!"+
                        "\nWe must keep them from landing on our planet,"+
                        "\nbut security shields are down" +
                        "\nYou have armed space-ships,"+
                        "\nuse them wisely to protect our loved families!", new Vector2(450, 300));
            SharedSpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            InputState inputState = (InputState)Game.Services.GetService(typeof(InputState));

            PlayerIndex playerIndex;
            timeRan += gameTime.ElapsedGameTime;

            if (timeRan >= maxTime)
                Game.ActiveScreen = new MainMenuScreen(Game, "MainMenuScreen");


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
