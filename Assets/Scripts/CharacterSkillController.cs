using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkillController : MonoBehaviour
{
    [SerializeField]
    private GameObject skillPaddle;

    private bool isCooldownActive = false;
    private float cooldownTime = 5.0f;
    private float currentcooldownTimeLeft = 0.0f;
    private Coroutine StartCoolDownCoroutine;
    private Coroutine ActivateSkillCoroutine;

    public void UseSkill()
    {
        if (!isCooldownActive) {

            ActivateSkillCoroutine = StartCoroutine(ActivateSkill());
            StartCoolDownCoroutine = StartCoroutine(StartCoolDown());
        }
    }

    public float CurrentCooldownLeft()
    {
        return currentcooldownTimeLeft / cooldownTime;
    }

    private IEnumerator ActivateSkill()
    {
        skillPaddle.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        skillPaddle.SetActive(false);
        ActivateSkillCoroutine = null;
        yield return null;

    }

    private IEnumerator StartCoolDown()
    {
        isCooldownActive = true;
        currentcooldownTimeLeft = cooldownTime;
        while(currentcooldownTimeLeft >= 0)
        {
            currentcooldownTimeLeft -= Time.deltaTime;
            yield return null;
        }

        isCooldownActive = false;
        currentcooldownTimeLeft = 0;
        StartCoolDownCoroutine = null;
        yield return null;
    }
}
