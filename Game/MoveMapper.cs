using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    internal static class MoveMapper
    {
        // Returns the options for a move based on board size
        public static void InitializeMoveCoordinateOptions(int boardSize, int[,,] CoordinateOptions)
        {
            int center = boardSize / 2;

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    // Check if the coordinate is on the outside ring
                    if (i == 0 || i == boardSize - 1 || j == 0 || j == boardSize - 1)
                    {
                        CoordinateOptions[i, j, 0] = i - center;
                        CoordinateOptions[i, j, 1] = j - center;
                    }
                    else
                    {
                        // Setting not used coordinates to 0
                        CoordinateOptions[i, j, 0] = 0;
                        CoordinateOptions[i, j, 1] = 0;
                    }
                }
            }
        }

    }
}
