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
        /// The action that should be performed when the button is pressed
        /// </summary>
        public virtual void Action()
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

    public class ButtonBuyHouse : Button
    {
        public ButtonBuyHouse() : base(UI.buttonBuyHousePos, "house")
        {

        }

        public override void Action()
        {
            base.Action();
            if (TownHall.gold >= 10 && TownHall.stone >= 25 && TownHall.lumber >= 25 && House.houses < 4)
            {
                new House();

                TownHall.gold -= 10;
                TownHall.stone -= 25;
                TownHall.lumber -= 25;
                TownHall.population += 2;
            }
        }
    }

    public class ButtonBuyWorker : Button
    {
        public ButtonBuyWorker() : base(UI.buttonBuyWorkerPos, "Knuckles")
        {

        }

        public override void Action()
        {
            base.Action();

            if (TownHall.gold >= 20 && TownHall.food >= 20 && TownHall.population >= Worker.workers)
            {
                new Worker();

                TownHall.gold -= 20;
                TownHall.food -= 20;
            }
        }

    }
}