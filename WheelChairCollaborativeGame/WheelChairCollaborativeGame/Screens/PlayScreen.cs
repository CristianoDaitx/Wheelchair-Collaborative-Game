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
        private int lastSecond2 = -1;
        private TimeSpan countdown;
        public int Score;
        public int Invaders;

        private bool timeExpired = false;
        private TimeSpan timeRanInExpiredTime = new TimeSpan();
        private TimeSpan maxTimeInExpiredTime = TimeSpan.FromSeconds(5);
        private TimeSpan explodeTimeInExpiredTime = TimeSpan.FromSeconds(3);
        private bool exploded = false;

        TankGameObject playerTank;
        SmallHuman smallHuman;

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

            playerTank = new TankGameObject(Game, "playerTank");
            Game.Components.Add(playerTank);

            KinectInput kinectInput = new KinectInput(Game, "kinectInput");
            Game.Components.Add(kinectInput);

            smallHuman = new SmallHuman(Game, "smallHuman");
            Game.Components.Add(smallHuman);

            backgroundSong = Game.Content.Load<Song>("AsteroidDance");
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            SharedSpriteBatch.Begin();
            GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                        string.Format("{0:mm\\:ss}", countdown), new Vector2(600, 30), 1.5f);
            GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                       Invaders.ToString(), new Vector2(Config.resolution.X - 60, 650), 1.5f);
            GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                       "Invaders", new Vector2(Config.resolution.X - 150, 600), 1.5f);
            GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                     Score.ToString(), new Vector2(60, 650), 1.5f);
            GUImessage.MessageDraw(SharedSpriteBatch, Game.Content,
                       "Score", new Vector2(0, 600), 1.5f);
            SharedSpriteBatch.End();
            base.Draw(gameTime);


        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            InputState inputState = (InputState)Game.Services.GetService(typeof(InputState));

            PlayerIndex playerIndex;


            //adjust representational humans
            smallHuman.representations = Invaders / 20;

            countdown = (maxTime - timeRan);

            //session time

            if (!timeExpired && timeRan > maxTime)
            {
                timeExpired = true;
                Game.Components.Add(new Shield(Game, "shield"));
                playerTank.goAway();
            }



            if (!timeExpired)
            {
                addExplosion(gameTime);
                addEnemies(gameTime);
            }
            else
            {
                timeRanInExpiredTime += gameTime.ElapsedGameTime;
                if (!exploded && timeRanInExpiredTime > explodeTimeInExpiredTime)
                {
                    //Game.ActiveScreen = new GameOverScreen(Game, "GameOverScreen");
                    exploded = true;


                    //this approach is necessary because when and enemy dies, it creates the explosion, and it causes the component list to change.
                    //a copy is made just to call the execution of the explosion
                    foreach (GameComponent component in Game.Components.Where(x => typeof(EnemyGameObject).IsAssignableFrom(x.GetType())).ToArray())
                    {
                        ((EnemyGameObject)component).die();
                    }
                    //here the actual components are marked to be removed, and a trick is used to play the explosion sound
                    foreach (GameComponent component in Game.Components.Where(x => typeof(EnemyGameObject).IsAssignableFrom(x.GetType())))
                    {
                        ((EnemyGameObject)component).ToBeRemoved = true;
                        ((EnemyGameObject)component).explosionSound.Play();
                    }
                }
                if (timeRanInExpiredTime > maxTimeInExpiredTime)
                    Game.ActiveScreen = new GameOverScreen(Game, "GameOverScreen");

            }


            if (inputState.IsKeyPressed(Keys.Escape, null, out playerIndex))
            {
                Game.ActiveScreen = new GameOverScreen(Game, "GameOverScreen");

            }
        }

        /// <summary>
        /// scripted add of enemies
        /// </summary>
        private void addEnemies(GameTime gameTime)
        {
            timeRan += gameTime.ElapsedGameTime;

            if (timeRan.Seconds != lastSecond) // the second has changed
            {
                if (timeRan.Seconds == 0)
                {

                    Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Right));
                }

                if (timeRan.Seconds == 1)
                {
                    Game.Components.Add(new HumanCharacter("Look!\nBaby aliens!", Game, "HumanCharacter"));
                }

                if (timeRan.Seconds == 2)
                {

                    Game.Components.Add(new WeakEnemy(Game, "weakEnemy2", WeakEnemy.Type.Left));
                    Game.Components.Add(new WierdEnemy(Game, "wierdEnemy"));
                }

                if (timeRan.Seconds == 5)
                {
                    Game.Components.Add(new AvarageEnemy(Game, "avarageEnemy"));
                    Game.Components.Add(new AlienCharacter("Lets defend\nwhile the shields is\nnot fixed!", Game, "AlienCharacter"));
                }

                if (timeRan.Seconds == 20)
                {
                    Game.Components.Add(new HardEnemy(Game, "hardEnemy"));
                    Game.Components.Add(new HumanCharacter("We will study\nyour planet!", Game, "HumanCharacter"));
                }

                if (timeRan.Seconds == 28)
                {
                    Game.Components.Add(new WeakEnemy(Game, "weakEnemy2", WeakEnemy.Type.Left));
                    Game.Components.Add(new WierdEnemy(Game, "wierdEnemy"));
                }

                if (timeRan.Seconds == 29)
                {
                    Game.Components.Add(new AlienCharacter("Shields will be\nup in 1:30!", Game, "AlienCharacter"));
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
                    Game.Components.Add(new HumanCharacter("Keep going!", Game, "HumanCharacter"));
                }
                if (timeRan.Seconds == 40)
                {
                    Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Left));
                }
                if (timeRan.Seconds == 41)
                {
                    Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Left));
                }

                if (timeRan.Seconds == 53)
                {
                    Game.Components.Add(new HardEnemy(Game, "hardEnemy"));
                }


                lastSecond = timeRan.Seconds;

            }
        }

        private void addExplosion(GameTime gameTime)
        {
            

            if (timeRan.Seconds != lastSecond2) // the second has changed
            {

                if (timeRan.Seconds == 20)
                {
                    if(Invaders > 50)
                        Game.Components.Add(new BigExplosionGameObject(new Vector2(550,650), Game, 2));
                }
                
                if (timeRan.Seconds == 35)
                {
                    if(Invaders > 100){
                        Game.Components.Add(new BigExplosionGameObject(new Vector2(500,680), Game, 2));
                    }
                }
                if (timeRan.Seconds == 38)
                {
                    if (Invaders > 100)
                    {
                        Game.Components.Add(new BigExplosionGameObject(new Vector2(500, 680), Game, 2));
                    }
                }
               
                if (timeRan.Seconds == 55)
                {
                    if (Invaders > 150)
                    {
                        Game.Components.Add(new BigExplosionGameObject(new Vector2(550, 660), Game, 2));
                        Game.Components.Add(new BigExplosionGameObject(new Vector2(500, 680), Game, 2));
                        Game.Components.Add(new BigExplosionGameObject(new Vector2(600, 675), Game, 2));
                        Game.Components.Add(new BigExplosionGameObject(new Vector2(450, 680), Game, 2));
                    }
                }
                /*
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
                   Game.Components.Add(new HumanCharacter("Keep going!", Game, "HumanCharacter"));
               }
               if (timeRan.Seconds == 40)
               {
                   Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Left));
               }
               if (timeRan.Seconds == 41)
               {
                   Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Left));
               }

               if (timeRan.Seconds == 53)
               {
                   Game.Components.Add(new HardEnemy(Game, "hardEnemy"));
               }
              */

                lastSecond2 = timeRan.Seconds;

            }
        }

        public override void ExitScreen()
        {
            Game.RemoveAllButEssentialComponents();
            MediaPlayer.Stop();
        }
    }
}
