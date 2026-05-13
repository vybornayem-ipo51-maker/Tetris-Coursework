namespace Tetris.src.Logic.Tetrominoes;
using Tetris.src.Logic.Abstract;

public class TTetromino : Tetromino
{
    public TTetromino()
    {
        // Ініціалізуємо матрицю конкретної фігури
        shape = new int[,] 
        {
            {0, 1, 0},
            {1, 1, 1}
        };
    }
}