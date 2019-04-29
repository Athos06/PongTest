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
    public bool KickOffLaunch { get; set; }
    public bool ChargedShot { get; set; }

    public void Initialize(Vector3 startPosition, BallManager ballManager)
    {
        if (rigidBody == null)
            rigidBody = GetComponent<Rigidbody>();

        this.ballManager = ballManager;
        this.startPosition = startPosition;
        KickOffLaunch = false;
        ChargedShot = false;
    }

    public void ResetBall()
    {
        rigidBody.velocity = Vector3.zero;
        KickOffLaunch = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (KickOffLaunch)
            {
                ballManager.SetNormalSpeed();
                KickOffLaunch = false;
            }
            if (ChargedShot)
            {
                ballManager.SetNormalSpeed();
                ChargedShot = false;
            }
            CharacterController charController = collision.gameObject.GetComponent<CharacterController>();
            if (charController.SkillController.IsSkillActive) {
                ChargedShot = true;
                ballManager.SetChargedSpeed();
            }
            if (charController != null)
            {
                ballManager.ModifyBallDirection(charController.MovementVector.normalized.x / 2);
            }
        }
        else if (collision.gameObject.CompareTag("ChallengeWall"))
        {
            if (KickOffLaunch)
            {
                ballManager.SetNormalSpeed();
                KickOffLaunch = false;
            }
            if(ChargedShot)
            {
                ballManager.SetNormalSpeed();
                ChargedShot = false;
            }


            WallController wallController = collision.gameObject.GetComponent<WallController>();
            float dir = wallController.GetRandomizerPosition();
            ballManager.ModifyBallDirection(dir);
        }
    }

}
