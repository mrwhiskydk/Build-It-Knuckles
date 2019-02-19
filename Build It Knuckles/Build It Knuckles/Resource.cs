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
        /// Semaphore Class that that contains & sets the initial and maximum amount of worker entries
        /// </summary>
        public Semaphore ResourceSemaphore = new Semaphore(0, 3);

        public int type;
        /// <summary>
        /// Resource's Constructor that sets the default starting position and sprite name of the current Resource GameObject
        /// </summary>
        /// <param name="startPosition">The default position of where the current Resource is set in the game, on the X and Y Axis</param>
        /// <param name="spriteName">The default name of the Resource sprite</param>
        public Resource(int type) : base(new Vector2(600, 100), "castle")
        {
            ResourceSemaphore.Release(3);
            this.type = type;
            
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