namespace Tetris.src.Logic.Tetrominoes;
using Tetris.src.Logic.Abstract;
using System;

public class STetromino : Tetromino
{
    public override ConsoleColor Color => ConsoleColor.Green;
    
    public STetromino()
    {
        shape = new int[,] 
        {
            {0, 1, 1},
            {1, 1, 0}
        };
    }
}