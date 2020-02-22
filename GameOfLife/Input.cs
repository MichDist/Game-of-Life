using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    class Input : GameComponent
    {
        public Input(Game game) : base(game)
        {

        }

        KeyboardState oldState;
        public override void Update(GameTime gameTime)
        {
            KeyboardState currentState = Keyboard.GetState();

            SpaceTrigger = false;
            
            if(currentState.IsKeyDown(Keys.Space) && !oldState.IsKeyDown(Keys.Space))
            {
                SpaceTrigger = true;
            }

            oldState = currentState;
        }

        public bool SpaceTrigger { get; private set; }
    }
}
