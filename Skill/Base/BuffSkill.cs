using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffSkill : SkillData
{
    Image icon; // Imange Type을 Filled로 바꾸기

    public override void Initialization(Creature user, int Index)
    {
        if (isCooldown || !CheckManaCost(user.MyStatus.Mana))
            return;

        base.Initialization(user, Index);
        //icon.fillAmount = 1.0f;
        //base.Execute();
    }

    WaitForSeconds waitSeconds = new WaitForSeconds(0.1f);
    protected IEnumerator BuffDuration()
    {
        StartCoroutine(base.Activation());

        while (durationTime > 0)
        {
            durationTime -= 0.1f;
            //icon.fillAmount = currentTime / skillData.duration;
            yield return waitSeconds;
        }

        DeActivation();
        //icon.fillAmount = 0.0f;
    }

    protected virtual void DeActivation()
    {
        Destroy(gameObject);
    }

    protected override void Execute()
    {
        StartCoroutine(Activation());

        StartCoroutine(BuffDuration());

        StartCoroutine(CoolDown());
    }
}
