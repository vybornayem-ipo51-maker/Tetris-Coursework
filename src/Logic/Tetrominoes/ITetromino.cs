namespace Tetris.src.Logic.Tetrominoes;
using Tetris.src.Logic.Abstract;

public class ITetromino : Tetromino
{
    public ITetromino()
    {
        // Ініціалізуємо матрицю конкретної фігури
        shape = new int[,] 
        {
            {1, 1, 1, 1}
        };
    }
}