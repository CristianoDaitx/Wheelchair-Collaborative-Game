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

        private TimeSpan timeRan = new TimeSpan();//TimeSpan.FromSeconds(90);
        private TimeSpan maxTime = TimeSpan.FromSeconds(120);
        private int lastSecond = -1;
        private int lastSecond2 = -1;
        private Random rnd = new Random();
        private TimeSpan countdown;
        public int Score;
        public int Invaders;

        private bool hasStarted = false;
        private TimeSpan timeRanInPreGame = new TimeSpan();
        private TimeSpan maxTimeInPreGame = TimeSpan.FromSeconds(8);

        private bool timeExpired = false;
        private TimeSpan timeRanInExpiredTime = new TimeSpan();
        private TimeSpan maxTimeInExpiredTime = TimeSpan.FromSeconds(5);
        private TimeSpan explodeTimeInExpiredTime = TimeSpan.FromSeconds(3);
        private bool exploded = false;

        TankGameObject playerTank;
        SmallHuman smallHuman;

        Background background;
        Planet planet;
        Shield shield;

        public PlayScreen(GameEnhanced game, string tag)
            : base(game, tag)
        {
            DrawOrder++;
        }

        public override void Initialize()
        {
            base.Initialize();
            MediaPlayer.Play(backgroundSong);
            MediaPlayer.IsRepeating = false;

        }

        protected override void LoadContent()
        {
            background = new Background(Game, 100);
            Game.Components.Add(background);

            planet = new Planet(Game, "Planet");
            Game.Components.Add(planet);

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

            GUImessage.DrawString(SharedSpriteBatch, Game.Content, Invaders.ToString(), new Rectangle((int)Config.resolution.X - 200, 600, 300, 18), GUImessage.Alignment.Left, Color.White);

            GUImessage.DrawString(SharedSpriteBatch, Game.Content, "Invaders", new Rectangle((int)Config.resolution.X - 200, 560, 300, 18), GUImessage.Alignment.Left, Color.White);

            GUImessage.DrawString(SharedSpriteBatch, Game.Content, Score.ToString(), new Rectangle(0, 600, 200, 18), GUImessage.Alignment.Right, Color.White);

            GUImessage.DrawString(SharedSpriteBatch, Game.Content, "Score", new Rectangle(0, 560, 200, 18), GUImessage.Alignment.Right, Color.White);



            if (!hasStarted)
            {
                if (timeRanInPreGame > maxTimeInPreGame)
                    GUImessage.MessageDraw(SharedSpriteBatch, Game.Content, "             Can't shot to start?"+
                        "\nTry the tutorial from the main menu!", new Vector2((Config.resolution.X / 2 - 200), Config.resolution.Y / 2 - 20), 1f);
                else
                    GUImessage.MessageDraw(SharedSpriteBatch, Game.Content, "Shot to start!", new Vector2((Config.resolution.X / 2 - 70), Config.resolution.Y / 2 - 20), 1f);
            }



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

            if (!hasStarted)
                timeRanInPreGame += gameTime.ElapsedGameTime;

            //session time

            if (!timeExpired && timeRan > maxTime)
            {
                timeExpired = true;
                shield = new Shield(Game, "Shield");
                Game.Components.Add(shield);
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
                    GameOverScreen gameOverScreen = new GameOverScreen(Game, "GameOverScreen");
                    planet.stopMoving();
                    Game.ActiveScreen = gameOverScreen;
                    gameOverScreen.Score = Score;
                }


            }


            if (inputState.IsKeyPressed(Keys.Escape, null, out playerIndex))
            {
                {
                    Game.ActiveScreen = new MainMenuScreen(Game, "MainMenuScreen");
                }
            }
        }

        public void playerShot()
        {
            if (hasStarted == false)
            {
                hasStarted = true;
                planet.startMoving();
            }
        }


        /// <summary>
        /// scripted add of enemies
        /// </summary>
        private void addEnemies(GameTime gameTime)
        {
            if (hasStarted)
                timeRan += gameTime.ElapsedGameTime;

            if (hasStarted && (int)timeRan.TotalSeconds != lastSecond) // the second has changed
            {
                switch ((int)timeRan.TotalSeconds)
                {
                    case 0:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Right));
                        break;
                    case 1:
                        Game.Components.Add(new HumanCharacter("Look!\nBaby, aliens!", Game, "HumanCharacter"));
                        break;
                    case 5:
                        Game.Components.Add(new AvarageEnemy(Game, "avarageEnemy", AvarageEnemy.Type.Left));
                        Game.Components.Add(new AlienCharacter("Lets defend\nwhile the shields is\nnot fixed!", Game, "AlienCharacter"));
                        break;
                    case 9:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy2", WeakEnemy.Type.Left));
                        break;
                    case 14:
                        Game.Components.Add(new WierdEnemy(Game, "wierdEnemy"));
                        break;
                    case 20:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy2", WeakEnemy.Type.Left));
                        Game.Components.Add(new HumanCharacter("We will study\nyour planet!", Game, "HumanCharacter"));
                        break;
                    case 24:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy2", WeakEnemy.Type.Right));
                        break;
                    case 25:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy2", WeakEnemy.Type.Right));
                        break;
                    case 28:
                        Game.Components.Add(new AlienCharacter("Shields will be\nup in 1:30!", Game, "AlienCharacter"));
                        break;
                    case 30:
                        Game.Components.Add(new WierdEnemy(Game, "wierdEnemy"));
                        break;
                    case 34:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy2", WeakEnemy.Type.Left));
                        break;
                    case 37:
                        Game.Components.Add(new AvarageEnemy(Game, "avarageEnemy", AvarageEnemy.Type.Left));
                        break;
                    case 39:
                        Game.Components.Add(new HumanCharacter("So much to explore!", Game, "HumanCharacter"));
                        break;
                    case 40:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Right));
                        break;
                    case 41:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Right));
                        break;
                    case 45:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Left));
                        break;
                    case 46:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Left));
                        break;
                    case 51:
                        Game.Components.Add(new AvarageEnemy(Game, "avarageEnemy", AvarageEnemy.Type.Left));
                        break;
                    case 53:
                        Game.Components.Add(new AvarageEnemy(Game, "avarageEnemy", AvarageEnemy.Type.Right));
                        break;
                    case 56:
                        Game.Components.Add(new WierdEnemy(Game, "wierdEnemy"));
                        break;
                    case 60:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Right));
                        break;
                    case 61:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Right));
                        break;
                    case 62:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Right));
                        break;
                    case 63:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Right));
                        break;
                    case 65:
                        Game.Components.Add(new HumanCharacter("Look!\nThey have gold!", Game, "HumanCharacter"));
                        Game.Components.Add(new AvarageEnemy(Game, "avarageEnemy", AvarageEnemy.Type.Left));
                        break;
                    case 68:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Left));
                        break;
                    case 69:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Left));
                        break;
                    case 70:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Left));
                        break;
                    case 71:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Left));
                        break;
                    case 75:
                        Game.Components.Add(new WierdEnemy(Game, "wierdEnemy"));
                        break;
                    case 80:
                        Game.Components.Add(new WierdEnemy(Game, "wierdEnemy"));
                        break;
                    case 84:
                        Game.Components.Add(new AvarageEnemy(Game, "avarageEnemy", AvarageEnemy.Type.Right));
                        break;
                    case 86:
                        Game.Components.Add(new AvarageEnemy(Game, "avarageEnemy", AvarageEnemy.Type.Left));
                        break;
                    case 89:
                        Game.Components.Add(new AlienCharacter("Shields will be\nup in 30 seconds!", Game, "AlienCharacter"));
                        break;
                    case 90:
                        Game.Components.Add(new WierdEnemy(Game, "wierdEnemy"));
                        break;
                    case 93:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Left));
                        break;
                    case 94:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Left));
                        break;
                    case 97:
                        Game.Components.Add(new AvarageEnemy(Game, "avarageEnemy", AvarageEnemy.Type.Left));
                        break;
                    case 99:
                        Game.Components.Add(new AlienCharacter("The shield is \nalmost fixed, \nhang in there!", Game, "AlienCharacter"));
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Right));
                        break;
                    case 101:
                        Game.Components.Add(new AvarageEnemy(Game, "avarageEnemy", AvarageEnemy.Type.Right));
                        break;
                    case 104:
                        Game.Components.Add(new HardEnemy(Game, "hardEnemy"));
                        Game.Components.Add(new HumanCharacter("Keep going!", Game, "HumanCharacter"));
                        break;
                    case 115:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Left));
                        break;
                    case 116:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Right));
                        break;
                    case 117:
                        Game.Components.Add(new WeakEnemy(Game, "weakEnemy", WeakEnemy.Type.Right));
                        break;
                }
                lastSecond = (int)timeRan.TotalSeconds;

            }
        }

        private void addExplosion(GameTime gameTime)
        {


            if (timeRan.Seconds != lastSecond2) // the second has changed
            {

                if (timeRan.Seconds % 20 == 0)
                {

                    if (Invaders > 50)
                        Game.Components.Add(new BigExplosionGameObject(new Vector2(rnd.Next(400, 800), rnd.Next(650, 750)), Game, 2));
                }



                if (timeRan.Seconds % 10 == 0)
                {

                    if (Invaders > 150)
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
            //if terminated normaly
            if (timeRanInExpiredTime > maxTimeInExpiredTime)
                Game.RemoveAllButEssentialComponents(new List<IGameComponent>() { planet, shield, background });
            else
                Game.RemoveAllButEssentialComponents();
            MediaPlayer.Stop();
        }
    }
}
