using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Build_It_Knuckles
{
    /// <summary>
    /// This Public Class is the main type of the game.
    /// Various GameObjects are instantiated in the game, static variables of Classes are created for them to be used within other Classes and sets the overall functionality of the game.
    /// </summary>
    public class GameWorld : Game
    {
        private SpriteBatch spriteBatch;
        public static List<GameObject> gameObjects = new List<GameObject>();
        private static List<GameObject> toBeAdded = new List<GameObject>();
        private static List<GameObject> toBeRemoved = new List<GameObject>();

        public static List<GameObjectPassive> gameObjectPassive = new List<GameObjectPassive>();
        private static List<GameObjectPassive> toBeAddedPassive = new List<GameObjectPassive>();
        private static List<GameObjectPassive> toBeRemovedPassive = new List<GameObjectPassive>();

        public static SpriteFont font;
        public static SpriteFont font18;
        public static SpriteFont font24;
        private Texture2D collisionTexture;
        private Texture2D map;

        private static GraphicsDeviceManager graphics;

        public static Worker knuckles;
        public static TownHall townHall;
        public static Resource ResourceGold;
        public static Resource ResourceStone;
        public static Resource ResourceFood;
        public static Resource ResourceLumber;
        public static Cursor mouse;
        public static ButtonBuyHouse btnBuyHouse;

        public static Rectangle ScreenSize
        {
            get
            {
                return graphics.GraphicsDevice.Viewport.Bounds;
            }
        }


        private static ContentManager _content;
        public static ContentManager ContentManager
        {
            get
            {
                return _content;
            }
        }




        public GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1920;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 1080;   // set this value to the desired height of your window
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            _content = Content;
        }

        public static void AddGameObject(GameObject go)
        {
            toBeAdded.Add(go);
        }

        public static void RemoveGameObject(GameObject go)
        {
            toBeRemoved.Add(go);
        }

        public static void AddGameObjectPassive(GameObjectPassive go)
        {
            toBeAddedPassive.Add(go);
        }

        public static void RemoveGameObjectPassive(GameObjectPassive go)
        {
            toBeRemovedPassive.Remove(go);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("ExampleFont");
            font18 = Content.Load<SpriteFont>("font18");
            font24 = Content.Load<SpriteFont>("font24");
            collisionTexture = Content.Load<Texture2D>("CollisionTexture");
            map = Content.Load<Texture2D>("map");
            knuckles = new Worker();
            townHall = new TownHall();
            ResourceGold = new Resource(new Vector2(400, 400), "gold");
            ResourceStone = new Resource(new Vector2(750, 100), "stone");
            ResourceFood = new Resource(new Vector2(1150, 100), "food");
            ResourceLumber = new Resource(new Vector2(1520, 400), "lumber");


            //UI stuff
            new UI();
            btnBuyHouse = new ButtonBuyHouse();
            new ButtonBuyWorker();

            //mouse/cursor needs to be initialized last
            mouse = new Cursor();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (GameObject go in gameObjects)
            {
                go.Update(gameTime);
                foreach (GameObject other in gameObjects)
                {
                    if (go != other && go.IsColliding(other))
                    {
                        go.DoCollision(other);
                    }
                }
            }

            foreach(GameObjectPassive go in gameObjectPassive)
            {
                go.Update(gameTime);
            }


            foreach (GameObject go in toBeRemoved)
            {
                gameObjects.Remove(go);
            }
            toBeRemoved.Clear();

            gameObjects.AddRange(toBeAdded);
            toBeAdded.Clear();


            foreach(GameObjectPassive go in toBeRemovedPassive)
            {
                gameObjectPassive.Remove(go);
            }
            toBeRemovedPassive.Clear();

            gameObjectPassive.AddRange(toBeAddedPassive);
            toBeAddedPassive.Clear();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGreen);
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            spriteBatch.Draw(map, new Vector2(0, 0), Color.White);
            
            foreach (GameObject go in gameObjects)
            {
                go.Draw(spriteBatch);
#if DEBUG
                DrawCollisionBox(go);
#endif
            }

            foreach(GameObjectPassive go in gameObjectPassive)
            {
                go.Draw(spriteBatch);
            }

            //Important draws that must be on top
            //mouse.Draw(spriteBatch);

            spriteBatch.DrawString(font,"Goldmine", new Vector2(ResourceGold.Position.X - 30, ResourceGold.Position.Y - 60), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, "Stonemine", new Vector2(ResourceStone.Position.X - 30, ResourceStone.Position.Y - 60), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, "Food place", new Vector2(ResourceFood.Position.X - 30, ResourceFood.Position.Y - 60), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, "Lumbermill", new Vector2(ResourceLumber.Position.X - 30, ResourceLumber.Position.Y - 60), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);            

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawCollisionBox(GameObject go)
        {
            Rectangle collisionBox = go.CollisionBox;
            Rectangle topLine = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, 1);
            Rectangle bottomLine = new Rectangle(collisionBox.X, collisionBox.Y + collisionBox.Height, collisionBox.Width, 1);
            Rectangle rightLine = new Rectangle(collisionBox.X + collisionBox.Width, collisionBox.Y, 1, collisionBox.Height);
            Rectangle leftLine = new Rectangle(collisionBox.X, collisionBox.Y, 1, collisionBox.Height);

            spriteBatch.Draw(collisionTexture, topLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(collisionTexture, bottomLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(collisionTexture, rightLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(collisionTexture, leftLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
        }
    }
}
