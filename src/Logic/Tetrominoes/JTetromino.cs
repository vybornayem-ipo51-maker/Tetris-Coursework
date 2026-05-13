namespace Tetris.src.Logic.Tetrominoes;
using Tetris.src.Logic.Abstract;

public class JTetromino : Tetromino
{
    public JTetromino()
    {
        // Ініціалізуємо матрицю конкретної фігури
        shape = new int[,] 
        {
            {0, 1},
            {0, 1},
            {1, 1}
        };
    }
}