namespace Tetris.src.Logic.Tetrominoes;
using Tetris.src.Logic.Abstract;
using System;
public class OTetromino : Tetromino
{
    public override ConsoleColor Color => ConsoleColor.Magenta;
    
    public OTetromino()
    {
        shape = new int[,] { {1, 1}, {1, 1} };
    }

    public override void Rotate() 
    {
        // Порожньо: квадрат не крутиться. Це поліморфізм!
    }
}