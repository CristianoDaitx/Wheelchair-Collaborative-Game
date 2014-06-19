using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using WheelChairGameLibrary.GameObjects;
using WheelChairGameLibrary.Helpers;

namespace WheelChairGameLibrary.Sprites
{
    public class Sprite
    {
        private readonly int BB_OFFSET_X = 10;
        private readonly int BB_WIDTH = 60;
        private readonly int BB_HEIGHT = 140;

        //TODO
        SpriteFont font;
        public int life = 100;

        private GameObject gameObject;
        public GameObject GameObject
        {
            get { return gameObject; }
        }

        public Texture2D texture { get; set; }    //  sprite texture, read-only property
        public Texture2D whitePixel;
        public Vector2 position;   //  sprite position on screen
        public Vector2 size { get; set; }         //  sprite size in pixels
        public Vector2 velocity;
        public Vector2 acceleration;
        //public Vector2 screenSize { get; set; }   // Size of the window operating in
        public float scale { get; set; }          // the scale size of the sprite
        public bool isFlipped { get; set; }            // has the sprite been flipped?

        private SpriteAnimation activeSpriteAnimation;
        public SpriteAnimation ActiveSpriteAnimation
        {
            get { return activeSpriteAnimation; }
            //set { activeSpriteAnimation = value; }
        }

        public Sprite()
        {
            scale = 1.0f;
            velocity = new Vector2();
            
        }

        public Sprite( GameObject gameObject, Texture2D newTexture, Texture2D whitePixel,  Vector2 newPosition, float newScale)
        {
            texture = newTexture;
            position = newPosition;
            scale = newScale;
            this.whitePixel = whitePixel;
            this.gameObject = gameObject;
            velocity = new Vector2();
            size = new Vector2(texture.Width * scale, texture.Height * scale);

            //font = GameObject.GameObjectManager.GameScreen.ScreenManager.Game.Content.Load<SpriteFont>("menufont");
        }

        public void setActiveSpriteAnimation(SpriteAnimation spriteAnimation){            
            activeSpriteAnimation = spriteAnimation;
            activeSpriteAnimation.resetAnimation();
        }

        public virtual void HandleInput(InputState input) 
        {
            if (input == null)
                throw new ArgumentNullException("input");
        }

        public void Update()
        {
            velocity += acceleration;

            if (!isFlipped)
                position += velocity;
            else
                position -= velocity;


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

            if (position.X < 0 - (BB_OFFSET_X * scale))
                position.X = 0 - (BB_OFFSET_X * scale);

            if (position.X > 1024 - (BB_OFFSET_X * scale) - (BB_WIDTH * scale))
                position.X = 1024 - (BB_OFFSET_X * scale) - (BB_WIDTH * scale);

            

            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //MathHelper.Pi * 0.5f

            //spriteBatch.Draw(texture, position, null, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0.0f);
            spriteBatch.Draw(texture, position, Color.White);

            Vector2 origin = new Vector2(0, font.LineSpacing / 2);
            //spriteBatch.DrawString(font, life.ToString(), position, Color.White, 0,
            //                       origin, 1, SpriteEffects.None, 0);

        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, position, Color.White);

            //Vector2 origin = new Vector2(0, font.LineSpacing / 2);
            //spriteBatch.DrawString(font, life.ToString(), position, Color.White, 0,
            //                       origin, 1, SpriteEffects.None, 0);
        }

        public Rectangle getBoundingBox()
        {
            return new Rectangle((int)position.X + BB_OFFSET_X, (int)position.Y, (int)(BB_WIDTH * scale), (int)(BB_HEIGHT* scale));
        }

        public Rectangle getBoundingBox2()
        {
            int offsetX = activeSpriteAnimation.getAnimationData().offsetX;  
            if (isFlipped)
            {
                offsetX = (80 - activeSpriteAnimation.getAnimationData().offsetX - activeSpriteAnimation.getAnimationData().sourceRectangle.Width);
            }
            return new Rectangle(
                    (int)position.X + (int)(offsetX * scale),
                    (int)position.Y + (int)(activeSpriteAnimation.getAnimationData().offsetY * scale),
                    (int)(activeSpriteAnimation.getAnimationData().sourceRectangle.Width * scale), (int)(activeSpriteAnimation.getAnimationData().sourceRectangle.Height * scale));
        }

        public virtual void collided() { }

        public virtual void hit(Sprite other, GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            if (activeSpriteAnimation != null)
            {
                bool isEndedAnimation;
                SpriteAnimationData spriteAnimationData = activeSpriteAnimation.getSourceRectangle(gameTime, out isEndedAnimation);

                SpriteEffects spriteEffect = SpriteEffects.None;
                int offsetX = spriteAnimationData.offsetX;

                if (isFlipped)
                {
                    spriteEffect = SpriteEffects.FlipHorizontally;
                    offsetX = (80 - spriteAnimationData.offsetX - spriteAnimationData.sourceRectangle.Width);
                }


                spriteBatch.Draw(texture,
                    new Rectangle(
                        (int)position.X + (int)(offsetX * scale),
                        (int)position.Y + (int)(spriteAnimationData.offsetY * scale), (int)(spriteAnimationData.sourceRectangle.Width * scale), (int)(spriteAnimationData.sourceRectangle.Height * scale)),
                    spriteAnimationData.sourceRectangle,
                    Color.White, 0.0f, new Vector2(), spriteEffect, 0.0f);

                PrimitiveDrawing.DrawRectangle(whitePixel, spriteBatch, getBoundingBox2(), 1, Color.Yellow);
                //PrimitiveDrawing.DrawRectangle(whitePixel, spriteBatch, getBoundingBox());

                //Vector2 origin = new Vector2(0, font.LineSpacing / 2);
                //spriteBatch.DrawString(font, life.ToString(), position, Color.White, 0,
                //                       origin, 1, SpriteEffects.None, 0);
                if (isEndedAnimation)
                    GameObject.endedAnimation();
            }
            else
            {
                Draw(spriteBatch, position);
            }
        }

        

        public void nextState()
        {
            activeSpriteAnimation.nextState();
        }
        public void previousState()
        {
            activeSpriteAnimation.previousState();
        }
    }
}
