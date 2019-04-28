using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameMode
{
    string GetLevelDescription();
    void StartGameMode();
    void GameModeOver();
    void FinishGameMode();
    void RestartGameMode();
}
