﻿using Game.GameBoard;
using Game.GameBoard.GameBoard;
using Game.MoveCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    internal class PostActionHandler
    {
        public Board GameBoard;
        private Tile? PreviousTileClicked;
        private GameMove GameMove;
        private Team TeamToMove;
        private Difficulty DifficultyLevel = Difficulty.Easy;

        public PostActionHandler()
        {
            TeamToMove = Team.Hoodie;
        }

        // Returns the visuals for the board
        public Tile[,] LoadVisuals(int Size) 
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
        public void TileClicked(Tile tile)
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
            AIMoveGenerator AIMove = new AIMoveGenerator(GameBoard.BoardTiles, TeamToMove);
            Tile[] move;

            if (DifficultyLevel == Difficulty.Easy)
            {
                move = AIMove.GetRandomMove();
            }
            else if (DifficultyLevel == Difficulty.Medium)
            {
                move = AIMove.GetBestMove1();
            }
            else
            {
                move = AIMove.GetBestMove1();
            }

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