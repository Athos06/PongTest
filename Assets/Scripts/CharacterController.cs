using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Movement properties")]
    [SerializeField]
    private float moveSpeed = 5.0f;
    [SerializeField]
    private float movementRange = 6.5f;
    [SerializeField]
    CharacterSkillController skillController;


    [Header("Player State flags")]

    public bool debugAI = false;
    private Vector3 movementVector;
    private ICharacterInput playerInput;

    
    private bool inputEnabled = false;
    private bool subcscribedToEvents = false;

    private void Start()
    {
        inputEnabled = false;

        if (debugAI)
            playerInput = new AIInput(transform);
        else
            playerInput = new PlayerInput();

        if (!subcscribedToEvents)
        {
            subcscribedToEvents = true;
            ReferencesHolder.Instance.GameManager.OnGameStarted += OnGameStarted;
            ReferencesHolder.Instance.GameManager.OnGameOver += OnGameOver;
        }
        
    }

    private void Update()
    {

    }

    private void LateUpdate()
    {
        UpdateMovement();
    }

    public void SetMovementVector(Vector3 movementVector)
    {
        this.movementVector = movementVector;
    }
    public void EnableInput(bool enabled)
    {
        inputEnabled = enabled;
    }

    private void UpdateMovement()
    {
        if (!inputEnabled)
            return;

        //We get the player input for this frame
        foreach (Command inputCommand in playerInput.GetInput())
        {
            inputCommand.Execute(this);
        }
    }

    public void SetMove()
    {
        ApplyingMovementVector(movementVector);
    }

    public void UseSkill()
    {
        skillController.UseSkill();
    }

    private void ApplyingMovementVector(Vector3 movementVec)
    {
        Vector3 previousPosition = transform.position;
        transform.Translate(movementVec*moveSpeed*Time.deltaTime, Space.World);
        if(Mathf.Abs(transform.position.x) > movementRange)
        {
            transform.position = previousPosition;
        }
    }

    private void OnDestroy()
    {
        
    }

    private void OnGameStarted()
    {
        inputEnabled = true;
    }

    private void OnGameOver()
    {
        inputEnabled = false;
    }
}
