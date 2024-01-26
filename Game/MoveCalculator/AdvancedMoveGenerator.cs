using Game.GameBoard.GameBoard;
using Microsoft.VisualBasic.Devices;
using System.Diagnostics;
using static Game.MoveCalculator.AdvancedMoveGenerator;

namespace Game.MoveCalculator
{
    internal class AdvancedMoveGenerator : BaseMoveGenerator
    {

        public AdvancedMoveGenerator(ITile[,] board, Team ForTeam) : base(board, ForTeam) { }

        List<MovesWithPerformanceCount> movesWithPerformanceCounts;

        public ITile[] GetAdvancedMove()
        {
            if (PostActionHandler.EndMoveCalulation == true) return movesWithPerformanceCounts?.MaxBy(p => p.performanceCount).moves[0] ?? new ITile[2];
            movesWithPerformanceCounts = new List<MovesWithPerformanceCount>();
            List<ITile[]>[] AllMovestemp = getAllMovesFromTiles();

            List<ITile[]> CopyMoves = AllMovestemp[0];
            List<ITile[]> MoveMoves = AllMovestemp[1];

            GameMove gameMove = new GameMove(CopyBoard(board));
            foreach (ITile[] item in MoveMoves)
            {
                MovesWithPerformanceCount movesWithPerformanceCount = new MovesWithPerformanceCount();
                movesWithPerformanceCount.moves.Add(
                    new ITile[] {
                        new TileToCalculate().CreateTile(item[0].x, item[0].y, item[0].team),
                        new TileToCalculate().CreateTile(item[1].x, item[1].y, item[1].team)
                    });
                movesWithPerformanceCount.performanceCount += gameMove.InfectTiles(item[1], ForTeam, true) + 1;

                movesWithPerformanceCounts.Add(movesWithPerformanceCount);
            }
            CalculatePerformance();

            return movesWithPerformanceCounts?.MaxBy(p => p.performanceCount).moves[0] ?? new ITile[2];
        }

        public void CalculatePerformance()
        {
            foreach (MovesWithPerformanceCount item in movesWithPerformanceCounts)
            {
                if (PostActionHandler.EndMoveCalulation == true) return;
                GameMove gameMove = new GameMove(CopyBoard(board));
                int i = item.moves.Count() - 1;
                gameMove.Move(item.moves[i][0], item.moves[i][1]);
                AdvancedMoveGenerator advancedMoveGenerator = new AdvancedMoveGenerator(gameMove.board, Team.Hoodie);
                advancedMoveGenerator.GetAdvancedMove();
            }
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


        public class MovesWithPerformanceCount
        {
            public int performanceCount { get; set; } = 0;

            public List<ITile[]> moves { get; set; } = new List<ITile[]>();
        }
    }
}
