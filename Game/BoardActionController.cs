﻿using Game.GameBoard;
using Game.GameBoard.GameBoard;
using Game.MoveCalculator;
using System.Diagnostics;

namespace Game
{
    internal class BoardActionController
    {
        public Board GameBoard;
        private ITile? PreviousTileClicked;
        private GameMove GameMove;
        private Team TeamToMove;
        private Difficulty DifficultyLevel = Difficulty.Easy;
        public static bool GameEnded = false;

        public BoardActionController(int Size)
        {
            TeamToMove = Team.Hoodie;
            GameBoard = new Board(Size);
            SetTeams();
            GameMove = new GameMove(GameBoard.BoardTiles);
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
            /*
            GameBoard.BoardTiles[6, 0].SetTeam(Team.Hoodie);

            GameBoard.BoardTiles[4, 3].SetTeam(Team.BaggySweater);

            GameBoard.BoardTiles[2, 6].SetTeam(Team.Hoodie);
            GameBoard.BoardTiles[3, 6].SetTeam(Team.Hoodie);
            GameBoard.BoardTiles[1, 3].SetTeam(Team.Hoodie);
            */
        }

        // Handles the click event on a tile
        public void TileClicked(TileWithButton tile)
        {

            if (PreviousTileClicked == null)
            {
                if (TeamToMove != tile.team)
                {
                    MessageBox.Show("U geen zet doen met deze tile");
                    return;
                }
                tile.BackColor = Color.Gray;
                PreviousTileClicked = tile;
            }
            else
            {
                if (GameMove.Move(PreviousTileClicked, tile))
                {
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

                ITile[] mediumMove = SimpleMove.GetMediumMove();

                if (move[0].x == mediumMove[0].x && move[0].y == mediumMove[0].y && move[1].x == mediumMove[1].x && move[1].y == mediumMove[1].y)
                {
                    Debug.WriteLine("Medium move");
                }

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
            if (DifficultyLevel == Difficulty.Easy)
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