namespace Tetris.src.Logic.Tetrominoes;
using Tetris.src.Logic.Abstract;
using System;

public class ITetromino : Tetromino
{
    public override ConsoleColor Color => ConsoleColor.Cyan;
    public ITetromino()
    {
        // Ініціалізуємо матрицю конкретної фігури
        shape = new int[,] 
        {
            {1, 1, 1, 1}
        };
    }
}