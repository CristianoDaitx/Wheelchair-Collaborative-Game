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
    public abstract class GameObject : DrawableGameComponent
    {
        private readonly bool DEBUG_GAME_OBJECT = true;

        public bool isFlipped { get; set; }            // has the sprite been flipped?

        /*private GameObjectManager gameObjectManager;
        public GameObjectManager GameObjectManager
        {
            get { return gameObjectManager; }
        }*/

        //used to force object to be destroyed in next update
        private bool toBeRemoved = false;
        public bool ToBeRemoved
        {
            set { toBeRemoved = value; }
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

        private Collider collider;
        public Collider Collider
        {
            get { return collider; }
            set { collider = value; }

        }

        /// <summary>
        /// Colliders must be instantiated inside constructor
        /// Contend must be lodade inside load content (if added before the call of Game.Initialize), otherwise, check if content is loaded (as xnaBasics example)
        /// </summary>
        /// <param name="game"></param>
        /// <param name="tag"></param>
        public GameObject(GameEnhanced game, String tag)
            :base(game)
        {
            //this.gameObjectManager = gameObjectManager;
            this.tag = tag;
        }

        public override void Update(GameTime gameTime)
        {
            //removes object if marked to be destroyed
            if (toBeRemoved)
            {
                Game.Components.Remove(this);
                return;
            }

            if (sprite != null)
                sprite.Update();


            velocity += acceleration;

            if (!isFlipped)
                Position += velocity;
            else
                Position -= velocity;


            if (!isFlipped)
            {
                if (velocity.X > 0 && acceleration.X > 0)
                {
                    acceleration.X = 0;
                    velocity.X = 0;
                }
                if (velocity.Y > 0 && acceleration.Y > 0)
                {
                    acceleration.Y = 0;
                    velocity.Y = 0;
                }
            }
            else
            {
                if (velocity.X > 0 && acceleration.X > 0)
                {
                    acceleration.X = 0;
                    velocity.X = 0;
                }
                if (velocity.Y > 0 && acceleration.Y > 0)
                {
                    acceleration.Y = 0;
                    velocity.Y = 0;
                }
            }


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (sprite != null)
                sprite.Draw(SharedSpriteBatch, gameTime);
            base.Draw(gameTime);
        }

        /*public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (sprite != null)
                sprite.Draw(spriteBatch, gameTime);

            if (DEBUG_GAME_OBJECT && collider != null)
                collider.Draw();
        }*/

        /// <summary>
        /// Called when an aniation in the Sprite reached it's end
        /// </summary>
        public virtual void endedAnimation() { }

        
        /// <summary>
        /// Called when an Collider is inside it's collider element
        /// </summary>
        /// <param name="collider">The other collider</param>
        public virtual void collisionEntered(Collider collider) { }











        public new GameEnhanced Game
        {
            get { return (GameEnhanced)base.Game; }
        }


        public Vector2 velocity;
        public Vector2 acceleration;


        // xna example stuff

        /// <summary>
        /// Gets or sets the position of the object.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the size of the object.
        /// </summary>
        public Vector2 Size { get; set; }

        /// <summary>
        /// Gets the KinectChooser from the services.
        /// </summary>
        public KinectChooser Chooser
        {
            get
            {
                return (KinectChooser)this.Game.Services.GetService(typeof(KinectChooser));
            }
        }

        /// <summary>
        /// Gets the SpriteBatch from the services.
        /// </summary>
        public SpriteBatch SharedSpriteBatch
        {
            get
            {
                return (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));
            }
        }

        

    }
}
