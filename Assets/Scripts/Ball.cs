using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rigidBody;

    [SerializeField]
    private float minZSpeed = 1.0f;

    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
      

    }

    public void Initialize(Vector3 startPosition)
    {
        if (rigidBody == null)
            rigidBody = GetComponent<Rigidbody>();

        this.startPosition = startPosition;
        LaunchBall();
    }

    //// Update is called once per frame
    void Update()
    {


        if (Mathf.Abs(rigidBody.velocity.z) < 8)
        {
            Vector3 velocity = rigidBody.velocity;
            if (rigidBody.velocity.z < 0)
            {
                velocity.z = -8;
            }
            else
            {
                velocity.z = 8;
            }

            rigidBody.velocity = velocity;

        }
    }


    [ContextMenu("DebugThrowBall")]
    void LaunchBall()
    {
        transform.position = startPosition;
        //transform.position = Vector3.zero;
        //Ball Chooses a direction
        //Flies that direction

        //Flip a coin, determine direction in x-axis
        int xDirection = Random.Range(0, 2);

        //Flip another coin, determine direction in y-axis
        int yDirection = Random.Range(0, 2);


        Vector3 launchDirection = new Vector3();

        //Check results of one coin toss
        if (xDirection == 0)
        {

            launchDirection.x = -8f;
        }
        if (xDirection == 1)
        {

            launchDirection.x = 8f;
        }

        //Check results of second coin toss
        if (yDirection == 0)
        {
            launchDirection.z = -8f;
        }
        if (yDirection == 1)
        {
            launchDirection.z = 8f;
        }

        //Assign velocity based off of where we launch ball
        rigidBody.velocity = launchDirection;
        //Debug.Log(launchDirection);
    }

    public void DisableBall()
    {
        gameObject.SetActive(false);
    }

    public void GoalScored()
    {
        gameObject.SetActive(false);
    }

    [ContextMenu("Respawn")]
    public void RespawnBall()
    {
        gameObject.SetActive(true);
        LaunchBall();
    }
}
