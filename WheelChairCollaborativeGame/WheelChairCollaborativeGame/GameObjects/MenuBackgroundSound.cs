using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelChairGameLibrary;
using Microsoft.Xna.Framework.Audio;

namespace WheelChairCollaborativeGame.GameObjects
{
    class MenuBackgroundSound : GameObject
    {
        private SoundEffectInstance backgroundSong;

        public MenuBackgroundSound(GameEnhanced game, String tag)
            : base(game, tag)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            backgroundSong.IsLooped = true;
            backgroundSong.Play();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            backgroundSong = Game.Content.Load<SoundEffect>("menu").CreateInstance();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            backgroundSong.Dispose();
        }
    }
}
