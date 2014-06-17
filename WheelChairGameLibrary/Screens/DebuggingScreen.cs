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
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class DebuggingScreen : MenuScreen
    {
        #region Fields

        //Sprite spritePlayer;
        FighterGameObject playerFighter;

        #endregion

        #region Initialization

        public DebuggingScreen()
            : base("")
        {
            //TransitionOnTime = TimeSpan.FromSeconds(1.5);
            //TransitionOffTime = TimeSpan.FromSeconds(0.5);

            MenuEntry victoryPose = new MenuEntry(this, "Victory Pose");
            MenuEntry win = new MenuEntry(this, "Win");
            MenuEntry hitLow = new MenuEntry(this, "Hit Low");
            MenuEntry hitHigh = new MenuEntry(this, "Hit High");
            MenuEntry hitWalk = new MenuEntry(this, "Hit Walk");
            MenuEntry fallBackward = new MenuEntry(this, "Fall Backward");
            MenuEntry dead = new MenuEntry(this, "Dead");
            MenuEntry fightingStanceMenuEntry = new MenuEntry(this, "Fighting Stance");
            MenuEntry dizzysMenuEntry = new MenuEntry(this, "Dizzy");
            MenuEntry highPunchMenuEntry = new MenuEntry(this, "High Punch");
            MenuEntry lowPunchMenuEntry = new MenuEntry(this, "Low Punch");
            MenuEntry kickHigh = new MenuEntry(this, "Kick High");
            MenuEntry kickLow = new MenuEntry(this, "Kick Low");
            MenuEntry walking = new MenuEntry(this, "Walking");


            victoryPose.Selected += new EventHandler<PlayerIndexEventArgs>(victoryPose_Selected);
            win.Selected += new EventHandler<PlayerIndexEventArgs>(win_Selected);
            hitLow.Selected += new EventHandler<PlayerIndexEventArgs>(hitLow_Selected);
            hitHigh.Selected += new EventHandler<PlayerIndexEventArgs>(hitHigh_Selected);
            hitWalk.Selected += new EventHandler<PlayerIndexEventArgs>(hitWalk_Selected);
            fallBackward.Selected += new EventHandler<PlayerIndexEventArgs>(fallBackward_Selected);
            dead.Selected += new EventHandler<PlayerIndexEventArgs>(dead_Selected);
            fightingStanceMenuEntry.Selected += new EventHandler<PlayerIndexEventArgs>(fightingStanceMenuEntry_Selected);
            dizzysMenuEntry.Selected += new EventHandler<PlayerIndexEventArgs>(dizzysMenuEntry_Selected);
            highPunchMenuEntry.Selected += new EventHandler<PlayerIndexEventArgs>(highPunchMenuEntry_Selected);
            lowPunchMenuEntry.Selected += new EventHandler<PlayerIndexEventArgs>(lowPunchMenuEntry_Selected);
            kickHigh.Selected += new EventHandler<PlayerIndexEventArgs>(kickHigh_Selected);
            kickLow.Selected += new EventHandler<PlayerIndexEventArgs>(kickLow_Selected);
            walking.Selected += new EventHandler<PlayerIndexEventArgs>(walking_Selected);

            // Add entries to the menu.
            MenuEntries.Add(fightingStanceMenuEntry);
            MenuEntries.Add(victoryPose);
            MenuEntries.Add(win);
            MenuEntries.Add(hitLow);
            MenuEntries.Add(hitHigh);
            MenuEntries.Add(hitWalk);
            MenuEntries.Add(fallBackward);
            MenuEntries.Add(dead);            
            MenuEntries.Add(dizzysMenuEntry);
            MenuEntries.Add(highPunchMenuEntry);
            MenuEntries.Add(lowPunchMenuEntry);
            MenuEntries.Add(kickHigh);
            MenuEntries.Add(kickLow);
            MenuEntries.Add(walking);
        }

        void win_Selected(object sender, PlayerIndexEventArgs e)
        {
            playerFighter.setStance(Stances.Stance.Win);
        }

        void dead_Selected(object sender, PlayerIndexEventArgs e)
        {
            playerFighter.setStance(Stances.Stance.Dead);
        }

        void fallBackward_Selected(object sender, PlayerIndexEventArgs e)
        {
            playerFighter.setStance(Stances.Stance.FallBackward);
        }

        void hitWalk_Selected(object sender, PlayerIndexEventArgs e)
        {
            playerFighter.setStance(Stances.Stance.HitWalk);
        }

        void hitHigh_Selected(object sender, PlayerIndexEventArgs e)
        {
            playerFighter.setStance(Stances.Stance.HitHigh);
        }

        void hitLow_Selected(object sender, PlayerIndexEventArgs e)
        {
            playerFighter.setStance(Stances.Stance.HitLow);
        }

        void victoryPose_Selected(object sender, PlayerIndexEventArgs e)
        {
            playerFighter.setStance(Stances.Stance.VictoryPose);
        }

        void walking_Selected(object sender, PlayerIndexEventArgs e)
        {
            playerFighter.setStance(Stances.Stance.WalkingForward);
        }

        void kickLow_Selected(object sender, PlayerIndexEventArgs e)
        {
            playerFighter.setStance(Stances.Stance.KickLow);
        }

        void kickHigh_Selected(object sender, PlayerIndexEventArgs e)
        {
            playerFighter.setStance(Stances.Stance.KickHigh);
        }

        void lowPunchMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            playerFighter.setStance(Stances.Stance.PunchLow1);
        }

        void highPunchMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            playerFighter.setStance(Stances.Stance.PunchHigh1);
        }

        void dizzysMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            playerFighter.setStance(Stances.Stance.Dizzy);
        }

        void fightingStanceMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            playerFighter.setStance(Stances.Stance.FightingStance);
        }


        public override void LoadContent()
        {
            playerFighter = new FighterGameObject(GameObjectManager, "fighterOne", ScreenManager.Game.Content, FighterGameObject.FighterType.KungLao, PlayerIndex.One);
            GameObjectManager.addGameObject(playerFighter);

            ScreenManager.Game.ResetElapsedTime();
        }

        public override void UnloadContent()
        {
            ScreenManager.Game.Content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, InputState input, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, input, otherScreenHasFocus, coveredByOtherScreen);

            if (IsActive)
            {

                // TODO: this game isn't very fun! You could probably improve
                // it by inserting something more interesting in this space :-)
            }
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            //int playerIndex = (int)ControllingPlayer.Value;
            int playerIndex = 0;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            PlayerIndex newPlayerIndex;

            if (input.IsKeyPressed(Keys.Space, ControllingPlayer, out newPlayerIndex))
            {
                playerFighter.Sprite.ActiveSpriteAnimation.setIsActive(!playerFighter.Sprite.ActiveSpriteAnimation.getIsActive());
            }

            if (input.IsKeyPressed(Keys.W, ControllingPlayer, out newPlayerIndex))
            {
                playerFighter.Sprite.ActiveSpriteAnimation.increaseSpriteAnimationOffsetY(1);
            }
            if (input.IsKeyPressed(Keys.D, ControllingPlayer, out newPlayerIndex))
            {
                playerFighter.Sprite.ActiveSpriteAnimation.increaseSpriteAnimationOffsetX(1);
            }
            if (input.IsKeyPressed(Keys.S, ControllingPlayer, out newPlayerIndex))
            {
                playerFighter.Sprite.ActiveSpriteAnimation.increaseSpriteAnimationOffsetY(-1);
            }
            if (input.IsKeyPressed(Keys.A, ControllingPlayer, out newPlayerIndex))
            {
                playerFighter.Sprite.ActiveSpriteAnimation.increaseSpriteAnimationOffsetX(-1);
            }

            if (input.IsKeyPressed(Keys.Right, ControllingPlayer, out newPlayerIndex))
            {
                playerFighter.Sprite.nextState();
            }

            if (input.IsKeyPressed(Keys.Left, ControllingPlayer, out newPlayerIndex))
            {
                playerFighter.Sprite.previousState();
            }

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);


            Vector2 position = new Vector2(700, 50);
            // Draw each menu entry in turn.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.Draw(this, position, isSelected, gameTime, 0.5f);

                position.Y += menuEntry.GetHeight(this);
            }

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);
        }


        #endregion
    }
}
