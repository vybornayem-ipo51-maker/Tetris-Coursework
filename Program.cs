using System;
using System.Threading;
using Tetris.src.Logic.Engine;
using Tetris.src.Logic.GameModes;
using Tetris.src.UI.Rendering;
using Tetris.src.Logic.Interfaces;

namespace Tetris;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        bool playAgain = true;

        while (playAgain)
        {
            Console.Clear();
            Console.CursorVisible = true;
            Console.WriteLine("=== ВІТАЄМО У ТЕТРІСІ ===");
            Console.WriteLine("Оберіть режим гри:");
            Console.WriteLine("1 - Класичний (до поразки)");
            Console.WriteLine("2 - Спринт (40 ліній)");
            Console.WriteLine("3 - Марафон (150 ліній)");

            IGameMode? selectedMode = null;
            while (selectedMode == null)
            {
                string? choice = Console.ReadLine();
                selectedMode = choice switch
                {
                    "1" => new ClassicMode(),
                    "2" => new SprintMode(),
                    "3" => new MarathonMode(),
                    _ => null
                };

                if (selectedMode == null)
                    Console.WriteLine("Невірний вибір. Введіть 1, 2 або 3:");
            }

            // Ініціалізація двигуна та рендерера
            GameEngine engine = new GameEngine(selectedMode);
            ConsoleRenderer renderer = new ConsoleRenderer();

            Console.WriteLine($"Гру розпочато в режимі: {selectedMode.Name}");
            Thread.Sleep(1000);

            Console.Clear();
            Console.CursorVisible = false;

            // Головний ігровий цикл
            while (!engine.IsGameOver)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    HandleInput(key, engine);
                }

                engine.Update();
                renderer.Draw(engine);
                Thread.Sleep(300);
            }

            // Екран завершення гри
            Console.Clear();
            Console.CursorVisible = true;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nГРА ЗАКІНЧЕНА!");
            Console.ResetColor();
            Console.WriteLine($"Ваш результат: {engine.Score}");
            Console.WriteLine("------------------------------");
            Console.WriteLine("Чи хочете зіграти ще раз?");
            Console.WriteLine("Натисніть Y (Так) або N (Ні)");

            bool validChoice = false;
            while (!validChoice)
            {
                var response = Console.ReadKey(true).Key;
                if (response == ConsoleKey.Y)
                {
                    playAgain = true;
                    validChoice = true;
                }
                else if (response == ConsoleKey.N)
                {
                    playAgain = false;
                    validChoice = true;
                    Console.WriteLine("\nДякуємо за гру! Бувай!");
                    Thread.Sleep(1500);
                }
            }
        }
    }

    static void HandleInput(ConsoleKey key, GameEngine engine)
    {
        // Кнопка P для паузи
        if (key == ConsoleKey.P)
        {
            engine.TogglePause();
            return;
        }

        // Якщо гра на паузі, стрілочки не працюють
        if (engine.IsPaused) return;

        switch (key)
        {
            case ConsoleKey.LeftArrow: engine.MoveLeft(); break;
            case ConsoleKey.RightArrow: engine.MoveRight(); break;
            case ConsoleKey.DownArrow: engine.MoveDown(); break;
            case ConsoleKey.UpArrow: engine.Rotate(); break;
        }
    }
}