using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    [SerializeField]
    private Ball ballPrefab;
    [SerializeField]
    private float prepareToLaunchTime = 2.0f;

    public void Initialize()
    {

    }

    public void DisableBall()
    {

    }

    public void DestroyBall()
    {

    }

    public void SpawnBall(Vector3 startPosition)
    {

    }

    public void KickOff()
    {

    }

    public void LaunchBall()
    {

    }


    private IEnumerator PrepareLaunch()
    {
        yield return new WaitForSeconds(prepareToLaunchTime);
        LaunchBall();
        yield return null;
    }

    public void ModifySpeed()
    {

    }
}
