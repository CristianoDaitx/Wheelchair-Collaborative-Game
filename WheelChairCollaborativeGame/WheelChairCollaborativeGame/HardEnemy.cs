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
    class HardEnemy : EnemyGameObject
    {
        public HardEnemy(GameObjectManager gameObjectManager, String tag)
            : base(gameObjectManager, tag)
        {
            this.maxhits = 3;
            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, gameObjectManager.GameScreen.ScreenManager.Game.Content.Load<Texture2D>("Space_InvaderHard"),
                    gameObjectManager.GameScreen.ScreenManager.WhitePixel, new Vector2(Config.resolution.X/2, 0), 0.5f);
            
            Sprite.velocity.Y = 0.7f;

        }
        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);
            if (Sprite.position.Y > Config.resolution.Y - 150)
            {
                Sprite.velocity.Y = 0;
                Sprite.velocity.X = 0.7f;
            }
        }

       
       

    }
}
