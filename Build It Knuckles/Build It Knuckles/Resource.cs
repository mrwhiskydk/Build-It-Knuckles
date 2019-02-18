using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;

namespace Build_It_Knuckles
{
    /// <summary>
    /// Public Class that represents the default functionality and game logic of the Resource GameObject
    /// </summary>
    public class Resource : GameObject
    {
        /// <summary>
        /// Semaphore Class that that contains the initial and maximum number amount of worker entries
        /// </summary>
        private Semaphore ResourceSemaphore;

        /// <summary>
        /// Resource's Constructor that sets the default starting position and sprite name of the current Resource GameObject
        /// </summary>
        /// <param name="startPosition">The default position of where the current Resource is set in the game, on the X and Y Axis</param>
        /// <param name="spriteName">The default name of the Resource sprite</param>
        public Resource() : base(new Vector2(600, 100), "castle")
        {
            //Sets the Semaphores initial number of entries as 0, and a maximum capacity amount as 3
            ResourceSemaphore = new Semaphore(0, 3);
        }

        /// <summary>
        /// Updates the Resource game logic
        /// </summary>
        /// <param name="gameTime">Time elapsed since last call in the update</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}