using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Build_It_Knuckles
{
    delegate void DeadEventhandler(Worker worker);
    
    /// <summary>
    /// Public Class that represents the default functionality and game logic of the Worker GameObject
    /// </summary>
    public class Worker : AnimatedGameObject
    {
        public bool selected = false;

        
        //Sets the moving speed amount for the current Worker GameObject
        private float movementSpeed;

        /// <summary>
        /// Resource List, that contains the value amount of resources, within the current Worker GameObject
        /// </summary>
        private List<int> resourceAmount;

        /// <summary>
        /// Sets an event, for when current Worker GameObject "dies".
        /// This is event is set to trigger should health reach or go below 0, makes the current Worker run away, and is then removed from the game
        /// </summary>
        private event DeadEventhandler DeadEvent;

        

        //Defines the health variable of current Worker GameObject
        private int health;

        /// <summary>
        /// Proberty that sets and returns the health value of current Worker GameObject.
        /// Also checks if the current health value is at or below 0, in order to trigger the current Workers event: 'DeadEvent'
        /// </summary>
        public int Health
        {
            get
            {
                return health;  //Returns the health value
            }
            set
            {
                health = value; //Sets the health variable as its value
                //Checks if current Worker health is at or below a value of 0
                if(health <= 0)
                {
                    OnDeadEvent();  //If true, the 'DeadEvent' of current Worker triggers
                }
            }
        }

        /// <summary>
        /// Worker's Constructor that sets the frame count, animations player per second, the starting position and sprite name, of the current Worker GameObject
        /// </summary>
        public Worker() : base(3, 10, new Vector2(600,300), "knuckles")
        {
            health = 100;   //Worker Health / Patience before running away, is set to X as default
            movementSpeed = 15; //Worker moving speed amount is set to X as default
            resourceAmount = new List<int>();   //New resource list is created, for the current Worker GameObject, upon being added to the game

            
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

        /// <summary>
        /// Method that checks if the event: 'DeadEvent' is currently existing, and sets the event onto the current Worker GameObject if value is set true
        /// </summary>
        protected virtual void OnDeadEvent()
        {
            if(DeadEvent != null)
            {
                DeadEvent(this);
            }
        }

        /// <summary>
        /// Collision Method, that checks wether or not the current Worker GameObject has collided with anohter GameObject's collision
        /// </summary>
        /// <param name="otherObject"></param>
        public override void DoCollision(GameObject otherObject)
        {
            base.DoCollision(otherObject);
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