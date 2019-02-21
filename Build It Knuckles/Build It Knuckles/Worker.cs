using System;
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

        /// <summary>
        /// Bool used to check if current Worker GameObject should be able to collide with a Resource GameObject
        /// </summary>
        private bool ignoreCollision = false;

        /// <summary>
        /// Sets the moving speed amount for the current Worker GameObject
        /// </summary>
        private float movementSpeed;

        /// <summary>
        /// The resource the worker is carrying. A value of 0 is set as default.
        /// </summary>
        private int resourceAmount = 0;

        /// <summary>
        /// Property that sets and returns the resource amount value of current Worker GameObject.
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
        /// The value of this bool is used to check, if current Worker GameObject is still gathering resources (and that its health is above value of 0)
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
        /// Worker's Constructor that sets the frame count, animations player per second, the starting position and sprite name, of the current Worker GameObject.
        /// Also sets the default values of it's Health and movement speed amount. As well as adding +1 to the 'workers' value.
        /// The ReactToDead method is added to the event: 'DeadEvent'.
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
        /// Updates the game logic of current Worker GameObject.
        /// Also updates the functionality of the Selection & WorkLoop methods.
        /// </summary>
        /// <param name="gameTime">Time elapsed since last call in the update</param>
        public override void Update(GameTime gameTime)
        {
            Selection(gameTime);
            WorkLoop(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Method that handles the functionality of the User/Player clicking on the current Worker GameObject, with the Cursor GameObject.
        /// Statements are responsible for checking if a Worker has been selected, and the Resource destination of the selected Worker
        /// </summary>
        /// <param name="gameTime">Time elapsed since last call in the update</param>
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

            if (selected && GameWorld.mouse.Click(GameWorld.ResourceGold))  //Statement that checks if Worker is selected and User Clicks on the Gold Resource GameObject
            {
                selected = false;
                working = true;
                miningGold = true;
            }
            else if (selected && GameWorld.mouse.Click(GameWorld.ResourceStone))  //Statement that checks if Worker is selected and User Clicks on the Stone Resource GameObject
            {
                selected = false;
                working = true;
                miningStone = true;
            }
            else if (selected && GameWorld.mouse.Click(GameWorld.ResourceFood))  //Statement that checks if Worker is selected and User Clicks on the Food Resource GameObject
            {
                selected = false;
                working = true;
                gatheringFood = true;
            }
            else if (selected && GameWorld.mouse.Click(GameWorld.ResourceLumber))  //Statement that checks if Worker is selected and User Clicks on the Lumber Resource GameObject
            {
                selected = false;
                working = true;
                choppingWood = true;
            }
        }

        /// <summary>
        /// Method that handles the functionality of checking if the worker should currently be moving towards the chosen Resource type, or towards the TownHall GameObject
        /// </summary>
        /// <param name="gameTime">Time elapsed since last call in the update</param>
        private void WorkLoop(GameTime gameTime)
        {                       
            if (working && ResourceAmount < 50 && alive) //Statement that checks if Worker is currently moving towards a Resource GameObject, is alive and its value amount of resources is below 50
            {
                Vector2 direction;
                if (miningGold) //Statement that checks if the Worker, should currently be gathering Gold resources. Letting the Worker know it should move towards the Gold Resource GameObject
                {
                    direction = GameWorld.ResourceGold.Position - position; //Sets the Vector values of local Vector2, makes the Worker moving towards Gold Resource GameObject
                    direction.Normalize();
                    position += direction * movementSpeed;
                }
                else if (miningStone) //Statement that checks if the Worker, should currently be gathering Stone resources. Letting the Worker know it should move towards the Stone Resource GameObject
                {
                    direction = GameWorld.ResourceStone.Position - position; //Sets the Vector values of local Vector2, makes the Worker moving towards Stone Resource GameObject
                    direction.Normalize();
                    position += direction * movementSpeed;
                }
                else if (gatheringFood) //Statement that checks if the Worker, should currently be gathering Food resources. Letting the Worker know it should move towards the Food Resource GameObject
                {
                    direction = GameWorld.ResourceFood.Position - position; //Sets the Vector values of local Vector2, makes the Worker moving towards Food Resource GameObject
                    direction.Normalize();
                    position += direction * movementSpeed;
                }
                else if (choppingWood) //Statement that checks if the Worker, should currently be gathering Lumber resources. Letting the Worker know it should move towards the Lumber Resource GameObject
                {
                    direction = GameWorld.ResourceLumber.Position - position; //Sets the Vector values of local Vector2, makes the Worker moving towards Lumber Resource GameObject
                    direction.Normalize();
                    position += direction * movementSpeed;
                }

            }
            else if (!working && ResourceAmount >= 50 && alive) //Statement that checks if Worker is currently moving away from a Resource GameObject (and towards the TownHall GameObject), is alive and its value amount of resources is at or above 50
            {
                Vector2 direction;
                if (miningGold) //Statement that checks if the Worker has been gathering Gold
                {
                    direction = GameWorld.townHall.Position - GameWorld.ResourceGold.Position;  //Sets the Vector value values of local Vector2, makes the Worker moving towards the position of TownHall GameObject (away from the position of Gold Resource gameObject)
                    direction.Normalize();
                    position += direction * movementSpeed;
                }
                else if (miningStone)   //Statement that checks if the Worker has been gatherring Stone
                {
                    direction = GameWorld.townHall.Position - GameWorld.ResourceStone.Position; //Sets the Vector value values of local Vector2, makes the Worker moving towards the position of TownHall GameObject (away from the position of Stone Resource gameObject)
                    direction.Normalize();
                    position += direction * movementSpeed;
                }
                else if (gatheringFood)  //Statement that checks if the Worker has been gatherring Food
                {
                    direction = GameWorld.townHall.Position - GameWorld.ResourceFood.Position; //Sets the Vector value values of local Vector2, makes the Worker moving towards the position of TownHall GameObject (away from the position of Food Resource gameObject)
                    direction.Normalize();
                    position += direction * movementSpeed;
                }
                else if (choppingWood)   //Statement that checks if the Worker has been gatherring Lumber
                {
                    direction = GameWorld.townHall.Position - GameWorld.ResourceLumber.Position; //Sets the Vector value values of local Vector2, makes the Worker moving towards the position of TownHall GameObject (away from the position of Lumber Resource gameObject)
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
        /// Method that handles the functionality of current workingThread. The Thread's lifeline is kept alive, until the Worker has reached a resource value of 50 or above.
        /// Thread first checks through various statements, which Resource type the worker has been assigned, in order for the Worker to wait for a Semaphore spot within the right type of Resource GameObject.
        /// While Loop of the Thread maintains the functionality of adding resources, and reducing the Worker's Health value every 1 second, as long as it's resource amount remains below a value of 50.
        /// As the Worker reaches a value of 50 or above, lifeline of the Thread (workingThread) is then killed, and the remaining functionality of Thread then checks up, through statements, which type of Resource the Worker is to Release a spot for, in the Semaphore
        /// </summary>
        private void EnterResource()
        {          

            if (miningGold) //Statement that checks if the Worker is currently set to be mining Gold resources
            {
                GameWorld.ResourceGold.ResourceSemaphore.WaitOne(); //Worker waits to check if the Resource of Gold Semaphore has enough entries space, in order to enter
                position = GameWorld.ResourceGold.Position; //As a spot within current Resource becomes available, the position of the Worker is then set to the position of current Resource GameObject                
            }
            else if (miningStone) //Statement that checks if the Worker is currently set to be mining Stone resources
            {
                GameWorld.ResourceStone.ResourceSemaphore.WaitOne(); //Worker waits to check if the Resource of Stone Semaphore has enough entries space, in order to enter
                position = GameWorld.ResourceStone.Position; //As a spot within current Resource becomes available, the position of the Worker is then set to the position of current Resource GameObject    
            }
            else if (gatheringFood) //Statement that checks if the Worker is currently set to be mining Food resources
            {
                GameWorld.ResourceFood.ResourceSemaphore.WaitOne(); //Worker waits to check if the Resource of Food Semaphore has enough entries space, in order to enter
                position = GameWorld.ResourceFood.Position; //As a spot within current Resource becomes available, the position of the Worker is then set to the position of current Resource GameObject
            }
            else if (choppingWood) //Statement that checks if the Worker is currently set to be mining Lumber resources
            {
                GameWorld.ResourceLumber.ResourceSemaphore.WaitOne(); //Worker waits to check if the Resource of Lumber Semaphore has enough entries space, in order to enter
                position = GameWorld.ResourceLumber.Position; //As a spot within current Resource becomes available, the position of the Worker is then set to the position of current Resource GameObject
            }

            while (occupied)    //Worker continues to gather resources and loses health every 1 second as long as its current resource amount is below a value of 50
            {
                ResourceAmount += 10;   //A value of 10 is added to the current Worker's resource amount, every 1 second
                Health -= 5;    //A value of 5 is substituted from the current Worker's Health amount, every 1 second
                Thread.Sleep(1000); //The Thread is set to Sleep every 1000 milliseconds (every 1 second)

                if (ResourceAmount >= 50)   //Statement that checks if the resource amount of current Worker has reached a value of 50 or above, in order to kill the lifeline of the Thread (workingThread) 
                {
                    occupied = false;   //Sets a value of false, which kills the lifeline of Thread (workingThread's While Loop)
                }

            }

            if (carryingGold)   //Statement that checks if current Worker has been collecting /and is now carrying Gold resources, in order to Release the right spot (Lock), for the right Resource Semaphore
            {
                GameWorld.ResourceGold.ResourceSemaphore.Release(); //Releases a spot inside the Gold Resource Semaphore
            }
            else if (carryingStone) //Statement that checks if current Worker has been collecting /and is now carrying Stone resources, in order to Release the right spot (Lock), for the right Resource Semaphore
            {
                GameWorld.ResourceStone.ResourceSemaphore.Release(); //Releases a spot inside the Stone Resource Semaphore 
            }
            else if (carryingFood)  //Statement that checks if current Worker has been collecting /and is now carrying Food resources, in order to Release the right spot (Lock), for the right Resource Semaphore
            {
                GameWorld.ResourceFood.ResourceSemaphore.Release(); //Releases a spot inside the Food Resource Semaphore 
            }
            else if (carryingLumber) //Statement that checks if current Worker has been collecting /and is now carrying Lumber resources, in order to Release the right spot (Lock), for the right Resource Semaphore
            {
                GameWorld.ResourceLumber.ResourceSemaphore.Release(); //Releases a spot inside the Lumber Resource Semaphore
            }
                       
        }

        /// <summary>
        /// Method that handles the functionality of creating a new Thread, which takes in the WorkerFleeing Method in it's parameters.
        /// This Method is called through the event: 'DeadEvent'.
        /// Method is first responsible of checking if value of 'occupied' bool is set true, to make sure the previous Thread (workingThread) is killed probably, before creating another.
        /// The new Thread is created, instantiated, set as background and Started within this method.
        /// </summary>
        /// <param name="worker">The current Worker GameObject</param>
        private void ReactToDead(Worker worker)
        {
            if (occupied)   //Statement that checks if occupied is set true, to make sure the previous Thread (workingThread) is killed probably, should the DeadEvent trigger within a specific timeline of miliseconds and not kill the Thread before moving on
            {
                occupied = false;   //Sets a value of false, if the value of occupied turned out to still be set as true
            }

            alive = false;  //value of 'alive' is set false, in order to let current Worker know, that it should not proceed it's routine between Resources and Townhall (Checking through WorkLoop Method) 

            Thread fleeThread = new Thread(WorkerFleeing);  //New Thread is created and instantiated, taking in the WorkerFleeing Method in its parameters
            fleeThread.IsBackground = true; //the instantiated fleeThread is set as a background Thread, since a Main Thread is already exising (Our main program), and we don't want the instantiated Thread running as another Main Thread
            fleeThread.Start(); //Instantiated Thread is Started immediately after instantiation (And set as a Background Thread) 
            workers--;  //Substitutes the value amount by 1, of total workers currently in the game, making it possible to instantiate new workers into the game
        }

        /// <summary>
        /// Method that handles the functionality of current fleeThread. The Thread's lifeline is kept alive, until the Worker runs out of screen vision (Screen's Collision), (and local bool 'flee' is set false), removing it from the game.
        /// While the Thread remains alive, the worker moves towards the position of the TownHall Object, and is then running towards a new Vector 2 destination.
        /// Method also changes the movement speed of current Worker while 'fleeing', making it possible for the Player/User to know wether or not a Worker is still working or fleeing
        /// </summary>
        private void WorkerFleeing()
        {
            bool flee = true;   //Local bool, setting a value of true, in order to keep the while loop/Thread alive as long as Worker is 'fleeing'
            Vector2 direction; //Local Vector2 created, its values are set within the while loop, for the worker to keep track of which X & Y axis it's supposed to run towards
            
            while (flee)    //While loop which is responsible of keeping the lifeline of Thread (fleeThread) alive, and contains the main functionality of the Thread
            {
                Thread.Sleep(5);    //Thread sleeps ever 5th millisecond, in order to create a more dynamic moving effect for as long as the current Worker is 'fleeing'
                if (resourceAmount >= 50)   //Statement that checks if the value amount of resources is at or above 50, in order to make sure the Worker is moving towards the TownHall GameObject
                {
                    direction = GameWorld.townHall.Position - Position; //Values of the local Vector2 'direction' is set between the current position of the Worker and the position of the TownHall GameObject
                    direction.Normalize();
                    position += direction * movementSpeed * 0.4f; //Current position of Worker is added to the Vector values and multiplied with Worker's movement speed. Also, the Movementspeed is multiplied with a value of 0.4f to slow Worker down (so Player/User can spot that the Worker is 'fleeing')
                }
                else
                {
                    direction = new Vector2(-100, 500) - position;  //New Values of the local Vector2 'direction' is then set between current position of the Worker and a new Vector2 position.
                    direction.Normalize();
                    position += direction * movementSpeed * 0.5f; //Current position of Worker is added to the Vector values and multiplied with Worker's movement speed. Also, the Movementspeed is multiplied with a value of 0.5f to slow Worker down (so Player/User can spot that the Worker is 'fleeing')
                }
                
                if(!GameWorld.ScreenSize.Intersects(CollisionBox))  //Statement that checks if the current Worker is no longer in collision of the Screen, in order to stop the lifeline of Thread (fleeThread)
                {
                    flee = false;   //Sets a value of false, which stops the Thread's while loop from running, and kills the lifeline of Thread (fleeThread)             
                }
            }

            GameWorld.RemoveGameObject(this);   //As the last functionality of the Thread (After the freeThread's while loop has stopped running), the current Worker GameObject is then removed from the game
        }

        /// <summary>
        /// Collision Method, that checks wether or not the current Worker GameObject has collided with anohter GameObject's collision.
        /// Handles Worker collision with another Resource GameObject, to check what type on Resource it has collided with, in order for it to know which type of resource value it is currently gathering.
        /// Method also creates a new Thread, which is kept alive through bool: 'occupied'. Thread is set as Background and is started upon collision.
        /// Bool: 'ignoreCollision' is set true, for letting current Worker know, that it has already collided with an Resource, 
        /// in order to avoid issues of trying to create an already instantiated Thread and overall to avoid constant collision check, since Worker will be in Resource collision while Thread is alive.
        /// Also checks if Worker has collided with the TownHall GameObject, in order to add the value amount of worker Resources to the TownHall's resource values.
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
                    carryingGold = true;    //Sets a value of true, since Worker collided with a Gold Resource
                    carryingStone = false;  //Sets a value of false, letting Worker know it did not collide with a Stone Resource
                    carryingFood = false;   //Sets a value of false, letting Worker know it did not collide with a Food Resource
                    carryingLumber = false; //Sets a value of false, letting Worker know it did not collide with a Lumber Resource
                }
                else if (resource.Equals(GameWorld.ResourceStone))
                {
                    carryingStone = true;   //Sets a value of true, since Worker collided with a Stone Resource
                    carryingGold = false;   //Sets a value of false, letting Worker know it did not collide with a Gold Resource
                    carryingFood = false;   //Sets a value of false, letting Worker know it did not collide with a Food Resource
                    carryingLumber = false; //Sets a value of false, letting Worker know it did not collide with a Lumber Resource
                }
                else if (resource.Equals(GameWorld.ResourceFood))
                {
                    carryingFood = true;    //Sets a value of true, since Worker collided with a Food Resource
                    carryingGold = false;   //Sets a value of false, letting Worker know it did not collide with a Gold Resource
                    carryingStone = false;  //Sets a value of false, letting Worker know it did not collide with a Stone Resource
                    carryingLumber = false; //Sets a value of false, letting Worker know it did not collide with a Lumber Resource
                }
                else if (resource.Equals(GameWorld.ResourceLumber))
                {
                    carryingLumber = true;  //Sets a value of true, since Worker collided with a Lumber Resource
                    carryingFood = false;   //Sets a value of false, letting Worker know it did not collide with a Food Resource
                    carryingGold = false;   //Sets a value of false, letting Worker know it did not collide with a Gold Resource
                    carryingStone = false;  //Sets a value of false, letting Worker know it did not collide with a Stone Resource
                }

                working = false;    //Sets a value of false, since worker has reached/and collided with a Resource GameObject, letting it know it is no longer moving towards a Resource 
                occupied = true;    //Sets a value of true, letting the Worker know it is currently gathering resource values within a Resource

                workingThread = new Thread(EnterResource);  //Instantiates a workingThread onto current Worker. Takes in the EnterResource method as it's parameters
                workingThread.IsBackground = true;  //the instantiated workingThread is set as a background Thread, since a Main Thread is already exising (Our main program), and we don't want the instantiated Thread running as another Main Thread
                workingThread.Start();  //Instantiated Thread is Started immediately after instantiation (And set as a Background Thread), since Worker has just collided with a Resource, and Thread functionality of gathering it's resources should already begin

                ignoreCollision = true; //Sets a value of true, Letting current Worker know that it has already collided with an Resource, in order to avoid constant collision check while the working Thread is alive, and avoid trying to instantiate an already instantiated Thread
            }
        
            if (otherObject is TownHall && ignoreCollision) //Statement that checks if current Worker has collided with the TownHall GameObject, and that ignoreCollision bool is set true, for the Worker to know it again can collide with a Resource (Setting ignoreCollision value as false)
            {
                if (carryingGold)
                {
                   TownHall.gold += ResourceAmount; //Adds the Worker's current Gold resource amount, to the value amount of the TownHall GameObject
                }
                else if (carryingStone)
                {
                    TownHall.stone += ResourceAmount;   //Adds the Worker's current Stone resource amount, to the value amount of the TownHall GameObject
                }
                else if (carryingFood)
                {
                    TownHall.food += ResourceAmount;    //Adds the Worker's current Food resource amount, to the value amount of the TownHall GameObject
                }
                else if (carryingLumber)
                {
                    TownHall.lumber += ResourceAmount;  //Adds the Worker's current Lumber resource amount, to the value amount of the TownHall GameObject
                }
                ResourceAmount = 0; //Resets current Worker resource value - Setting a value of 0 (In order for worker to refill it's resources during next collision with a Resource type)
                working = true; //Value is set true, since the Worker is now moving towards current chosen Resource Type
                ignoreCollision = false; //Value is set false, in order for letting current Worker know it can collide with a Resource GameObject
            }
        }

        /// <summary>
        /// Overriden Draw Method, that sets the base functionality of the Draw Method (from the GameObject Class).
        /// Also checks if the value of 'selected' bool is set true, in order for the sprite of current Worker, to be drawn out in a color of blue.
        /// The original color of current worker is drawn out as default, as long as value of 'selected' is set false.
        /// </summary>
        /// <param name="spriteBatch">The spritebatch which is used for drawing</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (selected)   //Statement that checks if value of 'selected' bool is set as true, for the current Worker sprite to be drawn out in a color of blue
            {
                spriteBatch.Draw(sprite, position, animationRectangles[currentAnimationIndex], Color.Blue, rotation, new Vector2(animationRectangles[currentAnimationIndex].Width * 0.5f, animationRectangles[currentAnimationIndex].Height * 0.5f), 1f, new SpriteEffects(), 0);
            }
            else //If value of 'selected' is set as false, the worker is instead drawn out in it's default colour(s)
            {
                spriteBatch.Draw(sprite, position, animationRectangles[currentAnimationIndex], Color.White, rotation, new Vector2(animationRectangles[currentAnimationIndex].Width * 0.5f, animationRectangles[currentAnimationIndex].Height * 0.5f), 1f, new SpriteEffects(), 0f);
            }
        }
    }
}