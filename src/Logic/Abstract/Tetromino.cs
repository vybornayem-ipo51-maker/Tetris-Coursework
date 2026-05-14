namespace Tetris.src.Logic.Abstract;
using System;

public abstract class Tetromino
{
    public int X { get; set; }
    public int Y { get; set; }
    public abstract ConsoleColor Color { get; }
    protected int[,]? shape;

    public virtual int[,] GetShape() => shape ?? new int[0, 0];

    public virtual void Rotate()
    {
        if (shape == null) return;

        int rows = shape.GetLength(0);
        int cols = shape.GetLength(1);
        int[,] newShape = new int[cols, rows];

        for (int y = 0; y < rows; y++)
        for (int x = 0; x < cols; x++)
            newShape[x, rows - 1 - y] = shape[y, x];

        shape = newShape;
    }

    // Повертає глибоку копію shape для збереження стану
    public int[,] CloneShape()
    {
        if (shape == null) return new int[0, 0];
        int rows = shape.GetLength(0);
        int cols = shape.GetLength(1);
        int[,] copy = new int[rows, cols];
        Array.Copy(shape, copy, shape.Length);
        return copy;
    }

    // Відновлює shape із збереженої копії
    public void RestoreShape(int[,] savedShape)
    {
        shape = savedShape;
    }
}