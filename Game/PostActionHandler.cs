using Game.GameBoard;
using Game.GameBoard.GameBoard;
using Game.MoveCalculator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    internal class PostActionHandler
    {
        public Board GameBoard;
        private ITile? PreviousTileClicked;
        private GameMove GameMove;
        private Team TeamToMove;
        private Difficulty DifficultyLevel = Difficulty.Easy;
        public static bool GameEnded = false;

        public PostActionHandler()
        {
            TeamToMove = Team.Hoodie;
        }

        // Returns the visuals for the board
        public TileWithButton[,] LoadVisuals(int Size) 
        {
            GameBoard = new Board(Size);
            SetTeams();
            GameMove = new GameMove(GameBoard.BoardTiles);
            return GameBoard.BoardTiles;
        }

        // Sets the teams on the board
        private void SetTeams()
        {
            for (int i = 0; i < 2; i++)
            {
                int ArrayLenth = GameBoard.BoardTiles.GetLength(1);
                for (int j = ArrayLenth - 2; j < ArrayLenth; j++)
                {
                    GameBoard.BoardTiles[i, j].SetTeam(Team.BaggySweater);
                    GameBoard.BoardTiles[j, i].SetTeam(Team.Hoodie);
                }
            }
        }

        // Handles the click event on a tile
        public void TileClicked(TileWithButton tile)
        {

            if (PreviousTileClicked == null)
            {
                if(TeamToMove != tile.team)
                {
                    MessageBox.Show("U geen zet doen met deze tile");
                    return;
                }
                tile.BackColor = Color.Gray;
                PreviousTileClicked = tile;
            }
            else
            {
                if (GameMove.Move(PreviousTileClicked, tile)){
                    if (TeamToMove == Team.Hoodie)
                    {
                        TeamToMove = Team.BaggySweater;
                        DoAIMove();
                    }
                    else
                    {
                        TeamToMove = Team.Hoodie;
                    }
                }
                PreviousTileClicked = null;
            }
        }

        // Does the AI move
        private void DoAIMove()
        {
            if (GameEnded) return;
            SimpleMoveCalculator SimpleMove = new SimpleMoveCalculator(GameBoard.BoardTiles, TeamToMove);
            ITile[] move = new ITile[2];

            if (DifficultyLevel == Difficulty.Easy)
            {
                move = SimpleMove.GetRandomMove();
            }
            else if (DifficultyLevel == Difficulty.Medium)
            {
                move = SimpleMove.GetMediumMove();
            }
            else
            {
                //move = new AdvancedMoveThreadHandler(GameBoard.BoardTiles, TeamToMove).GetAdvancedMove();

                move = new AdvancedMoveThreadHandler(GameBoard.BoardTiles, TeamToMove).StartCalculation().Result;

                move[0] = GameBoard.BoardTiles[move[0].x, move[0].y];
                move[1] = GameBoard.BoardTiles[move[1].x, move[1].y];
            }
            Debug.WriteLine(move[0].x + " " + move[0].y + " - " + move[1].x + " " + move[1].y);
            GameMove.Move(move[0], move[1]);

            TeamToMove = Team.Hoodie;
        }

        // Changes the difficulty
        public Difficulty changeDifficulty()
        {
            if(DifficultyLevel == Difficulty.Easy)
            {
                DifficultyLevel = Difficulty.Medium;
            }
            else if (DifficultyLevel == Difficulty.Medium)
            {
                DifficultyLevel = Difficulty.Hard;
            }
            else
            {
                DifficultyLevel = Difficulty.Easy;
            }
            return DifficultyLevel;
        }

        public enum Difficulty
        {
            Easy = 0,
            Medium = 1,
            Hard = 2
        }
    }
}