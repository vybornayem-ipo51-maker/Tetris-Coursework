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
        Console.WriteLine("=== ВІТАЄМО У ТЕТРІСІ ===");
        Console.WriteLine("Оберіть режим гри:");
        Console.WriteLine("1 - Класичний (до поразки)");
        Console.WriteLine("2 - Спринт (40 ліній)");
        Console.WriteLine("3 - Марафон (150 ліній)");

        var choice = Console.ReadLine();
        IGameMode selectedMode = choice switch
        {
            "2" => new SprintMode(),
            "3" => new MarathonMode(),
            _ => new ClassicMode()
        };

        GameEngine engine = new GameEngine(selectedMode);
        ConsoleRenderer renderer = new ConsoleRenderer();

        Console.WriteLine($"Гру розпочато в режимі: {selectedMode.Name}");
        Thread.Sleep(1000);

        while (!engine.IsGameOver)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                HandleInput(key, engine);
            }

            engine.Update();
            renderer.Draw(engine);
            Thread.Sleep(500);
        }

        Console.WriteLine("\nГРА ЗАКІНЧЕНА!");
        Console.WriteLine($"Ваш результат: {engine.Score}");
    }

    static void HandleInput(ConsoleKey key, GameEngine engine)
    {
        switch (key)
        {
            case ConsoleKey.LeftArrow: engine.MoveLeft(); break;
            case ConsoleKey.RightArrow: engine.MoveRight(); break;
            case ConsoleKey.DownArrow: engine.MoveDown(); break;
            case ConsoleKey.UpArrow: engine.Rotate(); break;
        }
    }
}