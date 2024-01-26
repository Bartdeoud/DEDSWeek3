using Game.GameBoard.GameBoard;

namespace Game.MoveCalculator
{
    internal class BaseMoveGenerator
    {
        protected ITile[,] board;
        protected Team ForTeam;

        protected int[,,] CopyCoordanateOptions { get; set; } = new int[3, 3, 2];
        protected int[,,] MoveCoordanateOptions { get; set; } = new int[5, 5, 2];

        public BaseMoveGenerator(ITile[,] board, Team ForTeam)
        {
            this.board = board;
            this.ForTeam = ForTeam;

            MoveMapper.InitializeMoveCoordinateOptions(3, CopyCoordanateOptions);
            MoveMapper.InitializeMoveCoordinateOptions(5, MoveCoordanateOptions);
        }

        // returns all moves that are possible for team ForTeam
        protected List<ITile[]>[] getAllMovesFromTiles()
        {
            List<ITile[]> AllCopyMoves = new List<ITile[]>();
            List<ITile[]> AllMoveMoves = new List<ITile[]>();
            foreach (ITile item in board)
            {
                if (item.team == ForTeam)
                {
                    AllCopyMoves.AddRange(getAllCopyMovesFromTile(item));
                    AllMoveMoves.AddRange(getAllMoveMovesFromTile(item));
                }
            }

            return new List<ITile[]>[] { AllCopyMoves, AllMoveMoves };
        }

        // returns all copy moves that are posible from tile
        private List<ITile[]> getAllMoveMovesFromTile(ITile tile)
        {
            List<ITile> tiles = new List<ITile>();
            tiles.AddRange(getAllMovesFromTile(tile, MoveCoordanateOptions));

            List<ITile[]> moves = new List<ITile[]>();

            tiles.ForEach(t => moves.Add(new ITile[] { tile, t }));

            return moves;
        }

        // returns all copy moves that are posible from tile
        private List<ITile[]> getAllCopyMovesFromTile(ITile tile)
        {
            List<ITile> tiles = new List<ITile>();
            tiles.AddRange(getAllMovesFromTile(tile, CopyCoordanateOptions));

            List<ITile[]> moves = new List<ITile[]>();

            tiles.ForEach(t => moves.Add(new ITile[] { tile, t }));

            return moves;
        }

        // returns all the moves that are inside the main board and inside the MoveCoordanates
        private List<ITile> getAllMovesFromTile(ITile tile, int[,,] MoveCoordanates)
        {
            int boardSize = MoveCoordanates.GetLength(0);
            int x = tile.x;
            int y = tile.y;

            List<ITile> tiles = new List<ITile>();

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (MoveCoordanates[i, j, 0] == 0 && MoveCoordanates[i, j, 1] == 0) continue;
                    int x2 = x + MoveCoordanates[i, j, 0];
                    int y2 = y + MoveCoordanates[i, j, 1];

                    if (x2 < board.GetLength(0) && x2 >= 0)
                        if (y2 < board.GetLength(1) && y2 >= 0)
                            if (board[x2, y2].team == Team.Neutral)
                            {
                                tiles.Add(board[x2, y2]);
                            }
                }
            }
            return tiles;
        }

        protected class TileWithInfectionRate
        {
            public ITile[] tile { get; set; }
            public int infectionCount { get; set; }

            public TileWithInfectionRate(ITile[] tile, int infectionCount)
            {
                this.tile = tile;
                this.infectionCount = infectionCount;
            }
        }
    }
}
