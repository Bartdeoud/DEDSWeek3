using Game.GameBoard.GameBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.GameBoard
{
    internal class Board
    {
        public TileWithButton[,] BoardTiles;

        // Creates a board with a given size
        public Board(int Size)
        {
            BoardTiles = new TileWithButton[Size,Size];
            for (int i = 0; i < Size; i++)
            {
                for (int i2 = 0; i2 < Size; i2++)
                {
                    // TODO maybe change this to an other type of ITile
                    TileWithButton tile = new TileWithButton() { x = i, y = i2 }; 
                    tile.Text = tile.team.ToString();
                    tile.BackColor = Color.White;
                    BoardTiles[i,i2] = tile;
                }
            }
        }
    }
}
