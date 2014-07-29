using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelChairGameLibrary;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WheelChairGameLibrary.Sprites;

namespace WheelChairCollaborativeGame.GameObjects
{
    class MessageCharacter : GameObject2D
    {

        protected int messageTimeSeconds = 5;
        protected string message;

        private TimeSpan time;

        public MessageCharacter(String message, GameEnhanced game, string tag)
            : base(game, tag)
        {
            this.message = message;
            time = new TimeSpan();
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            time+= gameTime.ElapsedGameTime;
            if (time.Seconds >= messageTimeSeconds)
            {
                base.ToBeRemoved = true;
            }
        }


    }
}
