#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using WheelChairGameLibrary.Helpers;
using WheelChairGameLibrary.Screens;
using WheelChairGameLibrary.GameObjects;
using WheelChairGameLibrary.Sprites;

using KinectForWheelchair;
using KinectForWheelchair.Listeners;

using Microsoft.Kinect;

#endregion

namespace WheelChairCollaborativeGame
{
    class TankGameObject : GameObject
    {


        private readonly int ATTACK_STANCE_Y = 20;

        private bool isAttackStance = false;

        private double time = 0;

        public TankGameObject(GameObjectManager gameObjectManager, String tag)
            : base(gameObjectManager, tag)
        {

            Sprite = new WheelChairGameLibrary.Sprites.Sprite (this, gameObjectManager.GameScreen.ScreenManager.Game.Content.Load<Texture2D>("Space_Invader"),
                    gameObjectManager.GameScreen.ScreenManager.WhitePixel, new Vector2(282, 65), 1);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);


            time += gameTime.ElapsedGameTime.TotalMilliseconds;


            Sprite.position.Y += 0.0001f;


            if (TimeSpan.FromMilliseconds(time).TotalSeconds > 2) //more than two seconds
            {
                //if (Sprite.position.Y > 160)
                setAttackStance();
                time = 0;
            }
        }

        public void setAttackStance()
        {
            Sprite.position.Y = ATTACK_STANCE_Y; 

            isAttackStance = true;
        }


        public void slideToRight()
        {
            Sprite.position.X += 3; 
        }

        public void slideToLeft()
        {
            Sprite.position.X -= 3;
        }


    }
}
