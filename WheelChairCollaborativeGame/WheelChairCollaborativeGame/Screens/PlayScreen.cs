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

namespace WheelChairCollaborativeGame
{
    class PlayScreen : Screen
    {
        private Song backgroundSong;

        private TimeSpan timeRan;
        private TimeSpan maxTime = TimeSpan.FromSeconds(120);
        private int lastSecond = -1;
        private TimeSpan countdown;

        public PlayScreen(GameEnhanced game, string tag)
            : base(game, tag)
        {
            DrawOrder++;
            timeRan = new TimeSpan();
        }

        public override void Initialize()
        {
            base.Initialize();
            MediaPlayer.Play(backgroundSong);
            MediaPlayer.IsRepeating = true;
            
        }

        protected override void LoadContent()
        {
            Background background = new Background(Game, 100);
            Game.Components.Add(background);

            Game.Components.Add(new Planet(Game, "Planet"));

            TankGameObject playerTank = new TankGameObject(Game, "playerTank");
            Game.Components.Add(playerTank);

            KinectInput kinectInput = new KinectInput(Game, "kinectInput");
            Game.Components.Add(kinectInput);       

            HumanCharacter humanCharacter = new HumanCharacter(Game, "HumanCharacter");
            Game.Components.Add(humanCharacter);
            AlienCharacter alienCharacter = new AlienCharacter(Game, "AlienCharacter");
            Game.Components.Add(alienCharacter);

            backgroundSong = Game.Content.Load<Song>("AsteroidDance");
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            SharedSpriteBatch.Begin();
            GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                        string.Format("{0:mm\\:ss}", countdown), new Vector2(600, 30), 1.5f);

            SharedSpriteBatch.End();
            base.Draw(gameTime);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            InputState inputState = (InputState)Game.Services.GetService(typeof(InputState));

            PlayerIndex playerIndex;

            timeRan += gameTime.ElapsedGameTime;
            countdown = (maxTime - timeRan);

            //session time

            if (timeRan > maxTime)
            {
                Game.ActiveScreen = new GameOverScreen(Game, "GameOverScreen");
                return;
            }

            addEnemies();


            if (inputState.IsKeyPressed(Keys.Escape, null, out playerIndex))
            {
                Game.ActiveScreen = new GameOverScreen(Game, "GameOverScreen");

            }
        }

        /// <summary>
        /// scripted add of enemies
        /// </summary>
        private void addEnemies()
        {
            if (timeRan.Seconds != lastSecond) // the second has changed
            {
                if (timeRan.Seconds == 0)
                {
                    Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Right));
                }

                if (timeRan.Seconds == 2)
                {
                    Game.Components.Add(new WeakEnemy(Game, "weakEnemy2", WeakEnemy.Type.Left));
                    Game.Components.Add(new WierdEnemy(Game, "wierdEnemy"));
                }

                if (timeRan.Seconds == 5)
                {
                    Game.Components.Add(new AvarageEnemy(Game, "avarageEnemy"));
                }

                if (timeRan.Seconds == 20)
                {
                    Game.Components.Add(new HardEnemy(Game, "hardEnemy"));
                }

                if (timeRan.Seconds == 28)
                {
                    Game.Components.Add(new WeakEnemy(Game, "weakEnemy2", WeakEnemy.Type.Left));
                    Game.Components.Add(new WierdEnemy(Game, "wierdEnemy"));
                }

                if (timeRan.Seconds == 30)
                {
                    Game.Components.Add(new WeakEnemy(Game, "weakEnemy2", WeakEnemy.Type.Right));
                }

                if (timeRan.Seconds == 31)
                {
                    Game.Components.Add(new WeakEnemy(Game, "weakEnemy2", WeakEnemy.Type.Left));
                }

                if (timeRan.Seconds == 35)
                {
                    Game.Components.Add(new AvarageEnemy(Game, "avarageEnemy"));
                }

                if (timeRan.Seconds == 36)
                {
                    Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Right));
                }

                if (timeRan.Seconds == 37)
                {
                    Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Right));
                }
                if (timeRan.Seconds == 38)
                {
                    Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Right));
                }

                if (timeRan.Seconds == 39)
                {
                    Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Left));
                }
                if (timeRan.Seconds == 40)
                {
                    Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Left));
                }
                if (timeRan.Seconds == 41)
                {
                    Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Left));
                }

                if (timeRan.Seconds == 50)
                {
                    Game.Components.Add(new HardEnemy(Game, "hardEnemy"));
                }

                lastSecond = timeRan.Seconds;

            }
        }

        public override void ExitScreen()
        {
            Game.RemoveAllButEssentialComponents();
            MediaPlayer.Stop();
        }
    }
}
