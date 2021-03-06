﻿using System.Collections;
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

    [SerializeField]
    private LeadersBoardManager leadersBoardManager;
    public LeadersBoardManager LeadersBoardManager
    {
        get { return leadersBoardManager; }
    }

    [SerializeField]
    private ScreenFader screenFader;
    public ScreenFader ScreenFader
    {
        get { return screenFader; }
    }

    [SerializeField]
    private CamerasController camerasController;
    public CamerasController CamerasController
    {
        get { return camerasController; }
    }

    [SerializeField]
    private CharacterFactory characterFactory;
    public CharacterFactory CharacterFactory
    {
        get { return characterFactory; }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        uiStateManager.Initialize();
        LeadersBoardManager.Initialize();
        gameManager.Initialize();
    }
}
