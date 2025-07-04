using Game.GameBoard.GameBoard;
using static Game.MoveCalculator.AdvancedMoveCalculator;

namespace Game.MoveCalculator
{
    internal class AdvancedMoveThreadHandler
    {
        private List<MovesWithPerformanceCount> movesWithPerformanceCounts = new List<MovesWithPerformanceCount>();
        private ITile[,] board;

        public AdvancedMoveThreadHandler(ITile[,] board, Team ForTeam)
        {
            this.board = board;
            AdvancedMoveCalculator advancedMoveCalculator = new AdvancedMoveCalculator(CopyBoard(board), ForTeam);
            movesWithPerformanceCounts = advancedMoveCalculator.CalculateMovesWithPerformanceCount().Result;
        }

        public async Task<ITile[]> StartCalculation()
        {
            //List<MovesWithPerformanceCount> list1 = await CalculationThread(movesWithPerformanceCounts, true, 2);

            //List<MovesWithPerformanceCount> list2 = list1.OrderByDescending(x => x.performanceCount).ToList();

            //MovesWithPerformanceCount movesWithPerformanceCount = new MovesWithPerformanceCount();
            //movesWithPerformanceCount.moves = movesWithPerformanceCounts;
            //MovesWithPerformanceCount list3 = FindBestMove(movesWithPerformanceCount, 100, true);

            //ITile[] move = list2.First().move;

            //return list3.move;
            return null;
        }

        public MovesWithPerformanceCount FindBestMove(MovesWithPerformanceCount currentMove, int depth, bool isMaximizingPlayer)
        {
            MovesWithPerformanceCount bestMove = null;
            double bestScore = isMaximizingPlayer ? double.MinValue : double.MaxValue;

            foreach (var move in currentMove.moves)
            {
                double score = CalculateBestMove(move, depth, !isMaximizingPlayer);

                if (isMaximizingPlayer && score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
                else if (!isMaximizingPlayer && score < bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
            }

            return bestMove;
        }


        private double CalculateBestMove(MovesWithPerformanceCount currentMove, int depth, bool isMaximizingPlayer)
        {
            if (depth == 0 || currentMove.moves.Count == 0)
            {
                return currentMove.performanceCount;
            }

            if (isMaximizingPlayer)
            {
                double maxEval = double.MinValue;
                foreach (var move in currentMove.moves)
                {
                    double eval = CalculateBestMove(move, depth - 1, false);
                    maxEval = Math.Max(maxEval, eval);
                }
                return maxEval;
            }
            else
            {
                double minEval = double.MaxValue;
                foreach (var move in currentMove.moves)
                {
                    double eval = CalculateBestMove(move, depth - 1, true);
                    minEval = Math.Min(minEval, eval);
                }
                return minEval;
            }
        }


        //TODO improve clauclations
        private async Task<List<MovesWithPerformanceCount>> CalculationThread(List<MovesWithPerformanceCount> list1, int MaxDeep, bool tja)
        {

            foreach (int i in Enumerable.Range(0, MaxDeep))
            {
                foreach (MovesWithPerformanceCount mwpc in list1)
                {

                    mwpc.moves = await CalculationThread(mwpc.moves, MaxDeep, tja);
                }
                list1 = await CalculationThread(list1, MaxDeep, tja);
            }
            MaxDeep--;

            List<MovesWithPerformanceCount> list2 = await CalculatePerformanceCount(list1, getOtherTeam(list1[0].move[0].team).Result);

            return list2;
        }

        private async Task<Team> getOtherTeam(Team team)
        {
            if (team == Team.Hoodie) return Team.BaggySweater;
            else return Team.Hoodie;
        }

        public async Task<List<MovesWithPerformanceCount>> CalculatePerformanceCount(List<MovesWithPerformanceCount> PCs, Team CurrentTeamToMove)
        {
            foreach (MovesWithPerformanceCount item in PCs)
            {
                ITile[] LastMove = item.move;

                ITile[,] boardToCoppy;
                if (item.board == null)
                {
                    boardToCoppy = CopyBoard(board);
                }
                else
                {
                    boardToCoppy = CopyBoard(item.board);
                }

                GameMove gameMove = new GameMove(boardToCoppy);
                gameMove.Move(LastMove[0], LastMove[1]);

                AdvancedMoveCalculator advancedMoveCalculator = new AdvancedMoveCalculator(gameMove.board, CurrentTeamToMove);
                item.moves = await advancedMoveCalculator.CalculateMovesWithPerformanceCount();
            }
            PCs = PCs.OrderByDescending(x => x.performanceCount).ToList();
            return PCs;
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