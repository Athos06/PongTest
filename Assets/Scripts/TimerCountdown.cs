using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimerCountdown : MonoBehaviour
{
    public Action OnCountdownFinished;

    [SerializeField]
    private TextMeshProUGUI timerText;

    private int countdownTime;
    private int countdownCurrentTime;

    private Coroutine timerCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartCountdown(int timeSeconds)
    {
        countdownTime = timeSeconds;
        countdownCurrentTime = countdownTime;
        timerCoroutine = StartCoroutine(CountdownCoroutine());
    }
    
    public void StopCountdown()
    {
        if(timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
    }

    private IEnumerator CountdownCoroutine()
    {
        float countdown = countdownTime;
        while(countdown >= 0)
        {
            countdown -= Time.deltaTime;
            countdownCurrentTime = (int)countdown;
            timerText.text = TimeFormatHelper.GetTimeInFormat(countdownCurrentTime);
            yield return null;
        }

        if (OnCountdownFinished != null)
            OnCountdownFinished.Invoke();

        yield return null;
    }
}
