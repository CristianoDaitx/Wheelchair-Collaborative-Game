#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WheelChairGameLibrary.Sprites;
using WheelChairGameLibrary.Helpers;
#endregion

namespace WheelChairGameLibrary
{
    public abstract class GameObject2D : GameObject
    {
        public bool isFlipped { get; set; }            // has the sprite been flipped?

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

        private Vector2 velocity;
        public Vector2 Velocity
        {
            set { velocity = value; }
            get { return velocity; }
        }

        private Vector2 acceleration;
        public Vector2 Acceleration
        {
            set { acceleration = value; }
            get { return acceleration; }
        }


        // xna example stuff

        /// <summary>
        /// Gets or sets the position of the object.
        /// Can only be set in instantiation
        /// </summary>
        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        

        /// <summary>
        /// Gets or sets the size of the object.
        /// </summary>
        public Vector2 Size { get; set; }

        
        /// <summary>
        /// return centerd position X or 0 if no spirte
        /// </summary>
        public float PositionCenterX
        {
            get
            {
                if (Sprite == null)
                    return 0;
                return Position.X + Size.X / 2;
            }
        }

        /// <summary>
        /// return centered positon y or 0 if no spirte
        /// </summary>
        public float PositionCenterY
        {
            get
            {
                if (Sprite == null)
                    return 0;
                return Position.Y + Size.Y / 2;
            }
        }

        /// <summary>
        /// return the ending X or 0 if no spirte
        /// </summary>
        public float PositionRightX
        {
            get
            {
                if (Sprite == null)
                    return 0;
                return Position.X + Size.X;
            }
        }

        /// <summary>
        /// return the ending Y or 0 if no spirte
        /// </summary>
        public float PositionBottomY
        {
            get
            {
                if (Sprite == null)
                    return 0;
                return Position.Y + Size.Y;
            }
        }

        /// <summary>
        /// Colliders must be instantiated inside constructor
        /// Contend must be lodade inside load content (if added before the call of Game.Initialize), otherwise, check if content is loaded (as xnaBasics example)
        /// </summary>
        /// <param name="game"></param>
        /// <param name="tag"></param>
        public GameObject2D(GameEnhanced game, String tag)
            : base(game, tag)
        {
            position = new Vector2();
        }

        public GameObject2D(Vector2 position, GameEnhanced game, String tag)
            : base(game, tag)
        {
            this.position = position;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Calculate physics

            velocity += acceleration;

            if (!isFlipped)
                position += velocity;
            else
                position -= velocity;


        }

        public override void Draw(GameTime gameTime)
        {
            //if there is a sprite, draw it
            if (sprite != null)
                sprite.Draw(SharedSpriteBatch, gameTime);
            base.Draw(gameTime);
        }



        /// <summary>
        /// Called when an aniation in the Sprite reached it's end
        /// </summary>
        public virtual void endedAnimation() { }

        /// <summary>
        /// Called when an Collider is inside it's collider element
        /// </summary>
        /// <param name="collider">The other collider</param>
        public virtual void collisionEntered(Collider collider) { }















    }
}
