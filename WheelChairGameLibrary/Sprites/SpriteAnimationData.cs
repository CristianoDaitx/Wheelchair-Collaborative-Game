using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WheelChairGameLibrary.Sprites
{
    public class SpriteAnimationData
    {
        public Rectangle sourceRectangle { get; set; }
        public int offsetX { get; set; }
        public int offsetY { get; set; }
        public bool isFlag { get; set; }

        public SpriteAnimationData(Rectangle sourceRectangle, int offsetX, int offsetY)
        {
            this.sourceRectangle = sourceRectangle;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.isFlag = false;
        }

        public SpriteAnimationData(Rectangle sourceRectangle, int offsetX, int offsetY, bool isFlag)
        {
            this.sourceRectangle = sourceRectangle;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.isFlag = isFlag;
        }

        public SpriteAnimationData(int x, int y, int width, int height, int offsetX, int offsetY)
        {
            this.sourceRectangle = new Rectangle(x, y, width, height);
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.isFlag = false;
        }

        public SpriteAnimationData(int x, int y, int width, int height, int offsetX, int offsetY, bool isFlag)
        {
            this.sourceRectangle = new Rectangle(x, y, width, height);
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.isFlag = isFlag;
        }

    }
}
