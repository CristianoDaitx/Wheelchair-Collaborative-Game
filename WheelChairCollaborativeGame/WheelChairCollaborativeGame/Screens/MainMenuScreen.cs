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
    class MainMenuScreen : Screen
    {

        private readonly Vector2 MENU_START_POSTION = new Vector2(500, 450);
        private readonly Vector2 MENU_SPACING = new Vector2(0, 40);

        private int menuSelected = 0;

        private TimeSpan timeRan;
        private int lastSecond = -1;

        private static SpriteFont spriteFont;

        public MainMenuScreen(GameEnhanced game, string tag)
            : base(game, tag)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Game.Components.Add(new Background(Game, 50));

            Game.Components.Add(new MainMenuAlien(Game, "MainMenuAlien"));
            spriteFont = Game.Content.Load<SpriteFont>(@"SpriteFont1");
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SharedSpriteBatch.Begin();

            SharedSpriteBatch.DrawString(spriteFont, "Human\nInvasion", new Vector2(550, 200), Color.White, MathHelper.ToRadians(-10), new Vector2(0, 0), 2, SpriteEffects.None, 0);

            SharedSpriteBatch.DrawString(spriteFont, "Start", MENU_START_POSTION, (0 == menuSelected ? Color.Yellow : Color.White));
            SharedSpriteBatch.DrawString(spriteFont, "Tutorial", MENU_START_POSTION + MENU_SPACING, (1 == menuSelected ? Color.Yellow : Color.White));
            SharedSpriteBatch.DrawString(spriteFont, "Settings", MENU_START_POSTION + MENU_SPACING * 2, (2 == menuSelected ? Color.Yellow : Color.White));
            SharedSpriteBatch.DrawString(spriteFont, "Exit", MENU_START_POSTION + MENU_SPACING * 3, (3 == menuSelected ? Color.Yellow : Color.White));

            SharedSpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            InputState inputState = InputState;

            PlayerIndex playerIndex;

            if (inputState.IsKeyPressed(Keys.Enter, null, out playerIndex))
            {
                switch (menuSelected)
                {
                    case 0:
                        Game.ActiveScreen = new PlayScreen(Game, "PlayScreen");
                        break;
                    case 3:
                        Game.Exit();
                        break;
                }                
            }

            if (inputState.IsKeyPressed(Keys.Up, null, out playerIndex))
                menuSelected--;
            if (inputState.IsKeyPressed(Keys.Down, null, out playerIndex))
                menuSelected++;

            if (menuSelected < 0)
                menuSelected = 3;
            if (menuSelected > 3)
                menuSelected = 0;

            addEnemies(gameTime);
        }

        public override void ExitScreen()
        {
            Game.RemoveAllButEssentialComponents();

        }


        private void addEnemies(GameTime gameTime)
        {
            timeRan += gameTime.ElapsedGameTime;
            if (timeRan.Seconds != lastSecond) // the second has changed
            {
                if (timeRan.Seconds % 5 == 0)
                {
                    Game.Components.Add(new MenuEnemy(Game, "MenuEnemy"));
                }

                lastSecond = timeRan.Seconds;
            }
        }
    }
}
