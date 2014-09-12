using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WheelChairGameLibrary;
using WheelChairGameLibrary.Helpers;
using Microsoft.Xna.Framework.Input;
using WheelChairCollaborativeGame.GameObjects;
using Microsoft.Xna.Framework.Graphics;

namespace WheelChairCollaborativeGame
{
    class ConfigScreen : Screen
    {
        private Vector2 startingPosition = new Vector2(542, 350);
        private Vector2 differencePosition = new Vector2(0, 50);

        private static SpriteFont spriteFont2;


        private enum ActiveSetting
        {
            INPUT,
            GROUP_ID
        }

        private ActiveSetting activeSetting = ActiveSetting.INPUT;



        public ConfigScreen(GameEnhanced game, string tag)
            : base(game, tag)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            spriteFont2 = Game.Content.Load<SpriteFont>(@"SpriteFont3");

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SharedSpriteBatch.Begin();

            if (!Chooser.IsAvailable)
            {
                SharedSpriteBatch.DrawString(spriteFont2, "Connect a Microsoft Kinect device\n" +
                "to use more input modes.", new Vector2(240, 200), Color.White);
            }

            //GUImessage.MessageDraw(SharedSpriteBatch, Game.Content, "Joystick", 1, startingPosition);
            SharedSpriteBatch.DrawString(spriteFont2, "A", getMenuTextPosition(Config.ControlSelect.Joystick), getMenuColor(Config.ControlSelect.Joystick));//Joystick
            SharedSpriteBatch.DrawString(spriteFont2, "B", getMenuTextPosition(Config.ControlSelect.Front), getMenuColor(Config.ControlSelect.Front));//Front movement
            SharedSpriteBatch.DrawString(spriteFont2, "C", getMenuTextPosition(Config.ControlSelect.Side), getMenuColor(Config.ControlSelect.Side));//Highfive Movement
            SharedSpriteBatch.DrawString(spriteFont2, "D", getMenuTextPosition(Config.ControlSelect.FrontAssyncronous), getMenuColor(Config.ControlSelect.FrontAssyncronous));//Front movement assyncronous

            SharedSpriteBatch.DrawString(spriteFont2, "Input mode:", new Vector2(240, 350), (activeSetting == ActiveSetting.INPUT ? Color.Yellow: Color.White));
            //GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
            //            "Input mode: " + Config.ControlSelected, new Vector2(450, 300));


            SharedSpriteBatch.DrawString(spriteFont2, "  Group Id:" + Config.GroupId, new Vector2(240, 550), (activeSetting == ActiveSetting.GROUP_ID ? Color.Yellow : Color.White));
            SharedSpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            InputState inputState = (InputState)Game.Services.GetService(typeof(InputState));

            PlayerIndex playerIndex;

            if (inputState.IsKeyPressed(Keys.Enter, null, out playerIndex) ||
                    inputState.IsKeyPressed(Keys.Escape, null, out playerIndex))
            {
                Game.ActiveScreen = new MainMenuScreen(Game, "MainMenuScreen");
            }

            if (activeSetting == ActiveSetting.INPUT)
            {
                

                //changes control type
                if (inputState.IsKeyPressed(Keys.Left, null, out playerIndex))
                    Config.ControlSelected--;
                if (inputState.IsKeyPressed(Keys.Right, null, out playerIndex))
                    Config.ControlSelected++;

                //check borders of control tipe
                if ((int)Config.ControlSelected == 4)
                {
                    Config.ControlSelected = Config.ControlSelect.Joystick;
                }
                if ((int)Config.ControlSelected < 0)
                {
                    Config.ControlSelected = Config.ControlSelect.FrontAssyncronous;
                }
            }

            if (!Chooser.IsAvailable)
                Config.ControlSelected = Config.ControlSelect.Joystick;

            if (activeSetting == ActiveSetting.GROUP_ID)
            {
                //changes control type
                if (inputState.IsKeyPressed(Keys.Left, null, out playerIndex))
                    Config.GroupId--;
                if (inputState.IsKeyPressed(Keys.Right, null, out playerIndex))
                    Config.GroupId++;
                if ((int)Config.GroupId < 1)
                {
                    Config.GroupId = 1;
                }
            }


            if (inputState.IsKeyPressed(Keys.Up, null, out playerIndex) || inputState.IsKeyPressed(Keys.Down, null, out playerIndex))
            {
                if (activeSetting == ActiveSetting.GROUP_ID)
                    activeSetting = ActiveSetting.INPUT;
                else
                    activeSetting = ActiveSetting.GROUP_ID;
            }

            
        }

        private Color getMenuColor(Config.ControlSelect controlType)
        {


            if (controlType == Config.ControlSelected)
            {
                if (activeSetting == ActiveSetting.GROUP_ID)
                    return Color.White;
                return Color.Yellow;
            }
            if (!Chooser.IsAvailable)
                return Color.Gray;
            return Color.White;
        }

        private Vector2 getMenuTextPosition(Config.ControlSelect controlType)
        {
            return startingPosition + differencePosition * (int)controlType - differencePosition * (int)Config.ControlSelected;
        }

        public override void ExitScreen()
        {
            //nothing to remove
        }
    }
}
