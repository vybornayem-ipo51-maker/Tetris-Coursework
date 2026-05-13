namespace Tetris.src.Data.Models;

public class Board
{
    public int Width { get; set; } = 10;

    public int Height { get; set; } = 20;

    public int[,] Grid { get; set; }

    public Board()
    {
        Grid = new int[Height, Width];
    }
}