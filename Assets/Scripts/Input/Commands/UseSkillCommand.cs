using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSkillCommand : Command
{
    /// <summary>
    /// Execute the command 
    /// </summary>
    /// <param name="character">The character this command controls</param>
    public override void Execute(CharacterController character)
    {
        character.UseSkill();
    }
}
