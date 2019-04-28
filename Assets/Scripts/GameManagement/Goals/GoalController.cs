using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class GoalController : MonoBehaviour
{
    [SerializeField]
    private int playerNumber;

    private Collider GoalTriggerCollider;
    private GoalsManager goalsManager;

    private void Awake()
    {
        GoalTriggerCollider = GetComponent<Collider>();
    }

    public void Initialize(GoalsManager goalsManager)
    {
        this.goalsManager = goalsManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        Ball ball = other.gameObject.GetComponent<Ball>();

        if (ball != null)
        {
            goalsManager.ScoredGoal(playerNumber);
        }
    }
}
