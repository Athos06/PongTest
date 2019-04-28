using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFactory : MonoBehaviour
{
    public enum CharactersType
    {
        Player,
        Enemy1,
        Enemy2,
        Enemy3
    }

    [SerializeField]
    public CharacterController enemy1Prefab;
    [SerializeField]
    public CharacterController enemy2Prefab;
    [SerializeField]
    public CharacterController enemy3Prefab;
    [SerializeField]
    private CharacterController HumanPlayerPrefab;

    public CharacterController CreateCharacter(CharactersType characterType)
    {
        switch (characterType)
        {
            case CharactersType.Player:
                return Instantiate(HumanPlayerPrefab);
            case CharactersType.Enemy1:
                return Instantiate(enemy1Prefab);
            case CharactersType.Enemy2:
                return Instantiate(enemy2Prefab);
            case CharactersType.Enemy3:
                return Instantiate(enemy3Prefab);
            default:
                return null;
        }
    }
}
