using Game.GameBoard.GameBoard;
using Microsoft.VisualBasic.Devices;
using System.Diagnostics;
using System.Threading;
using static Game.MoveCalculator.AdvancedMoveGenerator;

namespace Game.MoveCalculator
{
    internal class AdvancedMoveGenerator : BaseMoveGenerator
    {

        public AdvancedMoveGenerator(ITile[,] board, Team ForTeam) : base(board, ForTeam) { }

        public ITile[] GetAdvancedMove()
        {
            List<MovesWithPerformanceCount> PerformanceCounts = CalculateMovesWithPerformanceCount();
            return PerformanceCounts.MaxBy(p => p.performanceCount).moves[0];
        }

        // returns a new board with only the inportand values from the old board
        private ITile[,] CopyBoard(ITile[,] originalBoard)
        {
            int rows = originalBoard.GetLength(0);
            int cols = originalBoard.GetLength(1);
            ITile[,] copiedBoard = new ITile[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (originalBoard[i, j] != null)
                    {
                        copiedBoard[i, j] = new TileToCalculate().CreateTile(originalBoard[i, j].x, originalBoard[i, j].y, originalBoard[i, j].team);
                    }
                }
            }

            return copiedBoard;
        }

        private List<MovesWithPerformanceCount> CalculateMovesWithPerformanceCount()
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

        public class MovesWithPerformanceCount
        {
            public int performanceCount { get; set; } = 0;

            public List<ITile[]> moves { get; set; } = new List<ITile[]>();
        }
    }
}
