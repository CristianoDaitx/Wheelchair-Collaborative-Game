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
    class StrikeGameObjectCreator
    {
        public static StrikeGameObject createStrikeGameObject(GameObjectManager gameObjectManager, String tag, Rectangle colliderRectangle, StrikeGameObject.StrikeType type)
        {
            switch (type)
            {
                case StrikeGameObject.StrikeType.KickHigh:
                    return new StrikeGameObject(gameObjectManager, tag, colliderRectangle, type, 9, new Vector2(0.06f, 0.0f), new Vector2(-4f, 0.0f));
                case StrikeGameObject.StrikeType.KickLow:
                    return new StrikeGameObject(gameObjectManager, tag, colliderRectangle, type, 7, new Vector2(0.06f, 0.0f), new Vector2(-2f, 0.0f));
                case StrikeGameObject.StrikeType.PunchLow:
                    return new StrikeGameObject(gameObjectManager, tag, colliderRectangle, type, 4, new Vector2(0.0f, 0.0f), new Vector2(0.0f, 0.0f));
                case StrikeGameObject.StrikeType.PunchHigh:
                    return new StrikeGameObject(gameObjectManager, tag, colliderRectangle, type, 4, new Vector2(0.0f, 0.0f), new Vector2(0.0f, 0.0f));            
            }
            return null;
        }
    }
}
