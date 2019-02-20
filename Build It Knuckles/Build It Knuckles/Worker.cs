using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace Build_It_Knuckles
{
    delegate void DeadEventhandler(Worker worker);
    delegate void ResourceAmountEventHandler(Worker worker);
    
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

        private bool startWork = false;

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
        /// Checks if a worker is mining stone
        /// </summary>
        private bool miningStone = false;

        /// <summary>
        /// Checks if a worker is gathering food
        /// </summary>
        private bool gatheringFood = false;

        /// <summary>
        /// Checks if a worker is chopping wood
        /// </summary>
        private bool choppingWood = false;

        private bool ignoreCollision = false;
        
        //Sets the moving speed amount for the current Worker GameObject
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
                if(resourceAmount >= 50)
                {
                    OnResourceEvent();
                }
            }
        }

        /// <summary>
        /// Sets an event, for when current Worker GameObject "dies".
        /// This event is set to trigger, should health reach or go below 0, makes the current Worker run away, and is then removed from the game
        /// </summary>
        private event DeadEventhandler DeadEvent;

        /// <summary>
        /// Sets an event, for letting the current Worker GameObject know, that it has left the current Resource Type. 
        /// This event is set to trigger once the Worker reaches a specific amount of resources, Releasing the Workers spot in current Semaphore
        /// </summary>
        private event ResourceAmountEventHandler ResourceEvent;

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
        public Worker() : base(3, 10, new Vector2(workerPosX,300), "knuckles")
        {
            health = 50;   //Worker Health / Patience before running away, is set to X as default
            movementSpeed = 4; //Worker moving speed amount is set to X as default
            occupied = false;   //Value is set false as default, since the instantiated Worker haven't reached/collided with a Resource type yet
            alive = true;   //Value is set true as default, since the Worker GameObject should be instantiated as alive
            workingThread = new Thread(EnterResource);  //Instantiates a workingThread onto current Worker. Takes in the EnterResource method as it's parameters
            workingThread.IsBackground = true;  //the instantiated workingThread is set as a background Thread as default.
            workers++;

            DeadEvent += ReactToDead;   //Sets the ReactToDead Method, to take part of the event 'DeadEvent'
            ResourceEvent += ReactToResource;   //Sets the ReactToResource Method, to take part of the event 'ResourceEvent'
        }

        /// <summary>
        /// Updates the Worker game logic
        /// </summary>
        /// <param name="gameTime">Time elapsed since last call in the update</param>
        public override void Update(GameTime gameTime)
        {
            if (GameWorld.mouse.Click(this) && !occupied)   //Statement also checks if value of occupied is set false, in order to avoid selecting occupied workers within Resource type
            {
                selected = true;
                working = false;
                miningGold = false;
                miningStone = false;
                choppingWood = false;
                gatheringFood = false;
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

            WorkLoop(gameTime);

            base.Update(gameTime);
        }

        private void WorkLoop(GameTime gameTime)
        {                       
            if (working && resourceAmount < 50 && alive)
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
            else if (working && resourceAmount >= 50 && alive)
            {
                Vector2 direction;
                if (miningGold)
                {
                    direction = GameWorld.townHall.Position - Position;
                    direction.Normalize();
                    position += direction * movementSpeed;
                    if (resourceAmount < 50)
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
                    if (resourceAmount < 50)
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
                    if (resourceAmount < 50)
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
                    if (resourceAmount < 50)
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
        /// Method that checks if the event: 'ResourceEvent' is currently existing, and sets the event onto the current Worker GameObject if value is set true
        /// </summary>
        protected virtual void OnResourceEvent()
        {
            if(ResourceEvent != null)
            {
                ResourceEvent(this);
            }
        }

        /// <summary>
        /// Method that handles the functionality of current workingThread. The Thread's lifeline is kept alive, until the Worker's health reaches 0.
        /// Also handles the functionality gathering resources and losing health every 1 second, until a resource value of 50 has been reached
        /// </summary>
        private void EnterResource()
        {          
            GameWorld.workerEnter = true;

            while (alive)
            {                
                while (occupied)
                {
                    ResourceAmount += 10;
                    Health -= 5;
                    Thread.Sleep(1000);

                    if (ResourceAmount == 50)
                    {                      
                        occupied = false;
                        
                    }
                }
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
            alive = false;  //value of 'alive' is set false, which kills the current workingThread

            GameWorld.workerLeft = true;

            Thread fleeThread = new Thread(WorkerFleeing);
            fleeThread.IsBackground = true;
            fleeThread.Start();

        }

        private void WorkerFleeing()
        {
            bool flee = true;
            Vector2 direction;
            
            while (flee)
            {
                Thread.Sleep(5);
                direction = new Vector2(120, 1000) - position;
                direction.Normalize();
                position += direction * movementSpeed; 
                
                if(position.Y >= 900)
                {
                    flee = false;
                }
            }

            GameWorld.RemoveGameObject(this);
        }

        /// <summary>
        /// Method that handles the functionality of checking which Resource type the current worker has left,
        /// in order to release a spot in the Resource Semaphore, so that other Workers may take its place.
        /// This Method is called through the event: 'Resource Event'
        /// </summary>
        /// <param name="worker">The current Worker GameObject</param>
        private void ReactToResource(Worker worker)
        {
            if (miningGold)
            {
                GameWorld.ResourceGold.ResourceSemaphore.Release(); //Releases a spot inside the Gold Resource Semaphore
            }
            else if (miningStone)
            {
                GameWorld.ResourceStone.ResourceSemaphore.Release(); //Releases a spot inside the Stone Resource Semaphore 
            }
            else if (gatheringFood)
            {
                GameWorld.ResourceFood.ResourceSemaphore.Release(); //Releases a spot inside the Food Resource Semaphore 
            }
            else if (choppingWood)
            {
                GameWorld.ResourceLumber.ResourceSemaphore.Release(); //Releases a spot inside the Lumber Resource Semaphore
            }

            GameWorld.workerLeft = true; //Bool sets a value of true, in order to print text onto the screen, through draw method in gameworld
        }

        /// <summary>
        /// Method that sets the value of 'working' bool as false (since current worker has reached a Resource GameObject).
        /// Also checks which type of Resource current Worker has been assigned to, in order to check and wait for the right Resource Semaphore to enter
        /// </summary>
        private void InsideResource()
        {
            working = false;
            if (miningGold)
            {
                GameWorld.ResourceGold.ResourceSemaphore.WaitOne(); //Worker waits upon collision, to check if the Resource of Gold Semaphore has enough entries space
                position = GameWorld.ResourceGold.Position;
            }
            else if (miningStone)
            {
                GameWorld.ResourceStone.ResourceSemaphore.WaitOne(); //Worker waits upon collision, to check if the Resource of Stone Semaphore has enough entries space
                position = GameWorld.ResourceStone.Position;
            }
            else if (gatheringFood)
            {
                GameWorld.ResourceFood.ResourceSemaphore.WaitOne(); //Worker waits upon collision, to check if the Resource of Food Semaphore has enough entries space
                position = GameWorld.ResourceFood.Position;
            }
            else if (choppingWood)
            {
                GameWorld.ResourceLumber.ResourceSemaphore.WaitOne(); //Worker waits upon collision, to check if the Resource of Lumber Semaphore has enough entries space
                position = GameWorld.ResourceLumber.Position;
            }

        }

        /// <summary>
        /// Collision Method, that checks wether or not the current Worker GameObject has collided with anohter GameObject's collision
        /// </summary>
        /// <param name="otherObject"></param>
        public override void DoCollision(GameObject otherObject)
        {
            base.DoCollision(otherObject);

            if (otherObject is Resource && !ignoreCollision)
            {
                
                InsideResource();
                occupied = true;
                if (occupied && !startWork)
                {
                    startWork = true;
                    workingThread.Start();
                }

                ignoreCollision = true;
            }


            if (otherObject is TownHall && ignoreCollision)
            {
                if (miningGold)
                {
                   TownHall.gold += resourceAmount;
                }
                else if (miningStone)
                {
                    TownHall.stone += resourceAmount;
                }
                else if (gatheringFood)
                {
                    TownHall.food += resourceAmount;
                }
                else if (choppingWood)
                {
                    TownHall.lumber += resourceAmount;
                }
                ResourceAmount = 0;
                working = true;
                ignoreCollision = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (selected == false)
            {
                spriteBatch.Draw(sprite, position, animationRectangles[currentAnimationIndex], Color.White, rotation, new Vector2(animationRectangles[currentAnimationIndex].Width * 0.5f, animationRectangles[currentAnimationIndex].Height * 0.5f), 1f, new SpriteEffects(), 0);
            }
            else
            {
                spriteBatch.Draw(sprite, position, animationRectangles[currentAnimationIndex], Color.Blue, rotation, new Vector2(animationRectangles[currentAnimationIndex].Width * 0.5f, animationRectangles[currentAnimationIndex].Height * 0.5f), 1f, new SpriteEffects(), 0f);
            }
        }
    }
}