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
        
       
        public static void MessageDraw(SpriteBatch spriteBatch, ContentManager Content)
        {
            if (spriteFont == null)
            {
                spriteFont = Content.Load<SpriteFont>(@"SpriteFont1");
            }
            Vector2 textPosition = new Vector2(300.0f, 35.0f);
            
            // TODO: Add your drawing code here
            //Message goes here
            Vector2 textSize = spriteFont.MeasureString("0000");
            spriteBatch.DrawString(spriteFont, ("this"), textPosition - textSize / 2, Color.White);
        }
    }
}
