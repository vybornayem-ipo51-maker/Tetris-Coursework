namespace Tetris.src.Logic.Interfaces;

public interface IGameMode
{
    string Name { get; }

    bool CheckWinCondition(int linesCleared);
}