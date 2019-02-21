using System;
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

        public static int gold = 20;
        public static int stone = 0;
        public static int lumber = 0;
        public static int food = 20;
        public static int population = 4;

        public static Vector2 pos = new Vector2(GameWorld.ScreenSize.Width / 2, GameWorld.ScreenSize.Height / 2);

        /// <summary>
        /// The TownHall Constructor, that sets the default starting position and sprite name of the current TownHall GameObject
        /// </summary>
        /// <param name="startPosition">The default position of where the TownHall is set in the game, on the X and Y Axis</param>
        /// <param name="spriteName">The default name of the TownHall sprite</param>
        public TownHall() : base(pos, "townhall")
        {

        }

        /// <summary>
        /// Updates the game logic of the TownHall
        /// </summary>
        /// <param name="gameTime">Time elapsed since last call in the update</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}