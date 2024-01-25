using Game.GameBoard.GameBoard;
using System.Diagnostics;

namespace Game.MoveCalculator
{
    internal class AIMoveGenerator
    {
        private Tile[,] board;
        private Team ForTeam;
        Random random = new Random();

        private int[,,] CopyCoordanateOptions { get; set; } = new int[3, 3, 2];
        private int[,,] MoveCoordanateOptions { get; set; } = new int[5, 5, 2];

        public AIMoveGenerator(Tile[,] board, Team ForTeam)
        {
            this.board = board;
            this.ForTeam = ForTeam;

            MoveMapper.InitializeMoveCoordinateOptions(3, CopyCoordanateOptions);
            MoveMapper.InitializeMoveCoordinateOptions(5, MoveCoordanateOptions);
        }

        // returns random move
        public Tile[] GetRandomMove()
        {
            List<Tile[]> AllMoves = new List<Tile[]>();

            List<Tile[]>[] AllMovestemp = getAllMovesFromTiles();
            AllMoves.AddRange(AllMovestemp[0]);
            AllMoves.AddRange(AllMovestemp[1]);

            return AllMoves[random.Next(AllMoves.Count)];
        }

        // returns the best move with a calculations of 1 deep
        public Tile[] GetBestMove1()
        {
            List<Tile[]>[] AllMovestemp = getAllMovesFromTiles();

            List<Tile[]> CopyMoves = AllMovestemp[0];
            List<Tile[]> MoveMoves = AllMovestemp[1];


            GameMove gameMove = new GameMove(board);

            TileWithInfectionRate bestMove = new TileWithInfectionRate(CopyMoves.FirstOrDefault() ?? MoveMoves.First(), 0);
            foreach (Tile[] item in MoveMoves)
            {
                int i = gameMove.InfectTiles(item[1], ForTeam, true);
                if (bestMove.infectionCount < i)
                {
                    bestMove = new TileWithInfectionRate(item, i);
                }
            }

            return bestMove.tile;
        }

        // returns all moves that are possible for team ForTeam
        public List<Tile[]>[] getAllMovesFromTiles()
        {
            List<Tile[]> AllCopyMoves = new List<Tile[]>();
            List<Tile[]> AllMoveMoves = new List<Tile[]>();
            foreach (Tile item in board)
            {
                if (item.team == ForTeam)
                {
                    AllCopyMoves.AddRange(getAllCopyMovesFromTile(item));
                    AllMoveMoves.AddRange(getAllMoveMovesFromTile(item));
                }
            }

            return new List<Tile[]>[] { AllCopyMoves, AllMoveMoves};
        }

        // returns all copy moves that are posible from tile
        private List<Tile[]> getAllMoveMovesFromTile(Tile tile)
        {
            List<Tile> tiles = new List<Tile>();
            tiles.AddRange(getAllMovesFromTile(tile, MoveCoordanateOptions));

            List<Tile[]> moves = new List<Tile[]>();

            tiles.ForEach(t => moves.Add(new Tile[] { tile, t }));

            return moves;
        }

        // returns all copy moves that are posible from tile
        private List<Tile[]> getAllCopyMovesFromTile(Tile tile)
        {
            List<Tile> tiles = new List<Tile>();
            tiles.AddRange(getAllMovesFromTile(tile, CopyCoordanateOptions));

            List<Tile[]> moves = new List<Tile[]>();

            tiles.ForEach(t => moves.Add(new Tile[] {tile, t}));

            return moves;
        }

        // returns all the moves that are inside the main board and inside the MoveCoordanates
        private List<Tile> getAllMovesFromTile(Tile tile, int[,,] MoveCoordanates)
        {
            int boardSize = MoveCoordanates.GetLength(0);
            int x = tile.x;
            int y = tile.y;

            List<Tile> tiles = new List<Tile>();

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if(MoveCoordanates[i, j, 0] == 0 && MoveCoordanates[i, j, 1] == 0) continue;
                    int x2 = x + MoveCoordanates[i, j, 0];
                    int y2 = y + MoveCoordanates[i, j, 1];

                    if (x2 < board.GetLength(0) && x2 >= 0)
                        if (y2 < board.GetLength(1) && y2 >= 0)
                            if(board[x2, y2].team == Team.Neutral)
                            {
                                tiles.Add(board[x2, y2]);
                            }
                }
            }
            return tiles;
        }

        class TileWithInfectionRate
        {
            public Tile[] tile { get; set; }
            public int infectionCount { get; set; }

            public TileWithInfectionRate(Tile[] tile, int infectionCount)
            {
                this.tile = tile;
                this.infectionCount = infectionCount;
            }
        }
    }
}
