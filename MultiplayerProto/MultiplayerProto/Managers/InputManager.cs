using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiplayerProto.Managers
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public class InputManager: GameComponent
    {
        private KeyboardState keyboardState;
        private MouseState mouseState;

        private KeyboardState lastKeyboardState;
        private MouseState lastMouseState;

        private int ActivePlayerId { get; set; }

        public InputManager(Game game): base(game)
        {
            this.keyboardState = Keyboard.GetState();
            this.mouseState = Mouse.GetState();
        }

        public Vector2 MousePosition
        {
            get
            {
                return new Vector2(this.mouseState.X, this.mouseState.Y);
            }
        }

        public void Flush()
        {
            this.lastKeyboardState = this.keyboardState;
            this.lastMouseState = this.mouseState;
        }

        public bool isLeftButtonClicked()
        {
            return this.mouseState.LeftButton == ButtonState.Released && this.lastMouseState.LeftButton == ButtonState.Pressed;
        }

        public bool IsKeyPressed(Keys keyToTest)
        {
            return this.keyboardState.IsKeyUp(keyToTest) && this.lastKeyboardState.IsKeyDown(keyToTest);
        }

        public override void Update(GameTime gameTime)
        {
            this.Flush();

            this.keyboardState = Keyboard.GetState(PlayerIndex.One);
            this.mouseState = Mouse.GetState();
            
            base.Update(gameTime);
        }
    }
}
