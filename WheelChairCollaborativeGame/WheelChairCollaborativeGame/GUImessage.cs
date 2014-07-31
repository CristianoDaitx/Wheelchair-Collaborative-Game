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

namespace WheelChairCollaborativeGame
{
    class GUImessage
    {

        private static SpriteFont spriteFont;
        private static SpriteFont spriteFont2;
        private static SpriteFont spriteFont3;



        public static void MessageDraw(SpriteBatch spriteBatch, ContentManager Content, string message, int font, Vector2 textPosition)
        {
            if (font == 0)
            {

                if (spriteFont == null)
                {
                    spriteFont = Content.Load<SpriteFont>(@"SpriteFont1");
                }
                spriteBatch.DrawString(spriteFont, message, textPosition, Color.White);
            }

            if (font == 1)
            {
                if (spriteFont3 == null)
                {
                    spriteFont3 = Content.Load<SpriteFont>(@"SpriteFont5");
                }
                spriteBatch.DrawString(spriteFont3, message, textPosition, Color.White);

            }
        }

        public static void MessageDraw(SpriteBatch spriteBatch, ContentManager Content, string message, Vector2 textPosition)
        {
            if (spriteFont == null)
            {
                spriteFont = Content.Load<SpriteFont>(@"SpriteFont1");
            }
            spriteBatch.DrawString(spriteFont, message, textPosition, Color.White);
        }

        public static void MessageDraw(SpriteBatch spriteBatch, ContentManager Content, string message, Vector2 textPosition, float scale)
        {
            if (spriteFont == null)
            {
                spriteFont = Content.Load<SpriteFont>(@"SpriteFont1");
            }
            spriteBatch.DrawString(spriteFont, message, textPosition, Color.White, 0.0f, new Vector2(0, 0), scale, SpriteEffects.None, 0);
        }
    
        public enum Alignment { Center = 0, Left = 1, Right = 2, Top = 4, Bottom = 8 }

        public static void DrawString(SpriteBatch spriteBatch, ContentManager Content, string text, Rectangle bounds, Alignment align, Color color)
        {
            if (spriteFont2 == null)
            {
                spriteFont2 = Content.Load<SpriteFont>(@"SpriteFont4");
            }
            Vector2 size = spriteFont2.MeasureString(text);
            Vector2 pos = new Vector2(bounds.Center.X, bounds.Center.Y);
            Vector2 origin = size * 0.5f;
            color = Color.White;
            

            if (align.HasFlag(Alignment.Left))
                origin.X += bounds.Width / 2 - size.X / 2;

            if (align.HasFlag(Alignment.Right))
                origin.X -= bounds.Width / 2 - size.X / 2;

            if (align.HasFlag(Alignment.Top))
                origin.Y += bounds.Height / 2 - size.Y / 2;

            if (align.HasFlag(Alignment.Bottom))
                origin.Y -= bounds.Height / 2 - size.Y / 2;

            spriteBatch.DrawString(spriteFont2, text, pos, color, 0, origin, 1, SpriteEffects.None, 0);
        }
    }
}
