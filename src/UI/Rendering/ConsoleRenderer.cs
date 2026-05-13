namespace Tetris.src.UI.Rendering;
using Tetris.src.Logic.Engine;
using Tetris.src.Data.Models;


public class ConsoleRenderer
{
    public void Draw(GameEngine engine)
    {
        Console.SetCursorPosition(0, 0);
        // Створюємо копію поля для малювання
        int[,] displayGrid = (int[,])engine.Board.Grid.Clone();

        // Накладаємо поточну фігуру на копію поля
        int[,] shape = engine.CurrentFigure.GetShape();
        for (int y = 0; y < shape.GetLength(0); y++)
        {
            for (int x = 0; x < shape.GetLength(1); x++)
            {
                if (shape[y, x] != 0)
                {
                    int boardY = engine.CurrentFigure.Y + y;
                    int boardX = engine.CurrentFigure.X + x;
                    if (boardY >= 0 && boardY < engine.Board.Height && boardX >= 0 && boardX < engine.Board.Width)
                        displayGrid[boardY, boardX] = 2; // 2 означає активну фігуру
                }
            }
        }

        // Тепер малюємо displayGrid
        for (int y = 0; y < engine.Board.Height; y++)
        {
            Console.Write("|");
            for (int x = 0; x < engine.Board.Width; x++)
            {
                if (displayGrid[y, x] == 0) Console.Write(" .");
                else if (displayGrid[y, x] == 1) Console.Write("[]"); // Статичні блоки
                else Console.Write("()"); // Активна фігура
            }
            Console.WriteLine("|");
        }
    }
}