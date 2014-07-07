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
    class WeakEnemy : EnemyGameObject
    {
        public WeakEnemy(GameObjectManager gameObjectManager, String tag)
            : base(gameObjectManager, tag)
        {
            this.maxhits = 1;
            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, gameObjectManager.GameScreen.ScreenManager.Game.Content.Load<Texture2D>("Space_InvaderWeak"),
                    gameObjectManager.GameScreen.ScreenManager.WhitePixel, new Vector2(0, 0), 0.5f);
            Sprite.velocity.X = 0.95f;
            Sprite.velocity.Y = 0.7f;

        }
    }
}
