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
    class AvarageEnemy : EnemyGameObject
    {
        public AvarageEnemy(GameObjectManager gameObjectManager, String tag)
            : base(gameObjectManager, tag)
        {
            this.maxhits = 2;
            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, gameObjectManager.GameScreen.ScreenManager.Game.Content.Load<Texture2D>("Space_Invader"),
                    gameObjectManager.GameScreen.ScreenManager.WhitePixel, new Vector2(400, 0), 0.5f);
            Sprite.velocity.Y = 0.5f;
         
        }


        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);

            
            if (Sprite.position.Y == 0.10*Config.resolution.Y)
            {
                Sprite.velocity.Y = 0;
                if (Sprite.position.X > 50)
                {
                    Sprite.velocity.X = -0.7f;
                }
                else 
                {
                    Sprite.velocity.Y = 1.0f;
                    Sprite.velocity.X = 0.0f;
                }
            }
            if (Sprite.position.Y == 0.20 * Config.resolution.Y)
            {
                Sprite.velocity.Y = 0;
                if (Sprite.position.X < Config.resolution.X - 150)
                {
                    Sprite.velocity.X = 0.7f;
                }
                else
                {
                    Sprite.velocity.Y = 1.0f;
                    Sprite.velocity.X = 0.0f;
                }
            }
            if (Sprite.position.Y == 0.40 * Config.resolution.Y)
            {
                Sprite.velocity.Y = 0;
                if (Sprite.position.X > 50)
                {
                    Sprite.velocity.X = -0.7f;
                }
                else
                {
                    Sprite.velocity.Y = 1.0f;
                    Sprite.velocity.X = 0.0f;
                }
            }
            if (Sprite.position.Y == 0.5 *Config.resolution.Y)
            {
                Sprite.velocity.Y = 0;
                if (Sprite.position.X < Config.resolution.X - 150)
                {
                    Sprite.velocity.X = 0.7f;
                }
                else
                {
                    Sprite.velocity.Y = 1.0f;
                    Sprite.velocity.X = 0.0f;
                }
            }


           
        }
        
     

    }
}
