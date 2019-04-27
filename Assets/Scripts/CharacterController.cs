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

    [Header("Player State flags")]
    /// <summary>
    /// When the character is not moving isIdle is true
    /// </summary>
    public bool isIdle = true;

    public bool debugAI = false;

    private Vector3 movementVector;
   
    private ICharacterInput playerInput;
    //public PlayerAnimatorController playerAnimator;

    [SerializeField]
    CharacterSkillController skillController;

    private void Awake()
    {


    }

    private void Start()
    {
        if (debugAI)
            playerInput = new AIInput(transform);
        else
            playerInput = new PlayerInput();
    }

    private void Update()
    {
        //if (!Manager.Instance.CanPlay()) return;
        //if (isDead)
        //{
        //    UpdateAnimator();
        //    return;
        //}

        //if (isIdle)
        //{
        //    UpdateMovement();
        //}

        //UpdateAnimator();
    }

    private void LateUpdate()
    {
        UpdateMovement();
    }

    public void SetMovementVector(Vector3 movementVector)
    {
        this.movementVector = movementVector;
    }


    /// <summary>
    /// Calls the PlayerAnimatorController to play the proper animation
    /// </summary>
    //void UpdateAnimator()
    //{
    //    playerAnimator.UpdateAnimator();
    //}

    /// <summary>
    /// Called everyframe to get the input from the player
    /// </summary>
    void UpdateMovement()
    {
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


    void ApplyingMovementVector(Vector3 movementVec)
    {
        Vector3 previousPosition = transform.position;
        transform.Translate(movementVec*moveSpeed*Time.deltaTime, Space.World);
        if(Mathf.Abs(transform.position.x) > movementRange)
        {
            transform.position = previousPosition;
        }
    }
    
    
}
