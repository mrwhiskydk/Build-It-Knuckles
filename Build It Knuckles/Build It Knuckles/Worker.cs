﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace Build_It_Knuckles
{
    delegate void DeadEventHandler(Worker worker);
    
    /// <summary>
    /// Public Class that represents the default functionality and game logic of the Worker GameObject
    /// </summary>
    public class Worker : AnimatedGameObject
    {
        public static int workers;
        /// <summary>
        /// Checks if a worker is selected by the player or not.
        /// </summary>
        public bool selected = false;

        /// <summary>
        /// List of all the sound effects that can be randomly played when a worker gets selected
        /// </summary>
        private List<Sound> soundList = new List<Sound>();
        private Random rnd = new Random();

        private bool startWork = false;

        /// <summary>
        /// Sets a starting position on the x-axis for the workers, that gets larger after each new worker, so the workers don't stack on eachother
        /// </summary>
        public static int workerPosX = 600;

        /// <summary>
        /// Checks if the worker is in its work loop and sets a value of true if worker is moving towards chosen Resource GameObject
        /// </summary>
        public bool working = false;

        /// <summary>
        /// Checks if a worker is mining gold
        /// </summary>
        private bool miningGold = false;

        /// <summary>
        /// Checks if the worker should be converting his resources to gold, when colliding with the townhall object
        /// </summary
        private bool carryingGold = false;

        /// <summary>
        /// Checks if a worker is mining stone
        /// </summary>
        private bool miningStone = false;

        /// <summary>
        /// Checks if the worker should be converting his resources to stone, when colliding with the townhall object
        /// </summary>
        private bool carryingStone = false;

        /// <summary>
        /// Checks if a worker is gathering food
        /// </summary>
        private bool gatheringFood = false;

        /// <summary>
        /// Checks if the worker should be converting his resources to food, when colliding with the townhall object
        /// </summary
        private bool carryingFood = false;

        /// <summary>
        /// Checks if a worker is chopping wood
        /// </summary>
        private bool choppingWood = false;

        /// <summary>
        /// Checks if the worker should be converting his resources to lumber, when colliding with the townhall object
        /// </summary
        private bool carryingLumber = false;


        private bool ignoreCollision = false;

        /// <summary>
        /// Sets the moving speed amount for the current Worker GameObject
        /// </summary>
        private float movementSpeed;

        /// <summary>
        /// The resource the worker is carrying.
        /// </summary>
        private int resourceAmount = 0;

        /// <summary>
        /// Property that sets and returns the resource amount value of current Worker GameObject.
        /// Also checks if the current resource value amount is at or above 50, in order to trigger the current Workers event: 'ResourceEvent'
        /// </summary>
        public int ResourceAmount
        {
            get
            {
                return resourceAmount;
            }
            set
            {
                resourceAmount = value;
            }
        }

        /// <summary>
        /// Sets an event, for when current Worker GameObject "dies".
        /// This event is set to trigger, should health reach or go below 0, makes the current Worker run away, and is then removed from the game
        /// </summary>
        private event DeadEventHandler DeadEvent;

        /// <summary>
        /// Thread that is instantiated through current Workers Constructor, set as a background thread and is set to Start when Worker collides with any
        /// Resource type for the first time.
        /// </summary>
        Thread workingThread;

        /// <summary>
        /// If the value of this bool is set true, current Worker is gathering resources and loses health every 1 second, until it reaches a resource value of 50.
        /// Used within the workingThreads while loop, in order to check wether or not current Worker is working within a Resource type
        /// </summary>
        private bool occupied;

        /// <summary>
        /// The value of this bool controls and maintains the lifeline of current Worker's workingThread.
        /// Thread is kept alive, as long as the value is set true. If set false, the worker will run away and is afterwards removed from the game
        /// </summary>
        private bool alive;

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
                
                if(health <= 0) //Checks if current Worker health is at or below a value of 0
                {
                    OnDeadEvent();  //If true, the 'DeadEvent' of current Worker triggers
                }
            }
        }

        /// <summary>
        /// Worker's Constructor that sets the frame count, animations player per second, the starting position and sprite name, of the current Worker GameObject
        /// </summary>
        public Worker() : base(3, 10, new Vector2(workerPosX,300), "knuckles")
        {
            health = 250;   //Worker Health / Patience before running away, is set to X as default
            movementSpeed = 4; //Worker moving speed amount is set to X as default
            occupied = false;   //Value is set false as default, since the instantiated Worker haven't reached/collided with a Resource type yet
            alive = true;   //Value is set true as default, since the Worker GameObject should be instantiated as alive

            workers++;

            DeadEvent += ReactToDead;   //Sets the ReactToDead Method, to take part of the event 'DeadEvent'

            for (int i = 1; i <= 8; i++)
            {
                soundList.Add(new Sound("sound/worker" + i));
            }
        }

        /// <summary>
        /// Updates the Worker game logic
        /// </summary>
        /// <param name="gameTime">Time elapsed since last call in the update</param>
        public override void Update(GameTime gameTime)
        {
            Selection(gameTime);
            WorkLoop(gameTime);
            base.Update(gameTime);
        }

        private void Selection(GameTime gameTime)
        {
            if (GameWorld.mouse.Click(this) && !occupied)   //Statement also checks if value of occupied is set false, in order to avoid selecting occupied workers within Resource type
            {
                selected = true;
                working = false;
                miningGold = false;
                miningStone = false;
                choppingWood = false;
                gatheringFood = false;

                
                int index = rnd.Next(0, 8);
                soundList[index].Play();
            }

            if (selected && GameWorld.mouse.Click(GameWorld.ResourceGold))
            {
                selected = false;
                working = true;
                miningGold = true;
            }
            else if (selected && GameWorld.mouse.Click(GameWorld.ResourceStone))
            {
                selected = false;
                working = true;
                miningStone = true;
            }
            else if (selected && GameWorld.mouse.Click(GameWorld.ResourceFood))
            {
                selected = false;
                working = true;
                gatheringFood = true;
            }
            else if (selected && GameWorld.mouse.Click(GameWorld.ResourceLumber))
            {
                selected = false;
                working = true;
                choppingWood = true;
            }
        }
        private void WorkLoop(GameTime gameTime)
        {                       
            if (working && ResourceAmount < 50 && alive)
            {
                Vector2 direction;
                if (miningGold)
                {
                    direction = GameWorld.ResourceGold.Position - position;
                    direction.Normalize();
                    position += direction * movementSpeed;
                }
                else if (miningStone)
                {
                    direction = GameWorld.ResourceStone.Position - position;
                    direction.Normalize();
                    position += direction * movementSpeed;
                }
                else if (gatheringFood)
                {
                    direction = GameWorld.ResourceFood.Position - position;
                    direction.Normalize();
                    position += direction * movementSpeed;
                }
                else if (choppingWood)
                {
                    direction = GameWorld.ResourceLumber.Position - position;
                    direction.Normalize();
                    position += direction * movementSpeed;
                }

            }
            else if (!working && ResourceAmount >= 50 && alive)
            {
                Vector2 direction;
                if (miningGold)
                {
                    direction = GameWorld.townHall.Position - GameWorld.ResourceGold.Position;
                    direction.Normalize();
                    position += direction * movementSpeed;
                }
                else if (miningStone)
                {
                    direction = GameWorld.townHall.Position - GameWorld.ResourceStone.Position;
                    direction.Normalize();
                    position += direction * movementSpeed;
                }
                else if (gatheringFood)
                {
                    direction = GameWorld.townHall.Position - GameWorld.ResourceFood.Position;
                    direction.Normalize();
                    position += direction * movementSpeed;
                }
                else if (choppingWood)
                {
                    direction = GameWorld.townHall.Position - GameWorld.ResourceLumber.Position;
                    direction.Normalize();
                    position += direction * movementSpeed;
                }
            }
            else if (working && ResourceAmount >= 50 && alive)
            {
                Vector2 direction;
                if (miningGold)
                {
                    direction = GameWorld.townHall.Position - Position;
                    direction.Normalize();
                    position += direction * movementSpeed;
                    if (ResourceAmount < 50)
                    {
                        direction = GameWorld.ResourceGold.Position - position;
                        direction.Normalize();
                        position += direction * movementSpeed;
                    }
                }
                else if (miningStone)
                {
                    direction = GameWorld.townHall.Position - Position;
                    direction.Normalize();
                    position += direction * movementSpeed;
                    if (ResourceAmount < 50)
                    {
                        direction = GameWorld.ResourceStone.Position - position;
                        direction.Normalize();
                        position += direction * movementSpeed;
                    }
                }
                else if (gatheringFood)
                {
                    direction = GameWorld.townHall.Position - Position;
                    direction.Normalize();
                    position += direction * movementSpeed;
                    if (ResourceAmount < 50)
                    {
                        direction = GameWorld.ResourceFood.Position - position;
                        direction.Normalize();
                        position += direction * movementSpeed;
                    }
                }
                else if (choppingWood)
                {
                    direction = GameWorld.townHall.Position - Position;
                    direction.Normalize();
                    position += direction * movementSpeed;
                    if (ResourceAmount < 50)
                    {
                        direction = GameWorld.ResourceLumber.Position - position;
                        direction.Normalize();
                        position += direction * movementSpeed;
                    }
                }
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
        /// Method that handles the functionality of current workingThread. The Thread's lifeline is kept alive, until the Worker's health reaches 0.
        /// Also handles the functionality gathering resources and losing health every 1 second, until a resource value of 50 has been reached
        /// </summary>
        private void EnterResource()
        {          

            if (miningGold)
            {
                GameWorld.ResourceGold.ResourceSemaphore.WaitOne(); //Worker waits to check if the Resource of Gold Semaphore has enough entries space, in order to enter
                position = GameWorld.ResourceGold.Position;                
            }
            else if (miningStone)
            {
                GameWorld.ResourceStone.ResourceSemaphore.WaitOne(); //Worker waits to check if the Resource of Stone Semaphore has enough entries space, in order to enter
                position = GameWorld.ResourceStone.Position;
            }
            else if (gatheringFood)
            {
                GameWorld.ResourceFood.ResourceSemaphore.WaitOne(); //Worker waits to check if the Resource of Food Semaphore has enough entries space, in order to enter
                position = GameWorld.ResourceFood.Position;
            }
            else if (choppingWood)
            {
                GameWorld.ResourceLumber.ResourceSemaphore.WaitOne(); //Worker waits to check if the Resource of Lumber Semaphore has enough entries space, in order to enter
                position = GameWorld.ResourceLumber.Position;
            }

            while (occupied)    //Worker continues to gather resources and loses health every 1 second as long as its current resource amount is below a value of 50
            {
                ResourceAmount += 10;
                Health -= 5;
                Thread.Sleep(1000);

                if (ResourceAmount >= 50)
                {
                    occupied = false;
                }

            }

            if (carryingGold)
            {
                GameWorld.ResourceGold.ResourceSemaphore.Release(); //Releases a spot inside the Gold Resource Semaphore
            }
            else if (carryingStone)
            {
                GameWorld.ResourceStone.ResourceSemaphore.Release(); //Releases a spot inside the Stone Resource Semaphore 
            }
            else if (carryingFood)
            {
                GameWorld.ResourceFood.ResourceSemaphore.Release(); //Releases a spot inside the Food Resource Semaphore 
            }
            else if (carryingLumber)
            {
                GameWorld.ResourceLumber.ResourceSemaphore.Release(); //Releases a spot inside the Lumber Resource Semaphore
            }
                       
        }

        /// <summary>
        /// Method that handles the functionality of ending the Thread: 'workingThread', by setting value of 'alive' bool as false.
        /// Checks which resource type the worker was working on, in order to release a spot in the Resource Semaphore, so that other Workers may take its place.
        /// Also removes the current Worker GameObject from the game.
        /// This Method is called through the event: 'DeadEvent'
        /// </summary>
        /// <param name="worker">The current Worker GameObject</param>
        private void ReactToDead(Worker worker)
        {
            if (occupied)
            {
                occupied = false;
            }

            alive = false;  //value of 'alive' is set false, in order to let current Worker know, that it should not proceed it's routine between Resources and Townhall (Checking through WorkLoop Method) 

            Thread fleeThread = new Thread(WorkerFleeing);
            fleeThread.IsBackground = true;
            fleeThread.Start();
            workers--;
        }

        private void WorkerFleeing()
        {
            bool flee = true;
            Vector2 direction;
            
            while (flee)
            {
                Thread.Sleep(5);
                if (resourceAmount >= 50)
                {
                    direction = GameWorld.townHall.Position - Position;
                    direction.Normalize();
                    position += direction * movementSpeed * 0.4f;
                }
                else
                {
                    direction = new Vector2(-100, 500) - position;
                    direction.Normalize();
                    position += direction * movementSpeed * 0.5f;
                }
                
                if(!GameWorld.ScreenSize.Intersects(CollisionBox))
                {
                    flee = false;             
                }
            }

            GameWorld.RemoveGameObject(this);
        }

        /// <summary>
        /// Collision Method, that checks wether or not the current Worker GameObject has collided with anohter GameObject's collision.
        /// Handles Worker collision with another Resource GameObject, to check what type on Resource it has collided with, in order for it to know which type of resource value it is currently gathering.
        /// Method also creates a new Thread, which is kept alive through bool: 'occupied'. Thread is set as Background and is started upon collision.
        /// Bool: 'ignoreCollision' is set true, for letting current Worker know, that it has already collided with an Resource, in order to avoid issues of trying to create an already instantiated Thread
        /// </summary>
        /// <param name="otherObject"></param>
        public override void DoCollision(GameObject otherObject)
        {
            base.DoCollision(otherObject);

            if (otherObject is Resource && !ignoreCollision)
            {
                Resource resource = (Resource)otherObject;

                if (resource.Equals(GameWorld.ResourceGold))
                {
                    carryingGold = true;
                    carryingStone = false;
                    carryingFood = false;
                    carryingLumber = false;
                }
                else if (resource.Equals(GameWorld.ResourceStone))
                {
                    carryingStone = true;
                    carryingGold = false;
                    carryingFood = false;
                    carryingLumber = false;
                }
                else if (resource.Equals(GameWorld.ResourceFood))
                {
                    carryingFood = true;
                    carryingGold = false;
                    carryingStone = false;
                    carryingLumber = false;
                }
                else if (resource.Equals(GameWorld.ResourceLumber))
                {
                    carryingLumber = true;
                    carryingFood = false;
                    carryingGold = false;
                    carryingStone = false;
                }

                working = false;
                occupied = true;

                workingThread = new Thread(EnterResource); //Instantiates a workingThread onto current Worker. Takes in the EnterResource method as it's parameters
                workingThread.IsBackground = true; //the instantiated workingThread is set as a background Thread as default.
                workingThread.Start();

                ignoreCollision = true;
            }
        
            if (otherObject is TownHall && ignoreCollision)
            {
                if (carryingGold)
                {
                   TownHall.gold += ResourceAmount;
                }
                else if (carryingStone)
                {
                    TownHall.stone += ResourceAmount;
                }
                else if (carryingFood)
                {
                    TownHall.food += ResourceAmount;
                }
                else if (carryingLumber)
                {
                    TownHall.lumber += ResourceAmount;
                }
                ResourceAmount = 0;
                working = true;
                ignoreCollision = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (selected)
            {
                spriteBatch.Draw(sprite, position, animationRectangles[currentAnimationIndex], Color.Blue, rotation, new Vector2(animationRectangles[currentAnimationIndex].Width * 0.5f, animationRectangles[currentAnimationIndex].Height * 0.5f), 1f, new SpriteEffects(), 0);
            }
            else
            {
                spriteBatch.Draw(sprite, position, animationRectangles[currentAnimationIndex], Color.White, rotation, new Vector2(animationRectangles[currentAnimationIndex].Width * 0.5f, animationRectangles[currentAnimationIndex].Height * 0.5f), 1f, new SpriteEffects(), 0f);
            }
        }
    }
}