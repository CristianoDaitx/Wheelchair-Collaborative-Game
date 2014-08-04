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
    class ConfigScreen : Screen
    {

        public ConfigScreen(GameEnhanced game, string tag)
            : base(game, tag)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SharedSpriteBatch.Begin();
            GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                        "Input mode: " + Config.ControlSelected, new Vector2(450, 300));
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

            //changes control type
            if (inputState.IsKeyPressed(Keys.Left, null, out playerIndex))
                Config.ControlSelected--;
            if (inputState.IsKeyPressed(Keys.Right, null, out playerIndex))
                Config.ControlSelected++;

            //check borders of control tipe
            if ((int)Config.ControlSelected == 4)
            {
                Config.ControlSelected = Config.ControlSelect.FrontAssyncronous;
            }
            if ((int)Config.ControlSelected < 0)
            {
                Config.ControlSelected = Config.ControlSelect.Joystick;
            }

            if (!Chooser.IsAvailable)
                Config.ControlSelected = Config.ControlSelect.Joystick;
        }


        public override void ExitScreen()
        {
            //nothing to remove
        }
    }
}
