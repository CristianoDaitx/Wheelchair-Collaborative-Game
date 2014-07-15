#region Using Statements
using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using WheelChairGameLibrary;
using WheelChairGameLibrary.Helpers;

using WheelChairGameLibrary.Sprites;

using KinectForWheelchair;
using KinectForWheelchair.Listeners;

using Microsoft.Kinect;

#endregion

// Credits http://www.syntaxwarriors.com/2012/xna-2d-infinite-scrolling-space-background/
namespace WheelChairCollaborativeGame
{
    class Background : GameObject
    {
        private Texture2D _StarTexture;
        private Texture2D _CloudTexture;
 
        private List<Star> _Stars;
        private int _Intensity;
        private Random _Random;

       
 
        private int MaxX;
        private int MaxY;

        public Background(GameEnhanced game,  int Intensity)
            :base(game, "background")
        {
            _Intensity = Intensity;
            _Stars = new List<Star>();
            _Random = new Random(DateTime.Now.Millisecond);

            MaxX = (int)Config.resolution.X;
            MaxY = (int)Config.resolution.Y ;
            _StarTexture = Game.WhitePixel;
            

            for (int i = 0; i <= Intensity; i++)
            {
                int StarColorR = _Random.Next(25, 100);
                int StarColorG = _Random.Next(10, 100);
                int StarColorB = _Random.Next(90, 150);
                int StarColorA = _Random.Next(10, 50);
 
                float Scale = _Random.Next(100, 900) / 100f;
                int Depth = _Random.Next(4, 7);
 
                _Stars.Add(new Star(new Vector2(_Random.Next(MaxX / -2 - 500, MaxX / 2 + 500), _Random.Next(MaxY / -2 - 500, MaxY / 2 + 500)), new Color(StarColorR / 3, StarColorG / 3, StarColorB / 3, StarColorA / 3), Scale, Depth, true));
            }
 
            for (int i = 0; i <= Intensity/2; i++)
            {
                int StarColor = _Random.Next(100, 200);
                int Depth = _Random.Next(2, 6);
                float Scale = _Random.Next(2, 9) / 100f;
 
                _Stars.Add(new Star(new Vector2(_Random.Next(MaxX / -2 - 200, MaxX / 2 + 200), _Random.Next(MaxY / -2 - 200, MaxY / 2 + 200)), new Color(StarColor, StarColor, StarColor, StarColor), Scale, Depth, false));
            }
 
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _CloudTexture = Game.Content.Load<Texture2D>("fluffyball");
        }


        public override void  Update(GameTime gameTime)
{
 	 base.Update(gameTime);


            foreach (Star s in _Stars)
            {
                s.Position += new Vector2(0,-20) * -1f / s.Depth;
 
                if (s.Position.X > MaxX + 501)
                    s.Position.X -= s.Position.X +500;
 
                if (s.Position.Y > MaxY + 500)
                    s.Position.Y -= s.Position.Y + 500;
 
                if (s.Position.X < -560)
                    s.Position.X += MaxX +550;
 
                if (s.Position.Y < -570)
                    s.Position.Y += MaxY +510;
            }
        }
 
        public override void  Draw(GameTime gameTime)
{
 	 base.Draw(gameTime);
        

            SharedSpriteBatch.Begin();
            foreach(Star s in _Stars)
            {
                SharedSpriteBatch.Draw(_CloudTexture, s.Position, null, s.Color, 0, Vector2.Zero, s.Scale, SpriteEffects.None, 0);
            }
            SharedSpriteBatch.End();
        }
 
    }
  
   
    class Star{
        public Vector2 Position;
        public Color Color;
        public float Scale;
        public float Depth;
        public bool isCloud;
        public Star(Vector2 Position, Color Color, float Scale, int Depth, bool isCloud)
        {
            this.Position = Position;
            this.Color = Color;
            this.Scale = Scale;
            this.Depth = Depth;
            this.isCloud = isCloud;
        }
    }
}

