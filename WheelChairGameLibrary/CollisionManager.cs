﻿#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
#endregion


namespace WheelChairGameLibrary
{
    public class CollisionManager : DrawableGameComponent
    {
        /// <summary>
        /// list of active colliders
        /// </summary>
        private List<Collider> colliders = new List<Collider>();

        /// <summary>
        /// Used to mark if an collider has been removed inside the collision detection loop
        /// </summary>
        private bool isRemoved = false;

        public CollisionManager(Game game)
            : base(game)
        {
        }

        #region Update and Draw

        public override void Update(GameTime gameTime)
        {
            //isInUpdateLoop = true;

            calculateCollisions(gameTime);

            //isInUpdateLoop = false;





        }


        private void calculateCollisions(GameTime gameTime)
        {
            isRemoved = false;
            for (int x = 0; x < colliders.Count; x++)
            {
                for (int y = x; y < colliders.Count; y++)
                {
                    if (x == y)
                        continue;
                    Rectangle rectangle = Rectangle.Intersect(colliders[x].BoundingBox, colliders[y].BoundingBox);
                    if (Rectangle.Intersect(colliders[x].BoundingBox, colliders[y].BoundingBox).Width > 0)
                    {
                        //Debug.WriteLine("hit");
                        colliders[x].GameObject.collisionEntered(colliders[y]);

                        // the first call to collision may cause the other object to be removed. (Wierd timing)
                        if (!isRemoved)
                            colliders[y].GameObject.collisionEntered(colliders[x]);
                    }

                    //if (sprites[x].activeSpriteAnimation.getAnimationData().isStrike)
                    //    if (Rectangle.Intersect(sprites[x].getBoundingBox2(), sprites[y].getBoundingBox()).X > 0)
                    //    {
                    //        //Debug.WriteLine("hit");
                    //        sprites[x].hit(sprites[y], gameTime);
                    //        sprites[y].hit(sprites[x], gameTime);
                    //    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            foreach (Collider collider in colliders)
            {
                collider.Draw();
            }
        }


        #endregion

        #region public methods

        public void addCollider(Collider collider)
        {
            colliders.Add(collider);
        }

        public void removeCollider(Collider collider)
        {
            isRemoved = true;
            colliders.Remove(collider);

        }


        #endregion
    }
}
