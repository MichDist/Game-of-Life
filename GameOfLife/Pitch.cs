using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    class Pitch : DrawableGameComponent
    {
        // Pitch is a 3dim array = two 2dim array. 
        // One for the current frame and one for the next frame.
        // There is also a border
        const int PITCH_WIDTH = 400;
        const int PITCH_HEIGHT = 400;

        const int UPDATE_INTERVAL = 33;

        bool[,,] cells;

        Rectangle[,] rects;

        SpriteBatch spriteBatch;
        Texture2D livingCellTexture;
        Input input;

        public Pitch(Game game, SpriteBatch spriteBatch, Input input) : base(game)
        {
            cells = new bool[2, PITCH_WIDTH + 2, PITCH_HEIGHT + 2];
            rects = new Rectangle[PITCH_WIDTH, PITCH_HEIGHT];
            this.spriteBatch = spriteBatch;
            this.input = input;
        }

        protected override void LoadContent()
        {
            livingCellTexture = new Texture2D(GraphicsDevice, 1, 1);
            Color[] colors = new Color[1];
            colors[0] = Color.White;

            livingCellTexture.SetData(0, new Rectangle(0, 0, 1, 1), colors, 0, 1);

            base.LoadContent();
        }

        int millisecondsSinceLastUpdate = 0;
        int currentIndex = 0, futureIndex = 1;

        int oldWidth, oldHeight;

        public override void Update(GameTime gameTime)
        {
            if(input.SpaceTrigger)
            {
                createRandomCells(20);
            }

            #region Game_logic
            millisecondsSinceLastUpdate += gameTime.ElapsedGameTime.Milliseconds;

            if(millisecondsSinceLastUpdate >= UPDATE_INTERVAL)
            {
                millisecondsSinceLastUpdate = 0;

                for(int y = 1; y < PITCH_HEIGHT+1; y++)
                {
                    for(int x = 1; x < PITCH_WIDTH+1; x++)
                    {
                        int neighborsCount = 0;

                        // Left cell
                        if(cells[currentIndex, x - 1, y])
                        {
                            neighborsCount++;
                        }
                        // Right cell
                        if(cells[currentIndex, x + 1, y])
                        {
                            neighborsCount++;
                        }
                        // Top cell
                        if (cells[currentIndex, x , y - 1])
                        {
                            neighborsCount++;
                        }
                        // Bottom cell
                        if (cells[currentIndex, x, y + 1])
                        {
                            neighborsCount++;
                        }
                        // Top left
                        if (cells[currentIndex, x - 1, y - 1])
                        {
                            neighborsCount++;
                        }
                        // Bottom left
                        if (cells[currentIndex, x - 1, y + 1])
                        {
                            neighborsCount++;
                        }
                        // Bottom right
                        if (cells[currentIndex, x + 1, y + 1])
                        {
                            neighborsCount++;
                        }
                        // Top right
                        if (cells[currentIndex, x + 1, y - 1])
                        {
                            neighborsCount++;
                        }

                        // Rules
                        if(neighborsCount == 3)
                        {
                            cells[futureIndex, x, y] = true;
                        }
                        else if(neighborsCount < 2)
                        {
                            cells[futureIndex, x, y] = false;
                        }
                        else if(neighborsCount > 3)
                        {
                            cells[futureIndex, x, y] = false;
                        }
                        else if(neighborsCount == 2 && cells[currentIndex, x, y])
                        {
                            cells[futureIndex, x, y] = true;
                        }

                    }

                }

                if(currentIndex == 0)
                {
                    currentIndex = 1;
                    futureIndex = 0;
                }
                else
                {
                    currentIndex = 0;
                    futureIndex = 1;
                }
            }
            #endregion

            #region calculateRectangle

            int width = GraphicsDevice.Viewport.Width;
            int height = GraphicsDevice.Viewport.Height;

            if (oldWidth != width || oldHeight != height)
            {

                int cellWidth = width / PITCH_WIDTH;
                int cellHeight = height / PITCH_HEIGHT;

                int cellSize = Math.Min(cellWidth, cellHeight);

                // For the pitch
                int offsetX = (width - (cellSize * PITCH_WIDTH)) / 2;
                int offsetY = (height - (cellSize * PITCH_HEIGHT) / 2);

                for (int y = 0; y < PITCH_HEIGHT; y++)
                {
                    for (int x = 0; x < PITCH_WIDTH; x++)
                    {
                        rects[x, y] = new Rectangle(offsetX + x * cellSize, offsetY + y * cellSize, cellSize, cellSize);
                    }
                }

                oldWidth = width;
                oldHeight = height;
            }

            #endregion

            base.Update(gameTime); 
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            for(int y = 1; y < PITCH_HEIGHT + 1; y++)
            {
                for(int x = 1; x <PITCH_WIDTH + 1; x++)
                {
                    if (cells[currentIndex, x, y])
                    {
                        spriteBatch.Draw(livingCellTexture, rects[x - 1, y - 1], Color.White);
                    }          
                }
            }

            spriteBatch.End();
                                                                   
            base.Draw(gameTime);
        }

        private void createRandomCells(int probability)
        {
            Random r = new Random();

            for(int x = 1; x < PITCH_WIDTH + 1; x++)
            {
                for(int y = 1; y < PITCH_HEIGHT + 1; y++)
                {
                    if(r.Next(0, probability) == 0)
                    {
                        cells[currentIndex, x, y] = true;
                    }
                    else
                    {
                        cells[currentIndex, x, y] = false;
                    }
                }
            }
        }
    }
}