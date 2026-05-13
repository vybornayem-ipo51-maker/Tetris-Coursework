namespace Tetris.src.Logic.Engine;
using Tetris.src.Data.Models;
using Tetris.src.Logic.Abstract;
using Tetris.src.Logic.Factories;
using Tetris.src.Logic.Interfaces;
using Tetris.src.Logic.Services;

public class GameEngine
{
    public Board Board { get; set; }
    public Tetromino CurrentFigure { get; private set; }
    public IGameMode GameMode { get; set; }
    public int Score { get; private set; }
    public int LinesCleared { get; private set; }
    public bool IsGameOver { get; private set; }

    private readonly FigureFactory figureFactory;
    private readonly ValidationService validator;

    public GameEngine(IGameMode mode)
    {
        Board = new Board();
        GameMode = mode;
        figureFactory = new FigureFactory();
        validator = new ValidationService();
        CurrentFigure = figureFactory.CreateRandomFigure();
        
        // Початкова позиція фігури (по центру зверху)
        CurrentFigure.X = Board.Width / 2 - 1;
        CurrentFigure.Y = 0;
    }
    public void Rotate()
    {
        CurrentFigure.Rotate();
        // Якщо після повороту фігура стоїть невалідно - скасовуємо поворот
        if (!validator.IsValidPosition(Board, CurrentFigure.GetShape(), CurrentFigure.X, CurrentFigure.Y))
        {
            // Повертаємо ще 3 рази, щоб повернути у вихідний стан
            CurrentFigure.Rotate();
            CurrentFigure.Rotate();
            CurrentFigure.Rotate();
        }
    }

    public void Update()
    {
        if (IsGameOver) return;
        if (GameMode.CheckWinCondition(LinesCleared))
        {
            IsGameOver = true;
            Console.WriteLine($"ВІТАЄМО! Режим {GameMode.Name} пройдено!");
            return;
        }
        MoveDown();
    }

    public void MoveDown()
    {
        if (validator.IsValidPosition(Board, CurrentFigure.GetShape(), CurrentFigure.X, CurrentFigure.Y + 1))
        {
            CurrentFigure.Y++;
        }
        else
        {
            PlaceFigure();
            ClearLines();
            CurrentFigure = figureFactory.CreateRandomFigure();
            CurrentFigure.X = Board.Width / 2 - 1;
            CurrentFigure.Y = 0;

            if (!validator.IsValidPosition(Board, CurrentFigure.GetShape(), CurrentFigure.X, CurrentFigure.Y))
            {
                IsGameOver = true;
            }
        }
    }

    public void MoveLeft()
    {
        if (validator.IsValidPosition(Board, CurrentFigure.GetShape(), CurrentFigure.X - 1, CurrentFigure.Y))
            CurrentFigure.X--;
    }

    public void MoveRight()
    {
        if (validator.IsValidPosition(Board, CurrentFigure.GetShape(), CurrentFigure.X + 1, CurrentFigure.Y))
            CurrentFigure.X++;
    }

    private void PlaceFigure()
    {
        int[,] shape = CurrentFigure.GetShape();
        for (int y = 0; y < shape.GetLength(0); y++)
        {
            for (int x = 0; x < shape.GetLength(1); x++)
            {
                if (shape[y, x] != 0)
                    Board.Grid[CurrentFigure.Y + y, CurrentFigure.X + x] = 1;
            }
        }
    }

    private void ClearLines()
    {
        for (int y = Board.Height - 1; y >= 0; y--)
        {
            bool isFull = true;
            for (int x = 0; x < Board.Width; x++)
            {
                if (Board.Grid[y, x] == 0) { isFull = false; break; }
            }
            if (isFull)
            {
                Score += 100;
                LinesCleared++;
            
                // Зсуваємо всі ряди, що вище, на один вниз
                for (int rowToMove = y; rowToMove > 0; rowToMove--)
                {
                    for (int x = 0; x < Board.Width; x++)
                    {
                        Board.Grid[rowToMove, x] = Board.Grid[rowToMove - 1, x];
                    }
                }
                // Очищуємо самий верхній ряд
                for (int x = 0; x < Board.Width; x++) Board.Grid[0, x] = 0;
            
                y++; // Перевіряємо цей же ряд знову, бо зверху впав новий
            }
        }    
    }
}
