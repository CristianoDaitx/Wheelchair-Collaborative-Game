﻿using System;
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
    }
}
