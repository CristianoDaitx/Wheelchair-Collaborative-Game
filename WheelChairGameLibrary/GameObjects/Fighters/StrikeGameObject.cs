#region Using Statements
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MortalKomatG.GameObjects
{
    class StrikeGameObject : GameObject
    {

        public enum StrikeType
        {
            KickHigh,
            KickLow,
            PunchHigh,
            PunchLow
        }

        private int damage;
        public int Damage
        {
            get { return damage; }
            //set { damage = value; }
        }

        private Vector2 acceleration;
        public Vector2 Acceleration
        {
            get { return acceleration; }
        }

        private Vector2 velocity;
        public Vector2 Velocity
        {
            get { return velocity; }
        }
        private StrikeType type;
        public StrikeType Type
        {
            get { return type; }
        }



        public StrikeGameObject(GameObjectManager gameObjectManager, String tag, Rectangle colliderRectangle, StrikeType type, int damage, Vector2 acceleration, Vector2 velocity)
            : base(gameObjectManager,tag)
        {
            Collider = new Collider(this, colliderRectangle);
            this.type = type;
            this.damage = damage;
            this.acceleration = acceleration;
            this.velocity = velocity;
        }

    }
}
