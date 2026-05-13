namespace Tetris.src.Logic.Tetrominoes;
using Tetris.src.Logic.Abstract;

public class ZTetromino : Tetromino
{
    public ZTetromino()
    {
        // Ініціалізуємо матрицю конкретної фігури
        shape = new int[,] 
        {
            {1, 1, 0},
            {0, 1, 1}
        };
    }
}
