#region Using Statements
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace MortalKomatG.GameObjects
{


    class FighterDisplay : GameObject
    {



        FighterSpriteCreator fighterSpriteCreator = new FighterSpriteCreator();
        private FighterGameObject.FighterType fighterType;
        public FighterGameObject.FighterType FighterType
        {
            get { return fighterType; }
            set { fighterType = value; }
        }



        public FighterDisplay(GameObjectManager gameObjectManager, String tag,
            ContentManager content, FighterGameObject.FighterType fighterType, PlayerIndex playerController)
            : base(gameObjectManager, tag)
        {
            Sprite = new Sprite(this, content.Load<Texture2D>("tiles/" + fighterType.ToString()),
                content.Load<Texture2D>("tiles/whitePixel"), new Vector2(140, 330), 1.5f);

            this.fighterType = fighterType;
            this.Sprite.setActiveSpriteAnimation(fighterSpriteCreator.getSpriteAnimation(fighterType, Stances.Stance.FightingStance));



        }




        public override void Update(GameTime gameTime, InputState input)
        {
            base.Update(gameTime, input);

          


        }


        public void setStance(Stances.Stance stance)
        {

            this.Sprite.setActiveSpriteAnimation(fighterSpriteCreator.getSpriteAnimation(fighterType, stance));



        
        }



        
    }
}
