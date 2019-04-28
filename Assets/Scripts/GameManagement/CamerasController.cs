using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasController : MonoBehaviour
{
    [SerializeField]
    private Camera EnemyIntroCamera;
    [SerializeField]
    private Camera GameCamera;
    [SerializeField]
    private Camera MainMenuCamera;

    public void SetEnemyIntroCamera()
    {
        EnemyIntroCamera.gameObject.SetActive(true);
        GameCamera.gameObject.SetActive(false);
        MainMenuCamera.gameObject.SetActive(false);
    }

    public void SetGameCameraCamera()
    {
        EnemyIntroCamera.gameObject.SetActive(false);
        GameCamera.gameObject.SetActive(true);
        MainMenuCamera.gameObject.SetActive(false);
    }

    public void SetMainMenuCamera()
    {
        EnemyIntroCamera.gameObject.SetActive(false);
        GameCamera.gameObject.SetActive(false);
        MainMenuCamera.gameObject.SetActive(true);
    }
}
