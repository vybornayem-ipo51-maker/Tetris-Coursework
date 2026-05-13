using Tetris.src.Logic.Abstract;
using Tetris.src.Logic.Tetrominoes;

namespace Tetris.src.Logic.Factories;

public class FigureFactory
{
    private readonly Random random = new();

    public Tetromino CreateRandomFigure()
    {
        int value = random.Next(0, 5);

        return value switch
        {
            0 => new ITetromino(),
            1 => new OTetromino(),
            2 => new TTetromino(),
            3 => new LTetromino(),
            _ => new ZTetromino()
        };
    }
}