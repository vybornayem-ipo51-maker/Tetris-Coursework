using System;

namespace Tetris.src.Data.Models;

public class Board
{
    public int Width { get; init; } = 10;
    public int Height { get; init; } = 20;
    public int[,] Grid { get; set; }
    public ConsoleColor[,] ColorGrid { get; set; }

    public Board()
    {
        Grid = new int[Height, Width];
        ColorGrid = new ConsoleColor[Height, Width];

        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                ColorGrid[y, x] = ConsoleColor.Gray;
    }
}