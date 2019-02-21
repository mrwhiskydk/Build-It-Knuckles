using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Build_It_Knuckles
{
    /// <summary>
    /// Public Class that represents the default functionality and game logic of the Cursor GameObject
    /// </summary>
    public class Cursor : GameObject
    {

        private MouseState oldMouseState, currentMouseState;

        /// <summary>
        /// Cursor's Constructor that sets the default name of it's current sprite
        /// </summary>
        public Cursor() : base("Hand")
        {

        }

        /// <summary> 
        /// Updates the position of the crosshair to the mouse position
        /// </summary>
        /// <param name="gameTime">Time elapsed since last call in the update</param>
        public override void Update(GameTime gameTime)
        {
            position = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);

            oldMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }

        /// <summary>
        /// Method used to draw the crosshair
        /// </summary>
        /// <param name="spriteBatch">The spritebatch used for drawing</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, rotation, new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f), 1f, SpriteEffects.None, 1f);
        }

        /// <summary>
        /// Perform a check to see if we have clicked on the object we are calling this method from
        /// </summary>
        /// <param name="obj">The object to check if we have clicked on</param>
        /// <returns>Boolean: true or false</returns>
        public bool Click(GameObject obj)
        {

            if (this.CollisionBox.Intersects(obj.CollisionBox) && Mouse.GetState().LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Overriden DoCollision Method, that checks if the Cursor GameObject has collided with another GameObject's CollisionBox.
        /// </summary>
        /// <param name="otherObject">The other GameObject, that is being collided with</param>
        public override void DoCollision(GameObject otherObject)
        {
            base.DoCollision(otherObject);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                if (otherObject is Button)
                {
                    Button b = (Button)otherObject;
                    b.Action();
                }
            }
        }
    }
}
