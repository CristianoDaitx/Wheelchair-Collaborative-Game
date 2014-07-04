#region Using Statements
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WheelChairGameLibrary.Screens;
using WheelChairGameLibrary.Helpers;
#endregion

namespace WheelChairGameLibrary.GameObjects
{
    

    public class GameObjectManager
    {
        private GameScreen gameScreen;
        /// <summary>
        /// The GameScreen this manager belongs to
        /// </summary>
        public GameScreen GameScreen
        {
            get { return gameScreen; }
        }

        /// <summary>
        /// represent if the program is in the foreach loop during update, so no gameobjects can be added, removed at this time
        /// </summary>
        private bool isInUpdateLoop;

        
        /// <summary>
        /// list of active colliders
        /// </summary>
        private List<Collider> colliders = new List<Collider>();
         

        List<GameObject> gameObjectsToAdd = new List<GameObject>();
        List<GameObject> gameObjectsToRemove = new List<GameObject>();

        List<GameObject> gameObjects = new List<GameObject>();

        public GameObjectManager(GameScreen gameScreen)
        {
            this.gameScreen = gameScreen;
        }

        #region Update and Draw

        public void Update(GameTime gameTime, InputState inputState)
        {
            isInUpdateLoop = true;
            foreach (GameObject gameObject in gameObjects)
                gameObject.Update(gameTime, inputState);

            calculateCollisions(gameTime);

            isInUpdateLoop = false;

            if (gameObjectsToAdd.Count > 0)
            {
                foreach (GameObject gameObject in gameObjectsToAdd)
                    addGameObject(gameObject);
                gameObjectsToAdd.Clear();
            }

            if (gameObjectsToRemove.Count > 0)
            {
                foreach (GameObject gameObject in gameObjectsToRemove)
                    removeGameObject(gameObject);
                gameObjectsToRemove.Clear();
            }

            

        }

        
        private void calculateCollisions(GameTime gameTime)
        {
            for (int x = 0; x < colliders.Count; x++)
            {
                for (int y =0; y < colliders.Count; y++)
                {
                    if (x == y)
                        continue;
                    Rectangle rectangle = Rectangle.Intersect(colliders[x].BoundingBox, colliders[y].BoundingBox);
                    if (Rectangle.Intersect(colliders[x].BoundingBox, colliders[y].BoundingBox).Width > 0)
                    {
                        //Debug.WriteLine("hit");
                        colliders[x].GameObject.collisionEntered(colliders[y]);
                        //colliders[y].GameObject.collisionEntered(colliders[x]);
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

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (GameObject gameObject in gameObjects)
                gameObject.Draw(gameTime);
            spriteBatch.End();
        }

        #endregion

        #region public methods

        public void addGameObject(GameObject gameObject){
            if (!isInUpdateLoop)
            {
                gameObjects.Add(gameObject);
                if (gameObject.Collider != null)
                    colliders.Add(gameObject.Collider);
            }
            else
                gameObjectsToAdd.Add(gameObject);
        }

        public void removeGameObject(GameObject gameObject)
        {
            if (!isInUpdateLoop)
            {
                if (gameObject.Collider != null)
                    colliders.Remove(gameObject.Collider);
                gameObjects.Remove(gameObject);
            } else
                gameObjectsToRemove.Add(gameObject);
        }

        public GameObject getGameObject(string tag){
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.Tag == tag)
                    return gameObject;
            }
            return null;
        }

        #endregion
    }
}
