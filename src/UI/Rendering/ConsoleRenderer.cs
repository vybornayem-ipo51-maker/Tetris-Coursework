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

        int[,] shape = engine.CurrentFigure.GetShape();

        for (int y = 0; y < engine.Board.Height; y++)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("|");

            for (int x = 0; x < engine.Board.Width; x++)
            {
                int fY = y - engine.CurrentFigure.Y;
                int fX = x - engine.CurrentFigure.X;

                // 1) Поточна активна фігура
                bool isCurrent = fY >= 0 && fY < shape.GetLength(0)
                              && fX >= 0 && fX < shape.GetLength(1)
                              && shape[fY, fX] != 0;
                if (isCurrent)
                {
                    Console.ForegroundColor = engine.CurrentFigure.Color;
                    Console.Write("[]");
                    continue;
                }

                // 2) Зафіксований блок
                if (engine.Board.Grid[y, x] != 0)
                {
                    Console.ForegroundColor = engine.Board.ColorGrid[y, x];
                    Console.Write("[]");
                    continue;
                }

                // 3) Порожня клітинка
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(" .");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("|");

            Console.ForegroundColor = ConsoleColor.Cyan;
            string info = y switch
            {
                0  => $"   Режим: {engine.GameMode.Name,-12}",
                1  => $"   Рахунок: {engine.Score,-10}",
                2  => $"   Лінії:  {engine.LinesCleared,-10}",
                4  => "   КЕРУВАННЯ:        ",
                5  => "   ← →  : Рух        ",
                6  => "   ↑    : Поворот    ",
                7  => "   ↓    : Прискорити ",
                8  => "   Space: Скинути    ",
                9  => "   P    : Пауза      ",
                _  => "                     "
            };
            Console.Write(info);
            Console.WriteLine();
        }

        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("+");
        for (int x = 0; x < engine.Board.Width; x++) Console.Write("--");
        Console.WriteLine("+");
        Console.ResetColor();

        if (engine.IsPaused)
        {
            int cx = engine.Board.Width - 3;
            int cy = engine.Board.Height / 2;
            Console.SetCursorPosition(cx, cy);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" ██ ПАУЗА ██ ");
            Console.ResetColor();
        }
    }
}