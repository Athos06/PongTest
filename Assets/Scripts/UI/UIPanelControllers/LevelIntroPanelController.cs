using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UIControl;

public class LevelIntroPanelController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI IntroText;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        ReferencesHolder.Instance.UIStateManager.OnPanelOpen += OnPanelOpen;
    }

    private void Populate()
    {
        IntroText.text = ReferencesHolder.Instance.GameManager.GetLevelDescription() ;
    }

    private void OnPanelOpen(UIPanelsIDs id) 
    {
        if(id == UIPanelsIDs.EnemyNamePanel)
            Populate();
    }
}
