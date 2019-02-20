using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Build_It_Knuckles
{
    /// <summary>
    /// Public Class that represents the default functionality and game logic of the UI GameObjectPassive
    /// </summary>
    public class UI : GameObjectPassive
    {
        public static Vector2 buttonBuyHousePos;

        public static Vector2 buttonBuyWorkerPos;
        /// <summary>
        /// UI's Constructor that sets the starting position and sprite of the UI GameObjectPassive
        /// </summary>
        /// <param name="startPosition">The default position of where the UI is set in the game, on the X and Y Axis</param>
        /// <param name="spriteName">The default name of the UI sprite</param>
        public UI() : base(Vector2.Zero, "UI")
        {
            position = new Vector2(GameWorld.ScreenSize.Width * 0.5f, GameWorld.ScreenSize.Height - sprite.Height * 0.5f);
            buttonBuyHousePos = new Vector2(position.X + sprite.Width * 0.5f - 52, position.Y + sprite.Height * 0.5f - 48);
            buttonBuyWorkerPos = new Vector2(position.X + sprite.Width * 0.5f - 128, position.Y + sprite.Height * 0.5f - 48);
        }

        /// <summary>
        /// Updates the UI game logic
        /// </summary>
        /// <param name="gameTime">Time elapsed since last call in the update</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.DrawString(GameWorld.fontUI, TownHall.gold.ToString(), new Vector2(position.X - sprite.Width * 0.5f + 100, position.Y - 15), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.993f);
            spriteBatch.DrawString(GameWorld.fontUI, TownHall.stone.ToString(), new Vector2(position.X - sprite.Width * 0.5f + 350, position.Y - 15), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.993f);
            spriteBatch.DrawString(GameWorld.fontUI, TownHall.lumber.ToString(), new Vector2(position.X - sprite.Width * 0.5f + 600, position.Y - 15), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.993f);
            spriteBatch.DrawString(GameWorld.fontUI, TownHall.food.ToString(), new Vector2(position.X - sprite.Width * 0.5f + 850, position.Y - 15), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.993f);
        }
    }
}