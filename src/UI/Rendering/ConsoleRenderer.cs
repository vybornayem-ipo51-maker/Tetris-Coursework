namespace Tetris.src.UI.Rendering;
using System;
using Tetris.src.Logic.Engine;
using Tetris.src.Data.Models;

public class ConsoleRenderer
{
    public void Draw(GameEngine engine)
    {
        Console.SetCursorPosition(0, 0);
        Console.CursorVisible = false;

        for (int y = 0; y < engine.Board.Height; y++)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("|");
            for (int x = 0; x < engine.Board.Width; x++)
            {
                // 1. ПЕРЕВІРКА: Чи є тут активна фігура, що зараз падає?
                bool isCurrentFigure = false;
                int[,] shape = engine.CurrentFigure.GetShape();
            
                int fY = y - engine.CurrentFigure.Y;
                int fX = x - engine.CurrentFigure.X;

                if (fY >= 0 && fY < shape.GetLength(0) && fX >= 0 && fX < shape.GetLength(1))
                {
                    if (shape[fY, fX] != 0)
                    {
                        Console.ForegroundColor = engine.CurrentFigure.Color;
                        Console.Write("[]"); // Малюємо фігуру, що падає
                        isCurrentFigure = true;
                    }
                }

                if (!isCurrentFigure)
                {
                    if (engine.Board.Grid[y, x] != 0)
                    {
                        // 2. МАЛЮЄМО ВПАЛУ ФІГУРУ її рідним кольором
                        Console.ForegroundColor = engine.Board.ColorGrid[y, x];
                        Console.Write("[]");
                    }
                
                    else
                    {
                        // 3. ПОРОЖНЯ КЛІТИНКА
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" .");
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("|");

            // Додаємо інфо-панель праворуч від 2-го рядка
            if (y == 6) Console.Write("   КЕРУВАННЯ:");
            if (y == 7) Console.Write("   ← →  : Рух");
            if (y == 8) Console.Write("   ↑    : Поворот");
            if (y == 9) Console.Write("   ↓    : Прискорити");
            if (y == 10) Console.Write("   P    : Пауза / Старт");

            Console.WriteLine();
        }
        Console.ResetColor();
        Console.WriteLine($"\nРахунок: {engine.Score}");
        // --- ЛОГІКА НАПИСУ ПАУЗИ ПОВЕРХ ПОЛЯ ---
        if (engine.IsPaused)
        {
            // Розраховуємо центр (множимо на 2, бо один блок "[]" це 2 символи + 1 символ межі "|")
            int centerX = (engine.Board.Width * 2) / 2 - 3; 
            int centerY = engine.Board.Height / 2;

            Console.SetCursorPosition(centerX, centerY);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("  ПАУЗА  ");
            Console.ResetColor();
        }
    }
}
