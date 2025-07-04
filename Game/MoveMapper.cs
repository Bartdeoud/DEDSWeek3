namespace Game
{
    // Calculates the options for a move based on move range
    internal static class MoveMapper
    {
        public static int[,,] CopyCoordanateOptions { get; set; } = InitializeMoveCoordinateOptions(3);
        public static int[,,] MoveCoordanateOptions { get; set; } = InitializeMoveCoordinateOptions(5);

        public static int[,,] InitializeMoveCoordinateOptions(int boardSize)
        {
            int[,,] CoordinateOptions = new int[boardSize, boardSize, 2];

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

            return CoordinateOptions;
        }
    }
}
