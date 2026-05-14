namespace Tetris.src.Logic.Tetrominoes;
using Tetris.src.Logic.Abstract;
using System;

public class JTetromino : Tetromino
{
    public override ConsoleColor Color => ConsoleColor.Blue;

    public JTetromino()
    {
        shape = new int[,] 
        {
            {0, 1},
            {0, 1},
            {1, 1}
        };
    }
}