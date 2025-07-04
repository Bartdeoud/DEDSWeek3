using Game.GameBoard.GameBoard;

namespace Game
{
    internal class GameMove
    {
        public ITile[,] board { get; set; }
        private int[,,] CopyCoordanateOptions { get; set; } = new int[3, 3, 2];
        private int[,,] MoveCoordanateOptions { get; set; } = new int[5, 5, 2];


        public GameMove(ITile[,] board)
        {
            this.board = board;

            CopyCoordanateOptions = MoveMapper.CopyCoordanateOptions;
            MoveCoordanateOptions = MoveMapper.MoveCoordanateOptions;
        }

        // Does a move with tileFrom to TileTo
        public bool Move(ITile tileFrom, ITile TileTo)
        {
            bool ValidMove = false;
            tileFrom.SetTeam(tileFrom.team);

            if (tileFrom.team == TileTo.team) { return false; }
            if (TileTo.team != Team.Neutral) { return false; }

            // Check for copy move
            if (MoveValidator(tileFrom, TileTo, CopyCoordanateOptions))
            {
                TileTo.SetTeam(tileFrom.team);
                ValidMove = true;
            }
            // Check for Move move
            else if (MoveValidator(tileFrom, TileTo, MoveCoordanateOptions))
            {
                TileTo.SetTeam(tileFrom.team);
                InfectTiles(TileTo, tileFrom.team);
                tileFrom.SetTeam(Team.Neutral);
                ValidMove = true;
            }
            if (!ValidMove) return false;

            CheckIfGameFinished();
            // TODO set the moved tiles to a lighet color

            return ValidMove;
        }

        // Restarts the game if one team has won
        private void CheckIfGameFinished()
        {
            int hoodies = 0;
            int baggySweaters = 0;
            int neutral = 0;
            foreach (var item in board)
            {
                if (item.team == Team.Hoodie) hoodies++;
                if (item.team == Team.BaggySweater) baggySweaters++;
                if (item.team == Team.Neutral) neutral++;
            }
            if (hoodies == 0 || baggySweaters == 0 || neutral == 0)
            {
                if (hoodies > baggySweaters)
                {
                    MessageBox.Show("Hoodies Won");
                }
                else if (hoodies < baggySweaters)
                {
                    MessageBox.Show("Baggy Sweaters Won");
                }
                else
                {
                    MessageBox.Show("Draw");
                }
                BoardActionController.GameEnded = true;
                Application.Restart();
            }
        }

        // Infects all tiles around tile with team
        public int InfectTiles(ITile tile, Team team, bool OnlyGetNumber = false)
        {
            int boardSize = CopyCoordanateOptions.GetLength(0);
            int x = tile.x;
            int y = tile.y;


            int infectedTiles = 0;
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    int x2 = x + CopyCoordanateOptions[i, j, 0];
                    int y2 = y + CopyCoordanateOptions[i, j, 1];
                    // Check if the coordinates are within the bounds of the board
                    if (x2 < board.GetLength(0) && x2 >= 0)
                        if (y2 < board.GetLength(1) && y2 >= 0)
                        {
                            if (board[x2, y2].team != team && board[x2, y2].team != Team.Neutral)
                            {
                                if (!OnlyGetNumber)
                                    board[x2, y2].InfectWithTeam(team);
                                infectedTiles++;
                            }
                        }
                }
            }
            return infectedTiles;
        }

        // Checks if the move is valid
        private bool MoveValidator(ITile tileFrom, ITile TileTo, int[,,] CoordanateOptions)
        {
            int x = tileFrom.x - TileTo.x;
            int y = tileFrom.y - TileTo.y;

            int boardSize = CoordanateOptions.GetLength(0);

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (CoordanateOptions[i, j, 0] == x && CoordanateOptions[i, j, 1] == y)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
