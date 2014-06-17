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
    public class FightScreen : GameScreen
    {
        #region Fields

        Texture2D backgroundTexture;

        Cue cue;

        #endregion

        #region Initialization

        public FightScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            //TransitionOffTime = TimeSpan.FromSeconds(0.5);         
        }

        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void LoadContent()
        {

            cue = ((MortalKomatG.Game)ScreenManager.Game).getSoundBank().GetCue("fightScreen");
            cue.Play();

            GameObject firstFighter = new FighterGameObject(GameObjectManager, "fighterOne", ScreenManager.Game.Content, FighterGameObject.FighterType.Scorpion, PlayerIndex.One);
            GameObjectManager.addGameObject(firstFighter);
            firstFighter.Sprite.position.X = 211;

            GameObject secondFighter = new FighterGameObject(GameObjectManager, "fighterTwo", ScreenManager.Game.Content, FighterGameObject.FighterType.Scorpion, PlayerIndex.Two);
            secondFighter.Sprite.position.X = 723;
            secondFighter.Sprite.isFlipped = true;
            GameObjectManager.addGameObject(secondFighter);

            backgroundTexture = ScreenManager.Game.Content.Load<Texture2D>("tiles/map");

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }

        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            //ScreenManager.Game.Content.Unload();
            cue.Stop(AudioStopOptions.Immediate);
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(backgroundTexture, fullscreen, Color.White);
            ScreenManager.SpriteBatch.End();


            base.Draw(gameTime);


           /* // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);*/
        }


        public override void Update(GameTime gameTime, InputState input, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, input, otherScreenHasFocus, coveredByOtherScreen);

            if (!isActive)
                return;

            PlayerIndex controllingPlayer;

            if (input.IsMenuEnter(null, out controllingPlayer)){
                ScreenManager.AddScreen(new MessageBoxScreen("     Pause Menu", this), controllingPlayer);
                this.isActive = false;
            }
        }



        #endregion
    }
}
