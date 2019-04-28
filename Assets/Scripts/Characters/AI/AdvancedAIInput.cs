using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedAIInput : ICharacterInput
{
    private Command movementCommand = new MovementCommand();
    private Command skillCommand = new UseSkillCommand();
    private Ball ball;
    private Transform character;
    private float reactionTime = 0.01f;
    private float reactionTimeCounter = 0.0f;
    private float recalculateTime = 0.70f;
    private float recalculateTimeCounter = 0.0f;
    private float sensitivity = 0.1f;
    private Vector3 destinationPoint = Vector3.zero;

    public AdvancedAIInput(Transform character, Ball ball)
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
        recalculateTimeCounter += Time.deltaTime;


        if (reactionTimeCounter > reactionTime)
        {
            reactionTimeCounter = 0;
            RaycastHit info;
            Ray sphereRay = new Ray(character.transform.position, Vector3.up);
            if (Physics.SphereCast(character.position, 2.0f, character.forward, out info, 2.5f))
            {
                inputCommands.Add(skillCommand);
            }

            if (recalculateTimeCounter > recalculateTime)
            {
                recalculateTimeCounter = 0;
                Vector3 ballTrayectory = ball.RigidBody.velocity.normalized;
                Ray ray = new Ray(ball.transform.position, ballTrayectory);
                Plane plane = new Plane(character.forward, character.position.z);
                float enter = 0.0f;
                if (ballTrayectory.magnitude == 0)
                {
                    destinationPoint = Vector3.zero;
                }
                else if (plane.Raycast(ray, out enter))
                {
                    destinationPoint = ray.GetPoint(enter);
                }
                else
                {
                    destinationPoint = Vector3.zero;
                }
            }

            if (destinationPoint.x < character.position.x &&
                Mathf.Abs(destinationPoint.x - character.position.x) > sensitivity)
            {
                ((MovementCommand)movementCommand).MovementeVector += Vector3.left;
                inputCommands.Add(movementCommand);
            }
            if (destinationPoint.x > character.position.x &&
                Mathf.Abs(destinationPoint.x - character.position.x) > sensitivity)
            {
                ((MovementCommand)movementCommand).MovementeVector += Vector3.right;
                inputCommands.Add(movementCommand);
            }
        }


        return inputCommands;
    }

}
