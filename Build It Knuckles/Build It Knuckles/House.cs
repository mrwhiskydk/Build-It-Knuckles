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
        /// Sets the default cost amount of Gold, required to buy a House
        /// </summary>
        public static int costGold = 50;

        /// <summary>
        /// Sets the default cost amount of Stone, required to buy a House
        /// </summary>
        public static int costStone = 20;

        /// <summary>
        /// Sets the default cost amount of Lumber, required to buy a House
        /// </summary>
        public static int costLumber = 30;

        /// <summary>
        /// Sets the current amount of House GameObjects, instantiated within the game 
        /// </summary>
        public static int houses;

        /// <summary>
        /// Sets the maximum value amount of houses, currently able of being instantiated within the game
        /// </summary>
        public static int housesMax = 4;

        private static int thx = (int)TownHall.pos.X;
        private static int thy = (int)TownHall.pos.Y;
        private static int xSpacing = 150;
        private static Vector2[] housepos = { new Vector2(thx - xSpacing, thy), new Vector2(thx + xSpacing, thy), new Vector2(thx - xSpacing*2, thy), new Vector2(thx + xSpacing*2, thy) };

        /// <summary>
        /// The House's Constructor that sets the starting position and sprite name of the current House GameObject
        /// </summary>
        /// <param name="startPosition">The default position of where the current House is set in the game, on the X and Y Axis</param>
        /// <param name="spriteName">The default name of the House sprite</param>
        public House() : base(housepos[houses], "house")
        {
            houses++;
            TownHall.population += 2;

            //Double price of house after each built house
            costGold *= houses + 1;
            costStone *=  houses + 1;
            costLumber *= houses + 1;
        }

        /// <summary>
        /// Updates the House game logic
        /// </summary>
        /// <param name="gameTime">Time elapsed since last call in the update</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}