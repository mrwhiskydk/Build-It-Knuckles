﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Build_It_Knuckles
{
    /// <summary>
    /// Public Class that represents the default functionality and game logic of the TownHall GameObject
    /// </summary>
    public class TownHall : GameObject
    {

        /// <summary>
        /// The TownHall Constructor, that sets the default starting position and sprite name of the current TownHall GameObject
        /// </summary>
        /// <param name="startPosition">The default position of where the TownHall is set in the game, on the X and Y Axis</param>
        /// <param name="spriteName">The default name of the TownHall sprite</param>
        public TownHall(Vector2 startPosition, string spriteName) : base(startPosition, spriteName)
        {
        }
    }
}