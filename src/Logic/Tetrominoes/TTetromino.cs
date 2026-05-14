namespace Tetris.src.Logic.Tetrominoes;
using Tetris.src.Logic.Abstract;
using System;

public class TTetromino : Tetromino
{
    public override ConsoleColor Color => ConsoleColor.Red;
    
    public TTetromino()
    {
        shape = new int[,] 
        {
            {0, 1, 0},
            {1, 1, 1}
        };
    }
}