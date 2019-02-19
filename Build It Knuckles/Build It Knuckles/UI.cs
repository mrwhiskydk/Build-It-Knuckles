using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Build_It_Knuckles
{
    /// <summary>
    /// Public Class that represents the default functionality and game logic of the UI GameObjectPassive
    /// </summary>
    public class UI : GameObjectPassive
    {
        public static Vector2 buttonBuyHousePos = new Vector2(GameWorld.ScreenSize.Width - 100, GameWorld.ScreenSize.Height - 100);

        public static Vector2 buttonBuyWorkerPos = new Vector2(GameWorld.ScreenSize.Width - 100, GameWorld.ScreenSize.Height - 200);
        /// <summary>
        /// UI's Constructor that sets the starting position and sprite of the UI GameObjectPassive
        /// </summary>
        /// <param name="startPosition">The default position of where the UI is set in the game, on the X and Y Axis</param>
        /// <param name="spriteName">The default name of the UI sprite</param>
        public UI() : base(Vector2.Zero, "UI")
        {
            position = new Vector2(GameWorld.ScreenSize.Width * 0.5f, GameWorld.ScreenSize.Height - sprite.Height * 0.5f);
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