using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartGameCountdownController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI countdownText;

    public void DisplayCountdownText(string time)
    {
        countdownText.text = time;
    }

}
