using Game.GameBoard.GameBoard;
using Microsoft.VisualBasic.Devices;
using System.Diagnostics;
using System.Threading;
using static Game.MoveCalculator.AdvancedMoveCalculator;

namespace Game.MoveCalculator
{
    internal class AdvancedMoveCalculator : BaseMoveCalculator
    {
        public AdvancedMoveCalculator(ITile[,] board, Team ForTeam) : base(board, ForTeam) { }

        public List<MovesWithPerformanceCount> GetAdvancedMove()
        {
            return CalculateMovesWithPerformanceCount();
        }

        public List<MovesWithPerformanceCount> CalculateMovesWithPerformanceCount()
        {
            List<MovesWithPerformanceCount> movesWithPerformanceCounts = new List<MovesWithPerformanceCount>();

            List<ITile[]>[] AllMovestemp = getAllMovesFromTiles();

            // All copy moves
            foreach (ITile[] item in AllMovestemp[0])
            {
                MovesWithPerformanceCount movesWithPerformanceCount = new MovesWithPerformanceCount();
                movesWithPerformanceCount.performanceCount += 1;
                movesWithPerformanceCount.moves.Add(item);

                movesWithPerformanceCounts.Add(movesWithPerformanceCount);
            }

            // All move moves
            GameMove gameMove = new GameMove(board);
            foreach (ITile[] item in AllMovestemp[1])
            {
                MovesWithPerformanceCount movesWithPerformanceCount = new MovesWithPerformanceCount();
                movesWithPerformanceCount.performanceCount += gameMove.InfectTiles(item[1], ForTeam, true) + 1;
                movesWithPerformanceCount.moves.Add(item); 

                movesWithPerformanceCounts.Add(movesWithPerformanceCount);
            }

            return movesWithPerformanceCounts;
        }

        // TODO stop hier een list<MovesWithPerformanceCount> in
        public class MovesWithPerformanceCount
        {
            public int performanceCount { get; set; } = 0;

            public List<ITile[]> moves { get; set; } = new List<ITile[]>();
        }
    }
}
