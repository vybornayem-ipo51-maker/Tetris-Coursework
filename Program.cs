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

            GameEngine engine = new GameEngine(selectedMode);
            ConsoleRenderer renderer = new ConsoleRenderer();

            Console.WriteLine($"Гру розпочато в режимі: {selectedMode.Name}");
            Thread.Sleep(1000);
            Console.Clear();
            Console.CursorVisible = false;

            while (!engine.IsGameOver)
            {
                // Обробляємо всі натиснуті клавіші за кадр
                while (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    HandleInput(key, engine);
                }

                if (!engine.IsPaused)
                    engine.Update();

                Console.SetCursorPosition(0, 0);
                renderer.Draw(engine);
                Thread.Sleep(300);
            }

            renderer.Draw(engine);
            Thread.Sleep(1000);

            Console.Clear();
            Console.CursorVisible = true;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nГРА ЗАКІНЧЕНА!");
            Console.ResetColor();
            Console.WriteLine($"Ваш результат: {engine.Score}");
            Console.WriteLine($"Очищено ліній: {engine.LinesCleared}");
            Console.WriteLine("------------------------------");
            Console.WriteLine("Зіграти ще раз? Y / N");

            bool validChoice = false;
            while (!validChoice)
            {
                var r = Console.ReadKey(true).Key;
                if (r == ConsoleKey.Y) { playAgain = true;  validChoice = true; }
                else if (r == ConsoleKey.N)
                {
                    playAgain = false;
                    validChoice = true;
                    Console.WriteLine("\nДякуємо за гру!");
                    Thread.Sleep(1500);
                }
            }
        }
    }

    static void HandleInput(ConsoleKey key, GameEngine engine)
    {
        if (key == ConsoleKey.P) { engine.TogglePause(); return; }
        if (engine.IsPaused) return;

        switch (key)
        {
            case ConsoleKey.LeftArrow:  engine.MoveLeft();  break;
            case ConsoleKey.RightArrow: engine.MoveRight(); break;
            case ConsoleKey.DownArrow:  engine.MoveDown();  break;
            case ConsoleKey.UpArrow:    engine.Rotate();    break;
            case ConsoleKey.Spacebar:   engine.HardDrop();  break;
        }
    }
}