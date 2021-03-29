using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorType : Creature
{
    public override void OnDie()
    {
        base.OnDie();
    }

    protected override void ActiveSkill(int index)
    {
        base.ActiveSkill(index);
    }

    public override void OnDamage(Vector3 hitPoint, Vector3 hitNormal, float damage)
    {
        base.OnDamage(hitPoint, hitNormal, damage);
    }

    protected override IEnumerator OnAttack()
    {
        base.OnAttack();

        if (canAttack)
        {
            canAttack = false;
            animator.SetInteger("AttackNum", UnityEngine.Random.Range(0, 3));
            animator.SetBool("isAttack", true);
        }

        else
        {
            TargetLookAt();

            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            CurrentState = State.Idle;
            animator.SetBool("isAttack", false);
        }
    }

    protected virtual void ActiveWeaponCollider(int iValue)
    {
        MyWeapon.GetComponent<Collider>().enabled = iValue == 0 ? false : true;
    }

    void Start()
    {
        base.Initialize();
    }

    void Update()
    {
        CheckState();
        AttackTimer();
    }
}
