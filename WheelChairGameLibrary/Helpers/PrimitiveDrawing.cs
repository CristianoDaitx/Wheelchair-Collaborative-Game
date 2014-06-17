#region Using Statements
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace WheelChairGameLibrary.Helpers
{
    /// <summary>
    /// remarks http://stackoverflow.com/questions/13893959/how-to-draw-the-border-of-a-square
    /// </summary>
    static public class PrimitiveDrawing
    {
        static public void DrawRectangle(Texture2D whitePixel, SpriteBatch batch, Rectangle area, int width, Color color)
        {
            batch.Draw(whitePixel, new Rectangle(area.X, area.Y, area.Width, width), color);
            batch.Draw(whitePixel, new Rectangle(area.X, area.Y, width, area.Height), color);
            batch.Draw(whitePixel, new Rectangle(area.X + area.Width - width, area.Y, width, area.Height), color);
            batch.Draw(whitePixel, new Rectangle(area.X, area.Y + area.Height - width, area.Width, width), color);
        }
        static public void DrawRectangle(Texture2D whitePixel, SpriteBatch batch, Rectangle area, Color color)
        {
            DrawRectangle(whitePixel, batch, area, 1, color);
        }

        static public void DrawRectangle(Texture2D whitePixel, SpriteBatch batch, Rectangle area, Color color, bool solid)
        {
            for (int y = 0; y < area.Height; y++)
                DrawRectangle(whitePixel, batch, new Rectangle (area.X, area.Y + y, area.Width, 1), 1, color);
        }
        public static void DrawCircle(Texture2D whitePixel, SpriteBatch spritbatch, Vector2 center, float radius, Color color, int lineWidth = 2, int segments = 16)
        {
            Vector2[] vertex = new Vector2[segments];

            double increment = Math.PI * 2.0 / segments;
            double theta = 0.0;

            for (int i = 0; i < segments; i++)
            {
                vertex[i] = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                theta += increment;
            }

            DrawPolygon(whitePixel, spritbatch, vertex, segments, color, lineWidth);
        }
        public static void DrawPolygon(Texture2D whitePixel, SpriteBatch spriteBatch, Vector2[] vertex, int count, Color color, int lineWidth)
        {
            if (count > 0)
            {
                for (int i = 0; i < count - 1; i++)
                {
                    DrawLineSegment(whitePixel, spriteBatch, vertex[i], vertex[i + 1], color, lineWidth);
                }
                DrawLineSegment(whitePixel, spriteBatch, vertex[count - 1], vertex[0], color, lineWidth);
            }
        }
        public static void DrawLineSegment(Texture2D whitePixel, SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, int lineWidth)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            spriteBatch.Draw(whitePixel, point1, null, color,
            angle, Vector2.Zero, new Vector2(length, lineWidth),
            SpriteEffects.None, 0f);
        }
    }
}
