using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpecialEffectsExample
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _worldTexture;
        private Texture2D _playerTexture;
        private float _zoom;
        private float _rotation;
        private bool _shaking;
        private float _shakeTime;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 640;
            _graphics.PreferredBackBufferHeight = 480;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            _zoom = 1f;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _worldTexture = Content.Load<Texture2D>("world");
            _playerTexture = Content.Load<Texture2D>("player");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Up)) _zoom += 0.01f;
            if (ks.IsKeyDown(Keys.Down)) _zoom -= 0.01f;

            if (ks.IsKeyDown(Keys.Left)) _rotation += 0.01f;
            if (ks.IsKeyDown(Keys.Right)) _rotation -= 0.01f;

            if(ks.IsKeyDown(Keys.Space))
            {
                _shakeTime = 0;
                _shaking = true;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            Matrix zoomTranslation = Matrix.CreateTranslation(-640 / 2f, -480 / 2f, 0);
            Matrix zoomScale = Matrix.CreateScale(_zoom);
            Matrix zoomTransform =  zoomTranslation * zoomScale * Matrix.Invert(zoomTranslation);

            Matrix spinTranslation = Matrix.CreateTranslation(-341, -866, 0);
            Matrix spinRotation = Matrix.CreateRotationZ(_rotation);
            Matrix spinTransform = spinTranslation * spinRotation * Matrix.Invert(spinTranslation);

            Matrix shakeTransform = Matrix.Identity;
            if (_shaking)
            {
                _shakeTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
               // Matrix shakeRotation = Matrix.CreateRotationZ(MathF.Cos(_shakeTime));
                Matrix shakeTranslation = Matrix.CreateTranslation(10*MathF.Sin(_shakeTime), 10*MathF.Cos(_shakeTime), 0);
                shakeTransform = shakeTranslation;
                if (_shakeTime > 3000) _shaking = false;
            }

            // Transform the world by spinning, zooming, and shaking
            _spriteBatch.Begin(transformMatrix: spinTransform * zoomTransform * shakeTransform);
            _spriteBatch.Draw(_worldTexture, new Vector2(-500, 0), Color.White);
            _spriteBatch.End();

            // Transform the player by zooming and shaking
            _spriteBatch.Begin(transformMatrix: zoomTransform * shakeTransform);
            _spriteBatch.Draw(_playerTexture, new Vector2(320, 270), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
