using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCommand : Command
{
    /// <summary>
    /// The direction in which we are going to move
    /// </summary>
    private Vector3 movementVector;
    public Vector3 MovementeVector
    {
        get
        {
            return movementVector;
        }
        set
        {
            movementVector = value;
        }
    }

    /// <summary>
    /// Execute the command 
    /// </summary>
    /// <param name="character">The character this command controls</param>
    public override void Execute(CharacterController character)
    {
        character.SetMovementVector(movementVector);
        character.SetMove();
    }
}