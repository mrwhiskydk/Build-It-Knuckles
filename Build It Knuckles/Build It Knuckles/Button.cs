using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Build_It_Knuckles
{
    /// <summary>
    /// Public Class that represents the default functionality and game logic of the Button GameObject
    /// </summary>
    public class Button : GameObject
    {


        /// <summary>
        /// Button's Constructor that sets the starting position and sprite name of the current Button GameObject
        /// </summary>
        /// <param name="startPosition">The default position of where the current Button is set in the game, on the X and Y Axis</param>
        /// <param name="spriteName">The default name of the Button sprite</param>
        public Button(Vector2 startPosition, string spriteName) : base(startPosition, spriteName)
        {

        }

        /// <summary>
        /// Updates the Button game logic
        /// </summary>
        /// <param name="gameTime">Time elapsed since last call in the update</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}