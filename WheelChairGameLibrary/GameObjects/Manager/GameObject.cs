#region Using Statements
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using WheelChairGameLibrary.Sprites;
using WheelChairGameLibrary.Helpers;
#endregion

namespace WheelChairGameLibrary.GameObjects
{
    public abstract class GameObject
    {
        private readonly bool DEBUG_GAME_OBJECT = true;

        private GameObjectManager gameObjectManager;
        public GameObjectManager GameObjectManager
        {
            get { return gameObjectManager; }
        }

        private String tag;
        public String Tag
        {
            get { return tag; }

        }

        private Sprite sprite;
        public Sprite Sprite
        {
            get { return sprite; }
            set { sprite = value; }

        }

        /*private Collider collider;
        public Collider Collider
        {
            get { return collider; }
            set { collider = value; }

        }*/

        public GameObject(GameObjectManager gameObjectManager, String tag)
        {
            this.gameObjectManager = gameObjectManager;
            this.tag = tag;
        }

        public virtual void Update(GameTime gameTime, InputState inputState){
            if (sprite != null)
                sprite.Update();

            
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (sprite != null)
                sprite.Draw(spriteBatch, gameTime);

            /*if (DEBUG_GAME_OBJECT && collider != null)
                collider.Draw();*/
        }

        /// <summary>
        /// Called when an aniation in the Sprite reached it's end
        /// </summary>
        public virtual void endedAnimation() { }

        /*
        /// <summary>
        /// Called when an Collider is inside it's collider element
        /// </summary>
        /// <param name="collider">The other collider</param>
        public virtual void collisionEntered(Collider collider) { }
         * */

    }
}
