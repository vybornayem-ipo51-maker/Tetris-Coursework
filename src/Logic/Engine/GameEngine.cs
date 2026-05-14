namespace Tetris.src.Logic.Engine;

using Tetris.src.Data.Models;
using Tetris.src.Logic.Abstract;
using Tetris.src.Logic.Factories;
using Tetris.src.Logic.Interfaces;
using Tetris.src.Logic.Services;
using System;

public class GameEngine
{
    public Board Board { get; set; }
    public Tetromino CurrentFigure { get; private set; } = null!;
    public IGameMode GameMode { get; set; }
    public int Score { get; private set; }
    public int LinesCleared { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool IsPaused { get; private set; } = false;

    private readonly FigureFactory figureFactory;
    private readonly ValidationService validator;

    public GameEngine(IGameMode mode)
    {
        Board = new Board();
        GameMode = mode;
        figureFactory = new FigureFactory();
        validator = new ValidationService();
        SpawnNewFigure();
    }


    private void SpawnNewFigure()
    {
        CurrentFigure = figureFactory.CreateRandomFigure();

        int shapeWidth = CurrentFigure.GetShape().GetLength(1);
        int shapeHeight = CurrentFigure.GetShape().GetLength(0);

        CurrentFigure.X = (Board.Width - shapeWidth) / 2;
        CurrentFigure.Y = 0; // починаємо з рядка 0

        // Якщо навіть на старті немає місця — кінець гри
        if (!CanPlaceFigure(CurrentFigure.GetShape(), CurrentFigure.X, CurrentFigure.Y))
            IsGameOver = true;
    }

    public void TogglePause() => IsPaused = !IsPaused;

    private bool CanPlaceFigure(int[,] shape, int x, int y)
    {
        for (int row = 0; row < shape.GetLength(0); row++)
            for (int col = 0; col < shape.GetLength(1); col++)
            {
                if (shape[row, col] == 0) continue;

                int bx = x + col;
                int by = y + row;

                // Вихід за горизонтальні межі або низ — заборонено
                if (bx < 0 || bx >= Board.Width) return false;
                if (by >= Board.Height) return false;

                // Вище поля — дозволяємо рух але не фіксацію
                if (by < 0) continue;

                // Колізія з блоком
                if (Board.Grid[by, bx] != 0) return false;
            }
        return true;
    }
    private bool IsFullyInsideBoard(int[,] shape, int x, int y)
    {
        for (int row = 0; row < shape.GetLength(0); row++)
            for (int col = 0; col < shape.GetLength(1); col++)
            {
                if (shape[row, col] == 0) continue;
                int by = y + row;
                if (by < 0) return false; // хоч один блок вище поля
            }
        return true;
    }

    public void Rotate()
    {
        if (IsPaused || IsGameOver) return;

        // Зберігаємо повний стан перед поворотом
        int[,] savedShape = CurrentFigure.CloneShape();
        int savedX = CurrentFigure.X;
        int savedY = CurrentFigure.Y;

        CurrentFigure.Rotate();

        // Пробуємо поточну позицію
        if (CanPlaceFigure(CurrentFigure.GetShape(), CurrentFigure.X, CurrentFigure.Y))
            return;

        // Wall kick — зміщення по X
        int[] kicks = { 1, -1, 2, -2 };
        foreach (int dx in kicks)
        {
            if (CanPlaceFigure(CurrentFigure.GetShape(), CurrentFigure.X + dx, CurrentFigure.Y))
            {
                CurrentFigure.X += dx;
                return;
            }
        }

        // Нічого не підійшло — повний відкат
        CurrentFigure.RestoreShape(savedShape);
        CurrentFigure.X = savedX;
        CurrentFigure.Y = savedY;
    }

    public void Update()
    {
        if (IsPaused || IsGameOver) return;

        if (GameMode.CheckWinCondition(LinesCleared))
        {
            IsGameOver = true;
            return;
        }

        MoveDown();
    }

    public void MoveDown()
    {
        if (IsPaused || IsGameOver) return;

        int[,] shape = CurrentFigure.GetShape();

        // Якщо можна рухатись вниз — рухаємось
        if (CanPlaceFigure(shape, CurrentFigure.X, CurrentFigure.Y + 1))
        {
            CurrentFigure.Y++;
            return;
        }

        // Не можна рухатись вниз — перевіряємо чи фігура повністю в полі
        if (!IsFullyInsideBoard(shape, CurrentFigure.X, CurrentFigure.Y))
        {
            // Фігура застрягла вище поля — кінець гри
            IsGameOver = true;
            return;
        }

        // Фіксуємо фігуру
        PlaceFigure();
        int cleared = ClearLines();
        AddScore(cleared);
        SpawnNewFigure();
    }
    public void HardDrop()
    {
        if (IsPaused || IsGameOver) return;

        int[,] shape = CurrentFigure.GetShape();
        while (CanPlaceFigure(shape, CurrentFigure.X, CurrentFigure.Y + 1))
            CurrentFigure.Y++;

        if (!IsFullyInsideBoard(shape, CurrentFigure.X, CurrentFigure.Y))
        {
            IsGameOver = true;
            return;
        }

        PlaceFigure();
        int cleared = ClearLines();
        AddScore(cleared);
        SpawnNewFigure();
    }
    public void MoveLeft()
    {
        if (IsPaused || IsGameOver) return;
        if (CanPlaceFigure(CurrentFigure.GetShape(), CurrentFigure.X - 1, CurrentFigure.Y))
            CurrentFigure.X--;
    }

    public void MoveRight()
    {
        if (IsPaused || IsGameOver) return;
        if (CanPlaceFigure(CurrentFigure.GetShape(), CurrentFigure.X + 1, CurrentFigure.Y))
            CurrentFigure.X++;
    }

    private void PlaceFigure()
    {
        int[,] shape = CurrentFigure.GetShape();
        for (int row = 0; row < shape.GetLength(0); row++)
            for (int col = 0; col < shape.GetLength(1); col++)
            {
                if (shape[row, col] == 0) continue;

                int by = CurrentFigure.Y + row;
                int bx = CurrentFigure.X + col;

                if (by >= 0 && by < Board.Height && bx >= 0 && bx < Board.Width)
                {
                    // Якщо клітинка вже зайнята — це баг, але захищаємось
                    if (Board.Grid[by, bx] != 0) continue;

                    Board.Grid[by, bx] = 1;
                    Board.ColorGrid[by, bx] = CurrentFigure.Color;
                }
            }
    }

    private int ClearLines()
    {
        int cleared = 0;
        int y = Board.Height - 1;

        while (y >= 0)
        {
            bool isFull = true;
            for (int x = 0; x < Board.Width; x++)
            {
                if (Board.Grid[y, x] == 0) { isFull = false; break; }
            }

            if (isFull)
            {
                for (int row = y; row > 0; row--)
                    for (int x = 0; x < Board.Width; x++)
                    {
                        Board.Grid[row, x] = Board.Grid[row - 1, x];
                        Board.ColorGrid[row, x] = Board.ColorGrid[row - 1, x];
                    }

                for (int x = 0; x < Board.Width; x++)
                {
                    Board.Grid[0, x] = 0;
                    Board.ColorGrid[0, x] = ConsoleColor.Gray;
                }

                cleared++;
            }
            else
            {
                y--;
            }
        }

        LinesCleared += cleared;
        return cleared;
    }

    private void AddScore(int linesCleared)
    {
        Score += linesCleared switch
        {
            1 => 100,
            2 => 300,
            3 => 700,
            4 => 1500,
            _ => 0
        };
    }
}