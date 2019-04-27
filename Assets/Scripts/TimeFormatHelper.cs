using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeFormatHelper 
{
    public static string GetTimeInFormat(int numberOfSeconds)
    {
        int mins;
        int seconds;

        mins = numberOfSeconds / 60;
        seconds = numberOfSeconds % 60;

        return mins+":"+seconds.ToString("00");
    }
}
