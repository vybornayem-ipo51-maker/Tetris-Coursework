namespace Tetris.src.Logic.Tetrominoes;
using Tetris.src.Logic.Abstract;

public class LTetromino : Tetromino
{
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