using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public Ball ActiveBall { get; private set; }

    [SerializeField]
    private Ball ballPrefab;
    [SerializeField]
    private float prepareToLaunchTime = 2.0f;
    [SerializeField]
    private float ballDefaultSpeed = 8.0f;

    private Coroutine prepareLaunchCoroutine;
    private bool ballInPlay = false;
    private float ballCurrentMinSpeed = 8.0f;

    public void Initialize()
    {
        ballCurrentMinSpeed = ballDefaultSpeed;
        ActiveBall = Instantiate(ballPrefab);
        ActiveBall.Initialize(Vector3.zero, this);
        ActiveBall.gameObject.SetActive(false);
    }


    //// Update is called once per frame
    void Update()
    {
        if (!ballInPlay || ActiveBall == null)
            return;

        Vector3 rigidbodyVelocity = ActiveBall.RigidBody.velocity;

        //if (Mathf.Abs(rigidbodyVelocity.z) < ballCurrentMinSpeed)
        //{
        //    Vector3 velocity = rigidbodyVelocity;
        //    if (rigidbodyVelocity.z < 0)
        //    {
        //        velocity.z = -ballCurrentMinSpeed;
        //    }
        //    else
        //    {
        //        velocity.z = ballCurrentMinSpeed;
        //    }

        //    ActiveBall.RigidBody.velocity = velocity;

        //}
    }


    public void DisableBall()
    {
        ActiveBall.gameObject.SetActive(false);
    }

    public void DestroyBall()
    {
        if (prepareLaunchCoroutine != null)
        {
            StopCoroutine(prepareLaunchCoroutine);
            prepareLaunchCoroutine = null;
        }
        Destroy(ActiveBall.gameObject);
        ActiveBall = null;
    }

    public void SpawnBall(Vector3 startPosition)
    {
        Debug.Log("spawning ball");
        ActiveBall.gameObject.SetActive(true);
        ActiveBall.transform.position = startPosition;
    }

    public void KickOff(Vector3 startPosition, int player = -1)
    {
        ActiveBall.gameObject.SetActive(true);
        SpawnBall(startPosition);
        prepareLaunchCoroutine = StartCoroutine(PrepareLaunch(player));
    }

    public void LaunchBall(int player = -1)
    {
        ActiveBall.gameObject.SetActive(true);
        int xDirection = Random.Range(0, 2);
        int zDirection = Random.Range(0, 2);

        if (player != -1)
        {
            Debug.Log("player " + player);
            zDirection = (player == 0) ? 0 : 1;
        }
        Vector3 launchDirection = new Vector3();

        launchDirection.x = (xDirection == 0) ? -1f : 1f;
        launchDirection.z = (zDirection == 0) ? 1f : -1f;

        ActiveBall.RigidBody.velocity = launchDirection;
        ActiveBall.RigidBody.velocity *= ballCurrentMinSpeed;
        ballInPlay = true;
    }


    public void GoalScored()
    {
        ActiveBall.gameObject.SetActive(false);
        ballInPlay = false;
    }

    private IEnumerator PrepareLaunch(int player = -1)
    {
        yield return new WaitForSeconds(prepareToLaunchTime);
        LaunchBall(player);
        yield return null;
    }

    public void ModifyBallSpeed(float percentageChange)
    {
        ballCurrentMinSpeed = ballCurrentMinSpeed + (percentageChange * ballCurrentMinSpeed);
        ActiveBall.RigidBody.velocity = ActiveBall.RigidBody.velocity.normalized * ballCurrentMinSpeed;
    }

    public void ModifyBallDirection(float dirEffect)
    {
        Vector3 directionVector = ActiveBall.RigidBody.velocity.normalized;
        //Debug.Log("FIRST ball direction vector " + directionVector);
        directionVector.x += dirEffect;
        //Debug.Log("SECOND ball direction vector " + directionVector);
        ActiveBall.RigidBody.velocity = directionVector.normalized * ballCurrentMinSpeed;
    }
}
