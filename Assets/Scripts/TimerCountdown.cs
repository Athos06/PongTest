using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimerCountdown : MonoBehaviour
{
    public Action OnCountdownFinished;

    private TextMeshProUGUI timerText;

    private int countdownTime;
    private int countdownCurrentTime;

    private Coroutine timerCoroutine;


    public void StartCountdown(TextMeshProUGUI timerText, int timeSeconds)
    {
        this.timerText = timerText;
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


    public void StartTimer(TextMeshProUGUI timerText)
    {
        //countdownTime = timeSeconds;
        //countdownCurrentTime = countdownTime;
        this.timerText = timerText;
        timerCoroutine = StartCoroutine(TimerCoroutine());
    }

    public void StopTimer()
    {
        if (timerCoroutine != null)
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


    private IEnumerator TimerCoroutine()
    {
        float countdown = 0;
        while (countdown >= 0)
        {
            countdown += Time.deltaTime;
            countdownCurrentTime = (int)countdown;
            timerText.text = TimeFormatHelper.GetTimeInFormat(countdownCurrentTime);
            yield return null;
        }

        if (OnCountdownFinished != null)
            OnCountdownFinished.Invoke();

        yield return null;
    }

}
