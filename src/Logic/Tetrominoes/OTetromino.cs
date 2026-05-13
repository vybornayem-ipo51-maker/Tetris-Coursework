namespace Tetris.src.Logic.Tetrominoes;
using Tetris.src.Logic.Abstract;
public class OTetromino : Tetromino
{
    public OTetromino()
    {
        shape = new int[,] { {1, 1}, {1, 1} };
    }

    public override void Rotate() 
    {
        // Порожньо: квадрат не крутиться. Це поліморфізм!
    }
}