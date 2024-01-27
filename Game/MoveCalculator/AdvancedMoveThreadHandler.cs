using Game.GameBoard.GameBoard;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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
            List<MovesWithPerformanceCount> list1 = await CalculationThread(movesWithPerformanceCounts, true, 2);

            List<MovesWithPerformanceCount> list2 = list1.OrderByDescending(x => x.performanceCount).ToList();
            ITile[] move = list2.First().move;

            return move;
        }

        //TODO improve clauclations
        private async Task<List<MovesWithPerformanceCount>> CalculationThread(List<MovesWithPerformanceCount> list1, bool MainTeam, int MaxDeep)
        {
            List<MovesWithPerformanceCount> list2 = await CalculatePerformanceCount(list1, getOtherTeam(list1[0].move[0].team).Result);
            if (MaxDeep == 0)
            {
                foreach (var item in list2)
                {
                    int i = item.moves.Sum(m => m.performanceCount) / list2.Count;
                    if (MainTeam)
                    {
                        item.performanceCount += i;
                    }
                    else
                    {
                        item.performanceCount -= i;
                    }
                }
                return list2;
            }

            MaxDeep--;

            var tasks = list2.Select(item => CalculationThread(item.moves, !MainTeam, MaxDeep)).ToList();
            var results = await Task.WhenAll(tasks);

            for (int j = 0; j < list2.Count; j++)
            {
                list2[j].moves = results[j]; // Zorg dat dit correct is op basis van je logica
                int i = list2[j].moves.Sum(m => m.performanceCount) / list2[j].moves.Count;
                //writeCountsToConsole(list2);
                list2[j].performanceCount += MainTeam ? i : -i;
            }

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
