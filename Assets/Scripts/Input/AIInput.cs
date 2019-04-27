using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInput : ICharacterInput
{
    private Command movementCommand = new MovementCommand();
    private Ball ball;
    private Transform character;

    public AIInput(Transform character)
    {
        this.ball = GameManager.Instance.GetActiveBall();
        this.character = character;
    }

    public List<Command> GetInput()
    {
      
        ((MovementCommand)movementCommand).MovementeVector = Vector3.zero;
        List<Command> inputCommands = new List<Command>();

        if (ball.transform.position.x < character.position.x)
        {
            ((MovementCommand)movementCommand).MovementeVector += Vector3.left;
            inputCommands.Add(movementCommand);
            //return inputCommands;
        }
        if (ball.transform.position.x > character.position.x)
        {
            ((MovementCommand)movementCommand).MovementeVector += Vector3.right;
            inputCommands.Add(movementCommand);
            //return inputCommands;
        }

        return inputCommands;
    }
}
