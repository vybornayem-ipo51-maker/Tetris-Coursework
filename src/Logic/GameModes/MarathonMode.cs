namespace Tetris.src.Logic.GameModes;
using Tetris.src.Logic.Interfaces;

public class MarathonMode : IGameMode
{
    public string Name => "Marathon";

    public bool CheckWinCondition(int linesCleared)
    {
        return linesCleared >= 150;
    }
}