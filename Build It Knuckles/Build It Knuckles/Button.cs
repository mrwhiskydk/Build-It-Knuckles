using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, rotation, new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f), 1f, SpriteEffects.None, 0.992f);
        }
    }

    public class ButtonBuyHouse : Button
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ButtonBuyHouse() : base(UI.buttonBuyHousePos, "iconhouse")
        {

        }

        /// <summary>
        /// Button has been pressed to build a house. First check if we have enough resources and if then build a house
        /// </summary>
        public override void Action()
        {
            base.Action();
            if (TownHall.gold >= House.costGold && TownHall.stone >= House.costStone && TownHall.lumber >= House.costLumber && House.houses < House.housesMax)
            {
                TownHall.gold -= House.costGold;
                TownHall.stone -= House.costStone;
                TownHall.lumber -= House.costLumber;
                new House();
            }
        }
    }

    public class ButtonBuyWorker : Button
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ButtonBuyWorker() : base(UI.buttonBuyWorkerPos, "iconworker")
        {

        }

        /// <summary>
        /// Button has been pressed. Build a worker if we have enough resources and worker count less than max worker count
        /// </summary>
        public override void Action()
        {
            base.Action();

            if (TownHall.gold >= 20 && TownHall.food >= 20 && Worker.workers < TownHall.population)
            {
                TownHall.gold -= 20;
                TownHall.food -= 20;
                //change worker position for every new worker spawned so they dont spawn on top of each other
                if (Worker.workerPosX <= 1200)
                {
                    Worker.workerPosX += 50;
                }
                else
                {
                    Worker.workerPosX = 600;
                }
                new Worker();
            }
        }

    }
}