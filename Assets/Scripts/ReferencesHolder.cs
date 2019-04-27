using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIControl;

public class ReferencesHolder : MonoBehaviour
{
    private static ReferencesHolder instance;
    public static ReferencesHolder Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ReferencesHolder>();
            return instance;
        }
    }

    [SerializeField]
    private UIStateManager uiStateManager;
    public UIStateManager UIStateManager
    {
        get { return uiStateManager; }
    }
    [SerializeField]
    private GameManager gameManager;
    public GameManager GameManager
    {
        get { return gameManager; }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        uiStateManager.Initialize();
        gameManager.Initialize();
    }

}
