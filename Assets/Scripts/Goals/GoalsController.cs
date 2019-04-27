using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class GoalsController : MonoBehaviour
{
    public Action<int> OnScoredGoal;

    [SerializeField]
    private Collider GoalTriggerCollider;

    [SerializeField]
    private int playerNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Ball ball = other.gameObject.GetComponent<Ball>();

        if (ball != null)
        {
            if (OnScoredGoal != null) {
                OnScoredGoal.Invoke(playerNumber);
            }

        }
    }
}
