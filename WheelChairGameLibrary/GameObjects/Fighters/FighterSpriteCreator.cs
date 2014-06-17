using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MortalKomatG.GameObjects;

namespace MortalKomatG
{
    class FighterSpriteCreator
    {

        public Dictionary<FighterGameObject.FighterType, Dictionary<Stances.Stance, SpriteAnimation>> spriteAnimations =
            new Dictionary<FighterGameObject.FighterType, Dictionary<Stances.Stance, SpriteAnimation>>();


        public SpriteAnimation getSpriteAnimation(FighterGameObject.FighterType fighterType, Stances.Stance stance)
        {
            return spriteAnimations[fighterType][stance];
        }

        public FighterSpriteCreator()
        {
            #region Scorpion
            spriteAnimations.Add(FighterGameObject.FighterType.Scorpion, new Dictionary<Stances.Stance, SpriteAnimation>());

            spriteAnimations[FighterGameObject.FighterType.Scorpion].Add(Stances.Stance.FightingStance, new SpriteAnimation(
                 new SpriteAnimationData[]{  
                                         new SpriteAnimationData( 738, 139, 71, 138, 0, 0),
                                         new SpriteAnimationData( 735, 277, 70, 137, 1, 1),
                                         new SpriteAnimationData( 664, 277, 71, 136, 1, 2),
                                         new SpriteAnimationData( 770, 0, 73, 134, 2, 4),
                                         new SpriteAnimationData( 592, 270, 72, 135, 2, 3),
                                         new SpriteAnimationData( 0, 463, 70, 136, 2, 2),
                                         new SpriteAnimationData( 514, 277, 72, 138, 0, 0)
                                                   }));

            spriteAnimations[FighterGameObject.FighterType.Scorpion].Add(Stances.Stance.Dizzy, new SpriteAnimation(new SpriteAnimationData[]{  
                                         new SpriteAnimationData( 530, 695, 58, 130, 9, 8),
                                         new SpriteAnimationData( 471, 679, 59, 130, 5, 8),
                                         new SpriteAnimationData( 339, 703, 60, 128, 4, 10),
                                         new SpriteAnimationData( 771, 548, 53, 127, 11, 11),
                                         new SpriteAnimationData( 795, 414, 51, 131, 15, 7),
                                         new SpriteAnimationData( 717, 554, 54, 133, 12, 5),
                                         new SpriteAnimationData( 588, 696, 54, 133, 10, 5)
                                                   }));

            spriteAnimations[FighterGameObject.FighterType.Scorpion].Add(Stances.Stance.PunchGo, new SpriteAnimation(new SpriteAnimationData[]{  
                                         new SpriteAnimationData( 601, 557, 58, 139, 13, 0)                                         
                                                   }));

            spriteAnimations[FighterGameObject.FighterType.Scorpion].Add(Stances.Stance.PunchBack, new SpriteAnimation(new SpriteAnimationData[]{  
                                         new SpriteAnimationData( 601, 557, 58, 139, 13, 0)                                         
                                                   }));

            spriteAnimations[FighterGameObject.FighterType.Scorpion].Add(Stances.Stance.PunchLow1, new SpriteAnimation(new SpriteAnimationData[]{  
                                         //new SpriteAnimationData( 601, 557, 58, 139, 13, 0),                         
                                         new SpriteAnimationData( 537, 0, 80, 140, 9, 0),
                                         new SpriteAnimationData( 355, 0, 102, 139, 8, 1, true),
                                         
                                                   }));

            spriteAnimations[FighterGameObject.FighterType.Scorpion].Add(Stances.Stance.PunchLow2, new SpriteAnimation(new SpriteAnimationData[]{  
                                         //new SpriteAnimationData( 407, 566, 64, 141, 14, -4),                         
                                         new SpriteAnimationData( 457, 0, 80, 140, 9, -2),
                                         new SpriteAnimationData( 244, 139, 98, 139, 9, -1, true)                                        
                                                   }));

            spriteAnimations[FighterGameObject.FighterType.Scorpion].Add(Stances.Stance.PunchHigh1, new SpriteAnimation(new SpriteAnimationData[] {                                         
                                         //new SpriteAnimationData( 601, 557, 58, 139, 13, 0),
                                         new SpriteAnimationData( 100, 425, 69, 141, 13, -2),
                                         new SpriteAnimationData( 252, 0, 103, 139, 12, 0, true),                                         
                                                   }));

            spriteAnimations[FighterGameObject.FighterType.Scorpion].Add(Stances.Stance.PunchHigh2, new SpriteAnimation(new SpriteAnimationData[] {                                         
                                         //new SpriteAnimationData( 407, 566, 64, 141, 14, -3),
                                         new SpriteAnimationData( 358, 281, 81, 143, 14, -4),
                                         new SpriteAnimationData( 342, 139, 93, 142, 14, -3, true)
                                                   }));

            spriteAnimations[FighterGameObject.FighterType.Scorpion].Add(Stances.Stance.KickLow, new SpriteAnimation(new SpriteAnimationData[] {                                         
                                         new SpriteAnimationData( 673, 413, 62, 141, 8, -2),
                                         new SpriteAnimationData( 544, 415, 66, 142, 20, -4),
                                         new SpriteAnimationData( 100, 310, 94, 115, 15, 14),                                          
                                         new SpriteAnimationData( 0, 89, 125, 122, 14, 7, true),
                                         new SpriteAnimationData( 477, 554, 63, 125, 18, 4),
                                         new SpriteAnimationData( 666, 139, 72, 138, 13, -1),                                          
                                                   }));

            spriteAnimations[FighterGameObject.FighterType.Scorpion].Add(Stances.Stance.KickHigh, new SpriteAnimation(new SpriteAnimationData[] {                                         
                                         new SpriteAnimationData( 673, 413, 62, 141, 8, -2),
                                         new SpriteAnimationData( 544, 415, 66, 142, 20, -4),
                                         new SpriteAnimationData( 100, 310, 94, 115, 15, 14),                                         
                                         new SpriteAnimationData( 125, 116, 119, 125, 11, 4, true),
                                         new SpriteAnimationData( 477, 554, 63, 125, 18, 4),
                                         new SpriteAnimationData( 666, 139, 72, 138, 13, -1),                                            
                                                   }));

            spriteAnimations[FighterGameObject.FighterType.Scorpion].Add(Stances.Stance.WalkingForward, new SpriteAnimation(new SpriteAnimationData[] {                                         
                                         new SpriteAnimationData( 139, 587, 67, 142, 0, -1),
                                         new SpriteAnimationData( 276, 390, 67, 143, 0, -1),
                                         new SpriteAnimationData( 273, 673, 66, 145, 1, -2),
                                         new SpriteAnimationData( 206, 587, 67, 141, 0, -2),
                                         new SpriteAnimationData( 477, 415, 67, 139, 0, -1),
                                         new SpriteAnimationData( 273, 533, 67, 140, 0, 0),
                                         new SpriteAnimationData( 69, 694, 67, 142, 0, 0),
                                         new SpriteAnimationData( 343, 424, 67, 142, 0, -1),
                                         new SpriteAnimationData( 194, 310, 82, 141, 0, -2)
                                                   }));


            spriteAnimations[FighterGameObject.FighterType.Scorpion].Add(Stances.Stance.WalkingBackward, new SpriteAnimation(new SpriteAnimationData[] {                                         
                                         new SpriteAnimationData( 194, 310, 82, 141, 0, -2),
                                         new SpriteAnimationData( 343, 424, 67, 142, 0, -1),
                                         new SpriteAnimationData( 69, 694, 67, 142, 0, 0),
                                         new SpriteAnimationData( 273, 533, 67, 140, 0, 0),
                                         new SpriteAnimationData( 477, 415, 67, 139, 0, -1),
                                         new SpriteAnimationData( 206, 587, 67, 141, 0, -2),
                                         new SpriteAnimationData( 273, 673, 66, 145, 1, -2),
                                         new SpriteAnimationData( 276, 390, 67, 143, 0, -1),
                                         new SpriteAnimationData( 139, 587, 67, 142, 0, -1)
                                                   }));

            spriteAnimations[FighterGameObject.FighterType.Scorpion].Add(Stances.Stance.VictoryPose, new SpriteAnimation(new SpriteAnimationData[] {                                         
                                         new SpriteAnimationData( 0, 211, 100, 139, -13, -1),
                                         new SpriteAnimationData( 696, 0, 74, 139, -4, -1),
                                         new SpriteAnimationData( 410, 424, 67, 139, 3, -1)
                                                   }));

            spriteAnimations[FighterGameObject.FighterType.Scorpion].Add(Stances.Stance.Win, new SpriteAnimation(new SpriteAnimationData[] {                                         
                                         new SpriteAnimationData( 410, 424, 67, 139, 3, -1)
                                                   }));

            spriteAnimations[FighterGameObject.FighterType.Scorpion].Add(Stances.Stance.HitLow, new SpriteAnimation(new SpriteAnimationData[] {                                         
                                         new SpriteAnimationData( 169, 451, 69, 136, 0, 1),
                                         new SpriteAnimationData( 340, 566, 67, 137, 3, 2),
                                         new SpriteAnimationData( 439, 280, 74, 134, -4, 5)
                                                   }));

            spriteAnimations[FighterGameObject.FighterType.Scorpion].Add(Stances.Stance.HitHigh, new SpriteAnimation(new SpriteAnimationData[] {                                         
                                         new SpriteAnimationData( 0, 599, 69, 136, 1, 1),
                                         new SpriteAnimationData( 592, 140, 74, 130, -4, 7),
                                         new SpriteAnimationData( 617, 0, 79, 125, -9, 12)
                                                   }));

            spriteAnimations[FighterGameObject.FighterType.Scorpion].Add(Stances.Stance.HitWalk, new SpriteAnimation(new SpriteAnimationData[] {                                         
                                         new SpriteAnimationData( 70, 566, 69, 128, 2, 9),
                                         new SpriteAnimationData( 540, 557, 61, 138, 0, -1),
                                         new SpriteAnimationData( 659, 554, 58, 136, 0, 1),
                                         new SpriteAnimationData( 735, 414, 60, 134, 0, 3),
                                         new SpriteAnimationData( 610, 413, 63, 137, 0, 0),
                                         new SpriteAnimationData( 514, 140, 78, 137, 0, 0),
                                                   }));

            spriteAnimations[FighterGameObject.FighterType.Scorpion].Add(Stances.Stance.FallBackward, new SpriteAnimation(new SpriteAnimationData[] {                                         
                                         new SpriteAnimationData( 399, 707, 59, 126, 10, 10),
                                         new SpriteAnimationData( 0, 350, 100, 113, -10, 11),
                                         new SpriteAnimationData( 100, 241, 100, 69, -12, 34),
                                         new SpriteAnimationData( 276, 281, 82, 109, -9, 26),
                                         new SpriteAnimationData( 131, 41, 121, 75, -22, 75),
                                         new SpriteAnimationData( 0, 41, 131, 48, -22, 96)
                                         //new SpriteAnimationData( 0, 0, 139, 41, -23, 107)
                                                   }));

            spriteAnimations[FighterGameObject.FighterType.Scorpion].Add(Stances.Stance.Dead, new SpriteAnimation(new SpriteAnimationData[] {                                         
                                         new SpriteAnimationData( 0, 0, 139, 41, -23, 107)
                                                   }));










            spriteAnimations.Add(FighterGameObject.FighterType.LiuKang, new Dictionary<Stances.Stance, SpriteAnimation>());

            spriteAnimations[FighterGameObject.FighterType.LiuKang].Add(Stances.Stance.FightingStance, new SpriteAnimation(
                 new SpriteAnimationData[]{  
                                         new SpriteAnimationData( 0, 0, 71, 134, 0, 1),
                                         new SpriteAnimationData( 72, 0, 71, 133, 0, 2),
                                         new SpriteAnimationData( 144, 0, 71, 132, 0, 3),
                                         new SpriteAnimationData( 429, 0, 70, 132, 0, 3),
                                         new SpriteAnimationData( 358, 0, 70, 133, 0, 2),
                                         new SpriteAnimationData( 287, 0, 70, 134, 0, 1),
                                         new SpriteAnimationData( 216, 0, 70, 135, 0, 0)
                                                   }));




            spriteAnimations.Add(FighterGameObject.FighterType.KungLao, new Dictionary<Stances.Stance, SpriteAnimation>());

            spriteAnimations[FighterGameObject.FighterType.KungLao].Add(Stances.Stance.FightingStance, new SpriteAnimation(
                 new SpriteAnimationData[]{  
                                         new SpriteAnimationData( 72, 0, 70, 140, 1, -1),
                                         new SpriteAnimationData( 143, 0, 69, 139, 2, 1),
                                         new SpriteAnimationData( 351, 0, 66, 137, 5, 3),
                                         new SpriteAnimationData( 418, 0, 65, 135, 6, 5),
                                         new SpriteAnimationData( 0, 141, 65, 135, 6, 5),
                                         new SpriteAnimationData( 283, 0, 67, 137, 4, 3),
                                         new SpriteAnimationData( 0, 0, 71, 140, 0, -1),

                                                   }));

            #endregion
        }



    }
}
