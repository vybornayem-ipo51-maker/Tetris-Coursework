namespace Tetris.src.Logic.GameModes;
using Tetris.src.Logic.Interfaces;

public class ClassicMode : IGameMode
{
    public string Name => "Classic";

    public bool CheckWinCondition(int linesCleared)
    {
        return false;
    }
}