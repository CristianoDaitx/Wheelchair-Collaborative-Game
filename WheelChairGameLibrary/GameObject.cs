#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WheelChairGameLibrary.Sprites;
using WheelChairGameLibrary.Helpers;
#endregion

namespace WheelChairGameLibrary
{
    public abstract class GameObject : DrawableGameComponent
    {

        /// <summary>
        /// Hides original Game to return a EnhancedGame class
        /// </summary>
        public new GameEnhanced Game
        {
            get { return (GameEnhanced)base.Game; }
        }

        private String tag;
        public String Tag
        {
            get { return tag; }

        }

        //used to remove objects, it will be removed in next update
        private bool toBeRemoved = false;
        public bool ToBeRemoved
        {
            set { toBeRemoved = value; }
        }

        /// <summary>
        /// Gets the SpriteBatch from the services.
        /// </summary>
        public SpriteBatch SharedSpriteBatch
        {
            get
            {
                return (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));
            }
        }

        public InputState InputState
        {
            get
            {
                return (InputState)Game.Services.GetService(typeof(InputState));
            }
        }

        /// <summary>
        /// Gets the KinectChooser from the services.
        /// </summary>
        public KinectChooser Chooser
        {
            get
            {
                return (KinectChooser)this.Game.Services.GetService(typeof(KinectChooser));
            }
        }


        public GameObject(GameEnhanced game, String tag)
            :base(game)
        {
            this.tag = tag;
            DrawOrder = 10;
        }

        public override void Update(GameTime gameTime)
        {
            //removes object if marked to be destroyed
            if (toBeRemoved)
            {
                Game.Components.Remove(this);
                return;
            }
        }
    }
}
