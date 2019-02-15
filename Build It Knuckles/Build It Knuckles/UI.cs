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

        /// <summary>
        /// UI's Constructor that sets the starting position and sprite of the UI GameObjectPassive
        /// </summary>
        /// <param name="startPosition">The default position of where the UI is set in the game, on the X and Y Axis</param>
        /// <param name="spriteName">The default name of the UI sprite</param>
        public UI(Vector2 startPosition, string spriteName) : base(startPosition, spriteName)
        {

        }

        /// <summary>
        /// Updates the UI game logic
        /// </summary>
        /// <param name="gameTime">Time elapsed since last call in the update</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}