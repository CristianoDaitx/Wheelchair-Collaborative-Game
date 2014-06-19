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
    class EnemyGameObject : GameObject
    {

        private double time = 0;

        public EnemyGameObject(GameObjectManager gameObjectManager, String tag)
            : base(gameObjectManager, tag)
        {

            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, gameObjectManager.GameScreen.ScreenManager.Game.Content.Load<Texture2D>("Space_Invader"),
                    gameObjectManager.GameScreen.ScreenManager.WhitePixel, new Vector2(202, 15), 0.5f);

            Sprite.velocity.Y = 0.5f;
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);


            time += gameTime.ElapsedGameTime.TotalMilliseconds;


            //Sprite.position.Y += 0.01f;


        }

    }
}