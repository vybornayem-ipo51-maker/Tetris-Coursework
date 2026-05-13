namespace Tetris.src.Logic.GameModes;
using Tetris.src.Logic.Interfaces;

public class SprintMode : IGameMode
{
    public string Name => "Sprint";

    public bool CheckWinCondition(int linesCleared)
    {
        return linesCleared >= 40;
    }
}