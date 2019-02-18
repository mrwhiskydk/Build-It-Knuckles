using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Build_It_Knuckles
{
    /// <summary>
    /// Public Class that represents the default functionality and game logic of the Worker GameObject
    /// </summary>
    public class Worker : AnimatedGameObject
    {
        public bool selected = false;

        /// <summary>
        /// Worker's Constructor that sets the frame count, animations player per second, the starting position and sprite name, of the current Worker GameObject
        /// </summary>
        public Worker() : base(3, 10, new Vector2(600,300), "knuckles")
        {

        }

        /// <summary>
        /// Updates the Worker game logic
        /// </summary>
        /// <param name="gameTime">Time elapsed since last call in the update</param>
        public override void Update(GameTime gameTime)
        {
            if (GameWorld.mouse.Click(this))
            {
                selected = true;
            }

            if (selected && GameWorld.mouse.Click(GameWorld.Resource))
            {
                Vector2 direction;
                direction = GameWorld.Resource.Position - position;
                direction.Normalize();
                position += direction * 4;
            }
            base.Update(gameTime);
        }

        public override void DoCollision(GameObject otherObject)
        {
            base.DoCollision(otherObject);

            if (otherObject is Resource)
            {

            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (selected == false)
            {
                spriteBatch.Draw(sprite, position, animationRectangles[currentAnimationIndex], Color.White, rotation, new Vector2(animationRectangles[currentAnimationIndex].Width * 0.5f, animationRectangles[currentAnimationIndex].Height * 0.5f), 1f, new SpriteEffects(), 0f);
            }
            else
            {
                spriteBatch.Draw(sprite, position, animationRectangles[currentAnimationIndex], Color.Blue, rotation, new Vector2(animationRectangles[currentAnimationIndex].Width * 0.5f, animationRectangles[currentAnimationIndex].Height * 0.5f), 1f, new SpriteEffects(), 0f);
            }
        }
    }
}