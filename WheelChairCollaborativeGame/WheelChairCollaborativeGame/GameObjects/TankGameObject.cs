#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using WheelChairGameLibrary;
using WheelChairGameLibrary.Helpers;

using WheelChairGameLibrary.Sprites;

using KinectForWheelchair;
using KinectForWheelchair.Listeners;

using Microsoft.Kinect;

#endregion
namespace WheelChairCollaborativeGame
{
    class TankGameObject : GameObject2D
    {
        private readonly int POSITION_ENERGY_X = 30;
        private readonly int POSITION_ENERGY_Y = 650;
        private readonly int HEIGHT_ENERGY_X = 30;
        private readonly int WIDTH_ENERGY_X = 250;
        private int transparencyVal;
        private int r = 0;
        private int g = 0;
        private int b = 0;

        private readonly int MAX_ENERGY = 100;
        private readonly int SHOT_COST = 10;
        private readonly float ENERGY_RECHARGE = 0.15f;
        private bool lowEnergy = false;

        private readonly Vector2 MAX_VELOCITY = new Vector2(2, 0.5f);
        private readonly float ACCELERATION_X = 0.020f;
        private readonly Vector2 START_ACCELERATION = new Vector2(0, -0.005f);

        private bool hasStarted = false;

        private readonly int WIDTH_POSITION_CONSTANT_SPEED = 125;
        private float MaxLeft
        {
            get { return Config.resolution.X / 2 - WIDTH_POSITION_CONSTANT_SPEED; }
        }
        private float MaxRight
        {
            get { return Config.resolution.X / 2 + WIDTH_POSITION_CONSTANT_SPEED; }
        }

        private SoundEffect fireSoundEffect;

        private float energy;

        private bool isGoingAway = false;

        private double time = 0;

        public TankGameObject(GameEnhanced game, String tag)
            : base(game, tag)
        {
            energy = 100;
        }

        protected override void LoadContent()
        {
            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, this.Game.Content.Load<Texture2D>("Player"), 1);
            // (49, 0), (23, 30), (116, 30)
            SpriteAnimation spriteAnimation = new SpriteAnimation(49,
                    new SpriteAnimationData[] {
                        new SpriteAnimationData(2, 0, 46, 60, 0, 0),
                        new SpriteAnimationData(48, 0, 46, 60, 0, 0),
                        new SpriteAnimationData(94, 0, 46, 60, 0, 0),
                        new SpriteAnimationData(140, 0, 46, 60, 0, 0),
                        new SpriteAnimationData(186, 0, 46, 60, 0, 0)});
            
            spriteAnimation.AutoChangeState = false;
            Sprite.ActiveSpriteAnimation = spriteAnimation;
            Sprite.ActiveSpriteAnimation.ActualState = 2;
            Position = new Vector2(Config.resolution.X / 2 - Size.X / 2, Config.resolution.Y - 120);

            fireSoundEffect = Game.Content.Load<SoundEffect>("shoot");

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);            
            SharedSpriteBatch.Begin();
            
            PrimitiveDrawing.DrawRectangle(Game.WhitePixel, SharedSpriteBatch, new Rectangle(POSITION_ENERGY_X, POSITION_ENERGY_Y, WIDTH_ENERGY_X, HEIGHT_ENERGY_X), Color.Black, true);
            if (energy > (MAX_ENERGY / 2))
            {
                PrimitiveDrawing.DrawRectangle(Game.WhitePixel, SharedSpriteBatch, new Rectangle(POSITION_ENERGY_X, POSITION_ENERGY_Y, ((int)(energy / MAX_ENERGY * WIDTH_ENERGY_X)), HEIGHT_ENERGY_X), Color.LimeGreen, true);
            }
            else
            {
                PrimitiveDrawing.DrawRectangle(Game.WhitePixel, SharedSpriteBatch, new Rectangle(POSITION_ENERGY_X, POSITION_ENERGY_Y, (int)(energy / MAX_ENERGY * WIDTH_ENERGY_X), HEIGHT_ENERGY_X), Color.Red, true);
                if (energy < 10)
                {

                    PrimitiveDrawing.DrawRectangle(Game.WhitePixel, SharedSpriteBatch, new Rectangle(POSITION_ENERGY_X, POSITION_ENERGY_Y, WIDTH_ENERGY_X, HEIGHT_ENERGY_X), new Color(r,g,b,transparencyVal), true);
                    PrimitiveDrawing.DrawRectangle(Game.WhitePixel, SharedSpriteBatch, new Rectangle(POSITION_ENERGY_X, POSITION_ENERGY_Y, (int)(energy / MAX_ENERGY * WIDTH_ENERGY_X), HEIGHT_ENERGY_X), new Color(237, 28, 36, transparencyVal), true);
                }
                if (lowEnergy == true)
                {
                    GUImessage.MessageDraw(SharedSpriteBatch, Game.Content, "LOW ENERGY!!", new Vector2((Config.resolution.X / 2)-40, Config.resolution.Y / 2), 1f);
                }
            }

            
            
