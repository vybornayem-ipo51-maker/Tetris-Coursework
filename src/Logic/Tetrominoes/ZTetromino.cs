namespace Tetris.src.Logic.Tetrominoes;
using Tetris.src.Logic.Abstract;
using System;

public class ZTetromino : Tetromino
{
    public override ConsoleColor Color => ConsoleColor.Gray;
    
    public ZTetromino()
    {
        shape = new int[,] 
        {
            {1, 1, 0},
            {0, 1, 1}
        };
    }
}
