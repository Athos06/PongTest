using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimerCountdown : MonoBehaviour
{
    public Action OnCountdownFinished;

    private int countdownTime;
    private int timerCurrentTime;
    public int TimerCurrentTime {  get { return timerCurrentTime; } }

    private Coroutine timerCoroutine;


    public void StartCountdown(Action<string> displayTimer, int timeSeconds)
    {
        countdownTime = timeSeconds;
        timerCurrentTime = countdownTime;
        timerCoroutine = StartCoroutine(CountdownCoroutine(displayTimer));
    }


    public void StartTimer(Action<string> displayTimer)
    {
        timerCoroutine = StartCoroutine(TimerCoroutine(displayTimer));
    }

    public void StopTimer()
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);
    }

    private IEnumerator CountdownCoroutine(Action<string> displayTimer)
    {
        float countdown = countdownTime;
        while(countdown >= 0)
        {
            countdown -= Time.deltaTime;
            timerCurrentTime = (int)countdown;
            displayTimer(TimeFormatHelper.GetTimeInFormat(timerCurrentTime));
            yield return null;
        }

        if (OnCountdownFinished != null)
            OnCountdownFinished.Invoke();

        yield return null;
    }


    private IEnumerator TimerCoroutine(Action<string> displayTimer)
    {
        float countdown = 0;
        while (countdown >= 0)
        {
            countdown += Time.deltaTime;
            timerCurrentTime = (int)countdown;
            displayTimer(TimeFormatHelper.GetTimeInFormat(timerCurrentTime));
            yield return null;
        }

        if (OnCountdownFinished != null)
            OnCountdownFinished.Invoke();

        yield return null;
    }

}