            GUImessage.MessageDraw(SharedSpriteBatch, Game.Content, "Energy", new Vector2(POSITION_ENERGY_X, POSITION_ENERGY_Y));
            SharedSpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            time += gameTime.ElapsedGameTime.TotalMilliseconds;

            energy += ENERGY_RECHARGE;
            if (energy > MAX_ENERGY)
                energy = MAX_ENERGY;
            if (energy > SHOT_COST)
                lowEnergy = false;
            //Position.Y += 0.0001f;

            if (isGoingAway || !hasStarted)
                return;

            if (Math.Abs(Velocity.Y) > MAX_VELOCITY.Y)
            {
                Acceleration = new Vector2(Acceleration.X, -Acceleration.Y);
            }

            if (Math.Abs(Velocity.X) > MAX_VELOCITY.X)
            {

                Acceleration = new Vector2(0, Acceleration.Y);
                if(Velocity.X > MAX_VELOCITY.X)
                    Sprite.ActiveSpriteAnimation.ActualState = 4;
                else
                    Sprite.ActiveSpriteAnimation.ActualState = 0;
                    
            }

            if (PositionCenterX < MaxLeft)
            {
                Acceleration = new Vector2(ACCELERATION_X, Acceleration.Y);
                Sprite.ActiveSpriteAnimation.ActualState = 1;
                if( Velocity.X > -0.5f)
                    Sprite.ActiveSpriteAnimation.ActualState = 2;
                if( Velocity.X > 0.5f)
                    Sprite.ActiveSpriteAnimation.ActualState = 3;
            }
            if (PositionCenterX > MaxRight)
            {
                Acceleration = new Vector2(-ACCELERATION_X, Acceleration.Y);
                Sprite.ActiveSpriteAnimation.ActualState = 3;
                if (Velocity.X < 0.5f)
                    Sprite.ActiveSpriteAnimation.ActualState = 2;
                if (Velocity.X < -0.5f)
                    Sprite.ActiveSpriteAnimation.ActualState = 1;
            }


            if (transparencyVal < 255)
            {
                transparencyVal += 55;
                r = b = g = 100;
            }
            else
            {
                transparencyVal = 15;
                r = b = g = 0;
            }
        }

        public void start()
        {
            Sprite.ActiveSpriteAnimation.ActualState = 4;
            hasStarted = true;
            base.Velocity = MAX_VELOCITY;
            base.Acceleration = START_ACCELERATION;
        }

        public void fire()
        {
            if (energy < SHOT_COST)            
                lowEnergy = true;
            
            if (energy >= SHOT_COST)
            {
                Game.Components.Add(new BallGameObject(Position + new Vector2(Size.X / 2, 0), Game, "ball"));
                fireSoundEffect.Play();
                energy -= SHOT_COST;
                //Game.Log.ShotsFired++;
            }
            
            
         
        }

        public void goAway()
        {
            isGoingAway = true;
            Acceleration = new Vector2((Velocity.X > 0 ? 0.6f : -0.6f), -0.3f);
        }



    }
}
