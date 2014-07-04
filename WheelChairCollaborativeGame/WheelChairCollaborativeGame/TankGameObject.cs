﻿#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using WheelChairGameLibrary;
using WheelChairGameLibrary.Helpers;
using WheelChairGameLibrary.Screens;
using WheelChairGameLibrary.GameObjects;
using WheelChairGameLibrary.Sprites;

using KinectForWheelchair;
using KinectForWheelchair.Listeners;

using Microsoft.Kinect;

#endregion

namespace WheelChairCollaborativeGame
{
    class TankGameObject : GameObject
    {


        private readonly int ATTACK_STANCE_Y = 360;
        private readonly int DEFENCE_STANCE_Y = 380;

        private bool isAttackStance = false;

        private double time = 0;

        public TankGameObject(GameEnhanced game, String tag)
            : base(game, tag)
        {

            Sprite = new WheelChairGameLibrary.Sprites.Sprite (this, this.Game.Content.Load<Texture2D>("Space_Invader"),
                     new Vector2(282, DEFENCE_STANCE_Y), 1);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            time += gameTime.ElapsedGameTime.TotalMilliseconds;


            //Position.Y += 0.0001f;


            if (TimeSpan.FromMilliseconds(time).TotalSeconds > 2) //more than two seconds
            {
                //if (Sprite.position.Y > 160)
                setDefenceStance();
                time = 0;
            }
        }

        public void setAttackStance()
        {
            //TODO Sprite.position.Y = ATTACK_STANCE_Y; 

            isAttackStance = true;
        }

        public void setDefenceStance()
        {
            //TODO Sprite.position.Y = DEFENCE_STANCE_Y;
            isAttackStance = false;
        }

        public void slideToRight()
        {
            //this.Position.X = new Vector2(this.Position.X +3, this.Position.Y); 
        }

        public void slideToLeft()
        {
            //this.Position.X -= 3;
        }


    }
}
