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
        public Tile[,] BoardTiles;

        // Creates a board with a given size
        public Board(int Size)
        {
            BoardTiles = new Tile[Size,Size];
            for (int i = 0; i < Size; i++)
            {
                for (int i2 = 0; i2 < Size; i2++)
                {
                    Tile tile = new Tile(i, i2);
                    tile.BackColor = Color.White;
                    BoardTiles[i,i2] = tile;
                }
            }
        }
    }
}
