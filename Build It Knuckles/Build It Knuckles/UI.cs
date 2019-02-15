using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Build_It_Knuckles
{
    /// <summary>
    /// Public Class that represents the default functionality and game logic of the UI PassiveGameObject
    /// </summary>
    public class UI : GameObjectPassive
    {


        public UI(Vector2 startPosition, string spriteName) : base(startPosition, spriteName)
        {
        }
    }
}