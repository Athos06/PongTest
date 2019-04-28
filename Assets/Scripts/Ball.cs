using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rigidBody;
    public Rigidbody RigidBody { get { return rigidBody; } }

    [SerializeField]
    private float minZSpeed = 1.0f;

    private Vector3 startPosition;
    private BallManager ballManager;

    public void Initialize(Vector3 startPosition, BallManager ballManager)
    {
        if (rigidBody == null)
            rigidBody = GetComponent<Rigidbody>();

        this.ballManager = ballManager;
        this.startPosition = startPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CharacterController charController = collision.gameObject.GetComponent<CharacterController>();
            if (charController != null)
            {
                Debug.Log("player collision " + charController.MovementVector.normalized.x / 2);
                ballManager.ModifyBallDirection(charController.MovementVector.normalized.x/2);
            }
        }
        else if (collision.gameObject.CompareTag("ChallengeWall"))
        {
            WallController wallController = collision.gameObject.GetComponent<WallController>();
            float dir = wallController.GetRandomizerPosition();
            Debug.Log(" Test " + dir);
            ballManager.ModifyBallDirection(dir);
        }
    }

}
