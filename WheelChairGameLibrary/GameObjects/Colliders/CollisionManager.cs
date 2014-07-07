#region Using Statements
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WheelChairGameLibrary.Screens;
using WheelChairGameLibrary.Helpers;
#endregion


namespace WheelChairGameLibrary.GameObjects
{
    public class CollisionManager : DrawableGameComponent
    {
        /// <summary>
        /// represent if the program is in the foreach loop during update, so no gameobjects can be added, removed at this time
        /// </summary>
        //private bool isInUpdateLoop;


        /// <summary>
        /// list of active colliders
        /// </summary>
        private List<Collider> colliders = new List<Collider>();



        public CollisionManager(Game game)
            :base(game)
        {
        }

        #region Update and Draw

        public override void  Update(GameTime gameTime)
        {
            //isInUpdateLoop = true;

            calculateCollisions(gameTime);

            //isInUpdateLoop = false;





        }


        private void calculateCollisions(GameTime gameTime)
        {
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

            colliders.Remove(collider);

        }


        #endregion
    }
}
