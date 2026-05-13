namespace Tetris.src.Logic.Tetrominoes;
using Tetris.src.Logic.Abstract;

public class STetromino : Tetromino
{
    public STetromino()
    {
        // Ініціалізуємо матрицю конкретної фігури
        shape = new int[,] 
        {
            {0, 1, 1},
            {1, 1, 0}
        };
    }
}