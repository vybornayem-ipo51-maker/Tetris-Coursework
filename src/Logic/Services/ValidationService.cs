using Tetris.src.Data.Models;

namespace Tetris.src.Logic.Services;

public class ValidationService
{
    // БІЗНЕС-ЛОГІКА валідації
    public bool IsValidPosition(Board board, int[,] shape, int x, int y)
    {
        for (int row = 0; row < shape.GetLength(0); row++)
        {
            for (int col = 0; col < shape.GetLength(1); col++)
            {
                if (shape[row, col] != 0) // Якщо в цій клітинці є блок
                {
                    int newX = x + col;
                    int newY = y + row;

                    // ПЕРЕВІРКА МЕЖ:
                    if (newX < 0 || newX >= board.Width || newY >= board.Height)
                        return false;

                    // ПЕРЕВІРКА КОЛІЗІЇ:
                    if (newY >= 0 && board.Grid[newY, newX] != 0)
                        return false;
                }
            }
        }
        return true;
    }
}