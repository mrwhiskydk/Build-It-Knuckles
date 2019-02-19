using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Build_It_Knuckles
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameWorld : Game
    {
        private SpriteBatch spriteBatch;
        private List<GameObject> gameObjects = new List<GameObject>();
        private static List<GameObject> toBeAdded = new List<GameObject>();
        private static List<GameObject> toBeRemoved = new List<GameObject>();

        public static List<GameObjectPassive> gameObjectPassive = new List<GameObjectPassive>();
        private static List<GameObjectPassive> toBeAddedPassive = new List<GameObjectPassive>();
        private static List<GameObjectPassive> toBeRemovedPassive = new List<GameObjectPassive>();

        private SpriteFont font;
        private Texture2D collisionTexture;

        private static GraphicsDeviceManager graphics;

        public static Worker knuckles;
        public static TownHall townHall;
        public static Resource ResourceGold;
        public static Resource ResourceStone;
        public static Resource ResourceFood;
        public static Resource ResourceLumber;
        public static Cursor mouse;

        // ! TEST !
        public static bool workerEnter = false;
        // ! TEST !
        public static bool workerLeft = false;

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
            collisionTexture = Content.Load<Texture2D>("CollisionTexture");
            knuckles = new Worker();
            townHall = new TownHall();
            ResourceGold = new Resource(new Vector2(300, 100));
            ResourceStone = new Resource(new Vector2(750, 100));
            ResourceFood = new Resource(new Vector2(1250, 100));
            ResourceLumber = new Resource(new Vector2(1750, 100));
            new ButtonBuyHouse();

            //mouse/cursor needs to be initialized last
            mouse = new Cursor();
        }



        //go = new AnimatedGameObject(4,20,Content,"HeroStrip");

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
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.DrawString(font, $"Gold: {TownHall.gold}", new Vector2(100, 120), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, $"Stone: {TownHall.stone}", new Vector2(100, 140), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, $"Lumber: {TownHall.lumber}", new Vector2(100, 160), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, $"Food: {TownHall.food}", new Vector2(100, 180), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
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

            spriteBatch.DrawString(font, $"Health: {knuckles.Health}", new Vector2(600, 850), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, $"Gold: {knuckles.resourceAmount}", new Vector2(600, 800), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            if (workerEnter)
            {
                spriteBatch.DrawString(font, "Worker Has Entered the Mine!", new Vector2(600, 700), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }

            if (workerLeft)
            {
                spriteBatch.DrawString(font, "Worker Has Left the Mine!", new Vector2(600, 650), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }

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
