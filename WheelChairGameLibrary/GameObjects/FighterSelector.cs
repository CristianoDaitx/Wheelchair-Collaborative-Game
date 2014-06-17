#region Using Statements
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace MortalKomatG.GameObjects
{
    class FighterSelector : GameObject
    {

        PlayerIndex ControllingPlayer;

        private int START_X = 282;
        private int START_Y = 65;

        private int WIDTH = 115;
        private int HEIGHT = 157;

        private int MAX_X = 4;
        private int MAX_Y = 3;

        private int actualX;
        private int actualY;

        double timeToEnd = 0;
        bool isExit = false;

        bool isRight;

        FighterDisplay displayFighter;

        public FighterSelector(GameObjectManager gameObjectManager, String tag, bool isRight)
            : base(gameObjectManager, tag)
        {

            if (!isRight)
            {
                ControllingPlayer = PlayerIndex.One;
                Sprite = new Sprite(this, gameObjectManager.GameScreen.ScreenManager.Game.Content.Load<Texture2D>("tiles/player1"),
                    gameObjectManager.GameScreen.ScreenManager.WhitePixel, new Vector2(282, 65), 1);
            }
            else
            {
                Sprite = new Sprite(this, gameObjectManager.GameScreen.ScreenManager.Game.Content.Load<Texture2D>("tiles/player2"),
                    gameObjectManager.GameScreen.ScreenManager.WhitePixel, new Vector2(282, 65), 1);
                ControllingPlayer = PlayerIndex.Two;
            }

            this.isRight = isRight;

            actualX = 2;
            actualY = 2;

            displayFighter = new FighterDisplay(gameObjectManager, "displayLeft", gameObjectManager.GameScreen.ScreenManager.Game.Content, FighterGameObject.FighterType.Scorpion, PlayerIndex.One);

            if (isRight)
            {
                displayFighter.Sprite.position.X = 760;
                displayFighter.Sprite.isFlipped = true;
            }
            
            gameObjectManager.addGameObject(displayFighter);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {           
 	         base.Update(gameTime, inputState);
             PlayerIndex newPlayerIndex;

             if (isExit)
             {
                 timeToEnd += gameTime.ElapsedGameTime.TotalMilliseconds;
                 if (timeToEnd >= TimeSpan.FromSeconds(1).TotalMilliseconds)
                 {
                     GameObjectManager.GameScreen.ExitScreen();
                     GameObjectManager.GameScreen.ScreenManager.AddScreen(new FightScreen(), null);
                    
                 }


                 return;
             }


             if (inputState.IsMenuRight(ControllingPlayer))
            {
                if (actualX < MAX_X - 1)
                    actualX++;
            }

             if (inputState.IsMenuDown(ControllingPlayer))
             {
                 if (actualY < MAX_Y - 1)
                     actualY++;
             }

             if (inputState.IsMenuLeft(ControllingPlayer))
             {
                 if (actualX > 0)
                     actualX--;
             }

             if (inputState.IsMenuUp(ControllingPlayer))
             {
                 if (actualY > 0)
                     actualY--;
             }


             if (actualX == 0 && actualY == 0)
             {
                 if (displayFighter.FighterType != FighterGameObject.FighterType.LiuKang)
                 {
                     GameObjectManager.removeGameObject(displayFighter);
                     displayFighter = new FighterDisplay(GameObjectManager, "displayLeft", GameObjectManager.GameScreen.ScreenManager.Game.Content, FighterGameObject.FighterType.LiuKang, PlayerIndex.One);
                     if (isRight)
                     {
                         displayFighter.Sprite.position.X = 760;
                         displayFighter.Sprite.isFlipped = true;
                     }
                     GameObjectManager.addGameObject(displayFighter);
                 }
             } else

             if (actualX == 1 && actualY == 0)
             {
                 if (displayFighter.FighterType != FighterGameObject.FighterType.KungLao)
                 {
                     GameObjectManager.removeGameObject(displayFighter);
                     displayFighter = new FighterDisplay(GameObjectManager, "displayLeft", GameObjectManager.GameScreen.ScreenManager.Game.Content, FighterGameObject.FighterType.KungLao, PlayerIndex.One);
                     if (isRight)
                     {
                         displayFighter.Sprite.position.X = 760;
                         displayFighter.Sprite.isFlipped = true;
                     }
                     GameObjectManager.addGameObject(displayFighter);
                 }
             } else

                 if (actualX == 2 && actualY == 2)
                 {
                     if (displayFighter.FighterType != FighterGameObject.FighterType.Scorpion)
                     {
                         GameObjectManager.removeGameObject(displayFighter);
                         displayFighter = new FighterDisplay(GameObjectManager, "displayLeft", GameObjectManager.GameScreen.ScreenManager.Game.Content, FighterGameObject.FighterType.Scorpion, PlayerIndex.One);
                         if (isRight)
                         {
                             displayFighter.Sprite.position.X = 760;
                             displayFighter.Sprite.isFlipped = true;
                         }
                         GameObjectManager.addGameObject(displayFighter);
                     }
                 }
                 else
                 {
                     GameObjectManager.removeGameObject(displayFighter);
                     displayFighter.FighterType = FighterGameObject.FighterType.None;
                 }

             if (inputState.IsMenuSelect( ControllingPlayer, out newPlayerIndex))
             {
                 if (actualX == 2 && actualY == 2)
                 {
                     isExit = true;
                     ((MortalKomatG.Game)GameObjectManager.GameScreen.ScreenManager.Game).getSoundBank().GetCue("scorpion").Play();
                     
                 }
             }

             Sprite.position.X = START_X + WIDTH * actualX;
             Sprite.position.Y = START_Y + HEIGHT * actualY;
        }
    }
}
