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
    class TutorialScreen : Screen
    {
        private Song backgroundSong;

        private bool isTutorialStarted = false;
        private bool isTutorialEnded = false;

        private int maxEnemies = 5;
        WeakEnemy weakEnemy;
        FrontMovementSprite movementSprite;

        private TimeSpan timeRan;
        private TimeSpan timeRanEnded;
        private TimeSpan maxTime = TimeSpan.FromSeconds(6);

        TankGameObject playerTank;

        public TutorialScreen(GameEnhanced game, string tag)
            : base(game, tag)
        {
            DrawOrder++;
            timeRan = new TimeSpan();
            timeRanEnded = new TimeSpan();
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

            playerTank = new TankGameObject(Game, "playerTank");
            Game.Components.Add(playerTank);

            KinectInput kinectInput = new KinectInput(Game, "kinectInput");
            Game.Components.Add(kinectInput);


            backgroundSong = Game.Content.Load<Song>("AsteroidDance");

            movementSprite = new FrontMovementSprite(Game, "FrontMovementSprite");

            weakEnemy = new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Right);
            weakEnemy.DiedCompleted += new EnemyGameObject.DiedEventHandler(weakEnemy_DiedCompleted);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            SharedSpriteBatch.Begin();
            if (isTutorialEnded)
            {
                GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                            "Great!, You are now prepared to\n     stop the human invasion!", new Vector2(450, 300));
            }
            else if (isTutorialStarted)
            {
                GUImessage.DrawString(SharedSpriteBatch, Game.Content,
                            "Enemies: " + maxEnemies.ToString(), new Rectangle(600, 30, 100, 20), GUImessage.Alignment.Center, Color.White);
            }
            else
            {
                GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                           "Shoot the humans to prevent them\n    from invading your planet!\n\n"+
                           " Do not forget to keep an eye in\n         your energy bar!", new Vector2(450, 300));
            }
            SharedSpriteBatch.End();
            base.Draw(gameTime);


        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            InputState inputState = (InputState)Game.Services.GetService(typeof(InputState));

            PlayerIndex playerIndex;

            //capability to add enemies:
            if (inputState.IsKeyPressed(Keys.A, null, out playerIndex))
                maxEnemies++;

            if (!isTutorialEnded)
            {
                timeRan += gameTime.ElapsedGameTime;


                if (!isTutorialStarted && timeRan.Seconds >= maxTime.Seconds)
                {
                    isTutorialStarted = true;
                    Game.Components.Add(weakEnemy);
                    Game.Components.Add(movementSprite);
                }


                if (inputState.IsKeyPressed(Keys.Enter, null, out playerIndex))
                {
                    timeRan += maxTime;
                }
            }
            else
            {
                timeRanEnded += gameTime.ElapsedGameTime;
                if (inputState.IsKeyPressed(Keys.Enter, null, out playerIndex))
                {
                    Game.ActiveScreen = new MainMenuScreen(Game, "MainMenuScreen");
                }
                if (timeRanEnded.Seconds >= maxTime.Seconds)
                {
                    Game.ActiveScreen = new MainMenuScreen(Game, "MainMenuScreen");
                }
            }

            if (inputState.IsKeyPressed(Keys.Escape, null, out playerIndex))
            {
                Game.ActiveScreen = new MainMenuScreen(Game, "MainMenuScreen");
            }
        }

        void weakEnemy_DiedCompleted(object sender, EnemyGameObject.EnemyGameObjectEventArgs e)
        {
            if (e.wasShot)
                maxEnemies--;

            if (maxEnemies > -0)
            {
                WeakEnemy weakEnemy = new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Right);
                weakEnemy.DiedCompleted += new EnemyGameObject.DiedEventHandler(weakEnemy_DiedCompleted);
                Game.Components.Add(weakEnemy);
            }
            else
            {
                isTutorialEnded = true;
                Game.Components.Remove(movementSprite);
            }
        }

        public override void ExitScreen()
        {
            Game.RemoveAllButEssentialComponents();
            MediaPlayer.Stop();
        }
    }
}
