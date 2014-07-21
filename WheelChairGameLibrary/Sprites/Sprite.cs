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
        private GameObject gameObject;
        public GameObject GameObject
        {
            get { return gameObject; }
        }

        public Texture2D texture { get; set; }    //  sprite texture, read-only property


        public float scale { get; set; }          // the scale size of the sprite


        private SpriteAnimation activeSpriteAnimation;
        public SpriteAnimation ActiveSpriteAnimation
        {
            get { return activeSpriteAnimation; }
            set
            {
                activeSpriteAnimation = value;
                activeSpriteAnimation.resetAnimation();
            }
        }


        public Sprite(GameObject gameObject, Texture2D newTexture, float newScale)
        {
            this.gameObject = gameObject;
            this.texture = newTexture;
            this.scale = newScale;

            gameObject.Size = new Vector2(texture.Width * scale, texture.Height * scale);

        }


        public Rectangle activeSpriteBoundingBox()
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


                if (gameObject.Game.IsDebugMode)
                    PrimitiveDrawing.DrawRectangle(GameObject.Game.WhitePixel, spriteBatch, activeSpriteBoundingBox(), 1, Color.Yellow);

                spriteBatch.End();

                if (isEndedAnimation)
                    GameObject.endedAnimation();
            }
            else
            {
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
