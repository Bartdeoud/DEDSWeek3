using Game.GameBoard.GameBoard;

namespace Game.MoveCalculator
{
    // Class that calculates Easy and Medium moves
    internal class SimpleMoveCalculator : BaseMoveCalculator
    {
        Random random = new Random();

        public SimpleMoveCalculator(ITile[,] board, Team ForTeam) : base(board, ForTeam) { }

        // returns random move
        public ITile[] GetRandomMove()
        {
            List<ITile[]> AllMoves = new List<ITile[]>();

            List<ITile[]>[] AllMovestemp = getAllMovesFromTiles();
            AllMoves.AddRange(AllMovestemp[0]);
            AllMoves.AddRange(AllMovestemp[1]);

            return AllMoves[random.Next(AllMoves.Count)];
        }

        // returns the best move with a calculations of 1 deep
        public ITile[] GetMediumMove()
        {
            List<ITile[]>[] AllMovestemp = getAllMovesFromTiles();

            List<ITile[]> CopyMoves = AllMovestemp[0];
            List<ITile[]> MoveMoves = AllMovestemp[1];


            GameMove gameMove = new GameMove(board);

            TileWithInfectionRate bestMove = new TileWithInfectionRate(CopyMoves.FirstOrDefault() ?? MoveMoves.First(), 0);
            foreach (ITile[] item in MoveMoves)
            {
                int i = gameMove.InfectTiles(item[1], ForTeam, true);
                if (bestMove.infectionCount < i)
                {
                    bestMove = new TileWithInfectionRate(item, i);
                }
            }

            return bestMove.tile;
        }
    }
}
