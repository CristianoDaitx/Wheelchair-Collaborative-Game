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


    class FighterGameObject : GameObject
    {
        public enum FighterType
        {
            Scorpion,
            LiuKang,
            KungLao,
            None

        }

        private readonly int BB_OFFSET_X = 10;
        private readonly int BB_WIDTH = 60;
        private readonly int BB_HEIGHT = 140;


        FighterSpriteCreator fighterSpriteCreator = new FighterSpriteCreator();
        private FighterGameObject.FighterType fighterType;
        private Stances.Stance actualStance;

        private bool isAddedStrike;
        private StrikeGameObject strikeGameObject;
        PlayerIndex ControllingPlayer;

        SoundBank soundBank;
        Cue cue;

        double timeToEnd = 0;


        private Queue<Keys> pressedKeys = new Queue<Keys>();

        private bool isEndFight = false;

        private int hitsTakenConsecutively = 0;

        //private int life = 100;

        public FighterGameObject(GameObjectManager gameObjectManager, String tag,
            ContentManager content, FighterGameObject.FighterType fighterType, PlayerIndex playerController)
            : base(gameObjectManager, tag)
        {
            ControllingPlayer = playerController;
            Sprite = new Sprite(this, content.Load<Texture2D>("tiles/" + fighterType.ToString()),
                content.Load<Texture2D>("tiles/whitePixel"), new Vector2(100, 330), 1.5f);

            //texture = content.Load<Texture2D>("tiles/" + fighterType.ToString());
            //whitePixel = content.Load<Texture2D>("tiles/whitePixel");
            //size = new Vector2(texture.Width * scale, texture.Height * scale);
            //position = new Vector2(100, 300);
            this.fighterType = fighterType;
            actualStance = Stances.Stance.FightingStance;
            this.Sprite.setActiveSpriteAnimation(fighterSpriteCreator.getSpriteAnimation(fighterType, actualStance));
            //font = content.Load<SpriteFont>("menufont");


            Collider = new Collider(this, getBoundingBox());


            soundBank = ((MortalKomatG.Game)GameObjectManager.GameScreen.ScreenManager.Game).getSoundBank();
            cue = soundBank.GetCue("fight");
            cue.Play();
            
        }

        public Rectangle getBoundingBox()
        {
            return new Rectangle((int)Sprite.position.X + (int)(BB_OFFSET_X * Sprite.scale), (int)Sprite.position.Y, (int)(BB_WIDTH * Sprite.scale), (int)(BB_HEIGHT * Sprite.scale));
        }

        private Rectangle getStrikeBoundingBox()
        {
            int offsetX = (int)(Sprite.scale * -20) + (int)Sprite.position.X + (int)((Sprite.ActiveSpriteAnimation.getAnimationData().offsetX + Sprite.ActiveSpriteAnimation.getAnimationData().sourceRectangle.Width) * Sprite.scale);

            if (Sprite.isFlipped)
                offsetX = (int)Sprite.position.X + (int)(Sprite.scale * (80 - Sprite.ActiveSpriteAnimation.getAnimationData().offsetX - Sprite.ActiveSpriteAnimation.getAnimationData().sourceRectangle.Width));

            return new Rectangle(
                    (int)offsetX,
                    (int)Sprite.position.Y, (int)(Sprite.scale * 20), (int)(Sprite.scale * BB_HEIGHT));

        }

        private void pressedKeysEnqueue(Keys key)
        {
            if (pressedKeys.Count < 1)
                pressedKeys.Enqueue(key);
        }

        private bool isPunching()
        {
            return (
                actualStance == Stances.Stance.PunchGo ||
                actualStance == Stances.Stance.PunchBack ||
                actualStance == Stances.Stance.PunchHigh1 ||
                actualStance == Stances.Stance.PunchHigh2 ||
                actualStance == Stances.Stance.PunchLow1 ||
                actualStance == Stances.Stance.PunchLow2
                );
        }

        private bool isKicking()
        {
            return (
                actualStance == Stances.Stance.KickHigh ||
                actualStance == Stances.Stance.KickLow
                );
        }

        private bool isStriking()
        {
            return (
                isKicking()||
                isPunching());
        }

        private bool isWalking()
        {
            return (
                actualStance == Stances.Stance.WalkingBackward ||
                actualStance == Stances.Stance.WalkingForward);
        }

        private bool isBeingHit()
        {
            return (
                actualStance == Stances.Stance.HitWalk ||
                actualStance == Stances.Stance.HitLow ||
                actualStance == Stances.Stance.HitHigh);
        }

        public override void Update(GameTime gameTime, InputState input)
        {
            base.Update(gameTime, input);

            if (isEndFight)
            {
                timeToEnd += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timeToEnd >= TimeSpan.FromSeconds(3).TotalMilliseconds)
                {
                    GameObjectManager.GameScreen.ExitScreen();
                    GameObjectManager.GameScreen.ScreenManager.AddScreen(new FighterChoose(), null);
                }
            }

            //add collision box if animiation is a strike
            if (Sprite.ActiveSpriteAnimation.getAnimationData().isStrike)
            {
                if (!isAddedStrike)
                {

                    if (actualStance == Stances.Stance.KickHigh)
                        strikeGameObject = StrikeGameObjectCreator.createStrikeGameObject(GameObjectManager, "strike", getStrikeBoundingBox(), StrikeGameObject.StrikeType.KickHigh);
                        //strikeGameObject.Collider = new StrikeCollider(strikeGameObject, getStrikeBoundingBox(), new Vector2(-0.5f, 0.0f), new Vector2(4f, 0.0f));
                    else
                        if (actualStance == Stances.Stance.KickLow)
                            strikeGameObject = StrikeGameObjectCreator.createStrikeGameObject(GameObjectManager, "strike", getStrikeBoundingBox(), StrikeGameObject.StrikeType.KickLow);
                        else
                            if (actualStance == Stances.Stance.PunchHigh1 || actualStance == Stances.Stance.PunchHigh2)
                                strikeGameObject = StrikeGameObjectCreator.createStrikeGameObject(GameObjectManager, "strike", getStrikeBoundingBox(), StrikeGameObject.StrikeType.PunchHigh);
                            else
                                if (actualStance == Stances.Stance.PunchLow1 || actualStance == Stances.Stance.PunchLow2)
                                    strikeGameObject = StrikeGameObjectCreator.createStrikeGameObject(GameObjectManager, "strike", getStrikeBoundingBox(), StrikeGameObject.StrikeType.PunchLow);


                    GameObjectManager.addGameObject(strikeGameObject);
                    isAddedStrike = true;
                }
            }


            //reset after moved away from strike sprite
            if (isAddedStrike && !Sprite.ActiveSpriteAnimation.getAnimationData().isStrike)
            {
                isAddedStrike = false;
                GameObjectManager.removeGameObject(strikeGameObject);
            }

            

            Collider.BoundingBox = getBoundingBox();

            if (Sprite.life <= 0)
                return;

            // skip if being hit
            if (isBeingHit())
                return;
            //PlayerIndex newPlayerIndex;

            if (input.IsHighPunchPressed(ControllingPlayer))
            {
                pressedKeysEnqueue(Keys.W);
                //if (newPlayerIndex == ControllingPlayer && !isPunching())
                if (!isPunching())
                    setStance(Stances.Stance.PunchGo);
            }

            if (input.IsLowPunchPressed(ControllingPlayer))
            {
                pressedKeysEnqueue(Keys.S);
                //if (newPlayerIndex == ControllingPlayer && !isPunching())
                if (!isPunching())
                    setStance(Stances.Stance.PunchGo);
            }

            if (input.IsHighKickPressed(ControllingPlayer))
            {
                pressedKeysEnqueue(Keys.E);
                //if (newPlayerIndex == ControllingPlayer && !isPunching())
                if (!isPunching())
                    setStance(Stances.Stance.KickHigh);
            }

            if (input.IsLowKickPressed(ControllingPlayer))
            {
                pressedKeysEnqueue(Keys.D);
                //if (newPlayerIndex == ControllingPlayer && !isPunching())
                if (!isPunching())
                    setStance(Stances.Stance.KickLow);
            }

            if (input.IsLeftDown(ControllingPlayer))
            {
                //if (newPlayerIndex == ControllingPlayer)
                    if (Sprite.isFlipped)
                        setStance(Stances.Stance.WalkingForward);
                    else
                        setStance(Stances.Stance.WalkingBackward);
            }

            if (input.IsLeftPressed(ControllingPlayer))
            {
                //if (newPlayerIndex == ControllingPlayer)
                    if (Sprite.isFlipped)
                        setStance(Stances.Stance.WalkingForward);
                    else
                        setStance(Stances.Stance.WalkingBackward);
            }

            if (input.IsLeftRelease(ControllingPlayer))
            {
                if (!isStriking())
                    //if (newPlayerIndex == ControllingPlayer)
                        setStance(Stances.Stance.FightingStance);
            }

            if (input.IsRightDown(ControllingPlayer))
            {
                //if (newPlayerIndex == ControllingPlayer)
                    if (Sprite.isFlipped)
                        setStance(Stances.Stance.WalkingBackward);
                    else
                        setStance(Stances.Stance.WalkingForward);
            }

            if (input.IsRightPressed(ControllingPlayer))
            {
                //if (newPlayerIndex == ControllingPlayer)
                    if (Sprite.isFlipped)
                        setStance(Stances.Stance.WalkingBackward);
                    else                        
                        setStance(Stances.Stance.WalkingForward);
            }

            if (input.IsRightRelease(ControllingPlayer))
            {
                if (!isStriking())
                    //if (newPlayerIndex == ControllingPlayer)
                        setStance(Stances.Stance.FightingStance);
            }


        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            if (ControllingPlayer == PlayerIndex.Two)
            {
                PrimitiveDrawing.DrawRectangle(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                    new Rectangle(600, 30, 400, 40), Color.Red, true);

                PrimitiveDrawing.DrawRectangle(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                    new Rectangle(600 + ((100 - Sprite.life) * 4), 30, 400 - ((100 - Sprite.life) * 4), 40), Color.Yellow, true);
            }
            else
            {
                PrimitiveDrawing.DrawRectangle(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                    new Rectangle(20, 30, 400, 40), Color.Red, true);
                PrimitiveDrawing.DrawRectangle(GameObjectManager.GameScreen.ScreenManager.WhitePixel, GameObjectManager.GameScreen.ScreenManager.SpriteBatch,
                    new Rectangle(20, 30, 400 - ((100 - Sprite.life) * 4), 40), Color.Yellow, true);

            }
        }

        public override void endedAnimation()
        {
            /*if (!isWalking())
                setStance(Stances.Stance.FightingStance);*/
            switch (actualStance)
            {
                case Stances.Stance.PunchGo:
                    if (pressedKeys.Count > 0)
                    {
                        Keys key = pressedKeys.Dequeue();
                        if (key == Keys.W)
                        {
                            setStance(Stances.Stance.PunchHigh1);
                        }
                        if (key == Keys.S)
                        {
                            setStance(Stances.Stance.PunchLow1);
                        }
                    }
                    break;
                case Stances.Stance.PunchHigh1:
                    if (pressedKeys.Count > 0)
                    {
                        Keys key = pressedKeys.Dequeue();
                        switch (key)
                        {
                            case Keys.W:
                                setStance(Stances.Stance.PunchHigh2);
                                break;
                            case Keys.S:
                                setStance(Stances.Stance.PunchLow2);
                                break;
                            default:
                                setStance(Stances.Stance.PunchBack);
                                break;
                        }
                    }
                    else
                        setStance(Stances.Stance.PunchBack);
                    break;
                case Stances.Stance.PunchHigh2:
                    if (pressedKeys.Count > 0)
                    {
                        Keys key = pressedKeys.Dequeue();
                        switch (key)
                        {
                            case Keys.W:
                                setStance(Stances.Stance.PunchHigh1);
                                break;
                            case Keys.S:
                                setStance(Stances.Stance.PunchLow1);
                                break;
                            default:
                                setStance(Stances.Stance.PunchBack);
                                break;
                        }
                    }
                    else
                        setStance(Stances.Stance.PunchBack);
                    break;
                case Stances.Stance.PunchLow1:
                    if (pressedKeys.Count > 0)
                    {
                        Keys key = pressedKeys.Dequeue();
                        switch (key)
                        {
                            case Keys.W:
                                setStance(Stances.Stance.PunchHigh2);
                                break;
                            case Keys.S:
                                setStance(Stances.Stance.PunchLow2);
                                break;
                            default:
                                setStance(Stances.Stance.PunchBack);
                                break;
                        }
                    }
                    else
                        setStance(Stances.Stance.PunchBack);
                    break;
                case Stances.Stance.PunchLow2:
                    if (pressedKeys.Count > 0)
                    {
                        Keys key = pressedKeys.Dequeue();
                        switch (key)
                        {
                            case Keys.W:
                                setStance(Stances.Stance.PunchHigh1);
                                break;
                            case Keys.S:
                                setStance(Stances.Stance.PunchLow1);
                                break;
                            default:
                                setStance(Stances.Stance.PunchBack);
                                break;
                        }
                    }
                    else
                        setStance(Stances.Stance.PunchBack);
                    break;
                case Stances.Stance.PunchBack:
                    setStance(Stances.Stance.FightingStance);
                    pressedKeys.Clear();
                    break;

                case Stances.Stance.KickHigh:
                    setStance(Stances.Stance.FightingStance);
                    pressedKeys.Clear();
                    break;

                case Stances.Stance.KickLow:
                    setStance(Stances.Stance.FightingStance);
                    pressedKeys.Clear();
                    break;

                case Stances.Stance.FallBackward:
                    setStance(Stances.Stance.Dead);
                    FighterGameObject otherFighter;
                    if (ControllingPlayer == PlayerIndex.One)                     
                        otherFighter = (FighterGameObject) GameObjectManager.getGameObject("fighterTwo");
                    else
                        otherFighter = (FighterGameObject)GameObjectManager.getGameObject("fighterOne");

                    otherFighter.setStance(Stances.Stance.VictoryPose);
                    break;

                case Stances.Stance.HitHigh:
                case Stances.Stance.HitLow:
                case Stances.Stance.HitWalk:
                    if (Sprite.life <= 0)
                        setStance(Stances.Stance.Dizzy);
                    else
                        setStance(Stances.Stance.FightingStance);
                    hitsTakenConsecutively = 0;
                    break;

                case Stances.Stance.VictoryPose:
                    setStance(Stances.Stance.Win);
                    break;

                case Stances.Stance.Win:
                    isEndFight = true;
                    break;
            }
        }

        public void setStance(Stances.Stance stance)
        {
            // skip if in win  pose
            if (stance != Stances.Stance.Win && (actualStance == Stances.Stance.Win || actualStance == Stances.Stance.VictoryPose))
                return;
 

            // skip if stance is the same, except for hit animations
            if (stance!= Stances.Stance.HitLow && stance!=Stances.Stance.HitHigh && stance!=Stances.Stance.HitWalk && stance == actualStance)
                return;

            // dont allow to move while punching or kicking
            if ((stance == Stances.Stance.WalkingForward || stance == Stances.Stance.WalkingBackward) && isStriking())
                return;

            // dont allow to punch or kick while kicking
            if ((stance == Stances.Stance.PunchGo || stance == Stances.Stance.KickHigh || stance == Stances.Stance.KickLow) && isKicking())
                return;

            /*if (actualStance == Stances.Stance.PunchGo )
                return;*/

            actualStance = stance;
            this.Sprite.setActiveSpriteAnimation(fighterSpriteCreator.getSpriteAnimation(fighterType, stance));




            if (stance == Stances.Stance.WalkingForward)
            {
                this.Sprite.velocity.X = 2.5f;
                //if (this.Sprite.isFlipped)
                    //this.Sprite.velocity.X = -this.Sprite.velocity.X;

            }
            else if (stance == Stances.Stance.WalkingBackward)
            {
                this.Sprite.velocity.X = -2.5f;
                //if (this.Sprite.isFlipped)
                    //this.Sprite.velocity.X = -this.Sprite.velocity.X;

            }
            else
            {
                if (actualStance != Stances.Stance.HitWalk)
                    this.Sprite.velocity.X = 0;
            }



            //sounds
            switch (stance)
            {
                case Stances.Stance.PunchHigh1:
                case Stances.Stance.PunchHigh2:
                    cue = soundBank.GetCue("punchHigh");
                    break;
                case Stances.Stance.PunchLow1:
                case Stances.Stance.PunchLow2:
                    cue = soundBank.GetCue("punchLow");
                    break;
                case Stances.Stance.KickHigh:
                    cue = soundBank.GetCue("kickHigh");
                    break;
                case Stances.Stance.KickLow:
                    cue = soundBank.GetCue("kickLow");
                    break;
                case Stances.Stance.Dead:
                    cue = soundBank.GetCue("fall");
                    break;
                case Stances.Stance.HitHigh:
                    cue = soundBank.GetCue("hitPunch");
                    break;
                case Stances.Stance.HitLow:
                    cue = soundBank.GetCue("hitPunch2");
                    break;
                case Stances.Stance.HitWalk:
                    cue = soundBank.GetCue("hitKickH");
                    break;
                case Stances.Stance.FallBackward:
                    cue = soundBank.GetCue("die");
                    break;

            }
            if (cue != null)
                if (cue.IsPrepared)
                    cue.Play();
        }

        public override void collisionEntered(Collider collider)
        {
            base.collisionEntered(collider);

            if (isWalking())
            {
                if (!Sprite.isFlipped)
                {
                    Sprite.position.X -= Sprite.velocity.X;
                    Sprite.velocity.X = 0;
                }
                else
                {
                    Sprite.position.X += Sprite.velocity.X;
                    Sprite.velocity.X = 0;
                }
                //Sprite.position.X -= Sprite.velocity.X;
                //Sprite.position.X += -Sprite.velocity.X;
                //setStance(Stances.Stance.FightingStance);

            }

            

            if (collider.GameObject.GetType() == typeof(StrikeGameObject))
            {
                hitsTakenConsecutively++;

                StrikeGameObject strike = (StrikeGameObject)collider.GameObject;
                
                GameObjectManager.removeGameObject(collider.GameObject);

                //skpi if hit after dizzy
                if (actualStance == Stances.Stance.FallBackward && Sprite.life <= 0)
                    return;

                if (actualStance == Stances.Stance.Dizzy){
                    setStance(Stances.Stance.FallBackward);                    
                }
                else
                {

                    Sprite.acceleration = strike.Acceleration;
                    Sprite.velocity = strike.Velocity;

                    if (hitsTakenConsecutively > 5)
                    {
                        Sprite.acceleration = new Vector2(0.06f, 0.0f);
                        Sprite.velocity = new Vector2(-4f, 0.0f);
                        //skip if already took various hits
                        if (actualStance == Stances.Stance.HitWalk)
                        {
                            return;
                        }
                        else
                        {
                            Sprite.life -= strike.Damage;
                            setStance(Stances.Stance.HitWalk);
                        }
                    }
                    else
                    {
                        Sprite.life -= strike.Damage;

                        if (strike.Type == StrikeGameObject.StrikeType.KickHigh || strike.Type == StrikeGameObject.StrikeType.KickLow)
                        {
                            setStance(Stances.Stance.HitWalk);
                        }
                        if (strike.Type == StrikeGameObject.StrikeType.PunchLow)
                        {
                            setStance(Stances.Stance.HitLow);
                        }
                        if (strike.Type == StrikeGameObject.StrikeType.PunchHigh)
                        {
                            setStance(Stances.Stance.HitHigh);
                        }
                    }
                }
                

            }

            
        }
    }
}
