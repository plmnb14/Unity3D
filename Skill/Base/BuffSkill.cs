using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffSkill : SkillData
{
    protected Image icon;
    protected Image iconBack;
    public override void Initialization(Creature user, int Index)
    {
        if (isCooldown || !CheckManaCost(user.MyStatus.Mana))
            return;

        icon.fillAmount = 1.0f;

        SetDefault(user, Index);
    }

    WaitForSeconds waitSeconds = new WaitForSeconds(0.1f);
    protected IEnumerator BuffDuration()
    {
        StartCoroutine(base.Activation());

        while (durationTime > 0)
        {
            durationTime -= 0.1f;
            icon.fillAmount = durationTime / skillData.duration;
            yield return waitSeconds;
        }

        icon.fillAmount = 0.0f;
        DeActivation();
    }

    protected virtual void DeActivation()
    {
        Destroy(gameObject);
    }

    protected override void Execute()
    {
        if (Owner.Dead)
            return;

        StartCoroutine(Activation());

        StartCoroutine(BuffDuration());

        StartCoroutine(CoolDown());
    }

    protected void Loadchild()
    {
        icon = this.transform.GetChild(1).GetComponent<Image>();
        iconBack = this.transform.GetChild(0).GetComponent<Image>();
    }

    protected override IEnumerator CoolDown()
    {
        yield return base.CoolDown();

        this.gameObject.SetActive(false);
    }
}
