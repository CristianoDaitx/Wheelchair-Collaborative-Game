#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MortalKomatG.GameObjects;
#endregion

namespace MortalKomatG
{
    class StartOptionScreen : GameScreen
    {

        Texture2D textureStart;
        Texture2D textureOption;

        bool isStart = true;

        //int secondsToChangeScreen = 5;
        //d/ouble time = 0;

        public StartOptionScreen()
            :base()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1);
            TransitionOffTime = TimeSpan.FromSeconds(0);
        }


        public override void LoadContent()
        {
            base.LoadContent();

            textureOption = ScreenManager.Game.Content.Load<Texture2D>("tiles/options");
            textureStart = ScreenManager.Game.Content.Load<Texture2D>("tiles/start");
        }

        public override void Draw(GameTime gameTime)
        {
            
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            ScreenManager.SpriteBatch.Begin();
            if (isStart)
                ScreenManager.SpriteBatch.Draw(textureStart, fullscreen, Color.White);
            else
                ScreenManager.SpriteBatch.Draw(textureOption, fullscreen, Color.White);
            ScreenManager.SpriteBatch.End();


            base.Draw(gameTime);

        }

        public override void Update(GameTime gameTime, InputState input, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, input, otherScreenHasFocus, coveredByOtherScreen);


            PlayerIndex newPlayerIndex;
            if (input.IsMenuSelect(null, out newPlayerIndex)){
                if (isStart)
                {
                    ExitScreen();
                    ScreenManager.AddScreen(new FighterChoose(), null);
                }
                else
                {
                    ExitScreen();
                    ScreenManager.AddScreen(new OptionScreen(), null);
                }
            }

            if (input.IsMenuLeft(null) || input.IsMenuRight(null) || input.IsMenuUp(null) || input.IsMenuDown(null)){
                isStart = !isStart;
            }


        }
    }
}
