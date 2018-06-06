using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Core
{
    public class Life
    {
        private int[,] map; // 0 - empty, 1 - alive, 2 - dies, -1 - is born
        private int[,] neighborsSum; // neighborsSum[x, y] = amount of live cells at the right side and below
        public int Height { get; private set; }
        public int Width { get; private set; }

        public Life(int height, int width)
        {
            Height = height;
            Width = width;
            map = new int[width, height];
            neighborsSum = new int[width, height];
            //for (int i = 0; i < width; i++)
            //    for (int j = 0; j < height; j++)
            //        map[i, j] = 0;
        }

        public int Turn(int x, int y)
        {
            map[x, y] = map[x, y] == 0 ? 1 : 0;
            return map[x, y];
        }

        public int GetMap(int x, int y)
        {
            if (x < 0 || x >= Width)
                return 0;
            if (y < 0 || y >= Height)
                return 0;
            return map[x, y];
        }

        public int GetSum(int x, int y)
        {
            if (x >= Width)
                return 0;
            if (y >= Height)
                return 0;
            if (x < 0)
                x = 0;
            if (y < 0)
                y = 0;
            return neighborsSum[x, y];
        }

        private int Around(int x, int y)
        {
            return GetSum(x - 1, y - 1) -
                GetSum(x + 2, y - 1) - 
                GetSum(x - 1, y + 2) + 
                GetSum(x + 2, y + 2);
        }

        private void InitNeibhors()
        {
            for (int x = Width - 1; x >= 0; x--)
            {
                for (int y = Height - 1; y >= 0; y--)
                {
                    // A = A + B + C - D
                    neighborsSum[x, y] = (GetMap(x, y) > 0 ? 1 : 0) + 
                        GetSum(x + 1, y) + 
                        GetSum(x, y + 1) - 
                        GetSum(x + 1, y + 1);
                }
            }
        }

        private int Around1(int x, int y)
        {
            int sum = 0;
            for (int sx = -1; sx <= 1; sx++)
            {
                for (int sy = -1; sy <= 1; sy++)
                {
                    if (GetMap(x + sx, y + sy) > 0)
                        sum++;
                }
            }
            return sum;
        }
        public void PrepeareGeneration()
        {
            InitNeibhors();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int neighbors = Around(x, y);
                    if (map[x, y] == 1) // if alive
                    {
                        if (neighbors <= 2) // death by under population. (rule 1.)
                            map[x, y] = 2;
                        else if (neighbors >= 5) // death by overpopulation. (rule 3.)
                            map[x, y] = 2;
                    }
                    else // if empty
                    {
                        if (neighbors == 3)
                            map[x, y] = -1; // become a live cell, as if by reproduction. (rule 4.)
                    }
                }
            }
        }

        public void CommitGeneration()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (map[x, y] == -1)
                        map[x, y] = 1;  // empty cell
                    else if (map[x, y] == 2)
                        map[x, y] = 0;  // alive cell
                }
            }
        }
    }
}
