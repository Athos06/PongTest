using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillHudController : MonoBehaviour
{
    [SerializeField]
    private Image coolDownBar;
    [SerializeField]
    private CharacterSkillController characterSkillController;

    private bool Initialized = false;

    public void Initialize(CharacterSkillController character)
    {
        this.characterSkillController = character;
        Initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Initialized)
            return;

        coolDownBar.fillAmount = 1 - characterSkillController.CurrentCooldownLeft();
    }
}
