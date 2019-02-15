using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Build_It_Knuckles
{
    /// <summary>
    /// Public Class that represents the default functionality and game logic of the Resource GameObject
    /// </summary>
    public class Resource : GameObject
    {

        /// <summary>
        /// Resource's Constructor that sets the default starting position and sprite name of the current Resource GameObject
        /// </summary>
        /// <param name="startPosition">The default position of where the current Resource is set in the game, on the X and Y Axis</param>
        /// <param name="spriteName">The default name of the Resource sprite</param>
        public Resource() : base(new Vector2(600, 100), "castle")
        {
        }
    }
}