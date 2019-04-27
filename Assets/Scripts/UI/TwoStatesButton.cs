using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwoStatesButton : MonoBehaviour {

    [SerializeField]
    private GameObject _onImage;
    [SerializeField]
    private GameObject _offImage;

    public Button button; 

    private bool _isOn = false;
    public bool IsOn
    {
        get
        {
            return _isOn;
        }
    }
    public void SwitchTo(bool isOn)
    {
        if (isOn)
        {
            _onImage.SetActive(true);
            _offImage.SetActive(false);
            _isOn = true;
        }
        else
        {
            _onImage.SetActive(false);
            _offImage.SetActive(true);
            _isOn = false;
        }
    }
}
