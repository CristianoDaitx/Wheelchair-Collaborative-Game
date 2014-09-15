using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WheelChairGameLibrary;
using WheelChairGameLibrary.Helpers;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using WheelChairCollaborativeGame.GameObjects;
using log4net;

namespace WheelChairCollaborativeGame
{
    class GameOverScreen : Screen
    {

        private Song backgroundSong;
        public int Score;
        public GameOverScreen(GameEnhanced game, string tag)
            : base(game, tag)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            MediaPlayer.Play(backgroundSong);
            MediaPlayer.IsRepeating = false;

            

        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Game.Components.Add(new MainMenuAlien(Game, "MainMenuAlien"));
            backgroundSong = Game.Content.Load<Song>("Tyrian - 10 - End Of Level");

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SharedSpriteBatch.Begin();
            
            
            GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                        "        Fellow aliens!\nWe have successfully defended"+
            "\nour civilization and have caught " +"\n"+Score.ToString()+" humans, "+
            "thanks to your support.", new Vector2(450, 300));
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
            Game.RemoveAllButEssentialComponents();
            MediaPlayer.Stop();
        }
    }
}
