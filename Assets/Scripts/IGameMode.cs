using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameMode
{
    void StartGameMode();
    void GameModeOver();
    void FinishGameMode();
    void RestartGameMode();
}
