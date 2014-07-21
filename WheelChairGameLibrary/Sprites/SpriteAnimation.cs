using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WheelChairGameLibrary.Sprites
{
    public class SpriteAnimation
    {
        public static readonly double DEFAULT_TIME_INTERVAL = 80f;

        private GameTime gameTime;

        private SpriteAnimationData[] animationDatas;


        private int states;
        private int actualState;

        private bool changeState = false;

        private bool isActive;
        

        private double timer = 0;
        private readonly double timeInterval = DEFAULT_TIME_INTERVAL;

        public SpriteAnimation(SpriteAnimationData[] animationDatas)
        {
            this.animationDatas = animationDatas;
            this.states = animationDatas.GetLength(0);
            isActive = true;
            actualState = 0;
        }

        public SpriteAnimation( double timeInterval, SpriteAnimationData[] animationDatas)
        {
            this.animationDatas = animationDatas;
            this.states = animationDatas.GetLength(0);
            isActive = true;
            actualState = 0;
            this.timeInterval = timeInterval;
        }



        public SpriteAnimationData getAnimationData()
        {
            return animationDatas[actualState];
        }

        public SpriteAnimationData getSourceRectangle(GameTime gameTime, out bool endedAnimation)
        {
            this.gameTime = gameTime;
            endedAnimation = false;
            if (changeState == true)
            {
                changeState = false;                
                actualState++;
            } else {
                endedAnimation = false;
            }
            //actualState %= states;
            if (actualState == states)
            {                
                endedAnimation = true;
                actualState = 0;
                return animationDatas[states-1];
                
            }
            isNextState();
            /*if (isNextState())
                endedAnimation = true;
            else
                endedAnimation = false;*/
            
            return animationDatas[actualState];
        }


        public void resetAnimation(){
            actualState = 0;
        }

        public void increaseSpriteAnimationOffsetX(int value)
        {
            animationDatas[actualState].offsetX+= value;
        }

        public void increaseSpriteAnimationOffsetY(int value)
        {
            animationDatas[actualState].offsetY+= value;
        }

        private void isNextState()
        {
            //bool endedAnimation = false;

            if (isActive)
            {
                timer += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timer >= timeInterval)
                {
                    timer = 0;
                    changeState = true;
                    //actualState++;
                }
                
            }

            //return endedAnimation;
        }

        public void nextState()
        {
            actualState++;
            actualState %= states;
        }
       
        public void previousState()
        {
            actualState--;
            if (actualState < 0)
                actualState += states;
        }

        public bool getIsActive() {return isActive;}

        public void setIsActive(bool active)
        {
            /*if (!isActive && active)
            {
                timer = 0;
                actualState = 0;
            }*/
            isActive = active;
           }
        
    }
}
