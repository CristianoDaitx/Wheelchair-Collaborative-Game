using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WheelChairGameLibrary;
using WheelChairGameLibrary.Helpers;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WheelChairCollaborativeGame
{
    class PlayScreen : Screen
    {
        private Song backgroundSong;

        private bool gameOver = false;
        private TimeSpan timeRan;
        private TimeSpan maxTime = TimeSpan.FromMilliseconds(120000);
        private int lastSecond = -1;
        private TimeSpan countdown;

        public PlayScreen(GameEnhanced game, string tag)
            : base(game, tag)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            MediaPlayer.Play(backgroundSong);
            MediaPlayer.IsRepeating = true;
            
        }

        protected override void LoadContent()
        {
            TankGameObject playerTank = new TankGameObject(Game, "playerTank");
            Game.Components.Add(playerTank);

            KinectInput kinectInput = new KinectInput(Game, "kinectInput");
            Game.Components.Add(kinectInput);



            Background background = new Background(Game, 100);
            Game.Components.Add(background);

            backgroundSong = Game.Content.Load<Song>("AsteroidDance");
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            SharedSpriteBatch.Begin();
            GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,

                        "Timer: " + string.Format("{0:mm\\:ss}", countdown), new Vector2(30, 300));

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

            if (timeRan > maxTime && !gameOver)
            {
                gameOver = true;
                Console.WriteLine("Game Over!");

                return;
                // Change state
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
