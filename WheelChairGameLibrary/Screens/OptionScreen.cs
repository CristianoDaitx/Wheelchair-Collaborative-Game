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
    class OptionScreen : MenuScreen
    {
        Texture2D texture;

        bool isStart = true;

        //int secondsToChangeScreen = 5;
        //d/ouble time = 0;

        public OptionScreen()
            :base("")
        {
            TransitionOnTime = TimeSpan.FromSeconds(1);
            TransitionOffTime = TimeSpan.FromSeconds(0);

            MenuEntry p1KickHigh = new MenuEntry(this, "P1 Kick High");


            p1KickHigh.Selected += new EventHandler<PlayerIndexEventArgs>(p1KickHigh_Selected);


            MenuEntries.Add(p1KickHigh);
        }

        void p1KickHigh_Selected(object sender, PlayerIndexEventArgs e)
        {
            
        }


        public override void LoadContent()
        {
            base.LoadContent();

            texture = ScreenManager.Game.Content.Load<Texture2D>("tiles/splash");
        }

        public override void Draw(GameTime gameTime)
        {
            
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            ScreenManager.SpriteBatch.Begin();
                ScreenManager.SpriteBatch.Draw(texture, fullscreen, Color.White);
            ScreenManager.SpriteBatch.End();


            base.Draw(gameTime);

        }

        public override void Update(GameTime gameTime, InputState input, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, input, otherScreenHasFocus, coveredByOtherScreen);


            PlayerIndex newPlayerIndex;
            if (input.IsMenuCancel(null, out newPlayerIndex)){
                ExitScreen();
                ScreenManager.AddScreen(new StartOptionScreen(), null);
                
            }

           


        }
    }
}
