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
        TimeSpan timeRan = new TimeSpan();
        TimeSpan maxTime = TimeSpan.FromSeconds(4);

        public SplashScreen(GameEnhanced game, string tag)
            : base(game, tag)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            //Game.Components.Add(new MainMenuAlien(Game, "MainMenuAlien"));
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SharedSpriteBatch.Begin();
            GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
            "           A game produced by:\n"+
            "Cristiano Daitx & Gianei L. Sebastiany\n"+
            "       Directed by Kathrin Gerling", new Vector2(430, 300));
            SharedSpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            InputState inputState = (InputState)Game.Services.GetService(typeof(InputState));

            PlayerIndex playerIndex;


            timeRan += gameTime.ElapsedGameTime;

            if (timeRan >= maxTime)
                Game.ActiveScreen = new IntroScreen(Game, "IntroScreen");



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
