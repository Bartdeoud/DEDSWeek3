using Game.GameBoard.GameBoard;

namespace Game.MoveCalculator
{
    internal class AdvancedMoveCalculator : BaseMoveCalculator
    {
        public AdvancedMoveCalculator(ITile[,] board, Team ForTeam) : base(board, ForTeam) { }

        public async Task<List<MovesWithPerformanceCount>> CalculateMovesWithPerformanceCount()
        {
            List<MovesWithPerformanceCount> movesWithPerformanceCounts = new List<MovesWithPerformanceCount>();

            List<ITile[]>[] AllMovestemp = getAllMovesFromTiles();

            GameMove gameMove = new GameMove(board);

            // All copy moves
            foreach (ITile[] item in AllMovestemp[0])
            {
                MovesWithPerformanceCount movesWithPerformanceCount = new MovesWithPerformanceCount();
                movesWithPerformanceCount.performanceCount += 1;
                movesWithPerformanceCount.move = (item);
                movesWithPerformanceCount.board = gameMove.board;

                movesWithPerformanceCounts.Add(movesWithPerformanceCount);
            }

            // All move moves
            foreach (ITile[] item in AllMovestemp[1])
            {
                MovesWithPerformanceCount movesWithPerformanceCount = new MovesWithPerformanceCount();
                movesWithPerformanceCount.performanceCount += gameMove.InfectTiles(item[1], ForTeam, true) + 1;
                movesWithPerformanceCount.move = (item);
                movesWithPerformanceCount.board = gameMove.board;

                movesWithPerformanceCounts.Add(movesWithPerformanceCount);
            }

            return movesWithPerformanceCounts;
        }


        public class MovesWithPerformanceCount
        {
            public double performanceCount { get; set; } = 0;
            public ITile[,] board;
            public ITile[] move { get; set; }
            public List<MovesWithPerformanceCount> moves { get; set; } = new List<MovesWithPerformanceCount>();
        }
    }
}
