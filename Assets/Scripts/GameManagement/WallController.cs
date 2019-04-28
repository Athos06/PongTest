using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    public float GetRandomizerPosition()
    {
        return Random.Range(-1.0f, 1.0f);
    }
}
