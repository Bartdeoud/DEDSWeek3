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

        //TODO fix this

        // Creates a board with a given size
        public Board(int Size)
        {
            BoardTiles = new TileWithButton[Size,Size];
            for (int i = 0; i < Size; i++)
            {
                for (int i2 = 0; i2 < Size; i2++)
                {
                    TileWithButton tile = new TileWithButton() { x = i, y = i2 };  //.CreateTile(i, i2);
                    tile.Text = tile.team.ToString();
                    tile.BackColor = Color.White;
                    BoardTiles[i,i2] = tile;
                }
            }
        }
    }
}
