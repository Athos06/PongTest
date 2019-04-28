using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuPanelController : MonoBehaviour
{
    [SerializeField]
    private Button resume;
    [SerializeField]
    private Button quit;

    private void Awake()
    {
        Initialize();    
    }

    public void Initialize()
    {
        resume.onClick.RemoveAllListeners();
        resume.onClick.AddListener(OnResumeClick);
        quit.onClick.RemoveAllListeners();
        quit.onClick.AddListener(OnQuitClick);
    }

    private void OnResumeClick()
    {
        ReferencesHolder.Instance.GameManager.UnPauseGame();
    }

    private void OnQuitClick()
    {
        ReferencesHolder.Instance.GameManager.UnPauseGame();
        ReferencesHolder.Instance.UIStateManager.CloseAll();
        ReferencesHolder.Instance.GameManager.QuitGame();
    }
}
