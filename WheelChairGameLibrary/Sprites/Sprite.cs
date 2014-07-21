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

using WheelChairGameLibrary.Helpers;

namespace WheelChairGameLibrary.Sprites
{
    public class Sprite
    {
        //private readonly int BB_OFFSET_X = 10;
        //private readonly int BB_WIDTH = 60;
        //private readonly int BB_HEIGHT = 140;


        private GameObject gameObject;
        public GameObject GameObject
        {
            get { return gameObject; }
        }

        public Texture2D texture { get; set; }    //  sprite texture, read-only property

        
        //public Vector2 screenSize { get; set; }   // Size of the window operating in
        public float scale { get; set; }          // the scale size of the sprite
        

        private SpriteAnimation activeSpriteAnimation;
        public SpriteAnimation ActiveSpriteAnimation
        {
            get { return activeSpriteAnimation; }
            //set { activeSpriteAnimation = value; }
        }


        public Sprite( GameObject gameObject, Texture2D newTexture,   float newScale)
        {
            this.gameObject = gameObject;
            this.texture = newTexture;            
            this.scale = newScale;

            gameObject.Size = new Vector2(texture.Width * scale, texture.Height * scale);

            //font = GameObject.GameObjectManager.GameScreen.ScreenManager.Game.Content.Load<SpriteFont>("menufont");
        }

        public void setActiveSpriteAnimation(SpriteAnimation spriteAnimation){            
            activeSpriteAnimation = spriteAnimation;
            activeSpriteAnimation.resetAnimation();
        }


        public void Update()
        {
            
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //MathHelper.Pi * 0.5f

            //spriteBatch.Draw(texture, position, null, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0.0f);
            spriteBatch.Draw(texture, GameObject.Position, Color.White);

            Vector2 origin = new Vector2(0, GameObject.Game.DefaultFont.LineSpacing / 2);
            //spriteBatch.DrawString(font, life.ToString(), position, Color.White, 0,
            //                       origin, 1, SpriteEffects.None, 0);

        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.End();

            //Vector2 origin = new Vector2(0, font.LineSpacing / 2);
            //spriteBatch.DrawString(font, life.ToString(), position, Color.White, 0,
            //                       origin, 1, SpriteEffects.None, 0);
        }

        /*public Rectangle getBoundingBox()
        {
            return new Rectangle((int)position.X + BB_OFFSET_X, (int)position.Y, (int)(BB_WIDTH * scale), (int)(BB_HEIGHT* scale));
        }
        */
        public Rectangle getBoundingBox2()
        {
            int offsetX = activeSpriteAnimation.getAnimationData().offsetX;  
            if (GameObject.isFlipped)
            {
                offsetX = (80 - activeSpriteAnimation.getAnimationData().offsetX - activeSpriteAnimation.getAnimationData().sourceRectangle.Width);
            }
            return new Rectangle(
                    (int)GameObject.Position.X + (int)(offsetX * scale),
                    (int)GameObject.Position.Y + (int)(activeSpriteAnimation.getAnimationData().offsetY * scale),
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

                if (GameObject.isFlipped)
                {
                    spriteEffect = SpriteEffects.FlipHorizontally;
                    offsetX = (80 - spriteAnimationData.offsetX - spriteAnimationData.sourceRectangle.Width);
                }

                spriteBatch.Begin();

                spriteBatch.Draw(texture,
                    new Rectangle(
                        (int)GameObject.Position.X + (int)(offsetX * scale),
                        (int)GameObject.Position.Y + (int)(spriteAnimationData.offsetY * scale), (int)(spriteAnimationData.sourceRectangle.Width * scale), (int)(spriteAnimationData.sourceRectangle.Height * scale)),
                    spriteAnimationData.sourceRectangle,
                    Color.White, 0.0f, new Vector2(), spriteEffect, 0.0f);

                

                PrimitiveDrawing.DrawRectangle(GameObject.Game.WhitePixel, spriteBatch, getBoundingBox2(), 1, Color.Yellow);
                //PrimitiveDrawing.DrawRectangle(whitePixel, spriteBatch, getBoundingBox());
                spriteBatch.End();
                //Vector2 origin = new Vector2(0, font.LineSpacing / 2);
                //spriteBatch.DrawString(font, life.ToString(), position, Color.White, 0,
                //                       origin, 1, SpriteEffects.None, 0);
                if (isEndedAnimation)
                    GameObject.endedAnimation();
            }
            else
            {
                //Draw(spriteBatch, GameObject.Position);
                spriteBatch.Begin();
                spriteBatch.Draw(texture,
                    new Rectangle(
                        (int)GameObject.Position.X,
                        (int)GameObject.Position.Y, (int)(GameObject.Size.X), (int)(GameObject.Size.Y)),
                        Color.White);
                spriteBatch.End();
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
