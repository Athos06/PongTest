using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeDifficultyController : MonoBehaviour
{
    private Coroutine timerCoroutine;
    private float timeToIncreaseDifficulty = 5;
    private bool stop = true;
    private ChallengeModeManager modeManager;

    public void Initialize(ChallengeModeManager modeManager)
    {
        this.modeManager = modeManager;
        stop = true;
    }
    
    public void StartDiffiultyIncreasing()
    {
        stop = false;
        timerCoroutine = StartCoroutine(Timer());
    }

    public void Stop()
    {
        if (timerCoroutine != null) { 
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
        stop = true;
    }

    private IEnumerator Timer()
    {
        while (!stop)
        {
            yield return new WaitForSeconds(timeToIncreaseDifficulty);
            modeManager.IncreaseDifficulty();
        }
    }
}
