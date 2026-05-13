namespace Tetris.src.Logic.Tetrominoes;
using Tetris.src.Logic.Abstract;
using System;

public class LTetromino : Tetromino
{
    public override ConsoleColor Color => ConsoleColor.Yellow;
    
    public LTetromino()
    {
        // Ініціалізуємо матрицю конкретної фігури
        shape = new int[,] 
        {
            {1, 0},
            {1, 0},
            {1, 1}
        };
    }
}