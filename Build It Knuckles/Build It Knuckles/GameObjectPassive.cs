using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Build_It_Knuckles
{
    /// <summary>
    /// Public Class that represents the default functionality and game logic of the current GameObjectPassive
    /// </summary>
    public class GameObjectPassive
    {

        /// <summary>
        /// The sprite texture of the GameObjectPassive
        /// </summary>
        protected Texture2D sprite;
        /// <summary>
        /// The rotation of the GameObjectPassive
        /// </summary>
        protected float rotation;
        /// <summary>
        /// The position of the GameObjectPassive
        /// </summary>
        public Vector2 position;

        /// <summary>
        /// Property for the position of current GameObject
        /// </summary>
        public Vector2 Position { get => position; set => position = value; }


        /// <summary>
        /// The default constructor for a GameObject
        /// </summary>
        /// <param name="spriteName">The name of the texture resource the should be used for the sprite</param>
        public GameObjectPassive(string spriteName) : this(Vector2.Zero, spriteName)
        {

        }

        /// <summary>
        /// Constructor that sets the starting position of the GameObject
        /// </summary>
        /// <param name="startPosition">Start position</param>
        /// <param name="spriteName">The name of the texture resource the should be used for the sprite</param>
        public GameObjectPassive(Vector2 startPosition, string spriteName)
        {
            position = startPosition;
            sprite = GameWorld.ContentManager.Load<Texture2D>(spriteName);
            GameWorld.gameObjectPassive.Add(this);
        }

        /// <summary>
        /// Empty
        /// </summary>
        /// <param name="gameTime">Time elapsed since last call in the update</param>
        public virtual void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Detroys this GameObject
        /// </summary>
        public virtual void Destroy()
        {
            GameWorld.RemoveGameObjectPassive(this);
        }

        /// <summary>
        /// Enables the GameObject to be drawn. The std. functionality is to draw its sprite.
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to use for drawing</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, new Vector2(position.X, position.Y), null, Color.White, rotation, new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f), 1f, SpriteEffects.None, 0.991f);
        }

    }
}