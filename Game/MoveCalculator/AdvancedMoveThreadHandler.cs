using Game.GameBoard.GameBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Game.MoveCalculator.AdvancedMoveCalculator;

namespace Game.MoveCalculator
{
    internal class AdvancedMoveThreadHandler : BaseMoveCalculator
    {
        List<MovesWithPerformanceCount> movesWithPerformanceCounts = new List<MovesWithPerformanceCount>();

        public AdvancedMoveThreadHandler(ITile[,] board, Team ForTeam) : base(board, ForTeam)
        {
            foreach (ITile[] item in GetAllPossibleMoves())
            {
                movesWithPerformanceCounts.Add(new MovesWithPerformanceCount() { moves = new List<ITile[]>() { item } });
            }
        }

        public void StartCalculation(ITile[,] board, Team teamToMove)
        {
            Team CurrentTeamToMove = teamToMove;
            foreach (MovesWithPerformanceCount item in movesWithPerformanceCounts)
            {
                ITile[] LastMove = item.moves[item.moves.Count() - 1];

                GameMove gameMove = new GameMove(CopyBoard(board));
                gameMove.Move(LastMove[0], LastMove[1]);

                AdvancedMoveCalculator advancedMoveCalculator = new AdvancedMoveCalculator(gameMove.board, CurrentTeamToMove);
                List<MovesWithPerformanceCount> CalculatedMoves = advancedMoveCalculator.CalculateMovesWithPerformanceCount();
            }
        }

        // Get all possible moves from BaseMoveCalculator
        private List<ITile[]> GetAllPossibleMoves()
        {
            List<ITile[]> AllMoves = new List<ITile[]>();

            List<ITile[]>[] AllMovestemp = getAllMovesFromTiles();
            AllMoves.AddRange(AllMovestemp[0]);
            AllMoves.AddRange(AllMovestemp[1]);

            return AllMoves;
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
    }
}
