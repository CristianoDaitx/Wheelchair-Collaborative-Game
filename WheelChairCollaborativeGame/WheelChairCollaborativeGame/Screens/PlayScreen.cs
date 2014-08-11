﻿using System;
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
        private Random rnd= new Random();
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
            MediaPlayer.IsRepeating = false;

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

            backgroundSong = Game.Content.Load<Song>("Tyrian - 02 - Asteroid Dance Part 1");
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            SharedSpriteBatch.Begin();
            GUImessage.DrawString(SharedSpriteBatch, Game.Content,
                        string.Format("{0:mm\\:ss}", countdown), new Rectangle(600, 30, 100, 20), GUImessage.Alignment.Center, Color.White);
            
            GUImessage.DrawString(SharedSpriteBatch, Game.Content,Invaders.ToString(), new Rectangle((int)Config.resolution.X - 200, 600, 300, 18), GUImessage.Alignment.Left, Color.White);
           
            GUImessage.DrawString(SharedSpriteBatch, Game.Content, "Invaders", new Rectangle((int)Config.resolution.X - 200, 560, 300, 18), GUImessage.Alignment.Left, Color.White);

            GUImessage.DrawString(SharedSpriteBatch, Game.Content, Score.ToString(), new Rectangle(0, 600, 200, 18), GUImessage.Alignment.Right, Color.White);
            
            GUImessage.DrawString(SharedSpriteBatch, Game.Content, "Score", new Rectangle(0, 560, 200, 18), GUImessage.Alignment.Right, Color.White);
            SharedSpriteBatch.End();
            base.Draw(gameTime);


        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            InputState inputState = (InputState)Game.Services.GetService(typeof(InputState));

            PlayerIndex playerIndex;


            //adjust representational humans
            smallHuman.representations = Invaders / 40;

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
                {
                    Game.ActiveScreen = new GameOverScreen(Game, "GameOverScreen");
                    GameOverScreen gameOverScreen = (GameOverScreen)Game.Components.FirstOrDefault(x => x.GetType() == typeof(GameOverScreen));
                    if (gameOverScreen != null)
                        gameOverScreen.Score = Score;
                }
                   

            }


            if (inputState.IsKeyPressed(Keys.Escape, null, out playerIndex))
            {
                {
                    Game.ActiveScreen = new GameOverScreen(Game, "GameOverScreen");
                    GameOverScreen gameOverScreen = (GameOverScreen)Game.Components.FirstOrDefault(x => x.GetType() == typeof(GameOverScreen));
                    if (gameOverScreen != null)
                        gameOverScreen.Score = Score;
                }
                   

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
                if (timeRan.Minutes == 1)
                    if (timeRan.Seconds == 3)
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Right));

                if (timeRan.Seconds == 0 && timeRan.Minutes == 0)
                {
                    Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Right));
                }

                if (timeRan.Seconds == 1)
                {
                    if(timeRan.Minutes == 0)
                        Game.Components.Add(new HumanCharacter("Look!\nBaby, aliens!", Game, "HumanCharacter"));
                    else
                        Game.Components.Add(new HumanCharacter("Look!\nThey have gold!", Game, "HumanCharacter"));

                }

                if (timeRan.Seconds == 2 && timeRan.Minutes == 0)
                {

                    Game.Components.Add(new WeakEnemy(Game, "weakEnemy2", WeakEnemy.Type.Left));
                    Game.Components.Add(new WierdEnemy(Game, "wierdEnemy"));
                }

                if (timeRan.Seconds == 5)
                {
                    Game.Components.Add(new AvarageEnemy(Game, "avarageEnemy"));
                    if (timeRan.Minutes == 0)
                        Game.Components.Add(new AlienCharacter("Lets defend\nwhile the shields is\nnot fixed!", Game, "AlienCharacter"));
                    else
                        Game.Components.Add(new AlienCharacter("The shield is \nalmost fixed, \nhang in there!", Game, "AlienCharacter"));
                }

                if (timeRan.Seconds == 20)
                {
                    Game.Components.Add(new HardEnemy(Game, "hardEnemy"));
                    if (timeRan.Minutes == 0)
                        Game.Components.Add(new HumanCharacter("We will study\nyour planet!", Game, "HumanCharacter"));
                    else
                        Game.Components.Add(new HumanCharacter("So much to explore!", Game, "HumanCharacter"));
                }

                if (timeRan.Seconds == 30)
                {
                    Game.Components.Add(new WeakEnemy(Game, "weakEnemy2", WeakEnemy.Type.Left));
                    Game.Components.Add(new WierdEnemy(Game, "wierdEnemy"));
                }

                if (timeRan.Seconds == 30)
                {
                    if (timeRan.Minutes == 0)
                    Game.Components.Add(new AlienCharacter("Shields will be\nup in 1:30!", Game, "AlienCharacter"));
                    else
                        Game.Components.Add(new AlienCharacter("Shields will be\nup in 30 seconds!", Game, "AlienCharacter"));
                }

                if (timeRan.Seconds == 32)
                {
                    Game.Components.Add(new WeakEnemy(Game, "weakEnemy2", WeakEnemy.Type.Right));
                }

                if (timeRan.Seconds == 33)
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

                if (timeRan.Seconds == 54)
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

                if (timeRan.Seconds % 20 == 0)
                {
                    
                    if (Invaders > 50)
                        Game.Components.Add(new BigExplosionGameObject(new Vector2(rnd.Next(400,800),rnd.Next(650,750)), Game, 2));
                }



                if (timeRan.Seconds % 10 == 0)
                {
                    
                    if(Invaders > 150)
                        Game.Components.Add(new BigExplosionGameObject(new Vector2(rnd.Next(400, 800), rnd.Next(650, 750)), Game, 2));
                }

                if (timeRan.Seconds % 5 == 0)
                {
                    
                    if (Invaders > 250)
                        Game.Components.Add(new BigExplosionGameObject(new Vector2(rnd.Next(400, 800), rnd.Next(650, 750)), Game, 2));
                }

                if (timeRan.Seconds % 2 == 0)
                {
                   
                    if (Invaders > 350)
                        Game.Components.Add(new BigExplosionGameObject(new Vector2(rnd.Next(400, 800), rnd.Next(650, 750)), Game, 2));
                }

                if (timeRan.Seconds % 2 == 0)
                {
                    if (Invaders > 500)
                    {
                        Game.Components.Add(new BigExplosionGameObject(new Vector2(rnd.Next(400, 750), rnd.Next(650, 750)), Game, 2));
                        Game.Components.Add(new BigExplosionGameObject(new Vector2(rnd.Next(400, 750), rnd.Next(650, 750)), Game, 2));
                    }
                }

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
