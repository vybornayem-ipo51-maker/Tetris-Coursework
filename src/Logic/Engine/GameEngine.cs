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
    // Використовуємо null!, щоб уникнути попередження CS8618
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
        
        // Ініціалізуємо першу фігуру
        SpawnNewFigure(); 
    }

    private bool CanPlaceFigure(int[,] shape, int x, int y)
    {
        for (int row = 0; row < shape.GetLength(0); row++)
        {
            for (int col = 0; col < shape.GetLength(1); col++)
            {
                if (shape[row, col] == 0) continue;

                int boardX = x + col;
                int boardY = y + row;

                if (boardX < 0 || boardX >= Board.Width || boardY >= Board.Height)
                    return false;

                if (boardY >= 0 && Board.Grid[boardY, boardX] != 0)
                    return false;
            }
        }

        return true;
    }

    private void SpawnNewFigure()
    {
        CurrentFigure = figureFactory.CreateRandomFigure();
        
        // Встановлюємо початкові координати (по центру зверху)
        int shapeWidth = CurrentFigure.GetShape().GetLength(1);
        CurrentFigure.X = (Board.Width - shapeWidth) / 2;
        CurrentFigure.Y = 0;

        // Перевірка на миттєвий програш (Game Over)
        if (!CanPlaceFigure(CurrentFigure.GetShape(), CurrentFigure.X, CurrentFigure.Y))
        {
            IsGameOver = true;
        }
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;
    }

    public void Rotate()
    {
        if (IsPaused || IsGameOver) return;
        
        CurrentFigure.Rotate();
        if (!validator.IsValidPosition(Board, CurrentFigure.GetShape(), CurrentFigure.X, CurrentFigure.Y))
        {
            // Повертаємо ще 3 рази для скасування повороту
            CurrentFigure.Rotate();
            CurrentFigure.Rotate();
            CurrentFigure.Rotate();
        }
    }

    public void Update()
    {
        if (IsPaused || IsGameOver) return;

        // Перевірка умови перемоги (для Sprint або Marathon)
        if (GameMode.CheckWinCondition(LinesCleared))
        {
            IsGameOver = true;
            return;
        }

        // Автоматичне падіння вниз
        MoveDown();
    }

    public void MoveDown()
    {
        if (IsPaused || IsGameOver) return;

        int[,] shape = CurrentFigure.GetShape();

        if (CanPlaceFigure(shape, CurrentFigure.X, CurrentFigure.Y + 1))
        {
            CurrentFigure.Y++;
            return;
        }

        if (!CanPlaceFigure(shape, CurrentFigure.X, CurrentFigure.Y))
        {
            IsGameOver = true;
            return;
        }

        PlaceFigure();
        ClearLines();
        SpawnNewFigure();
    }

    public void MoveLeft()
    {
        if (IsPaused || IsGameOver) return;
        if (validator.IsValidPosition(Board, CurrentFigure.GetShape(), CurrentFigure.X - 1, CurrentFigure.Y))
            CurrentFigure.X--;
    }

    public void MoveRight()
    {
        if (IsPaused || IsGameOver) return;
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
                if (shape[y, x] != 0) // Якщо в цій клітині фігури є блок
                {
                    int boardY = CurrentFigure.Y + y;
                    int boardX = CurrentFigure.X + x;

                    // ПЕРЕВІРКА МЕЖ: Не записуємо, якщо y < 0, щоб не "зламати" гру
                    if (boardY >= 0 && boardY < Board.Height && boardX >= 0 && boardX < Board.Width)
                    {
                        // ЦЕЙ РЯДОК ПЕРЕТВОРЮЄ "ПОРОЖНЄ" МІСЦЕ НА БЛОК
                        Board.Grid[boardY, boardX] = 1; 
                        Board.ColorGrid[boardY, boardX] = CurrentFigure.Color;
                    }
                }
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
            
                // Зсуваємо ряди та їх кольори на один вниз
                for (int rowToMove = y; rowToMove > 0; rowToMove--)
                {
                    for (int x = 0; x < Board.Width; x++)
                    {
                        Board.Grid[rowToMove, x] = Board.Grid[rowToMove - 1, x];
                        Board.ColorGrid[rowToMove, x] = Board.ColorGrid[rowToMove - 1, x];
                    }
                }

                // Очищуємо самий верхній ряд
                for (int x = 0; x < Board.Width; x++) 
                {
                    Board.Grid[0, x] = 0;
                    Board.ColorGrid[0, x] = ConsoleColor.Gray; // Початковий колір
                }
            
                // Перевіряємо цей же ряд знову, бо верхні змістилися вниз
                y++; 
            }
        }    
    }
}