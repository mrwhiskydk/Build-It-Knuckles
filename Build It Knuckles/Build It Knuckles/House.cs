using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Build_It_Knuckles
{
    /// <summary>
    /// Public Class that represents the default functionality and game logic of the House GameObject
    /// </summary>
    public class House : GameObject
    {


        /// <summary>
        /// The House's Constructor that sets the starting position and sprite name of the current House GameObject
        /// </summary>
        /// <param name="startPosition">The default position of where the current House is set in the game, on the X and Y Axis</param>
        /// <param name="spriteName">The default name of the House sprite</param>
        public House(Vector2 startPosition, string spriteName) : base(startPosition, spriteName)
        {
        }
    }
}