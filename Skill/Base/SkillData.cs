using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Cooldown(int index);
public delegate void SkillEnd();
public class SkillData : MonoBehaviour
{
    private event Cooldown SkillcoolDown;
    private event SkillEnd SkillisEnd;

    public SkillBaseData skillData { get; set; }
    public LayerMask targetMask { get; set; }

    public bool isCooldown { get; set; } = false;
    protected float currentTime;
    protected float durationTime;
    protected float aniTime;
    protected int skillIndex;
    
    protected Creature Owner;
    protected Animator animator;

    public virtual void ActiveSkill()
    {

    }

    public bool CheckManaCost(float mana)
    {
        return mana >= skillData.manaCost ? true : false;
    }

    public virtual void SetDefault(Creature user, int Index)
    {
        Owner = user;
        animator = Owner.gameObject.GetComponent<Animator>();
        animator.SetTrigger(skillData.stateName);
        currentTime = skillData.cooltime;
        skillIndex = Index;
        durationTime = skillData.duration;

        SkillcoolDown += Owner.GetComponent<Creature>().CooldownEnd;
        SkillisEnd += Owner.GetComponent<Creature>().EndSkill;
    }

    public virtual void Initialization(Creature user, int Index)
    {
        if (isCooldown || !CheckManaCost(user.MyStatus.Mana))
            return;

        SetDefault(user, Index);
        Execute();
    }

    protected void ResetBaseInfo()
    {
        SkillisEnd -= Owner.GetComponent<Creature>().EndSkill;
        SkillcoolDown -= Owner.gameObject.GetComponent<Creature>().CooldownEnd;

        aniTime = 0.0f;
        currentTime = 0.0f;
        isCooldown = false;

        Owner = null;
        animator = null;
    }

    protected virtual void Execute()
    {
        StartCoroutine(Activation());
        StartCoroutine(CoolDown());
    }

    private float animationTime = 0.0f;
    protected virtual IEnumerator Activation()
    {
        while(true)
        {
            if (false == isCooldown)
            {
                yield return new WaitForEndOfFrame();

                isCooldown = true;
                animationTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime * 1.2f;
            }

            else
            {
                yield return new WaitForSeconds(animationTime);

                SkillisEnd();
                yield break;
            }
        }
    }

    WaitForSeconds waitSecond = new WaitForSeconds(0.1f);
    protected virtual IEnumerator CoolDown()
    {
        while (currentTime > 0)
        {
            if(isCooldown)
                currentTime -= 0.1f;

            yield return waitSecond;
        }

        SkillcoolDown(skillIndex);
        ResetBaseInfo();
    }

}
