using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalsManager : MonoBehaviour
{
    public Action<int> OnScoredGoal;

    [SerializeField]
    private GoalController[] goals;
    
    // Start is called before the first frame update
    public void Initialize()
    {
        foreach (var goal in goals)
            goal.Initialize(this);
    }

    public void ScoredGoal(int player)
    {
        if (OnScoredGoal != null)
            OnScoredGoal.Invoke(player);
    }
}
