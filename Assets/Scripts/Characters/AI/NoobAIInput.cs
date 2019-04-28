using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoobAIInput : ICharacterInput
{
    private Command movementCommand = new MovementCommand();
    private Ball ball;
    private Transform character;
    private float sensitivity = 0.1f;
    private float reactionTime = 0.02f;
    private float reactionTimeCounter = 0.0f;

    public NoobAIInput(Transform character, Ball ball)
    {
        this.character = character;
        this.ball = ball;
    }

    public List<Command> GetInput()
    {

        ((MovementCommand)movementCommand).MovementeVector = Vector3.zero;
        List<Command> inputCommands = new List<Command>();
        inputCommands.Add(movementCommand);

        if (ball == null)
            return inputCommands;

        reactionTimeCounter += Time.deltaTime;
        if (reactionTimeCounter > reactionTime)
        {
            reactionTimeCounter = 0;
            if (ball.transform.position.x < character.position.x
            && Mathf.Abs(ball.transform.position.x - character.position.x) > sensitivity)
            {
                ((MovementCommand)movementCommand).MovementeVector += Vector3.left;
                inputCommands.Add(movementCommand);
            }
            if (ball.transform.position.x > character.position.x
                && Mathf.Abs(ball.transform.position.x - character.position.x) > sensitivity)
            {
                ((MovementCommand)movementCommand).MovementeVector += Vector3.right;
                inputCommands.Add(movementCommand);
            }
        }


        return inputCommands;
    }
}
