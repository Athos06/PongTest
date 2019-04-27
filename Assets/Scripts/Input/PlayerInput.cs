using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class PlayerInput : ICharacterInput
{
    private Command movementCommand = new MovementCommand();
    private Command skillCommand = new UseSkillCommand();

    public List<Command> GetInput()
    {
        ((MovementCommand)movementCommand).MovementeVector = Vector3.zero;
        List<Command> inputCommands = new List<Command>();

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            ((MovementCommand)movementCommand).MovementeVector += Vector3.left;
            inputCommands.Add(movementCommand);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            ((MovementCommand)movementCommand).MovementeVector += Vector3.right;
            inputCommands.Add(movementCommand);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            inputCommands.Add(skillCommand);
        }

        return inputCommands;
    }
}