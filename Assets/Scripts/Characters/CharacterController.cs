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
    private CharacterSkillController skillController;
    public CharacterSkillController SkillController {  get { return skillController; } }
    [Space]
    [SerializeField]
    private bool AIPlayer = false;
    public bool IsAIPlayer { get { return AIPlayer; } }
    private Vector3 movementVector;
    public Vector3 MovementVector {  get { return movementVector; } }

    private ICharacterInput playerInput;
    private bool inputEnabled = false;
    private bool subcscribedToEvents = false;

    public void Intialize()
    {
        inputEnabled = false;

        if (!subcscribedToEvents)
        {
            subcscribedToEvents = true;
            ReferencesHolder.Instance.GameManager.OnGameStarted += OnGameStarted;
            ReferencesHolder.Instance.GameManager.OnGameOver += OnGameOver;
            ReferencesHolder.Instance.GameManager.OnGamePause += OnGamePause;
        }
    }

    public void SetInput (ICharacterInput inputSystem)
    {
        playerInput = inputSystem;
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

        if (playerInput != null)
        {
            //We get the player input for this frame
            foreach (Command inputCommand in playerInput.GetInput())
            {
                inputCommand.Execute(this);
            }
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

    private void OnGameStarted()
    {
        inputEnabled = true;
    }

    private void OnGameOver()
    {
        inputEnabled = false;
    }

    private void OnGamePause(bool paused)
    {
        inputEnabled = !paused;
    }
}
