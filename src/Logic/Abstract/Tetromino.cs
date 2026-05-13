namespace Tetris.src.Logic.Abstract;
public abstract class Tetromino
{
    public int X { get; set; }
    public int Y { get; set; }
    protected int[,] shape; // Додаємо поле для зберігання поточної форми

    public virtual int[,] GetShape() => shape;

    public virtual void Rotate()
    {
        int rows = shape.GetLength(0);
        int cols = shape.GetLength(1);
        int[,] newShape = new int[cols, rows];

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                // Магія повороту: новий стовпець = старий рядок
                // новий рядок = ширина - 1 - старий стовпець
                newShape[x, rows - 1 - y] = shape[y, x];
            }
        }

        shape = newShape;
    }
}