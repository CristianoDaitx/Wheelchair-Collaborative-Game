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
        protected int maxhits;
        public EnemyGameObject(GameObjectManager gameObjectManager, String tag)
            : base(gameObjectManager, tag)
        {
            
            Sprite = new WheelChairGameLibrary.Sprites.Sprite(this, gameObjectManager.GameScreen.ScreenManager.Game.Content.Load<Texture2D>("Space_Invader"),
                   gameObjectManager.GameScreen.ScreenManager.WhitePixel, new Vector2(282, 0), 0.5f);
           

            Collider = new Collider(this, new Rectangle(0, 0, (int)Sprite.size.X * 2, (int)Sprite.size.Y * 2));
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);


            time += gameTime.ElapsedGameTime.TotalMilliseconds;


            //Sprite.position.Y += 0.01f;
            
           

            Collider.BoundingBox = new Rectangle((int)Sprite.position.X, (int)Sprite.position.Y, Collider.BoundingBox.Width, Collider.BoundingBox.Height);

            //delete if exit screen
            if (Sprite.position.X >= Config.resolution.X || Sprite.position.X <= 0)
            {
                GameObjectManager.removeGameObject(this);
            }

            if (Sprite.position.Y >= Config.resolution.Y )
            {
                GameObjectManager.removeGameObject(this);
            }
        }
        // delete if destroyed
        public int hits = 0;
        public override void collisionEntered(Collider collider)
        {
            if (collider.GameObject.GetType() == typeof(BallGameObject))
            {
                hits++;
                if (hits == maxhits)
                {
                    GameObjectManager.removeGameObject(this);
                }
                
            }
        }

    }
}