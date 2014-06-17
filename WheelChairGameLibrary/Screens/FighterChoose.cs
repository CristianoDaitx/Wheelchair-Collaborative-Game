#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using MortalKomatG.GameObjects;
#endregion
namespace MortalKomatG
{
    class FighterChoose : GameScreen
    {
        Texture2D backgroundTexture;
        Cue cue;

        public FighterChoose()
            : base ()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1);

            
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            cue.Stop(AudioStopOptions.Immediate);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            GameObjectManager.addGameObject(new FighterSelector(GameObjectManager, "player1Selector", false));
            GameObjectManager.addGameObject(new FighterSelector(GameObjectManager, "player2Selector", true));

            backgroundTexture = ScreenManager.Game.Content.Load<Texture2D>("tiles/fighters");

            cue = ((MortalKomatG.Game)ScreenManager.Game).getSoundBank().GetCue("fighterSelect");
            cue.Play();

        }

        public override void Update(GameTime gameTime, InputState input, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, input, otherScreenHasFocus, coveredByOtherScreen);
            //PlayerIndex newPlayerIndex;
            /*if (input.IsKeyPressed(Keys.Enter, null, out newPlayerIndex)){
                ExitScreen();
                ScreenManager.AddScreen(new FightScreen(), null);
            }*/

           /* if (!cue.IsPlaying)
            {
                cue = ((MortalKomatG.Game)ScreenManager.Game).getSoundBank().GetCue("fighterSelect");
                cue.Play();
            }*/

            PlayerIndex newPlayerIndex;
            if (input.IsMenuCancel(null, out newPlayerIndex))
            {
                ScreenManager.AddScreen(new MessageBoxScreen("     Quit"+
                    "\n\nPress button A, Space or Enter to resume" +
                    "\nPress button B or Esc to quit game", false, this), newPlayerIndex);
                this.isActive = false;
            }
        }

        public override void Draw(GameTime gameTime)
        {

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(backgroundTexture, fullscreen, Color.White);
            ScreenManager.SpriteBatch.End();


            base.Draw(gameTime);

        }
    }
}
